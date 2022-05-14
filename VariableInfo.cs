using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;

namespace Renderer
{
    public class VariableInfo
    {
        public string Name { get; }
        public ActiveUniformType Type { get; }
        public bool IsAttribute { get; }
        public int Location { get; }

        public float floatValue;
        public Vector2 vector2Value;
        public Vector3 vector3Value;
        public Vector4 vector4Value;
        public Matrix4 matrix4Value;
        public Buffer bufferValue;
        public Texture textureValue;

        public VariableInfo(int shaderId, string name, ActiveUniformType type, bool isAttribute)
        {
            Name = name;
            Type = type;
            IsAttribute = isAttribute;
            Location = GetLocation(shaderId, name, isAttribute);
        }

        public static int GetLocation(int shaderId, string name, bool isAttribute)
        {
            int location = isAttribute ? GL.GetAttribLocation(shaderId, name) : GL.GetUniformLocation(shaderId, name);

            if (location != -1) { return location; }
            else throw new ArgumentException($"The uniform {name} was not found in the shader");
        }

        public static int GetAttributeSize(ActiveUniformType type)
        {
            if (type == ActiveUniformType.Float) return 1;
            else if (type == ActiveUniformType.FloatVec2) return 2;
            else if (type == ActiveUniformType.FloatVec3) return 3;
            else if (type == ActiveUniformType.FloatVec4) return 4;
            else throw new ArgumentException($"Buffer type {type} is invalid");
        }
    }
}
