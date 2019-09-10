using System;

namespace NoZ.Platform.OpenGL {
    struct OpenGLState {
        [Flags]
        public enum Flags {
            Alpha                   = (1<<0),
            LinearFilterOverride    = (1<<1),
            Statistics              = (1<<2),
            Debug                   = (1<<3),
            Stencil                 = (1<<4)
        }

#if false
        /// Current vertex format.
        public OpenGLVertex vertexFormat;

        /// Flags 
        public Flags flags;

        /// GLSL Program to use for batch.
        public OpenGLShader shader;

        /// Textures bound to batch
        public OpenGLTexture texture0;
        public OpenGLTexture texture1;

        /// GL primitive type
        public GL.PrimitiveType primitiveType;

        /// The current depth of the stencil test (represents the nesting level, 0-8)
        public int stencilDepth;

        public int stencilId;

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != GetType()) return false;

            OpenGLState state = (OpenGLState)obj;
            if (flags != state.flags)
                return false;
            if (!Object.ReferenceEquals(shader, state.shader))
                return false;
//            if (vertexFormat != state.vertexFormat)
//                return false;
            if (Object.ReferenceEquals(texture0,state.texture0))
                return false;
            if (Object.ReferenceEquals(texture1,state.texture1))
                return false;
            if (primitiveType != state.primitiveType)
                return false;
            if (stencilDepth != state.stencilDepth)
                return false;
            if (stencilId != state.stencilId)
                return false;
            return true;
        }

        public override int GetHashCode() {
            return flags.GetHashCode()
                ^ shader.GetHashCode()
                ^ vertexFormat.GetHashCode()
                ^ texture0.GetHashCode()
                ^ texture1.GetHashCode()
                ^ primitiveType.GetHashCode()
                ^ stencilDepth.GetHashCode()
                ^ stencilId.GetHashCode();
        }
#endif
    }
}
