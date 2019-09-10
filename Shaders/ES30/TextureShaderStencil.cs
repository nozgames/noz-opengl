namespace NoZ.Platform.OpenGL.ES30 {

    class TextureShaderStencil : TextureShader {
        public override string FragmentSource {
            get {
                return @"
                    precision highp float;
	                uniform sampler2D u_texture;

	                in vec2 v_texcoord;
	                out vec4 color;

	                void main(void) {
                        vec4 tcolor = texture(u_texture,v_texcoord);
                        if(tcolor.w < 0.8)
                            discard;

		                color = vec4(1,1,1,1);
	                }
                ";
            }
        }
    }

}
