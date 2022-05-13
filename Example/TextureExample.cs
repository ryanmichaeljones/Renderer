using OpenTK.Mathematics;

namespace Renderer.Example
{
    public class TextureExample
    {
        private readonly Shader shader;
        private readonly Mesh mesh;
        private readonly Texture texture;
        private readonly RenderTexture renderTexture;
        private double _time;

        public TextureExample()
        {
            shader = new Shader();
            mesh = new Mesh();
            texture = new Texture();
            renderTexture = new RenderTexture();
        }

        public void OnLoad()
        {
            shader.Parse("shader.vert", "shader.frag");
            mesh.Parse("model.obj");

            texture.Parse("energy.jpg");
            mesh.SetTexture("texture0", texture);
        }

        public void OnRenderFrame(double time)
        {
            _time += time * 10.0f;

            var model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(45.0f));
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", Matrix4.Identity);
            shader.SetMatrix4("projection", Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(15.0f)) *
                Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(15.0f)) * Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(_time)));

            shader.SetMesh(mesh);
            shader.Render(/*renderTexture*/);
        }
    }
}
