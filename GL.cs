using System;
using System.Runtime.InteropServices;

namespace NoZ.Platform.OpenGL {
    static partial class GL
    {
        public static partial class Imports
        {
            public static readonly int GL_RENDERBUFFER = 0x8D41;
            public static readonly int GL_FRAMEBUFFER = 0x8D40;
            public static readonly int GL_COLOR_ATTACHMENT0 = 0x8CE0;

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glViewport(int x, int y, int width, int height);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glClear(int mask);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glClearColor(float red, float green, float blue, float alpha);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public extern static unsafe void glGenRenderbuffers(int n, [Out] uint* renderbuffers);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glBindRenderbuffer(int target, uint renderbuffer);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glGenFramebuffers(int n, [Out] uint* framebuffers);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glBindFramebuffer(int target, uint renderbuffer);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glFramebufferRenderbuffer(int target, int attachment, int renderbuffertarget, uint renderbuffer);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glEnable(int cap);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glDisable(int cap);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glBlendFunc(int sfactor, int dfactor);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glDrawElements(int mode, int count, int type, IntPtr indices);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glDrawArrays(int mode, int first, int count);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glBindTexture(TextureTarget target, uint texture);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glGenTextures(int n, [Out] uint* textures);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glTexImage2D(int target, int level, int internalformat, int width, int height, int border, int format, int type, IntPtr pixels);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glTexParameterf(int target, int pname, float param);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glTexParameteri(int target, int pname, int param);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glColorMask(bool red, bool green, bool blue, bool alpha);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glDepthMask(bool depth);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glStencilFunc(int func, int reference, uint mask);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glStencilOp(int stencilFail, int depthFail, int depthPass);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glStencilMask(uint mask);

            [DllImport(OpenGL32, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glClearStencil(uint value);
        };


        [Flags]
        public enum ClearBuffer : int
        {
            Depth = 0x00000100,
            Stencil = 0x00000400,
            Color = 0x00004000
        }

        public enum ProgramParameter : int
        {
            LinkStatus = 0x8B82
        }

        public enum ShaderParameter : int
        {
            ShaderType = 0x8B4F,
            DeleteStatus = 0x8B80,
            CompileStatus = 0x8B81,
            InfoLogLength = 0x8B84,
            ShaderSourceLength = 0x8B88
        }

        public enum ShaderType : int
        {
            FragmentShader = 0x8B30,
            FragmentShaderArb = 0x8B30,
            VertexShader = 0x8B31,
            VertexShaderArb = 0x8B31,
            GeometryShader = 0x8DD9,
            GeometryShaderExt = 0x8DD9,
            TessEvaluationShader = 0x8E87,
            TessControlShader = 0x8E88,
            ComputeShader = 0x91B9
        }

        public enum BufferTarget : int
        {
            ArrayBuffer = 0x8892,
            ElementArrayBuffer = 0x8893,
            PixelPackBuffer = 0x88EB,
            PixelUnpackBuffer = 0x88EC,
            UniformBuffer = 0x8A11,
            TransformFeedbackBuffer = 0x8C8E,
            CopyReadBuffer = 0x8F36,
            CopyWriteBuffer = 0x8F37
        }

        public enum VertexAttribPointerType : int
        {
            UnsignedByte = 0x1401,
            Float = 0X1406,
            UnsignedShort = 0x1403,
            Int = 0x1404,
            UnsignedInt = 0x1405
        }
        public enum PrimitiveType : int
        {
            Lines = 0x0001,
            Triangles = 0x0004,
            TriangleStrip = 0x0005
        }

        public enum EnableCapability : int
        {
            DepthTest = 0x0b71,
            AlphaTest = 0x0bc0,
            StencilTest = 0x0B90,
            Blend = 0x0be2,
        }


        public enum BlendFactorSrc : int
        {
            SrcAlpha = 0x0302
        }

        public enum BlendFactorDest : int
        {
            OneMinusSrcAlpha = 0x0303
        }

        public enum BufferUsage : int
        {
            StreamDraw = 0x88E0
        }

        public enum DrawElementsType : int
        {
            UnsignedByte = 0x1401,
            UnsignedShort = 0x1403,
            UnsignedInt = 0x1405
        }

        public enum TextureUnit : int
        {
            None = 0,
            Texture0 = 0x84c0,
            Texture1 = 0x84c1
        }

        public enum TextureTarget : int
        {
            Texture1D = 0x0DE0,
            Texture2D = 0x0DE1
        }

        public enum TextureTarget2d : int
        {
            Texture2D = 0x0de1
        }

        public enum TextureFormat : int
        {
            Alpha = 0x1906,
            Rgb = 0x1907,
            Rgba = 0x1908
        }

        public enum PixelFormat : int
        {
            Alpha = 0x1906,
            Rgb = 0x1907,
            Rgba = 0x1908
        };

        public enum PixelType : int
        {
            Byte = 0x1400,
            UnsignedByte = 0x1401
        }

        public enum TextureParameterName : int
        {
            TextureMagFilter = 0x2800,
            TextureMinFilter = 0x2801,
            TextureWrapS = 0x2802,
            TextureWrapT = 0x2803
        }

        public enum TextureClamp
        {
            Repeat = 0x2901,
            Edge = 0x812f
        }

        public enum TextureMagFilter : int
        {
            Nearest = 0x2600,
            Linear = 0x2601
        };

        public enum TestFunction : int
        {
            Never = 0x200,
            LessThan = 0x201,
            LessThanOrEqualTo = 0x203,
            GreaterThan = 0x204,
            GreaterThanOrEqualTo = 0x206,
            Equal = 0x202,
            Always = 0x207,
            NotEqual = 0x205
        }

        public enum StencilAction : int
        {
            Keep = 0x1E00,
            Zero = 0,
            Replace = 0x1E01
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct FloatColor
        {
            public float R;
            public float G;
            public float B;
            public float A;
        }

        public static unsafe uint GenRenderBuffer()
        {
            uint id;
            Imports.glGenFramebuffers(1, &id);
            return id;
        }

        public static unsafe void BindRenderBuffer(uint id)
        {
            Imports.glBindRenderbuffer(Imports.GL_RENDERBUFFER, id);
        }

        public static unsafe uint GenFrameBuffer()
        {
            uint id;
            Imports.glGenFramebuffers(1, &id);
            return id;
        }

        public static unsafe void BindFrameBuffer(uint id)
        {
            Imports.glBindFramebuffer(Imports.GL_FRAMEBUFFER, id);
        }

        public static void FrameBufferRenderBuffer(uint id)
        {
            Imports.glFramebufferRenderbuffer(Imports.GL_FRAMEBUFFER, Imports.GL_COLOR_ATTACHMENT0, Imports.GL_RENDERBUFFER, id);
        }

        public static void Viewport(int x, int y, int width, int height)
        {
            Imports.glViewport(x, y, width, height);
        }

        public static void ClearColor(float r, float g, float b, float a)
        {
            Imports.glClearColor(r, g, b, a);
        }

        public static void ClearColor(Color color)
        {
            Imports.glClearColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }

        public static void Clear(ClearBuffer mask)
        {
            Imports.glClear((int)mask);
        }

        public static uint CreateProgram()
        {
            return Imports.glCreateProgram();
        }

        public static void UseProgram(uint program)
        {
            Imports.glUseProgram(program);
        }

        public static void DeleteShader(uint shader)
        {
            Imports.glDeleteShader(shader);
        }

        public static void AttachShader(uint program, uint shader)
        {
            Imports.glAttachShader(program, shader);
        }

        public static void LinkProgram(uint program)
        {
            Imports.glLinkProgram(program);
        }

        public static void DeleteProgram(uint program)
        {
            Imports.glDeleteProgram(program);
        }

        public static void GetProgram(uint program, ProgramParameter param, out int value)
        {
            unsafe
            {
                fixed (int* a = &value)
                {
                    Imports.glGetProgramiv(program, (int)param, a);
                }
            }
        }

        public static void GetShader(uint shader, ShaderParameter param, out int value)
        {
            unsafe
            {
                fixed (int* a = &value)
                {
                    Imports.glGetShaderiv(shader, (int)param, a);
                }
            }
        }

        public static uint CreateShader(ShaderType type)
        {
            return (uint)Imports.glCreateShader(type);
        }

        public static void ShaderSource(uint shader, string source)
        {
            Imports.glShaderSource(shader, 1, new string[] { source }, new int[] { source.Length });
        }

        public static void CompileShader(uint shader)
        {
            Imports.glCompileShader(shader);
        }

        public static string GetShaderInfoLog(uint shader)
        {
            unsafe
            {
                int maxLength = 1024;
                IntPtr infoLogPtr = Marshal.AllocHGlobal(maxLength * Marshal.SizeOf(typeof(byte)) + 1);
                Imports.glGetShaderInfoLog(shader, 1000, &maxLength, infoLogPtr);
                string infoLog = Marshal.PtrToStringAnsi(infoLogPtr);
                Marshal.FreeHGlobal(infoLogPtr);
                return infoLog;
            }
        }

        public static string GetProgramInfoLog(uint program)
        {
            unsafe
            {
                int maxLength = 1024;
                IntPtr infoLogPtr = Marshal.AllocHGlobal(maxLength * Marshal.SizeOf(typeof(byte)) + 1);
                Imports.glGetProgramInfoLog(program, 1000, &maxLength, infoLogPtr);
                string infoLog = Marshal.PtrToStringAnsi(infoLogPtr);
                Marshal.FreeHGlobal(infoLogPtr);
                return infoLog;
            }
        }

        public static uint GetUniformBlockIndex(uint program, string name)
        {
            return (uint)Imports.glGetUniformBlockIndex(program, name);
        }

        public static uint UniformBlockBinding(uint program, uint blockIndex, uint bindingPointIndex)
        {
            return (uint)Imports.glUniformBlockBinding(program, blockIndex, bindingPointIndex);
        }

        public static int GetUniformLocation(uint program, string name)
        {
            return Imports.glGetUniformLocation(program, name);
        }

        public static void UniformMatrix4(int location, bool transpose, Matrix4 matrix)
        {
            unsafe
            {
                Imports.glUniformMatrix4fv(location, 1, transpose, (float*)&matrix);
            }
        }

        public static void UniformColor(uint location, Color color)
        {
            unsafe
            {
                var fc = new FloatColor
                {
                    R = color.R / 255f,
                    G = color.G / 255f,
                    B = color.B / 255f,
                    A = color.A / 255f
                };
                Imports.glUniform4fv((int)location, 1, (float*)&fc);
            }
        }

        public static void Uniform(int location, float value)
        {
            Imports.glUniform1f(location, value);
        }

        public static uint GenBuffer()
        {
            unsafe
            {
                uint id = 0;
                Imports.glGenBuffers(1, &id);
                return id;
            }
        }

        public static void BindBuffer(BufferTarget target, uint buffer)
        {
            Imports.glBindBuffer(target, buffer);
        }

        public static void BindBufferRange(BufferTarget target, uint index, uint buffer, IntPtr offset, IntPtr size)
        {
            Imports.glBindBufferRange((int)target, index, buffer, offset, size);
        }

        public static void EnableVertexAttribArray(uint index)
        {
            Imports.glEnableVertexAttribArray(index);
        }

        public static void VertexAttribPointer(uint index, int size, VertexAttribPointerType type, bool normalized, int stride, int offset)
        {
            Imports.glVertexAttribPointer(index, size, (int)type, normalized, stride, (IntPtr)offset);
        }

        public static void Enable(EnableCapability capability)
        {
            Imports.glEnable((int)capability);
        }

        public static void Disable(EnableCapability capability)
        {
            Imports.glDisable((int)capability);
        }

        public static void BlendFunc(BlendFactorSrc sfactor, BlendFactorDest dfactor)
        {
            Imports.glBlendFunc((int)sfactor, (int)dfactor);
        }

        public static void BufferData(BufferTarget target, int size, IntPtr data, BufferUsage usage)
        {
            Imports.glBufferData((int)target, (IntPtr)size, data, (int)usage);
        }

        public static void BufferSubData(BufferTarget target, int offset, int size, IntPtr data)
        {
            Imports.glBufferSubData((int)target, (IntPtr)offset, (IntPtr)size, data);
        }

        public static void DrawElements(PrimitiveType mode, int count, DrawElementsType type, IntPtr indicies)
        {
            Imports.glDrawElements((int)mode, count, (int)type, indicies);
        }

        public static void DrawArrays(PrimitiveType mode, int first, int count)
        {
            Imports.glDrawArrays((int)mode, first, count);
        }

        public static void ActiveTexture(TextureUnit texture)
        {
            Imports.glActiveTexture(texture);
        }

        public static void BindAttribLocation(uint program, uint index, string name)
        {
            Imports.glBindAttribLocation(program, index, name);
        }

        public static void BindTexture(TextureTarget target, uint texture)
        {
            Imports.glBindTexture(target, texture);
        }

        public static unsafe uint GenTexture()
        {
            uint id;
            Imports.glGenTextures(1, &id);
            return id;
        }

        public static unsafe void TexImage(
            TextureTarget2d target,
            int level,
            TextureFormat internalFormat,
            int width,
            int height,
            int border,
            PixelFormat format,
            PixelType type,
            byte[] pixels
            )
        {

            GCHandle handle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            Imports.glTexImage2D(
                (int)target,
                level,
                (int)internalFormat,
                width,
                height,
                border,
                (int)format,
                (int)type,
                handle.AddrOfPinnedObject()
            );
            handle.Free();
        }

        public static void TexParameter(TextureTarget target, TextureParameterName pname, float param)
        {
            Imports.glTexParameterf((int)target, (int)pname, param);
        }

        public static void TexParameter(TextureTarget target, TextureParameterName pname, int param)
        {
            Imports.glTexParameteri((int)target, (int)pname, param);
        }

        public static void ColorMask(bool red, bool green, bool blue, bool alpha)
        {
            Imports.glColorMask(red, green, blue, alpha);
        }

        public static void DepthMask(bool depth)
        {
            Imports.glDepthMask(depth);
        }

        public static void StencilFunc(TestFunction func, int reference, int mask)
        {
            Imports.glStencilFunc((int)func, reference, (uint)mask);
        }

        public static void StencilOp(StencilAction stencilFail, StencilAction depthFail, StencilAction depthPass)
        {
            Imports.glStencilOp((int)stencilFail, (int)depthFail, (int)depthPass);
        }

        public static void StencilMask(int mask)
        {
            Imports.glStencilMask((uint)mask);
        }

        public static void ClearStencil(int value)
        {
            Imports.glClearStencil((uint)value);
        }
    }
}
