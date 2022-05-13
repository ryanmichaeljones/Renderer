using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;

namespace Renderer
{
    public class VariableInfo
    {
        public string name;
        public ActiveUniformType type;
        public bool isAttribute = false;
        public int location = 0;

        public float floatValue = 0;
        public Vector2 vector2Value;
        public Vector3 vector3Value;
        public Vector4 vector4Value;
        public Matrix4 matrix4Value;
        public Buffer bufferValue;
        public Texture textureValue;

        public static int GetLocation(int shaderId, string name, bool isAttribute)
        {
            int location = isAttribute ? GL.GetAttribLocation(shaderId, name) : GL.GetUniformLocation(shaderId, name);

            if (location != -1) { return location; }
            else throw new ArgumentException($"The uniform {name} was not found in the shader");
        }
    }
}
