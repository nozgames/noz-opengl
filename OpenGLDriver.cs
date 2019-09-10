using System;
using System.IO;

#if !__IOS__
using System.Drawing;
#endif

namespace NoZ.Platform.OpenGL {
    class OpenGLDriver : IGraphicsDriver {
        public GraphicsContext CreateContext() {
            return new OpenGLRenderContext();
        }

        public Image CreateImage() {
            return new OpenGLImage();
        }

        public Image LoadImage(Stream stream) {
            throw new NotImplementedException();
        }

        public Image CreateImage(int width, int height, PixelFormat format) {
            return new OpenGLImage(width, height, format);
        }

        public virtual Cursor CreateCursor(Image image) {
            return null;
        }

        public virtual Cursor CreateCursor(SystemCursor systemCursor) {
            return null;
        }
    }
}
