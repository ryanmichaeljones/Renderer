using OpenTK.Mathematics;

namespace Renderer.Example
{
    public class TriangleExample
    {
        private readonly Shader shader;
        private readonly Buffer buffer;

        public TriangleExample()
        {
            shader = new Shader();
            buffer = new Buffer();
        }

        public void OnLoad()
        {
            shader.Parse("triangle-shader.vert", "triangle-shader.frag");

            buffer.Add(new Vector3(0, 0.5f, 0));
            buffer.Add(new Vector3(-0.5f, -0.5f, 0));
            buffer.Add(new Vector3(0.5f, -0.5f, 0));
        }

        public void OnRenderFrame()
        {
            shader.SetAttribute("a_Position", buffer);
            shader.Render();
        }
    }
}
