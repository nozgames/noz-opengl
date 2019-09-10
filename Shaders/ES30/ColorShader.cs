
using System;

namespace NoZ.Platform.OpenGL.ES30 {
    class ColorShader : OpenGLShader {
        public override string FragmentSource {
            get {
                return @"
                    precision mediump float;

	                in vec4 v_color;
                    in vec2 v_texcoord;
	                out vec4 color;

	                void main(void) {
		                color = v_color;
	                }
                ";
            }
        }
    }
}
