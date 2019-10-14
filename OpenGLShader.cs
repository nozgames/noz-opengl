/*
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


namespace NoZ.Platform.OpenGL
{
    abstract class OpenGLShader
    {
        private int _uniformProjection;

        public uint Id { get; private set; }

        public abstract string FragmentSource {
            get;
        }

        public virtual string VertexSource {
            get {
                return @"
	                uniform mat4 u_projection;
                
	                in vec2 a_xy;
	                in vec2 a_uv;
	                in vec4 a_color;

	                out vec4 v_color;
		            out vec2 v_texcoord;

	                void main(void) {
	                    v_texcoord = a_uv;
	                    v_color = a_color;
	                    gl_Position = u_projection * vec4(a_xy.x,a_xy.y,0.0f,1.0f);
	                }
                ";
            }
        }

        public void Use()
        {
            GL.UseProgram(Id);
        }

        /// Set the project matrix used for this program
        public Matrix4 Projection {
            set {
                GL.UniformMatrix4(_uniformProjection, false, value);
            }
        }

        public void Build()
        {
            uint vs = 0;
            uint fs = 0;

            try
            {
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

            }
            catch (OpenGLException)
            {
                if (vs != 0)
                    GL.DeleteShader(vs);
                if (fs != 0)
                    GL.DeleteShader(fs);

                throw;
            }
        }

        private void Link()
        {
            // Link the program    
            GL.LinkProgram(Id);

            // Check for errors
            int linkStatus;
            GL.GetProgram(Id, GL.ProgramParameter.LinkStatus, out linkStatus);
            if (linkStatus == 0)
            {
                string message = this.GetType().Name + GL.GetProgramInfoLog(Id).Substring(1);
                GL.DeleteProgram(Id);
                throw new OpenGLException(message);
            }

            // Use the program
            GL.UseProgram(Id);

            // Store the uniform locations for the modelview and projection matricies      
            _uniformProjection = GL.GetUniformLocation(Id, "u_projection");
        }

        private uint Compile(string source, GL.ShaderType shaderType)
        {
            uint id = GL.CreateShader(shaderType);

            // Load the shader source
#if __NOZ_IOS__
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
            if (compileStatus == 0)
            {
                string message = this.GetType().Name + "." + shaderType.ToString() + GL.GetShaderInfoLog(id).Substring(1);
                GL.DeleteShader(id);
                throw new OpenGLException(message);
            }

            return id;
        }

        protected virtual void BindAttributes(uint id)
        {
            GL.BindAttribLocation(id, 0, "a_xy");
            GL.BindAttribLocation(id, 1, "a_color");
            GL.BindAttribLocation(id, 2, "a_uv");
        }

        protected virtual void GetUniforms(uint id) { }
    }
}
