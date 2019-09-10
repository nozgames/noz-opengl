using System;
using System.Runtime.InteropServices;
using System.Text;

namespace NoZ.Platform.OpenGL {
    static class GL {
        public static class Imports {
            public static readonly int GL_RENDERBUFFER = 0x8D41;
            public static readonly int GL_FRAMEBUFFER = 0x8D40;
            public static readonly int GL_COLOR_ATTACHMENT0 = 0x8CE0;

#if __IOS__
            private const string Library = "/System/Library/Frameworks/OpenGLES.framework/OpenGLES";
#else
            private const string Library = "opengl32.dll";
#endif

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glViewport(int x, int y, int width, int height);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glClear(int mask);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glClearColor(float red, float green, float blue, float alpha);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public extern static unsafe void glGenRenderbuffers(int n, [Out] uint* renderbuffers);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glBindRenderbuffer(int target, uint renderbuffer);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glGenFramebuffers(int n, [Out] uint* framebuffers);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glBindFramebuffer(int target, uint renderbuffer);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glFramebufferRenderbuffer(int target, int attachment, int renderbuffertarget, uint renderbuffer);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glEnable(int cap);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glDisable(int cap);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glBlendFunc(int sfactor, int dfactor);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glDrawElements(int mode, int count, int type, IntPtr indices);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glDrawArrays(int mode, int first, int count);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glBindTexture(TextureTarget target, uint texture);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glGenTextures(int n, [Out] uint* textures);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glTexImage2D(int target, int level, int internalformat, int width, int height, int border, int format, int type, IntPtr pixels);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glTexParameterf(int target, int pname, float param);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glTexParameteri(int target, int pname, int param);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glColorMask(bool red, bool green, bool blue, bool alpha);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glDepthMask (bool depth);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glStencilFunc(int func, int reference, uint mask);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glStencilOp(int stencilFail, int depthFail, int depthPass);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glStencilMask (uint mask);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glClearStencil (uint value);

#if !__IOS__
            [DllImport(Library, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            public static extern IntPtr wglGetProcAddress(string functionName);

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

            public unsafe delegate void glUniform1fDelegate (int location, float value);
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

            public delegate void glActiveTextureDelegate (TextureUnit texture);
            public static glActiveTextureDelegate glActiveTexture;

            public delegate void wglSwapIntervalEXTDelegate (int interval);
            public static wglSwapIntervalEXTDelegate wglSwapIntervalEXT;

            static Imports() {
                glCreateShader = Marshal.GetDelegateForFunctionPointer<glCreateShaderDelegate>(wglGetProcAddress("glCreateShader"));
                glShaderSource = Marshal.GetDelegateForFunctionPointer<glShaderSourceDelegate>(wglGetProcAddress("glShaderSource"));
                glCreateProgram = Marshal.GetDelegateForFunctionPointer<glCreateProgramDelegate>(wglGetProcAddress("glCreateProgram"));
                glAttachShader = Marshal.GetDelegateForFunctionPointer<glAttachShaderDelegate>(wglGetProcAddress("glAttachShader"));
                glDeleteShader = Marshal.GetDelegateForFunctionPointer<glDeleteShaderDelegate>(wglGetProcAddress("glDeleteShader"));
                glLinkProgram = Marshal.GetDelegateForFunctionPointer<glLinkProgramDelegate>(wglGetProcAddress("glLinkProgram"));
                glDeleteProgram = Marshal.GetDelegateForFunctionPointer<glDeleteProgramDelegate>(wglGetProcAddress("glDeleteProgram"));
                glGetProgramiv = Marshal.GetDelegateForFunctionPointer<glGetProgramivDelegate>(wglGetProcAddress("glGetProgramiv"));
                glGetProgramInfoLog = Marshal.GetDelegateForFunctionPointer<glGetProgramInfoLogDelegate>(wglGetProcAddress("glGetProgramInfoLog"));
                glUseProgram = Marshal.GetDelegateForFunctionPointer<glUseProgramDelegate>(wglGetProcAddress("glUseProgram"));
                glCompileShader = Marshal.GetDelegateForFunctionPointer<glCompileShaderDelegate>(wglGetProcAddress("glCompileShader"));
                glGetShaderiv = Marshal.GetDelegateForFunctionPointer<glGetShaderivDelegate>(wglGetProcAddress("glGetShaderiv"));
                glGetShaderInfoLog = Marshal.GetDelegateForFunctionPointer<glGetShaderInfoLogDelegate>(wglGetProcAddress("glGetShaderInfoLog"));
                glGetUniformBlockIndex = Marshal.GetDelegateForFunctionPointer<glGetUniformBlockIndexDelegate>(wglGetProcAddress("glGetUniformBlockIndex"));
                glUniformBlockBinding = Marshal.GetDelegateForFunctionPointer<glUniformBlockBindingDelegate>(wglGetProcAddress("glUniformBlockBinding"));
                glGetUniformLocation = Marshal.GetDelegateForFunctionPointer<glGetUniformLocationDelegate>(wglGetProcAddress("glGetUniformLocation"));
                glUniformMatrix4fv = Marshal.GetDelegateForFunctionPointer<glUniformMatrix4fvDelegate>(wglGetProcAddress("glUniformMatrix4fv"));
                glUniform4fv = Marshal.GetDelegateForFunctionPointer<glUniform4fvDelegate>(wglGetProcAddress("glUniform4fv"));
                glUniform1f = Marshal.GetDelegateForFunctionPointer<glUniform1fDelegate>(wglGetProcAddress("glUniform1f"));
                glGenBuffers = Marshal.GetDelegateForFunctionPointer<glGenBuffersDelegate>(wglGetProcAddress("glGenBuffers"));
                glBindBuffer = Marshal.GetDelegateForFunctionPointer<glBindBufferDelegate>(wglGetProcAddress("glBindBuffer"));
                glBindBufferRange = Marshal.GetDelegateForFunctionPointer<glBindBufferRangeDelegate>(wglGetProcAddress("glBindBufferRange"));
                glEnableVertexAttribArray = Marshal.GetDelegateForFunctionPointer<glEnableVertexAttribArrayDelegate>(wglGetProcAddress("glEnableVertexAttribArray"));
                glVertexAttribPointer = Marshal.GetDelegateForFunctionPointer<glVertexAttribPointerDelegate>(wglGetProcAddress("glVertexAttribPointer"));
                glBufferData = Marshal.GetDelegateForFunctionPointer<glBufferDataDelegate>(wglGetProcAddress("glBufferData"));
                glBufferSubData = Marshal.GetDelegateForFunctionPointer<glBufferSubDataDelegate>(wglGetProcAddress("glBufferSubData"));
                glBindAttribLocation = Marshal.GetDelegateForFunctionPointer<glBindAttribLocationDelegate>(wglGetProcAddress("glBindAttribLocation"));
                glActiveTexture = Marshal.GetDelegateForFunctionPointer<glActiveTextureDelegate>(wglGetProcAddress("glActiveTexture"));
                wglSwapIntervalEXT= Marshal.GetDelegateForFunctionPointer<wglSwapIntervalEXTDelegate>(wglGetProcAddress("wglSwapIntervalEXT"));                
            }
#else
            [DllImport(Library, ExactSpelling = true)]
            public static extern unsafe int glCreateShader(ShaderType type);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glShaderSource(uint shader, int count, [In] string[] source, [In] int[] length);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe uint glCreateProgram();

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glAttachShader(uint program, uint shader);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glUseProgram(uint program);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glDeleteShader(uint shader);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glLinkProgram(uint program);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glDeleteProgram(uint program);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glGetProgramiv(uint program, int pname, [Out] int* @params);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glCompileShader(uint shader);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glGetShaderiv(uint shader, int pname, [Out] int* @params);
            
            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glGetShaderInfoLog(uint shader, int bufSize, int* length, IntPtr infoLog);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glGetProgramInfoLog(uint program, int bufSize, int* length, IntPtr infoLog);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern int glGetUniformLocation(uint program, string name);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glUniformMatrix4fv(int location, int count, bool transpose, float* value);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glUniform4fv(int location, int count, float* value);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern unsafe void glGenBuffers(int n, [Out] uint* buffers);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glBindBuffer(BufferTarget target, uint buffer);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glEnableVertexAttribArray(uint index);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glVertexAttribPointer(uint index, int size, int type, bool normalized, int stride, IntPtr pointer);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glBufferData(int target, IntPtr size, IntPtr data, int usage);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glBindAttribLocation(uint program, uint index, string name);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glActiveTexture(TextureUnit texture);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glUniform1f(int location, float v0);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern int glGetUniformBlockIndex(uint program, string name);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glBindBufferRange (int target, uint index, uint buffer, IntPtr offset, IntPtr size);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern void glBufferSubData (int target, IntPtr offset, IntPtr size, IntPtr data);

            [DllImport(Library, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            public static extern int glUniformBlockBinding (uint program, uint blockIndex, uint bindingPointIndex);


#endif
        };

        [Flags]
        public enum ClearBuffer : int {
            Depth = 0x00000100,
            Stencil = 0x00000400,
            Color = 0x00004000
        }

        public enum ProgramParameter : int {
            LinkStatus = 0x8B82
        }

        public enum ShaderParameter : int {
            ShaderType = 0x8B4F,
            DeleteStatus = 0x8B80,
            CompileStatus = 0x8B81,
            InfoLogLength = 0x8B84,
            ShaderSourceLength = 0x8B88
        }

        public enum ShaderType : int {
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

        public enum BufferTarget : int {
            ArrayBuffer = 0x8892,
            ElementArrayBuffer = 0x8893,
            PixelPackBuffer = 0x88EB,
            PixelUnpackBuffer = 0x88EC,
            UniformBuffer = 0x8A11,
            TransformFeedbackBuffer = 0x8C8E,
            CopyReadBuffer = 0x8F36,
            CopyWriteBuffer = 0x8F37
        }

        public enum VertexAttribPointerType : int {
            UnsignedByte = 0x1401,
            Float = 0X1406,
            UnsignedShort = 0x1403,
            Int = 0x1404,
            UnsignedInt = 0x1405
        }
        public enum PrimitiveType : int {
            Lines = 0x0001,
            Triangles = 0x0004,
            TriangleStrip = 0x0005
        }

        public enum EnableCapability : int {
            DepthTest = 0x0b71,
            AlphaTest = 0x0bc0,
            StencilTest = 0x0B90,
            Blend = 0x0be2,            
        }


        public enum BlendFactorSrc : int {
            SrcAlpha = 0x0302
        }

        public enum BlendFactorDest : int {
            OneMinusSrcAlpha = 0x0303
        }

        public enum BufferUsage : int {
            StreamDraw = 0x88E0
        }

        public enum DrawElementsType : int {
            UnsignedByte = 0x1401,
            UnsignedShort = 0x1403,
            UnsignedInt = 0x1405
        }

        public enum TextureUnit : int {
            None = 0,
            Texture0 = 0x84c0,
            Texture1 = 0x84c1
        }

        public enum TextureTarget : int {
            Texture1D = 0x0DE0,
            Texture2D = 0x0DE1
        }

        public enum TextureTarget2d : int {
            Texture2D = 0x0de1
        }

        public enum TextureFormat : int {
            Alpha = 0x1906,
            Rgb = 0x1907,
            Rgba = 0x1908
        }

        public enum PixelFormat : int {
            Alpha = 0x1906,
            Rgb = 0x1907,
            Rgba = 0x1908
        };

        public enum PixelType : int {
            Byte = 0x1400,
            UnsignedByte = 0x1401
        }

        public enum TextureParameterName : int {
            TextureMagFilter = 0x2800,
            TextureMinFilter = 0x2801,
            TextureWrapS = 0x2802,
            TextureWrapT = 0x2803                
        }

        public enum TextureClamp {
            Repeat = 0x2901,
            Edge = 0x812f
        }

        public enum TextureMagFilter : int {
            Nearest = 0x2600,
            Linear = 0x2601
        };

        public enum TestFunction : int {
            Never = 0x200,
            LessThan = 0x201,
            LessThanOrEqualTo = 0x203,
            GreaterThan = 0x204,
            GreaterThanOrEqualTo = 0x206,
            Equal = 0x202,
            Always = 0x207,
            NotEqual = 0x205
        }

        public enum StencilAction : int {
            Keep = 0x1E00,
            Zero = 0,
            Replace = 0x1E01
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct FloatColor {
            public float R;
            public float G;
            public float B;
            public float A;
        }

        public static unsafe uint GenRenderBuffer() {
            uint id;
            Imports.glGenFramebuffers(1, &id);
            return id;
        }

        public static unsafe void BindRenderBuffer(uint id) {
            Imports.glBindRenderbuffer(Imports.GL_RENDERBUFFER, id);
        }

        public static unsafe uint GenFrameBuffer() {
            uint id;
            Imports.glGenFramebuffers(1, &id);
            return id;
        }

        public static unsafe void BindFrameBuffer(uint id) {
            Imports.glBindFramebuffer(Imports.GL_FRAMEBUFFER, id);
        }

        public static void FrameBufferRenderBuffer(uint id) {
            Imports.glFramebufferRenderbuffer(Imports.GL_FRAMEBUFFER, Imports.GL_COLOR_ATTACHMENT0, Imports.GL_RENDERBUFFER, id);
        }

        public static void Viewport(int x, int y, int width, int height) {
            Imports.glViewport(x, y, width, height);
        }

        public static void ClearColor(float r, float g, float b, float a) {
            Imports.glClearColor(r, g, b, a);
        }

        public static void ClearColor(Color color) {
            Imports.glClearColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }

        public static void Clear(ClearBuffer mask) {
            Imports.glClear((int)mask);
        }

        public static uint CreateProgram() {
            return Imports.glCreateProgram();
        }

        public static void UseProgram(uint program) {
            Imports.glUseProgram(program);
        }

        public static void DeleteShader(uint shader) {
            Imports.glDeleteShader(shader);
        }

        public static void AttachShader(uint program, uint shader) {
            Imports.glAttachShader(program, shader);
        }

        public static void LinkProgram(uint program) {
            Imports.glLinkProgram(program);
        }

        public static void DeleteProgram(uint program) {
            Imports.glDeleteProgram(program);
        }

        public static void GetProgram(uint program, ProgramParameter param, out int value) {
            unsafe {
                fixed (int* a = &value) {
                    Imports.glGetProgramiv(program, (int)param, a);
                }
            }
        }

        public static void GetShader(uint shader, ShaderParameter param, out int value) {
            unsafe {
                fixed (int* a = &value) {
                    Imports.glGetShaderiv(shader, (int)param, a);
                }
            }
        }

        public static uint CreateShader(ShaderType type) {
            return (uint)Imports.glCreateShader(type);
        }

        public static void ShaderSource(uint shader, string source) {
            Imports.glShaderSource(shader, 1, new string[] { source }, new int[] { source.Length });
        }

        public static void CompileShader(uint shader) {
            Imports.glCompileShader(shader);
        }

        public static string GetShaderInfoLog(uint shader) {
            unsafe {
                int maxLength = 1024;
                IntPtr infoLogPtr = Marshal.AllocHGlobal(maxLength * Marshal.SizeOf(typeof(byte)) + 1);
                Imports.glGetShaderInfoLog(shader, 1000, &maxLength, infoLogPtr);
                string infoLog = Marshal.PtrToStringAnsi(infoLogPtr);
                Marshal.FreeHGlobal(infoLogPtr);
                return infoLog;
            }
        }

        public static string GetProgramInfoLog(uint program) {
            unsafe {
                int maxLength = 1024;
                IntPtr infoLogPtr = Marshal.AllocHGlobal(maxLength * Marshal.SizeOf(typeof(byte)) + 1);
                Imports.glGetProgramInfoLog(program, 1000, &maxLength, infoLogPtr);
                string infoLog = Marshal.PtrToStringAnsi(infoLogPtr);
                Marshal.FreeHGlobal(infoLogPtr);
                return infoLog;
            }
        }

        public static uint GetUniformBlockIndex(uint program, string name) {
            return (uint)Imports.glGetUniformBlockIndex(program, name);
        }

        public static uint UniformBlockBinding (uint program, uint blockIndex, uint bindingPointIndex) {
            return (uint)Imports.glUniformBlockBinding(program, blockIndex, bindingPointIndex);
        }

        public static int GetUniformLocation(uint program, string name) {
            return Imports.glGetUniformLocation(program, name);
        }

        public static void UniformMatrix4(int location, bool transpose, Matrix4 matrix) {
            unsafe {
                Imports.glUniformMatrix4fv(location, 1, transpose, (float*)&matrix);
            }
        }

        public static void UniformColor (uint location, Color color) {
            unsafe {
                var fc = new FloatColor {
                    R = color.R / 255f,
                    G = color.G / 255f,
                    B = color.B / 255f,
                    A = color.A / 255f
                };
                Imports.glUniform4fv((int)location, 1, (float*)&fc);
            }
        }

        public static void Uniform (int location, float value) {
            Imports.glUniform1f(location, value);
        }

        public static uint GenBuffer() {
            unsafe {
                uint id = 0;
                Imports.glGenBuffers(1, &id);
                return id;
            }
        }

        public static void BindBuffer(BufferTarget target, uint buffer) {
            Imports.glBindBuffer(target, buffer);
        }

        public static void BindBufferRange(BufferTarget target, uint index, uint buffer, IntPtr offset, IntPtr size) {
            Imports.glBindBufferRange((int)target, index, buffer, offset, size);
        }

        public static void EnableVertexAttribArray(uint index) {
            Imports.glEnableVertexAttribArray(index);
        }

        public static void VertexAttribPointer(uint index, int size, VertexAttribPointerType type, bool normalized, int stride, int offset) {
            Imports.glVertexAttribPointer(index, size, (int)type, normalized, stride, (IntPtr)offset);
        }

        public static void Enable(EnableCapability capability) {
            Imports.glEnable((int)capability);
        }

        public static void Disable (EnableCapability capability) {
            Imports.glDisable((int)capability);
        }

        public static void BlendFunc(BlendFactorSrc sfactor, BlendFactorDest dfactor) {
            Imports.glBlendFunc((int)sfactor, (int)dfactor);
        }

        public static void BufferData(BufferTarget target, int size, IntPtr data, BufferUsage usage) {
            Imports.glBufferData((int)target, (IntPtr)size, data, (int)usage);
        }

        public static void BufferSubData(BufferTarget target, int offset, int size, IntPtr data) {
            Imports.glBufferSubData((int)target, (IntPtr)offset, (IntPtr)size, data);
        }

        public static void DrawElements(PrimitiveType mode, int count, DrawElementsType type, IntPtr indicies) {
            Imports.glDrawElements((int)mode, count, (int)type, indicies);
        }

        public static void DrawArrays(PrimitiveType mode, int first, int count) {
            Imports.glDrawArrays((int)mode, first, count);
        }

        public static void ActiveTexture(TextureUnit texture) {
            Imports.glActiveTexture(texture);
        }

        public static void BindAttribLocation(uint program, uint index, string name) {
            Imports.glBindAttribLocation(program, index, name);
        }

        public static void BindTexture (TextureTarget target, uint texture) {
            Imports.glBindTexture(target, texture);
        }

        public static unsafe uint GenTexture () {
            uint id;
            Imports.glGenTextures(1, &id);
            return id;
        }

        public static unsafe void TexImage (
            TextureTarget2d target, 
            int level, 
            TextureFormat internalFormat, 
            int width, 
            int height, 
            int border,
            PixelFormat format,
            PixelType type,
            byte[] pixels
            ) {

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

        public static void TexParameter (TextureTarget target, TextureParameterName pname, float param) {
            Imports.glTexParameterf((int)target, (int)pname, param);
        }

        public static void TexParameter(TextureTarget target, TextureParameterName pname, int param) {
            Imports.glTexParameteri((int)target, (int)pname, param);
        }

        public static void ColorMask (bool red, bool green, bool blue, bool alpha) {
            Imports.glColorMask(red, green, blue, alpha);
        }

        public static void DepthMask (bool depth) {
            Imports.glDepthMask(depth);
        }

        public static void StencilFunc (TestFunction func, int reference, int mask) {
            Imports.glStencilFunc((int)func, reference, (uint)mask);
        }

        public static void StencilOp(StencilAction stencilFail, StencilAction depthFail, StencilAction depthPass) {
            Imports.glStencilOp((int)stencilFail, (int)depthFail, (int)depthPass);
        }

        public static void StencilMask(int mask) {
            Imports.glStencilMask((uint)mask);
        }

        public static void ClearStencil(int value) {
            Imports.glClearStencil((uint)value);
        }
    }
}
