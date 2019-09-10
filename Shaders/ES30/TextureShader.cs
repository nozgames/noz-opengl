
namespace NoZ.Platform.OpenGL.ES30 {

    abstract class TextureShader : OpenGLShader {
        protected override void GetUniforms(uint program) {
            base.GetUniforms(program);

            // Use texture0 for the texture.
            GL.Uniform(GL.GetUniformLocation(program, "u_texture"), 0);
        }
    }
}
