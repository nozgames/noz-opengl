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

namespace NoZ.Platform.OpenGL
{
    internal class OpenGLImage : Image
    {
        private byte[] _bytes;
        private bool _locked;

        public uint Id { get; private set; }

        public OpenGLImage (string name) : base(name) { }

        public OpenGLImage (string name, int width, int height, PixelFormat format) : base (name, width, height, format) {
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
#if !__NOZ_IOS__
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
            _bytes = null;

            return true;
        }

    }

}
