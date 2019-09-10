using System;

namespace NoZ.Platform.OpenGL {
    abstract class OpenGLShader {
        private int _uniformProjection;

        public uint Id { get; private set; }

        public abstract string FragmentSource {
            get;
        }

        public virtual string VertexSource {
            get {
                return @"
	                uniform mat4 u_projection;
                
                    struct Group {
                        float M11;
                        float M21;
                        float M31;
                        float M12;
                        float M22;
                        float M32;
                        float pad1;
                        float pad2;
                        vec4 color;
                    };

                    layout (std140) uniform u_groups {
                      Group groups [256];
                    };

	                in vec2 a_xy;
	                in vec2 a_uv;
	                in vec4 a_color;
                    in float a_group;

	                out vec4 v_color;
		            out vec2 v_texcoord;

	                void main(void) {
                        int group = int(a_group);
	                    v_texcoord = a_uv;
	                    v_color = a_color * groups[group].color;
	                    gl_Position = u_projection * 
                            vec4(
                                a_xy.x * groups[group].M11 + a_xy.y * groups[group].M21 + groups[group].M31,
                                a_xy.x * groups[group].M12 + a_xy.y * groups[group].M22 + groups[group].M32,
                                0.0,1.0);
	                }
                ";
            }
        }

        public void Use() {
            GL.UseProgram(Id);
        }

        /// Set the project matrix used for this program
        public Matrix4 Projection {
            set {
                GL.UniformMatrix4(_uniformProjection, false, value);
            }
        }

        public uint UniformGroups { get; private set; }

        public void Build () {
            uint vs = 0;
            uint fs = 0;

            try {
                vs = Compile(VertexSource, GL.ShaderType.VertexShader);
                fs = Compile(FragmentSource, GL.ShaderType.FragmentShader);

                Id = GL.CreateProgram();
                GL.AttachShader(Id, vs);
                GL.AttachShader(Id, fs);
                vs = 0;
                fs = 0;

                BindAttributes(Id);

                Link();

                GetUniforms(Id);
                
            } catch (OpenGLException) {
                if (vs!=0)
                    GL.DeleteShader(vs);
                if (fs!=0)
                    GL.DeleteShader(fs);

                throw;
            }
        }

        private void Link() {
            // Link the program    
            GL.LinkProgram(Id);

            // Check for errors
            int linkStatus;
            GL.GetProgram(Id, GL.ProgramParameter.LinkStatus, out linkStatus);
            if (linkStatus == 0) {
                string message = this.GetType().Name + GL.GetProgramInfoLog(Id).Substring(1);
                GL.DeleteProgram(Id);
                throw new OpenGLException(message);
            }

            // Use the program
            GL.UseProgram(Id);

            // Store the uniform locations for the modelview and projection matricies      
            _uniformProjection = GL.GetUniformLocation(Id, "u_projection");
            UniformGroups = GL.GetUniformBlockIndex(Id, "u_groups");
        }

        private uint Compile (string source, GL.ShaderType shaderType) {
            uint id = GL.CreateShader(shaderType);

            // Load the shader source
#if __IOS__
//			if(GetOpenGLShaderVersion()!=100) {
                GL.ShaderSource(id, "#version 300 es\r\n" + source);
//			}
#else
            GL.ShaderSource(id, "#version 150 core\r\n" + source);
#endif

            // Compile the shader
            GL.CompileShader(id);

            // Check for errors
            int compileStatus;
            GL.GetShader(id, GL.ShaderParameter.CompileStatus, out compileStatus);
            if (compileStatus == 0) {
                string message = this.GetType().Name + "." + shaderType.ToString() + GL.GetShaderInfoLog(id).Substring(1);
                GL.DeleteShader(id);
                throw new OpenGLException(message);
            }

            return id;
        }

        protected virtual void BindAttributes(uint id) {
            GL.BindAttribLocation(id, 0, "a_xy");
            GL.BindAttribLocation(id, 1, "a_group");
            GL.BindAttribLocation(id, 2, "a_color");
            GL.BindAttribLocation(id, 3, "a_uv");
        }

        protected virtual void GetUniforms(uint id) { }
    }
}
