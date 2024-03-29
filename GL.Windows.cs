﻿/*
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

#if __NOZ_WINDOWS__

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace NoZ.Platform.OpenGL
{
    public static partial class GL
    {
        private const string OpenGL32 = "opengl32.dll";

        public static class Win32
        {
            private const string User32 = "user32.dll";
            private const string Gdi32 = "gdi32.dll";
            private const string Kernel32 = "kernel32.dll";

            static Win32()
            {
                LoadLibrary(OpenGL32);
            }

            [DllImport(Kernel32, SetLastError = true)]
            public static extern IntPtr LoadLibrary(string dllName);

            [DllImport(Gdi32, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetPixelFormat(IntPtr dc, int format, ref PixelFormatDescriptor pfd);

            [DllImport(User32)]
            public static extern IntPtr GetDC(IntPtr hwnd);

            [DllImport(OpenGL32, ExactSpelling = true, SetLastError = true)]
            public extern static unsafe int wglChoosePixelFormat(IntPtr hdc, ref PixelFormatDescriptor ppfd);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(OpenGL32, ExactSpelling = true, SetLastError = true)]
            public extern static IntPtr wglCreateContext(IntPtr hDc);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(OpenGL32, ExactSpelling = true, SetLastError = true)]
            public extern static bool wglMakeCurrent(IntPtr hDc, IntPtr newContext);

            [DllImport(OpenGL32, ExactSpelling = true, SetLastError = true)]
            public extern static bool wglSwapBuffers(IntPtr hdc);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(OpenGL32, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            public static extern IntPtr wglGetProcAddress(string functionName);

            [Flags]
            public enum PixelFormatDescriptorFlags : int
            {
                // PixelFormatDescriptor flags
                DOUBLEBUFFER = 0x01,
                STEREO = 0x02,
                DRAW_TO_WINDOW = 0x04,
                DRAW_TO_BITMAP = 0x08,
                SUPPORT_GDI = 0x10,
                SUPPORT_OPENGL = 0x20,
                GENERIC_FORMAT = 0x40,
                NEED_PALETTE = 0x80,
                NEED_SYSTEM_PALETTE = 0x100,
                SWAP_EXCHANGE = 0x200,
                SWAP_COPY = 0x400,
                SWAP_LAYER_BUFFERS = 0x800,
                GENERIC_ACCELERATED = 0x1000,
                SUPPORT_DIRECTDRAW = 0x2000,
                SUPPORT_COMPOSITION = 0x8000,

                // PixelFormatDescriptor flags for use in ChoosePixelFormat only
                DEPTH_DONTCARE = unchecked((int)0x20000000),
                DOUBLEBUFFER_DONTCARE = unchecked((int)0x40000000),
                STEREO_DONTCARE = unchecked((int)0x80000000)
            }

            public enum PixelType : byte
            {
                Rgba = 0,
                Indexed = 1
            }


            [StructLayout(LayoutKind.Sequential)]
            public struct PixelFormatDescriptor
            {
                public short Size;
                public short Version;
                public PixelFormatDescriptorFlags Flags;
                public PixelType PixelType;
                public byte ColorBits;
                public byte RedBits;
                public byte RedShift;
                public byte GreenBits;
                public byte GreenShift;
                public byte BlueBits;
                public byte BlueShift;
                public byte AlphaBits;
                public byte AlphaShift;
                public byte AccumBits;
                public byte AccumRedBits;
                public byte AccumGreenBits;
                public byte AccumBlueBits;
                public byte AccumAlphaBits;
                public byte DepthBits;
                public byte StencilBits;
                public byte AuxBuffers;
                public byte LayerType;
                private byte Reserved;
                public int LayerMask;
                public int VisibleMask;
                public int DamageMask;
            }
        }

        public static partial class Imports
        {
            public delegate int glCreateShaderDelegate(ShaderType type);
            public static glCreateShaderDelegate glCreateShader;

            public delegate void glShaderSourceDelegate(uint shader, int count, string[] source, int[] length);
            public static glShaderSourceDelegate glShaderSource;

            public delegate uint glCreateProgramDelegate();
            public static glCreateProgramDelegate glCreateProgram;

            public delegate void glAttachShaderDelegate(uint program, uint shader);
            public static glAttachShaderDelegate glAttachShader;

            public delegate void glDeleteShaderDelegate(uint shader);
            public static glDeleteShaderDelegate glDeleteShader;

            public delegate void glLinkProgramDelegate(uint program);
            public static glLinkProgramDelegate glLinkProgram;

            public delegate void glDeleteProgramDelegate(uint program);
            public static glDeleteProgramDelegate glDeleteProgram;

            public unsafe delegate void glGetProgramivDelegate(uint program, int pname, int* @values);
            public static glGetProgramivDelegate glGetProgramiv;

            public delegate void glUseProgramDelegate(uint program);
            public static glUseProgramDelegate glUseProgram;

            public unsafe delegate void glGetProgramInfoLogDelegate(uint shader, int bufSize, int* length, IntPtr text);
            public static glGetProgramInfoLogDelegate glGetProgramInfoLog;

            public delegate void glCompileShaderDelegate(uint shader);
            public static glCompileShaderDelegate glCompileShader;

            public unsafe delegate void glGetShaderivDelegate(uint shader, int pname, int* @values);
            public static glGetShaderivDelegate glGetShaderiv;

            public unsafe delegate void glGetShaderInfoLogDelegate(uint shader, int bufSize, int* length, IntPtr text);
            public static glGetShaderInfoLogDelegate glGetShaderInfoLog;

            public delegate int glGetUniformLocationDelegate(uint program, string name);
            public static glGetUniformLocationDelegate glGetUniformLocation;

            public delegate int glGetUniformBlockIndexDelegate(uint program, string name);
            public static glGetUniformBlockIndexDelegate glGetUniformBlockIndex;

            public delegate int glUniformBlockBindingDelegate(uint program, uint blockIndex, uint bindingPointIndex);
            public static glUniformBlockBindingDelegate glUniformBlockBinding;

            public unsafe delegate void glUniformMatrix4fvDelegate(int location, int count, bool transpose, float* value);
            public static glUniformMatrix4fvDelegate glUniformMatrix4fv;

            public unsafe delegate void glUniform4fvDelegate(int location, int count, float* value);
            public static glUniform4fvDelegate glUniform4fv;

            public unsafe delegate void glUniform1fDelegate(int location, float value);
            public static glUniform1fDelegate glUniform1f;

            public unsafe delegate void glGenBuffersDelegate(int n, [Out] uint* buffers);
            public static glGenBuffersDelegate glGenBuffers;

            public delegate void glBindBufferDelegate(BufferTarget target, uint buffer);
            public static glBindBufferDelegate glBindBuffer;

            public delegate void glBindBufferRangeDelegate(int target, uint index, uint buffer, IntPtr offset, IntPtr size);
            public static glBindBufferRangeDelegate glBindBufferRange;


            public delegate void glEnableVertexAttribArrayDelegate(uint index);
            public static glEnableVertexAttribArrayDelegate glEnableVertexAttribArray;

            public delegate void glVertexAttribPointerDelegate(uint index, int size, int type, bool normalized, int stride, IntPtr pointer);
            public static glVertexAttribPointerDelegate glVertexAttribPointer;

            public delegate void glBufferDataDelegate(int target, IntPtr size, IntPtr data, int usage);
            public static glBufferDataDelegate glBufferData;

            public delegate void glBufferSubDataDelegate(int target, IntPtr offset, IntPtr size, IntPtr data);
            public static glBufferSubDataDelegate glBufferSubData;

            public delegate void glBindAttribLocationDelegate(uint program, uint index, string name);
            public static glBindAttribLocationDelegate glBindAttribLocation;

            public delegate void glActiveTextureDelegate(TextureUnit texture);
            public static glActiveTextureDelegate glActiveTexture;

            public delegate void wglSwapIntervalEXTDelegate(int interval);
            public static wglSwapIntervalEXTDelegate wglSwapIntervalEXT;

            static Imports()
            {
                glCreateShader = Marshal.GetDelegateForFunctionPointer<glCreateShaderDelegate>(GL.Win32.wglGetProcAddress("glCreateShader"));
                glShaderSource = Marshal.GetDelegateForFunctionPointer<glShaderSourceDelegate>(GL.Win32.wglGetProcAddress("glShaderSource"));
                glCreateProgram = Marshal.GetDelegateForFunctionPointer<glCreateProgramDelegate>(GL.Win32.wglGetProcAddress("glCreateProgram"));
                glAttachShader = Marshal.GetDelegateForFunctionPointer<glAttachShaderDelegate>(GL.Win32.wglGetProcAddress("glAttachShader"));
                glDeleteShader = Marshal.GetDelegateForFunctionPointer<glDeleteShaderDelegate>(GL.Win32.wglGetProcAddress("glDeleteShader"));
                glLinkProgram = Marshal.GetDelegateForFunctionPointer<glLinkProgramDelegate>(GL.Win32.wglGetProcAddress("glLinkProgram"));
                glDeleteProgram = Marshal.GetDelegateForFunctionPointer<glDeleteProgramDelegate>(GL.Win32.wglGetProcAddress("glDeleteProgram"));
                glGetProgramiv = Marshal.GetDelegateForFunctionPointer<glGetProgramivDelegate>(GL.Win32.wglGetProcAddress("glGetProgramiv"));
                glGetProgramInfoLog = Marshal.GetDelegateForFunctionPointer<glGetProgramInfoLogDelegate>(GL.Win32.wglGetProcAddress("glGetProgramInfoLog"));
                glUseProgram = Marshal.GetDelegateForFunctionPointer<glUseProgramDelegate>(GL.Win32.wglGetProcAddress("glUseProgram"));
                glCompileShader = Marshal.GetDelegateForFunctionPointer<glCompileShaderDelegate>(GL.Win32.wglGetProcAddress("glCompileShader"));
                glGetShaderiv = Marshal.GetDelegateForFunctionPointer<glGetShaderivDelegate>(GL.Win32.wglGetProcAddress("glGetShaderiv"));
                glGetShaderInfoLog = Marshal.GetDelegateForFunctionPointer<glGetShaderInfoLogDelegate>(GL.Win32.wglGetProcAddress("glGetShaderInfoLog"));
                glGetUniformBlockIndex = Marshal.GetDelegateForFunctionPointer<glGetUniformBlockIndexDelegate>(GL.Win32.wglGetProcAddress("glGetUniformBlockIndex"));
                glUniformBlockBinding = Marshal.GetDelegateForFunctionPointer<glUniformBlockBindingDelegate>(GL.Win32.wglGetProcAddress("glUniformBlockBinding"));
                glGetUniformLocation = Marshal.GetDelegateForFunctionPointer<glGetUniformLocationDelegate>(GL.Win32.wglGetProcAddress("glGetUniformLocation"));
                glUniformMatrix4fv = Marshal.GetDelegateForFunctionPointer<glUniformMatrix4fvDelegate>(GL.Win32.wglGetProcAddress("glUniformMatrix4fv"));
                glUniform4fv = Marshal.GetDelegateForFunctionPointer<glUniform4fvDelegate>(GL.Win32.wglGetProcAddress("glUniform4fv"));
                glUniform1f = Marshal.GetDelegateForFunctionPointer<glUniform1fDelegate>(GL.Win32.wglGetProcAddress("glUniform1f"));
                glGenBuffers = Marshal.GetDelegateForFunctionPointer<glGenBuffersDelegate>(GL.Win32.wglGetProcAddress("glGenBuffers"));
                glBindBuffer = Marshal.GetDelegateForFunctionPointer<glBindBufferDelegate>(GL.Win32.wglGetProcAddress("glBindBuffer"));
                glBindBufferRange = Marshal.GetDelegateForFunctionPointer<glBindBufferRangeDelegate>(GL.Win32.wglGetProcAddress("glBindBufferRange"));
                glEnableVertexAttribArray = Marshal.GetDelegateForFunctionPointer<glEnableVertexAttribArrayDelegate>(GL.Win32.wglGetProcAddress("glEnableVertexAttribArray"));
                glVertexAttribPointer = Marshal.GetDelegateForFunctionPointer<glVertexAttribPointerDelegate>(GL.Win32.wglGetProcAddress("glVertexAttribPointer"));
                glBufferData = Marshal.GetDelegateForFunctionPointer<glBufferDataDelegate>(GL.Win32.wglGetProcAddress("glBufferData"));
                glBufferSubData = Marshal.GetDelegateForFunctionPointer<glBufferSubDataDelegate>(GL.Win32.wglGetProcAddress("glBufferSubData"));
                glBindAttribLocation = Marshal.GetDelegateForFunctionPointer<glBindAttribLocationDelegate>(GL.Win32.wglGetProcAddress("glBindAttribLocation"));
                glActiveTexture = Marshal.GetDelegateForFunctionPointer<glActiveTextureDelegate>(GL.Win32.wglGetProcAddress("glActiveTexture"));
                wglSwapIntervalEXT = Marshal.GetDelegateForFunctionPointer<wglSwapIntervalEXTDelegate>(GL.Win32.wglGetProcAddress("wglSwapIntervalEXT"));
            }
        }
    }
}

#endif
