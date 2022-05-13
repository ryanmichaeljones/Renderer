using OpenTK.Mathematics;

namespace Renderer.Example
{
    public class ModelExample
    {
        private readonly Shader shader;
        private readonly Mesh mesh;

        public ModelExample()
        {
            shader = new Shader();
            mesh = new Mesh();
        }

        public void OnLoad()
        {
            shader.Parse("model-shader.vert", "model-shader.frag");

            mesh.Parse("square.obj");
        }

        public void OnRenderFrame()
        {
            shader.SetMatrix4("u_Projection", Matrix4.CreatePerspectiveFieldOfView(45.0f.ToRadians(), 1.0f, 0.1f, 100.0f));
            shader.SetMesh(mesh);
            shader.Render();
        }
    }
}
