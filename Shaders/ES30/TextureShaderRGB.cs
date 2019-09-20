﻿namespace NoZ.Platform.OpenGL.ES30 {
    class TextureShaderRGB : TextureShader {
        public override string FragmentSource {
            get {
                return @"
                    precision highp float;
	                uniform sampler2D u_texture;

	                in vec2 v_texcoord;
	                in vec4 v_color;
	                out vec4 color;

	                void main(void) {
		                color = v_color * texture(u_texture,v_texcoord);
	                }
                ";
            }
        }
    }
}
