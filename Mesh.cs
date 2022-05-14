using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Renderer
{
    public class Mesh
    {
        public List<BufferData> buffers = new List<BufferData>();
        public List<TextureData> textures = new List<TextureData>();

        public void SetBuffer(string name, Buffer buffer)
        {
            if (buffers.TryGetValue(b => b.name.Equals(name), out BufferData data))
            {
                data.buffer = buffer;
            }
            else
            {
                data = new BufferData(name, buffer);
                buffers.Add(data);
            }
        }

        public void SetTexture(string name, Texture texture)
        {
            if (textures.TryGetValue(b => b.name.Equals(name), out TextureData data))
            {
                data.texture = texture;
            }
            else
            {
                data = new TextureData(name, texture);
                textures.Add(data);
            }
        }

        public void Parse(string path)
        {
            List<Vector3> positions = new List<Vector3>();
            List<Vector2> texCoords = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<Face> faces = new List<Face>();

            IEnumerable<string> lines = GetModelData(path);

            foreach (string line in lines)
            {
                string[] lineTokens = line.Split(' ');
                string lineType = lineTokens.First();

                if (lineType.Equals("v"))
                {
                    var vector = new Vector3
                    {
                        X = float.Parse(lineTokens[1]),
                        Y = float.Parse(lineTokens[2]),
                        Z = float.Parse(lineTokens[3])
                    };
                    positions.Add(vector);
                }
                else if (lineType.Equals("vt"))
                {
                    var vector = new Vector2
                    {
                        X = float.Parse(lineTokens[1]),
                        Y = float.Parse(lineTokens[2])
                    };
                    texCoords.Add(vector);
                }
                else if (lineType.Equals("vn"))
                {
                    var vector = new Vector3
                    {
                        X = float.Parse(lineTokens[1]),
                        Y = float.Parse(lineTokens[2]),
                        Z = float.Parse(lineTokens[3])
                    };
                    normals.Add(vector);
                }
                else if (lineType.Equals("f"))
                {
                    Face face = new Face();

                    for (int i = 1; i < lineTokens.Length; i++)
                    {
                        var sub = lineTokens[i].Split('/');
                        if (sub.Length >= 1) face.positions.Add(positions[int.Parse(sub[0]) - 1]);
                        if (sub.Length >= 2) face.texCoords.Add(texCoords[int.Parse(sub[1]) - 1]);
                        if (sub.Length >= 3) face.normals.Add(normals[int.Parse(sub[2]) - 1]);
                    }

                    faces.Add(face);
                }
                else throw new ArgumentException("The line type was not valid model data");
            }

            if (positions.Count > 0)
            {
                var buffer = new Buffer();

                foreach (Face face in faces)
                {
                    face.positions.ForEach(p => buffer.Add(p));
                }

                SetBuffer("aPosition", buffer);
            }

            if (texCoords.Count > 0)
            {
                var buffer = new Buffer();

                foreach (Face face in faces)
                {
                    face.texCoords.ForEach(t => buffer.Add(t));
                }

                SetBuffer("aTexCoord", buffer);
            }

            if (normals.Count > 0)
            {
                var buffer = new Buffer();

                foreach (Face face in faces)
                {
                    face.normals.ForEach(n => buffer.Add(n));
                }

                SetBuffer("aNormal", buffer);
            }
        }

        private IEnumerable<string> GetModelData(string path)
        {
            using var reader = new StreamReader(path, Encoding.UTF8);
            var lines = new List<string>();

            while (!reader.EndOfStream)
            {
                lines.Add(reader.ReadLine());
            }

            return lines.Where(l => l.StartsWith('v') || l.StartsWith('f'));
        }
    }
}