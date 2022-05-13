using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace Renderer
{
    public class Buffer
    {
        public readonly int id = 0;
        public ActiveUniformType type;
        private bool dirty = false;
        private readonly List<float> floatData;

        public Buffer()
        {
            GL.GenBuffers(1, out id);
            floatData = new List<float>();
        }

        public int GetSize()
        {
            if (type.Equals(ActiveUniformType.Float)) return floatData.Count;
            else if (type.Equals(ActiveUniformType.FloatVec2)) return floatData.Count / 2;
            else if (type.Equals(ActiveUniformType.FloatVec3)) return floatData.Count / 3;
            else if (type.Equals(ActiveUniformType.FloatVec4)) return floatData.Count / 4;
            else throw new ArgumentException("Other data types are not currently supported");
        }

        public void Add(float value)
        {
            if (type != ActiveUniformType.Float && type != 0) 
                throw new ArgumentException("Attempted to mix types");

            type = ActiveUniformType.Float;
            floatData.Add(value);
            dirty = true;
        }

        public void Add(Vector2 value)
        {
            if (type != ActiveUniformType.FloatVec2 && type != 0) 
                throw new ArgumentException("Attempted to mix types");

            type = ActiveUniformType.FloatVec2;
            floatData.Add(value.X);
            floatData.Add(value.Y);
            dirty = true;
        }

        public void Add(Vector3 value)
        {
            if (type != ActiveUniformType.FloatVec3 && type != 0) 
                throw new ArgumentException("Attempted to mix types");

            type = ActiveUniformType.FloatVec3;
            floatData.Add(value.X);
            floatData.Add(value.Y);
            floatData.Add(value.Z);
            dirty = true;
        }

        public void Add(Vector4 value)
        {
            if (type != ActiveUniformType.FloatVec4 && type != 0) 
                throw new ArgumentException("Attempted to mix types");

            type = ActiveUniformType.FloatVec4;
            floatData.Add(value.X);
            floatData.Add(value.Y);
            floatData.Add(value.Z);
            floatData.Add(value.W);
            dirty = true;
        }

        public int GetId()
        {
            if (dirty)
            {
                int vertexArrayObject = GL.GenVertexArray();
                GL.BindVertexArray(vertexArrayObject);

                GL.BindBuffer(BufferTarget.ArrayBuffer, id);

                if (type == ActiveUniformType.Float || type == ActiveUniformType.FloatVec2 ||
                    type == ActiveUniformType.FloatVec3 || type == ActiveUniformType.FloatVec4)
                {
                    GL.BufferData(BufferTarget.ArrayBuffer, floatData.Count * sizeof(float), floatData.ToArray(), BufferUsageHint.StaticDraw);
                }
                else throw new ArgumentException("Other data types are not currently supported");

                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

                dirty = false;
            }

            return id;
        }
    }
}
