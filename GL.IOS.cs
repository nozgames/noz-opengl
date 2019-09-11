using System;
using System.Collections.Generic;
using System.Text;

#if __IOS__

namespace NoZ.Platform.OpenGL
{
    static partial class GL
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
