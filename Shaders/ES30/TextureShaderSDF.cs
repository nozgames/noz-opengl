namespace NoZ.Platform.OpenGL.ES30 {
    class TextureShaderSDF : TextureShader {
        private int _uniformRange;

        public override string FragmentSource {
            get {
                return @"
                    precision highp float;
	                uniform sampler2D u_texture;
	                uniform float u_range;

	                in vec2 v_texcoord;
	                in vec4 v_color;
	                out vec4 color;

	                void main(void) {
		                vec2 tsize = vec2(textureSize(u_texture,0));
		                float alpha = texture(u_texture, v_texcoord).a;
		                float dist  = (alpha - 0.5) * u_range * 2.0;
		                float width = max(fwidth (v_texcoord.x) * tsize.x, fwidth (v_texcoord.y) * tsize.y);
                        width = min(width, u_range) * 0.5;

                        // Brighten up smaller sizes                        
                        float grow = -clamp((width - 1.0) * 0.25, 0.0, u_range-width);

		                alpha = smoothstep(-width + grow, width + grow, dist);

		                color = vec4(v_color.xyz, v_color.a * alpha);
	                }                
                ";
            }
        }

        protected override void GetUniforms(uint program) {
            base.GetUniforms(program);

            _uniformRange = GL.GetUniformLocation(program, "u_range");

            SetRange(8.0f);
        }

        public void SetRange (float range) {
            GL.Uniform(_uniformRange, range);
        }
    }
}
