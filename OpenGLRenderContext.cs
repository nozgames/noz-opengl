using System;
using System.Runtime.InteropServices;

using NoZ.Platform.OpenGL.ES30;
using NoZ.Graphics;

namespace NoZ.Platform.OpenGL {
    class OpenGLRenderContext : GraphicsContext {
        private static readonly int VertexBufferSize = 1024;
        private static readonly int IndexBufferSize = VertexBufferSize * 4;

        private Vector2Int _viewSize;
        private Vector2Int _projectionSize;

        private ColorShader _colorShader;
        private TextureShaderA8 _textureShaderA8;
        private TextureShaderRGBA _textureShaderRGBA;
        private TextureShaderSDF _textureShaderSDF;
        private TextureShaderStencil _textureShaderStencil;

        private Matrix3 _currentTransform;
        private Image _currentImage;
        private OpenGLShader _currentShader;
        private Color _currentColor;
        private int _maskDepth;
        private MaskMode _maskMode = MaskMode.None;

        private OpenGLBatch _batch = new OpenGLBatch();

        public OpenGLRenderContext() {

            try {
                _colorShader = new ColorShader();
                _colorShader.Build();

                _textureShaderA8 = new TextureShaderA8();
                _textureShaderA8.Build();

                _textureShaderRGBA = new TextureShaderRGBA();
                _textureShaderRGBA.Build();

                _textureShaderSDF = new TextureShaderSDF();
                _textureShaderSDF.Build();

                _textureShaderStencil = new TextureShaderStencil();
                _textureShaderStencil.Build();

            } catch (OpenGLException e) {
                Console.WriteLine($"error: {e.Message}");
                throw new ApplicationException();
            }
        }

        public override void Begin(Vector2Int size, Color backgroundColor) {
            base.Begin(size, backgroundColor);

            _viewSize = size;

            // Update the projection in all of the shaders if the view size changes.
            if (_projectionSize != _viewSize) {
                _projectionSize = _viewSize;
                Matrix4 projection = Matrix4.CreateOrtho(0.0f, size.x, size.y, 0.0f, -1000.0f, 1000.0f);
                _colorShader.Use();
                _colorShader.Projection = projection;
                _textureShaderA8.Use();
                _textureShaderA8.Projection = projection;
                _textureShaderRGBA.Use();
                _textureShaderRGBA.Projection = projection;
                _textureShaderSDF.Use();
                _textureShaderSDF.Projection = projection;
                _textureShaderStencil.Use();
                _textureShaderStencil.Projection = projection;

                if(null != _batch.Shader)
                    _batch.Shader.Use();
                GL.ClearStencil(0);
            }

            _maskMode = MaskMode.None;
            _maskDepth = 0;

            GL.Viewport(0, 0, size.x, size.y);

            GL.Enable(GL.EnableCapability.Blend);
            GL.BlendFunc(GL.BlendFactorSrc.SrcAlpha, GL.BlendFactorDest.OneMinusSrcAlpha);

            GL.Disable(GL.EnableCapability.StencilTest);
            GL.ColorMask(true, true, true, true);
            GL.DepthMask(true);

            if (backgroundColor.A > 0) {
                GL.ClearColor(backgroundColor);
                GL.Clear(GL.ClearBuffer.Color | GL.ClearBuffer.Depth);
            } else {
                GL.Clear(GL.ClearBuffer.Depth);
            }            
        }

        public override void End() {
            _batch.Commit();
            base.End();
        }

        public override void SetMaskMode (MaskMode mode) {
            _maskMode = mode;
        }

        public override void SetColor(Color color) {
            _currentColor = color;
        }

        public override void SetTransform(in Matrix3 transform) {
            _currentTransform = Matrix3.Multiply(transform, WorldToScreen);
        }

        public override void SetImage(in Image image) {
            _currentImage = image;

            if (_currentImage != null && _currentImage.PixelFormat == PixelFormat.A8) {
                _currentShader = _textureShaderSDF;
            } else if (_currentImage != null && _currentImage.PixelFormat == PixelFormat.R8G8B8A8) {
                _currentShader = _textureShaderRGBA;
            } else {
                _currentShader = _colorShader;
            }
        }

        public void CommitBatchIfNecessary(PrimitiveType primitive) {
            if (_batch.Image == _currentImage &&
                _batch.PrimitiveType == primitive &&
                _batch.MaskMode == _maskMode &&
                _batch.MaskDepth == _maskDepth && 
                _batch.Shader == _currentShader) {
                return;
            }

            _batch.Commit();
        }

        public override void Draw(PrimitiveType primitive, Vertex[] vertexBuffer, int vertexCount, short[] indexBuffer, int indexCount) {
            if (_currentColor.A == 0)
                return;

            CommitBatchIfNecessary(primitive);

            _batch.Image = (OpenGLImage)_currentImage;
            _batch.PrimitiveType = PrimitiveType.TriangleList;
            _batch.Shader = _currentShader;
            _batch.MaskDepth = _maskDepth;
            _batch.MaskMode = _maskMode;

            if (_batch.MaskMode == MaskMode.Draw && _batch.Image != null)
                _batch.Shader = _textureShaderStencil;


            if (!_batch.Add(vertexBuffer, vertexCount, indexBuffer, indexCount, _currentTransform, _currentColor)) {
                _batch.Commit();
                _batch.Add(vertexBuffer, vertexCount, indexBuffer, indexCount, _currentTransform, _currentColor);
            }
        }

        public override void Draw(PrimitiveType primitive, Vertex[] vertexBuffer, int vertexCount) {
            if (_currentColor.A == 0)
                return;

            CommitBatchIfNecessary(primitive);

            _batch.Image = (OpenGLImage)_currentImage;
            _batch.Shader = _currentShader;
            _batch.MaskDepth = _maskDepth;
            _batch.MaskMode = _maskMode;

            if (_batch.MaskMode == MaskMode.Draw && _batch.Image != null)
                _batch.Shader = _textureShaderStencil;

            switch (primitive) {
                case PrimitiveType.TriangleStrip:
                    _batch.PrimitiveType = PrimitiveType.TriangleList;
                    if (!_batch.AddTriangleStrip(vertexBuffer, vertexCount, _currentTransform, _currentColor)) {
                        _batch.Commit();
                        _batch.AddTriangleStrip(vertexBuffer, vertexCount, _currentTransform, _currentColor);
                    }
                    break;

                case PrimitiveType.LineList:
                    _batch.PrimitiveType = PrimitiveType.LineList;
                    if (!_batch.AddLineList(vertexBuffer, vertexCount, _currentTransform, _currentColor)) {
                        _batch.Commit();
                        _batch.AddLineList(vertexBuffer, vertexCount, _currentTransform, _currentColor);
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public override void Draw (in Quad quad) {
            if (_currentColor.A == 0)
                return;

            CommitBatchIfNecessary(PrimitiveType.TriangleList);

            _batch.Image = (OpenGLImage)_currentImage;
            _batch.PrimitiveType = PrimitiveType.TriangleList;
            _batch.Shader = _currentShader;
            _batch.MaskDepth = _maskDepth;
            _batch.MaskMode = _maskMode;

            if (_batch.MaskMode == MaskMode.Draw && _batch.Image != null)
                _batch.Shader = _textureShaderStencil;

            if (!_batch.AddQuad(quad, _currentTransform, _currentColor)) {
                _batch.Commit();
                _batch.AddQuad(quad, _currentTransform, _currentColor);
            }
        }

        public override void Draw (Quad[] quads, int count) {
            if (_currentColor.A == 0)
                return;

            CommitBatchIfNecessary(PrimitiveType.TriangleList);

            _batch.Image = (OpenGLImage)_currentImage;
            _batch.PrimitiveType = PrimitiveType.TriangleList;
            _batch.Shader = _currentShader;
            _batch.MaskDepth = _maskDepth;
            _batch.MaskMode = _maskMode;

            if (_batch.MaskMode == MaskMode.Draw && _batch.Image != null)
                _batch.Shader = _textureShaderStencil;


            if (!_batch.AddQuads(quads, count, _currentTransform, _currentColor)) {
                _batch.Commit();
                _batch.AddQuads(quads, count, _currentTransform, _currentColor);
            }
        }

        public override void PushMask () {
            _maskDepth++;
        }

        public override void PopMask() {
            _maskDepth--;
        }
    }
}
