/*
  NoZ Game Engine

  Copyright(c) 2019 NoZ Games, LLC

  Permission is hereby granted, free of charge, to any person obtaining a copy
  of this software and associated documentation files(the "Software"), to deal
  in the Software without restriction, including without limitation the rights
  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
  copies of the Software, and to permit persons to whom the Software is
  furnished to do so, subject to the following conditions :

  The above copyright notice and this permission notice shall be included in all
  copies or substantial portions of the Software.

  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
  SOFTWARE.
*/

using System;
using System.Diagnostics;

namespace NoZ.Platform.OpenGL
{
    class OpenGLBatch
    {
        /// <summary>
        /// Primative type used to render the batch
        /// </summary>
        public DrawNodeType DrawNodeType { get; set; }

        /// <summary>
        /// Shader used to render the batch
        /// </summary>
        public OpenGLShader Shader { get; set; }

        /// <summary>
        /// Image used to render the batch
        /// </summary>
        public OpenGLImage Image { get; set; }

        /// <summary>
        /// Mask mode used to render the batch
        /// </summary>
        public MaskMode MaskMode { get; set; } = MaskMode.None;

        /// <summary>
        /// Depth of the mask
        /// </summary>
        public int MaskDepth { get; set; }

        /// <summary>
        /// Vertex buffer used to render the batch
        /// </summary>
        private OpenGLBuffer<OpenGLVertex> _vertexBuffer;

        /// <summary>
        /// Index buffer used to render the batch
        /// </summary>
        private OpenGLBuffer<ushort> _indexBuffer;

        private static uint _activeImage;
        private static uint _activeShader;
        private static uint _activeVertexBuffer;
        private static uint _activeIndexBuffer;
        private static int _activeMaskDepth;
        private static MaskMode _activeMaskMode = MaskMode.None;
        private static GL.TextureUnit _activeTextureUnit = GL.TextureUnit.None;

        /// <summary>
        /// The maximum batch size is measured in individual quads.
        /// </summary>
        private const int MaxQuads = 2048;
        private const int MaxVerts = MaxQuads * 4;
        private const int MaxIndicies = MaxQuads * 6;

        public OpenGLBatch()
        {
            _vertexBuffer = new OpenGLBuffer<OpenGLVertex>(GL.BufferTarget.ArrayBuffer, MaxVerts);
            _indexBuffer = new OpenGLBuffer<ushort>(GL.BufferTarget.ElementArrayBuffer, MaxIndicies);
        }

        public bool Add (in Quad quad, Color color)
        {
            // Do we have enough room?
            if (_vertexBuffer.Count + 4 > _vertexBuffer.Capacity)
                return false;
            if (_indexBuffer.Count + 6 > _indexBuffer.Capacity)
                return false;

            // Copy the verticies to the vertex buffer.            
            var baseVertex = _vertexBuffer.Count;
            _vertexBuffer.Add(new OpenGLVertex
            {
                xy = quad.TL.XY,
                uv = quad.TL.UV,
                color = (quad.TL.Color * color).Value
            });
            _vertexBuffer.Add(new OpenGLVertex
            {
                xy = quad.TR.XY,
                uv = quad.TR.UV,
                color = (quad.TR.Color * color).Value
            });
            _vertexBuffer.Add(new OpenGLVertex
            {
                xy = quad.BR.XY,
                uv = quad.BR.UV,
                color = (quad.BR.Color * color).Value
            });
            _vertexBuffer.Add(new OpenGLVertex
            {
                xy = quad.BL.XY,
                uv = quad.BL.UV,
                color = (quad.BL.Color * color).Value
            });

            _indexBuffer.Add((ushort)baseVertex);
            _indexBuffer.Add((ushort)(baseVertex + 1));
            _indexBuffer.Add((ushort)(baseVertex + 3));
            _indexBuffer.Add((ushort)(baseVertex + 1));
            _indexBuffer.Add((ushort)(baseVertex + 2));
            _indexBuffer.Add((ushort)(baseVertex + 3));

            return true;
        }

        public bool Add(in Vertex from, in Vertex to, Color color)
        {
            // Do we have enough room?
            if (_vertexBuffer.Count + 2 > _vertexBuffer.Capacity)
                return false;
            if (_indexBuffer.Count + 2 > _indexBuffer.Capacity)
                return false;

            // Copy the verticies to the vertex buffer.            
            var baseVertex = _vertexBuffer.Count;
            _vertexBuffer.Add(new OpenGLVertex
            {
                xy = from.XY,
                uv = from.UV,
                color = (from.Color * color).Value
            });
            _vertexBuffer.Add(new OpenGLVertex
            {
                xy = to.XY,
                uv = to.UV,
                color = (to.Color * color).Value
            });

            _indexBuffer.Add((ushort)baseVertex);
            _indexBuffer.Add((ushort)(baseVertex + 1));

            return true;
        }


        public void Commit()
        {
            if (_vertexBuffer.Count == 0)
                return;

            if (Image != null)
                Image.Bind();

            UpdateMaskMode();

            if (Image != null)
            {
                if (_activeTextureUnit != GL.TextureUnit.Texture0)
                {
                    GL.ActiveTexture(GL.TextureUnit.Texture0);
                    _activeTextureUnit = GL.TextureUnit.Texture0;
                }

                if (_activeImage != Image.Id)
                {
                    GL.BindTexture(GL.TextureTarget.Texture2D, Image.Id);
                    _activeImage = Image.Id;
                }

                Image.Upload();
            }

            if (Shader.Id != _activeShader)
            {
                _activeShader = Shader.Id;
                GL.UseProgram(Shader.Id);
            }

            var indexCount = _indexBuffer.Count;
            Debug.Assert(indexCount > 0);

            // Send all the buffers to the GPU
            _indexBuffer.Commit(_activeIndexBuffer);
            _vertexBuffer.Commit(_activeVertexBuffer);

            _activeIndexBuffer = _indexBuffer.Id;
            _activeVertexBuffer = _vertexBuffer.Id;

            switch (DrawNodeType)
            {
                case DrawNodeType.Quad:
                    GL.DrawElements(GL.PrimitiveType.Triangles, indexCount, GL.DrawElementsType.UnsignedShort, IntPtr.Zero);
                    break;

                case DrawNodeType.DebugLine:
                    GL.DrawElements(GL.PrimitiveType.Lines, indexCount, GL.DrawElementsType.UnsignedShort, IntPtr.Zero);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void UpdateMaskMode()
        {
            // If the depth changed then clear
            if (MaskDepth != _activeMaskDepth)
            {
                // Clear the stencil buffer for the depth to make sure 
                // there are not any residuals.
                // TODO: we could optimize this by instead just drawing a rectangle
                // to the stencil buffer over the bounds that was last used to render to
                // that depth.
                if (MaskDepth > 0 && MaskDepth > _activeMaskDepth)
                {
                    GL.StencilMask(1 << (MaskDepth - 1));
                    GL.Clear(GL.ClearBuffer.Stencil);
                }

                _activeMaskDepth = MaskDepth;
            }

            if (MaskMode == _activeMaskMode)
                return;

            // Transition out state
            switch (_activeMaskMode)
            {
                case MaskMode.Draw:
                    GL.ColorMask(true, true, true, true);
                    GL.DepthMask(true);
                    break;

                case MaskMode.None:
                    if (_activeMaskDepth > 0)
                        GL.Enable(GL.EnableCapability.StencilTest);
                    break;
            }

            switch (MaskMode)
            {
                case MaskMode.None:
                    GL.Disable(GL.EnableCapability.StencilTest);
                    break;

                case MaskMode.Draw:
                    GL.Enable(GL.EnableCapability.StencilTest);

                    // Stop writing to color and depth buffer
                    GL.ColorMask(false, false, false, false);
                    GL.DepthMask(false);

                    // Enable writing to stencil buffer but only for the current depth
                    GL.StencilFunc(GL.TestFunction.Never, 0xFF, 0xFF);
                    GL.StencilOp(GL.StencilAction.Replace, GL.StencilAction.Keep, GL.StencilAction.Keep);
                    GL.StencilMask(1 << (_activeMaskDepth - 1));
                    break;

                case MaskMode.Inside:
                    GL.StencilFunc(GL.TestFunction.Equal, ((1 << _activeMaskDepth) - 1), 0xFF);
                    if (_activeMaskMode != MaskMode.Outside)
                    {
                        GL.Enable(GL.EnableCapability.StencilTest);
                        GL.StencilOp(GL.StencilAction.Keep, GL.StencilAction.Keep, GL.StencilAction.Keep);
                        GL.StencilMask(0);
                    }
                    break;

                case MaskMode.Outside:
                    GL.StencilFunc(GL.TestFunction.NotEqual, ((1 << _activeMaskDepth) - 1), 0xFF);
                    if (_activeMaskMode != MaskMode.Inside)
                    {
                        GL.Enable(GL.EnableCapability.StencilTest);
                        GL.StencilOp(GL.StencilAction.Keep, GL.StencilAction.Keep, GL.StencilAction.Keep);
                        GL.StencilMask(0);
                    }
                    break;
            }

            _activeMaskMode = MaskMode;
        }
    }
}
