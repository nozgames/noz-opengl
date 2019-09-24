using System;

namespace NoZ.Platform.OpenGL {
    class OpenGLTexture  {
        private uint _id;

        /// <summary>
        /// Bind the texture 
        /// </summary>
        public void Bind () {
            GL.ActiveTexture(GL.TextureUnit.Texture0);
            GL.BindTexture(GL.TextureTarget.Texture2D, _id);
        }

        /// <summary>
        /// Upload image data to the texture.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="format"></param>
        /// <param name="bytes"></param>
        public void Upload (int width, int height, PixelFormat format, byte[] bytes) {
            if (bytes == null)
                return;

            if (_id == 0) {
                _id = GL.GenTexture();
                if (_id == 0)
                    return;
            }

            var textureFormat = GL.TextureFormat.Rgba;
            var pixelFormat = GL.PixelFormat.Rgba;
            switch (format) {
                case PixelFormat.A8:
                    textureFormat = GL.TextureFormat.Alpha;
                    pixelFormat = GL.PixelFormat.Alpha;
                    break;
                case PixelFormat.R8G8B8:
                    textureFormat = GL.TextureFormat.Rgb;
                    pixelFormat = GL.PixelFormat.Rgb;
                    break;
                case PixelFormat.R8G8B8A8:
                    break;
                default:
                    throw new NotImplementedException();
            }


            GL.BindTexture(GL.TextureTarget.Texture2D, _id);
            GL.TexImage(
                GL.TextureTarget2d.Texture2D,
                0,
                textureFormat,
                width,
                height,
                0,
                pixelFormat,
                GL.PixelType.UnsignedByte,
                bytes
               );

            GL.Disable(GL.EnableCapability.DepthTest);
#if !__IOS__
            GL.Disable(GL.EnableCapability.AlphaTest);
#endif
#if false
            GL.TexParameter(GL.TextureTarget.Texture2D, GL.TextureParameterName.TextureMinFilter, (int)GL.TextureMagFilter.Linear);
            GL.TexParameter(GL.TextureTarget.Texture2D, GL.TextureParameterName.TextureMagFilter, (int)GL.TextureMagFilter.Linear);
#else
            GL.TexParameter(GL.TextureTarget.Texture2D, GL.TextureParameterName.TextureMinFilter, (int)GL.TextureMagFilter.Nearest);
            GL.TexParameter(GL.TextureTarget.Texture2D, GL.TextureParameterName.TextureMagFilter, (int)GL.TextureMagFilter.Nearest);
#endif

            GL.TexParameter(GL.TextureTarget.Texture2D, GL.TextureParameterName.TextureWrapS, (int)GL.TextureClamp.Edge);
            GL.TexParameter(GL.TextureTarget.Texture2D, GL.TextureParameterName.TextureWrapT, (int)GL.TextureClamp.Edge);

            // Release the bytes since they are now on the GPU
            bytes = null;
        }
    }
}
