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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NoZ.Platform.OpenGL
{
    public class OpenGLDriver : IGraphicsDriver
    {
#if !__NOZ_IOS__
#else
        private uint _renderBufferId;
#endif

        private OpenGLDriver() {}

        public static OpenGLDriver Create () => new OpenGLDriver();

        public GraphicsContext CreateContext() => new OpenGLRenderContext();

        public Image CreateImage() => new OpenGLImage(null);

        public Image CreateImage(string name, int width, int height, PixelFormat format) => new OpenGLImage(name, width, height, format);

        public void BeginFrame()
        {            
        }

        public void EndFrame()
        {            
        }
    }
}
