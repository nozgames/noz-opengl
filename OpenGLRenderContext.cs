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

using NoZ.Platform.OpenGL.ES30;

namespace NoZ.Platform.OpenGL
{
    class OpenGLRenderContext : GraphicsContext
    {
        private Vector2Int _viewSize;
        private Vector2Int _projectionSize;

        private ColorShader _colorShader;
        private TextureShaderA8 _textureShaderA8;
        private TextureShaderRGBA _textureShaderRGBA;
        private TextureShaderRGB _textureShaderRGB;
        private TextureShaderSDF _textureShaderSDF;
        private TextureShaderStencil _textureShaderStencil;

        private OpenGLBatch _batch = new OpenGLBatch();

        public OpenGLRenderContext()
        {
            try
            {
                _colorShader = new ColorShader();
                _colorShader.Build();

                _textureShaderA8 = new TextureShaderA8();
                _textureShaderA8.Build();

                _textureShaderRGBA = new TextureShaderRGBA();
                _textureShaderRGBA.Build();

                _textureShaderRGB = new TextureShaderRGB();
                _textureShaderRGB.Build();

                _textureShaderSDF = new TextureShaderSDF();
                _textureShaderSDF.Build();

                _textureShaderStencil = new TextureShaderStencil();
                _textureShaderStencil.Build();

            }
            catch (OpenGLException e)
            {
                Console.WriteLine($"error: {e.Message}");
                throw new ApplicationException();
            }
        }

        protected override void Begin(Vector2Int size, Color backgroundColor)
        {
            base.Begin(size, backgroundColor);

            _viewSize = size;

            // Update the projection in all of the shaders if the view size changes.
            if (_projectionSize != _viewSize)
            {
                _projectionSize = _viewSize;
                Matrix4 projection = Matrix4.CreateOrtho(0.0f, size.x, size.y, 0.0f, -1000.0f, 1000.0f);
                _colorShader.Use();
                _colorShader.Projection = projection;
                _textureShaderA8.Use();
                _textureShaderA8.Projection = projection;
                _textureShaderRGBA.Use();
                _textureShaderRGBA.Projection = projection;
                _textureShaderRGB.Use();
                _textureShaderRGB.Projection = projection;
                _textureShaderSDF.Use();
                _textureShaderSDF.Projection = projection;
                _textureShaderStencil.Use();
                _textureShaderStencil.Projection = projection;

                if (null != _batch.Shader)
                    _batch.Shader.Use();
                GL.ClearStencil(0);
            }

            // Initialize the viewport to the given size
            GL.Viewport(0, 0, size.x, size.y);

            GL.Enable(GL.EnableCapability.Blend);
            GL.BlendFunc(GL.BlendFactorSrc.SrcAlpha, GL.BlendFactorDest.OneMinusSrcAlpha);

            GL.Disable(GL.EnableCapability.StencilTest);
            GL.ColorMask(true, true, true, true);
            GL.DepthMask(true);

            // Clear the background if a background color was given
            if (backgroundColor.A > 0)
            {
                GL.ClearColor(backgroundColor);
                GL.Clear(GL.ClearBuffer.Color | GL.ClearBuffer.Depth);
            }
            else
            {
                GL.Clear(GL.ClearBuffer.Depth);
            }
        }

        protected override void End()
        {
            base.End();
            _batch.Commit();
        }

        /// <summary>
        /// Return the default shader used to render the given image
        /// </summary>
        private OpenGLShader GetDefaultShader (Image image) 
        { 
            if (image != null && image.PixelFormat == PixelFormat.A8)
                return _textureShaderSDF;
            else if (image != null && image.PixelFormat == PixelFormat.R8G8B8A8)
                return _textureShaderRGBA;
            else if (image != null && image.PixelFormat == PixelFormat.R8G8B8)
                return _textureShaderRGB;
            else
                return _colorShader;
        }

        /// <summary>
        /// Draw a single node
        /// </summary>
        protected override void Draw (DrawNode node)
        {
            var shader = GetDefaultShader(node.image);

            if (_batch.Image != node.image || _batch.Shader != shader || _batch.DrawNodeType != node.Type)
                _batch.Commit();

            switch(node.Type)
            {
                case DrawNodeType.Quad:
                    _batch.Image = node.image as OpenGLImage;
                    _batch.DrawNodeType = node.Type;
                    _batch.Shader = shader;
                    _batch.Add(node.quad, node.color);
                    break;

                case DrawNodeType.DebugLine:
                    _batch.Image = null;
                    _batch.DrawNodeType = node.Type;
                    _batch.Shader = shader;
                    _batch.Add(node.quad.TL, node.quad.TR, node.color);
                    break;
            }
        }
    }
}
