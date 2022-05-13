using OpenTK.Mathematics;

namespace Renderer.Example
{
    public class MeshExample
    {
        private float angle = 0;

        private readonly Shader shader;
        private readonly Mesh mesh;
        private readonly Buffer buffer1;
        private readonly Buffer buffer2;

        public MeshExample()
        {
            shader = new Shader();
            mesh = new Mesh();
            buffer1 = new Buffer();
            buffer2 = new Buffer();
        }

        public void OnLoad()
        {
            shader.Parse("mesh-shader.vert", "mesh-shader.frag");

            buffer1.Add(new Vector2(0, 1));
            buffer1.Add(new Vector2(-1, -1));
            buffer1.Add(new Vector2(1, -1));
            mesh.SetBuffer("a_Position", buffer1);

            buffer2.Add(new Vector4(1, 0, 1, 1));
            buffer2.Add(new Vector4(1, 1, 0, 1));
            buffer2.Add(new Vector4(1, 0, 0, 0.5f));
            mesh.SetBuffer("a_Color", buffer2);
        }

        public void OnRenderFrame()
        {
            shader.SetMatrix4("u_Projection", Matrix4.CreatePerspectiveFieldOfView(60.0f.ToRadians(), 1.0f, 0.1f, 100.0f));
            shader.SetMatrix4("u_Model", Matrix4.CreateRotationZ(angle) * Matrix4.CreateTranslation(new Vector3(0, 0, -10)));

            shader.SetMesh(mesh);
            shader.Render();

            angle += 0.005f;
        }
    }
}
