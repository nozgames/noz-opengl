using System;

namespace NoZ.Platform.OpenGL {

    internal class OpenGLImage : Image {
        private byte[] _bytes;
        private bool _locked;

        public uint Id { get; private set; }

        public OpenGLImage () { }

        public OpenGLImage (int width, int height, PixelFormat format) : base (width, height, format) {
            _bytes = new byte[height * Stride];
        }
    
        protected override byte[] LockBytes () {
            if(_bytes == null) {
                if(Id == 0) {
                    _bytes = new byte[Height * Stride];
                } else {
                    // TODO: read the bytes from the GPU
                    throw new NotImplementedException();
                }
            }

            _locked = true;

            return _bytes;
        }

        protected override void UnlockBytes() {
            _locked = false;
        }

        public void Bind() {
            if (null != _bytes && !_locked)
                Upload();

        }

        public bool Upload() {
            if (_locked)
                return false;

            if (_bytes == null)
                return true;

            if (Id == 0) {
                Id = GL.GenTexture();
                if (Id == 0)
                    return false;
            }

            var textureFormat = GL.TextureFormat.Rgba;
            var pixelFormat = GL.PixelFormat.Rgba;
            switch(PixelFormat) {
                case PixelFormat.A8: textureFormat = GL.TextureFormat.Alpha; pixelFormat = GL.PixelFormat.Alpha;  break;
                case PixelFormat.R8G8B8: textureFormat = GL.TextureFormat.Rgb; pixelFormat = GL.PixelFormat.Rgb;  break;
                case PixelFormat.R8G8B8A8: break;
                default:
                    throw new NotImplementedException();
            }


            GL.BindTexture(GL.TextureTarget.Texture2D, Id);
            GL.TexImage(
                GL.TextureTarget2d.Texture2D,
                0,
                textureFormat,
                Width,
                Height,
                0,
                pixelFormat,
                GL.PixelType.UnsignedByte,
                _bytes
               );

            GL.Disable(GL.EnableCapability.DepthTest);
#if !__IOS__
            GL.Disable(GL.EnableCapability.AlphaTest);
#endif
            GL.TexParameter(GL.TextureTarget.Texture2D, GL.TextureParameterName.TextureMinFilter, (int)GL.TextureMagFilter.Linear);
            GL.TexParameter(GL.TextureTarget.Texture2D, GL.TextureParameterName.TextureMagFilter, (int)GL.TextureMagFilter.Linear);

            GL.TexParameter(GL.TextureTarget.Texture2D, GL.TextureParameterName.TextureWrapS, (int)GL.TextureClamp.Edge);
            GL.TexParameter(GL.TextureTarget.Texture2D, GL.TextureParameterName.TextureWrapT, (int)GL.TextureClamp.Edge);

            // Release the bytes since they are now on the GPU
            _bytes = null;

            return true;
        }

    }

}
