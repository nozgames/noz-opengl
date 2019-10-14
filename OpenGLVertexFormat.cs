using System;
using System.Runtime.InteropServices;

namespace NoZ.Platform.OpenGL {
    /// Vertex with X, Y, Z, U, V, and Color
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    struct OpenGLVertex {
        public static readonly int SizeInBytes = Marshal.SizeOf<OpenGLVertex>();
        public static readonly int OffsetUV = (int)Marshal.OffsetOf<OpenGLVertex>("uv");
        public static readonly int OffsetXY = (int)Marshal.OffsetOf<OpenGLVertex>("xy");
        public static readonly int OffsetColor = (int)Marshal.OffsetOf<OpenGLVertex>("color");

        public Vector2 xy;
        public Vector2 uv;
        public uint color;
    };
}
