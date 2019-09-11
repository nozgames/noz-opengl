
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using NoZ.Graphics;

namespace NoZ.Platform.OpenGL {
    class OpenGLBatch {
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct Group {
            public static readonly int SizeInBytes = Marshal.SizeOf<OpenGLVertex>();

            public float M11;
            public float M21;
            public float M31;
            public float M12;
            public float M22;
            public float M32;
            float pad1;
            float pad2;
            public float R;
            public float G;
            public float B;
            public float A;
        };

        public PrimitiveType PrimitiveType { get; set; }
        public OpenGLShader Shader { get; set; }
        public OpenGLImage Image { get; set; }
        public MaskMode MaskMode { get; set; }
        public int MaskDepth { get; set; }

        private OpenGLBuffer<OpenGLVertex> _vertexBuffer;
        private OpenGLBuffer<ushort> _indexBuffer;
        private OpenGLBuffer<Group> _groupBuffer;

        private static uint _activeImage;
        private static uint _activeShader;
        private static uint _activeVertexBuffer;
        private static uint _activeIndexBuffer;
        private static uint _activeGroupBuffer;
        private static int _activeMaskDepth;
        private static MaskMode _activeMaskMode = MaskMode.None;
        private static GL.TextureUnit _activeTextureUnit = GL.TextureUnit.None;

        /// <summary>
        /// The maximum batch size is measured in individual quads.
        /// </summary>
        private const int MaxQuads = 256;
        private const int MaxVerts = MaxQuads * 4;
        private const int MaxIndicies = MaxQuads * 6;

        public OpenGLBatch( ) {
            _vertexBuffer = new OpenGLBuffer<OpenGLVertex>(GL.BufferTarget.ArrayBuffer, MaxVerts);
            _indexBuffer = new OpenGLBuffer<ushort>(GL.BufferTarget.ElementArrayBuffer, MaxIndicies);
            _groupBuffer = new OpenGLBuffer<Group>(GL.BufferTarget.UniformBuffer, MaxQuads);
        }

        public bool Add (
            Vertex[] vertexBuffer, 
            int vertexCount,
            short[] indexBuffer,
            int indexCount,
            in Matrix3 transform,
            Color color
            ) {

            // Do we have enough room?
            if (_vertexBuffer.Count + vertexCount > _vertexBuffer.Capacity)
                return false;
            if (_indexBuffer.Count + indexCount > _indexBuffer.Capacity)
                return false;
            if (_groupBuffer.Count + 1 >= _groupBuffer.Capacity)
                return false;

            // Copy the verticies to the vertex buffer.
            var baseVertex = _vertexBuffer.Count;
            for (int i = 0; i < vertexCount; i++) {
                _vertexBuffer.Add(new OpenGLVertex {
                    xy = vertexBuffer[i].XY,
                    uv = vertexBuffer[i].UV,
                    color = vertexBuffer[i].Color.Value,
                    group = (uint)_groupBuffer.Count
                });
            }

            for (int i = 0; i < indexCount; i++)
                _indexBuffer.Add((ushort)(indexBuffer[i] + baseVertex));

            _groupBuffer.Add(new Group {
                M11 = transform.M11,
                M12 = transform.M12,
                M21 = transform.M21,
                M22 = transform.M22,
                M31 = transform.M31,
                M32 = transform.M32,
                A = color.A / 255.0f,
                B = color.B / 255.0f,
                R = color.R / 255.0f,
                G = color.G / 255.0f
            });

            return true;
        }


        public bool AddTriangleStrip (
            Vertex[] vertexBuffer,
            int vertexCount,
            in Matrix3 transform,
            Color color
            ) {

            if (vertexCount < 3)
                return true;

            var triangleCount = vertexCount - 2;
            var indexCount = triangleCount * 3;

            // Do we have enough room?
            if (_vertexBuffer.Count + vertexCount > _vertexBuffer.Capacity)
                return false;
            if (_indexBuffer.Count + indexCount > _indexBuffer.Capacity)
                return false;
            if (_groupBuffer.Count + 1 >= _groupBuffer.Capacity)
                return false;

            // Copy the verticies to the vertex buffer.
            var baseVertex = _vertexBuffer.Count;
            for (int i = 0; i < vertexCount; i++) {
                _vertexBuffer.Add(new OpenGLVertex {
                    xy = vertexBuffer[i].XY,
                    uv = vertexBuffer[i].UV,
                    color = vertexBuffer[i].Color.Value,
                    group = (uint)_groupBuffer.Count
                });
            }

            for (int i=0; i< triangleCount; i++, baseVertex++) {
                if((i&1)==0) {
                    _indexBuffer.Add((ushort)(baseVertex + 2));
                    _indexBuffer.Add((ushort)baseVertex);
                    _indexBuffer.Add((ushort)(baseVertex + 1));
                } else {
                    _indexBuffer.Add((ushort)(baseVertex + 2));
                    _indexBuffer.Add((ushort)(baseVertex + 1));
                    _indexBuffer.Add((ushort)baseVertex);
                }
            }

            _groupBuffer.Add(new Group {
                M11 = transform.M11,
                M12 = transform.M12,
                M21 = transform.M21,
                M22 = transform.M22,
                M31 = transform.M31,
                M32 = transform.M32,
                A = color.A / 255.0f,
                B = color.B / 255.0f,
                R = color.R / 255.0f,
                G = color.G / 255.0f
            });

            return true;
        }

        public bool AddLineList(
            Vertex[] vertexBuffer,
            int vertexCount,
            in Matrix3 transform,
            Color color
            ) {

            // Do we have enough room?
            if (_vertexBuffer.Count + vertexCount > _vertexBuffer.Capacity)
                return false;
            if (_indexBuffer.Count + vertexCount > _indexBuffer.Capacity)
                return false;
            if (_groupBuffer.Count + 1 >= _groupBuffer.Capacity)
                return false;

            // Copy the verticies to the vertex buffer.
            var baseVertex = _vertexBuffer.Count;
            for (int i = 0; i < vertexCount; i++) {
                _vertexBuffer.Add(new OpenGLVertex {
                    xy = vertexBuffer[i].XY,
                    uv = vertexBuffer[i].UV,
                    color = vertexBuffer[i].Color.Value,
                    group = (uint)_groupBuffer.Count
                });
                _indexBuffer.Add((ushort)i);
            }

            _groupBuffer.Add(new Group {
                M11 = transform.M11,
                M12 = transform.M12,
                M21 = transform.M21,
                M22 = transform.M22,
                M31 = transform.M31,
                M32 = transform.M32,
                A = color.A / 255.0f,
                B = color.B / 255.0f,
                R = color.R / 255.0f,
                G = color.G / 255.0f
            });

            return true;
        }
        public bool AddQuad (in Quad quad, in Matrix3 transform, Color color) {
            // Do we have enough room?
            if (_vertexBuffer.Count + 4 > _vertexBuffer.Capacity)
                return false;
            if (_indexBuffer.Count + 6 > _indexBuffer.Capacity)
                return false;
            if (_groupBuffer.Count + 1 >= _groupBuffer.Capacity)
                return false;

            // Copy the verticies to the vertex buffer.            
            var baseVertex = _vertexBuffer.Count;
            _vertexBuffer.Add(new OpenGLVertex {
                xy = quad.TL.XY,
                uv = quad.TL.UV,
                color = quad.TL.Color.Value,
                group = _groupBuffer.Count
            });
            _vertexBuffer.Add(new OpenGLVertex {
                xy = quad.TR.XY,
                uv = quad.TR.UV,
                color = quad.TR.Color.Value,
                group = _groupBuffer.Count
            });
            _vertexBuffer.Add(new OpenGLVertex {
                xy = quad.BR.XY,
                uv = quad.BR.UV,
                color = quad.BR.Color.Value,
                group = _groupBuffer.Count
            });
            _vertexBuffer.Add(new OpenGLVertex {
                xy = quad.BL.XY,
                uv = quad.BL.UV,
                color = quad.BL.Color.Value,
                group = _groupBuffer.Count
            });
            _indexBuffer.Add((ushort)baseVertex);
            _indexBuffer.Add((ushort)(baseVertex + 1));
            _indexBuffer.Add((ushort)(baseVertex + 3));
            _indexBuffer.Add((ushort)(baseVertex + 1));
            _indexBuffer.Add((ushort)(baseVertex + 2));
            _indexBuffer.Add((ushort)(baseVertex + 3));

            _groupBuffer.Add(new Group {
                M11 = transform.M11,
                M12 = transform.M12,
                M21 = transform.M21,
                M22 = transform.M22,
                M31 = transform.M31,
                M32 = transform.M32,
                A = color.A / 255.0f,
                B = color.B / 255.0f,
                R = color.R / 255.0f,
                G = color.G / 255.0f
            });

            return true;
        }

        public bool AddQuads(Quad[] quads, int count, in Matrix3 transform, Color color) {
            var triangleCount = count * 2;
            var indexCount = triangleCount * 3;
            var vertexCount = count * 4;

            // Do we have enough room?
            if (_vertexBuffer.Count + vertexCount > _vertexBuffer.Capacity)
                return false;
            if (_indexBuffer.Count + indexCount > _indexBuffer.Capacity)
                return false;
            if (_groupBuffer.Count + 1 >= _groupBuffer.Capacity)
                return false;

            // Copy the verticies to the vertex buffer.            
            for (int i = 0; i < count; i++) {
                var baseVertex = _vertexBuffer.Count;
                _vertexBuffer.Add(new OpenGLVertex {
                    xy = quads[i].TL.XY,
                    uv = quads[i].TL.UV,
                    color = quads[i].TL.Color.Value,
                    group = _groupBuffer.Count
                });
                _vertexBuffer.Add(new OpenGLVertex {
                    xy = quads[i].TR.XY,
                    uv = quads[i].TR.UV,
                    color = quads[i].TR.Color.Value,
                    group = _groupBuffer.Count
                });
                _vertexBuffer.Add(new OpenGLVertex {
                    xy = quads[i].BR.XY,
                    uv = quads[i].BR.UV,
                    color = quads[i].BR.Color.Value,
                    group = _groupBuffer.Count
                });
                _vertexBuffer.Add(new OpenGLVertex {
                    xy = quads[i].BL.XY,
                    uv = quads[i].BL.UV,
                    color = quads[i].BL.Color.Value,
                    group = _groupBuffer.Count
                });
                _indexBuffer.Add((ushort)baseVertex);
                _indexBuffer.Add((ushort)(baseVertex + 1));
                _indexBuffer.Add((ushort)(baseVertex + 3));
                _indexBuffer.Add((ushort)(baseVertex + 1));
                _indexBuffer.Add((ushort)(baseVertex + 2));
                _indexBuffer.Add((ushort)(baseVertex + 3));
            }

            _groupBuffer.Add(new Group {
                M11 = transform.M11,
                M12 = transform.M12,
                M21 = transform.M21,
                M22 = transform.M22,
                M31 = transform.M31,
                M32 = transform.M32,
                A = color.A / 255.0f,
                B = color.B / 255.0f,
                R = color.R / 255.0f,
                G = color.G / 255.0f
            });

            return true;
        }

        public void Commit () {
            if (_groupBuffer.Count == 0)
                return;

            if (Image != null)
                Image.Bind();

            UpdateMaskMode();

            if (Image != null) {
                if (_activeTextureUnit != GL.TextureUnit.Texture0) {
                    GL.ActiveTexture(GL.TextureUnit.Texture0);
                    _activeTextureUnit = GL.TextureUnit.Texture0;
                }

                if (_activeImage != Image.Id) {                    
                    GL.BindTexture(GL.TextureTarget.Texture2D, Image.Id);
                    _activeImage = Image.Id;
                }

                Image.Upload();
            }

            if(Shader.Id != _activeShader) {
                _activeShader = Shader.Id;
                GL.UseProgram(Shader.Id);
            }

            // Bind the group bock to the uniform
            GL.UniformBlockBinding(Shader.Id, Shader.UniformGroups, 0);

            var indexCount = _indexBuffer.Count;
            Debug.Assert(indexCount > 0);

            // Send all the buffers to the GPU
            _groupBuffer.Commit(_activeGroupBuffer);
            _indexBuffer.Commit(_activeIndexBuffer);
            _vertexBuffer.Commit(_activeVertexBuffer);

            _activeGroupBuffer = _groupBuffer.Id;
            _activeIndexBuffer = _indexBuffer.Id;
            _activeVertexBuffer = _vertexBuffer.Id;

            switch (PrimitiveType) {
                case PrimitiveType.TriangleList:
                    GL.DrawElements(GL.PrimitiveType.Triangles, indexCount, GL.DrawElementsType.UnsignedShort, IntPtr.Zero);
                    break;

                case PrimitiveType.LineList:
                    GL.DrawElements(GL.PrimitiveType.Lines, indexCount, GL.DrawElementsType.UnsignedShort, IntPtr.Zero);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void UpdateMaskMode ( ) {
            // If the depth changed then clear
            if (MaskDepth != _activeMaskDepth) {
                // Clear the stencil buffer for the depth to make sure 
                // there are not any residuals.
                // TODO: we could optimize this by instead just drawing a rectangle
                // to the stencil buffer over the bounds that was last used to render to
                // that depth.
                if (MaskDepth > 0 && MaskDepth > _activeMaskDepth) {
                    GL.StencilMask(1 << (MaskDepth - 1));
                    GL.Clear(GL.ClearBuffer.Stencil);
                }

                _activeMaskDepth = MaskDepth;
            }

            if (MaskMode == _activeMaskMode)
                return;

            // Transition out state
            switch (_activeMaskMode) {
                case MaskMode.Draw:
                    GL.ColorMask(true, true, true, true);
                    GL.DepthMask(true);
                    break;

                case MaskMode.None:
                    if (_activeMaskDepth > 0)
                        GL.Enable(GL.EnableCapability.StencilTest);
                    break;
            }

            switch (MaskMode) {
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
                    if (_activeMaskMode != MaskMode.Outside) {
                        GL.Enable(GL.EnableCapability.StencilTest);
                        GL.StencilOp(GL.StencilAction.Keep, GL.StencilAction.Keep, GL.StencilAction.Keep);
                        GL.StencilMask(0);
                    }
                    break;

                case MaskMode.Outside:
                    GL.StencilFunc(GL.TestFunction.NotEqual, ((1 << _activeMaskDepth) - 1), 0xFF);
                    if (_activeMaskMode != MaskMode.Inside) {
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
