
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

#if !__IOS__
using System.Drawing;
#endif

namespace NoZ.Platform.OpenGL {
    public class OpenGLDriver : IGraphicsDriver {
#if !__IOS__
        private IntPtr _hglrc;
        private IntPtr _hdc;
#endif

        public static OpenGLDriver Create (Window window)
        {
#if !__IOS__
            // Set the pixel format for this DC
            var pfd = new GL.Win32.PixelFormatDescriptor
            {
                Size = (short)Marshal.SizeOf<GL.Win32.PixelFormatDescriptor>(),
                Version = 1,
                Flags = GL.Win32.PixelFormatDescriptorFlags.DRAW_TO_WINDOW |
                        GL.Win32.PixelFormatDescriptorFlags.SUPPORT_OPENGL |
                        GL.Win32.PixelFormatDescriptorFlags.DOUBLEBUFFER,
                PixelType = GL.Win32.PixelType.Rgba,
                ColorBits = 32,
                RedBits = 0,
                RedShift = 0,
                GreenBits = 0,
                GreenShift = 0,
                BlueBits = 0,
                BlueShift = 0,
                AlphaBits = 0,
                AlphaShift = 0,
                AccumBits = 0,
                AccumRedBits = 0,
                AccumGreenBits = 0,
                AccumBlueBits = 0,
                AccumAlphaBits = 0,
                DepthBits = 32,
                StencilBits = 8,
                AuxBuffers = 0,
                LayerType = 0,
                LayerMask = 0,
                DamageMask = 0
            };

            var driver = new OpenGLDriver();
            var hwnd = window.GetNativeHandle();
            driver._hdc = GL.Win32.GetDC(hwnd);
            var id = GL.Win32.wglChoosePixelFormat(driver._hdc, ref pfd);
            GL.Win32.SetPixelFormat(driver._hdc, id, ref pfd);
            driver._hglrc = GL.Win32.wglCreateContext(driver._hdc);
            GL.Win32.wglMakeCurrent(driver._hdc, driver._hglrc);
#endif

            GL.ClearColor(1, 0, 0, 1);
            GL.Clear(GL.ClearBuffer.Color);
            GL.Win32.wglSwapBuffers(driver._hdc);

            // Disable V-Sync ?
            //GL.Imports.wglSwapIntervalEXT(0);#endif
            return driver;
        }

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

        public void BeginFrame()
        {
            
        }

        public void EndFrame()
        {
            GL.Win32.wglSwapBuffers(_hdc);
        }

#if false
        public virtual Cursor CreateCursor(Image image) {
            return null;
        }

        public virtual Cursor CreateCursor(SystemCursor systemCursor) {
            return null;
        }
#endif
    }
}
