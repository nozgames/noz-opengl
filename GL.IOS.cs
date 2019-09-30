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

#if __NOZ_IOS__

using System;
using System.Runtime.InteropServices;

namespace NoZ.Platform.OpenGL
{
    public static partial class GL
    {
        private const string OpenGL32 = "/System/OpenGL32/Frameworks/OpenGLES.framework/OpenGLES";

        static partial class Imports
        {
            [DllImport(OpenGL32, ExactSpelling = true)]
            public static extern unsafe int glCreateShader(ShaderType type);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glShaderSource(uint shader, int count, [In] string[] source, [In] int[] length);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe uint glCreateProgram();

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glAttachShader(uint program, uint shader);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glUseProgram(uint program);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glDeleteShader(uint shader);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glLinkProgram(uint program);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glDeleteProgram(uint program);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glGetProgramiv(uint program, int pname, [Out] int* @params);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glCompileShader(uint shader);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glGetShaderiv(uint shader, int pname, [Out] int* @params);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glGetShaderInfoLog(uint shader, int bufSize, int* length, IntPtr infoLog);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glGetProgramInfoLog(uint program, int bufSize, int* length, IntPtr infoLog);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern int glGetUniformLocation(uint program, string name);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glUniformMatrix4fv(int location, int count, bool transpose, float* value);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glUniform4fv(int location, int count, float* value);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glGenBuffers(int n, [Out] uint* buffers);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glBindBuffer(BufferTarget target, uint buffer);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glEnableVertexAttribArray(uint index);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glVertexAttribPointer(uint index, int size, int type, bool normalized, int stride, IntPtr pointer);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glBufferData(int target, IntPtr size, IntPtr data, int usage);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glBindAttribLocation(uint program, uint index, string name);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glActiveTexture(TextureUnit texture);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glUniform1f(int location, float v0);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern int glGetUniformBlockIndex(uint program, string name);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glBindBufferRange(int target, uint index, uint buffer, IntPtr offset, IntPtr size);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glBufferSubData(int target, IntPtr offset, IntPtr size, IntPtr data);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern int glUniformBlockBinding(uint program, uint blockIndex, uint bindingPointIndex);
        }
    }
}

#endif
