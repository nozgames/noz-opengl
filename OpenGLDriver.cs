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
using System.Runtime.InteropServices;

using NoZ.Graphics;

namespace NoZ.Platform.OpenGL
{
    public class OpenGLDriver : IGraphicsDriver
    {
#if !__IOS__
        private IntPtr _hglrc;
        private IntPtr _hdc;
#endif

        private OpenGLDriver() {}

        public static OpenGLDriver Create ()
        {
            return new OpenGLDriver();
        }

        public void Bind (Window window)
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

            var hwnd = window.GetNativeHandle();
            _hdc = GL.Win32.GetDC(hwnd);
            var id = GL.Win32.wglChoosePixelFormat(_hdc, ref pfd);
            GL.Win32.SetPixelFormat(_hdc, id, ref pfd);
            _hglrc = GL.Win32.wglCreateContext(_hdc);
            GL.Win32.wglMakeCurrent(_hdc, _hglrc);
#endif

            GL.ClearColor(1, 0, 0, 1);
            GL.Clear(GL.ClearBuffer.Color);
            GL.Win32.wglSwapBuffers(_hdc);

            // Disable V-Sync ?
            //GL.Imports.wglSwapIntervalEXT(0);#endif
        }

        public GraphicsContext CreateContext() {
            return new OpenGLRenderContext();
        }

        public Image CreateImage() {
            return new OpenGLImage(null);
        }

        public Image CreateImage(string name, int width, int height, PixelFormat format) {
            return new OpenGLImage(name, width, height, format);
        }

        public void BeginFrame()
        {            
        }

        public void EndFrame()
        {
            GL.Win32.wglSwapBuffers(_hdc);
        }
    }
}
