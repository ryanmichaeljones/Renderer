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
            List<Vector2> tcs = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<Face> faces = new List<Face>();

            using var reader = new StreamReader(path, Encoding.UTF8);
            List<string> lines = GetModelData(reader);

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
                    tcs.Add(vector);
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

                    string[] sub1 = lineTokens[1].Split('/');
                    if (sub1.Length >= 1) face.pa = positions[int.Parse(sub1[0]) - 1];
                    if (sub1.Length >= 2) face.tca = tcs[int.Parse(sub1[1]) - 1];
                    if (sub1.Length >= 3) face.na = normals[int.Parse(sub1[2]) - 1];

                    string[] sub2 = lineTokens[2].Split('/');
                    if (sub2.Length >= 1) face.pb = positions[int.Parse(sub2[0]) - 1];
                    if (sub2.Length >= 2) face.tcb = tcs[int.Parse(sub2[1]) - 1];
                    if (sub2.Length >= 3) face.nb = normals[int.Parse(sub2[2]) - 1];

                    string[] sub3 = lineTokens[3].Split('/');
                    if (sub3.Length >= 1) face.pc = positions[int.Parse(sub3[0]) - 1];
                    if (sub3.Length >= 2) face.tcc = tcs[int.Parse(sub3[1]) - 1];
                    if (sub3.Length >= 3) face.nc = normals[int.Parse(sub3[2]) - 1];

                    faces.Add(face);
                }
                else throw new ArgumentException("The line type was not valid model data");
            }

            if (positions.Count > 0)
            {
                var buffer = new Buffer();

                foreach (Face face in faces)
                {
                    buffer.Add(face.pa);
                    buffer.Add(face.pb);
                    buffer.Add(face.pc);
                }

                SetBuffer("aPosition", buffer);
            }

            if (tcs.Count > 0)
            {
                var buffer = new Buffer();

                foreach (Face face in faces)
                {
                    buffer.Add(face.tca);
                    buffer.Add(face.tcb);
                    buffer.Add(face.tcc);
                }

                SetBuffer("aTexCoord", buffer);
            }

            if (normals.Count > 0)
            {
                var buffer = new Buffer();

                foreach (Face face in faces)
                {
                    buffer.Add(face.na);
                    buffer.Add(face.nb);
                    buffer.Add(face.nc);
                }

                SetBuffer("aNormal", buffer);
            }
        }

        private static List<string> GetModelData(StreamReader reader) => ReadAllLines(reader)
            .Where(l => l.StartsWith('v') || l.StartsWith('f')).ToList();

        private static List<string> ReadAllLines(StreamReader reader)
        {
            List<string> lines = new List<string>();

            while (!reader.EndOfStream)
            {
                lines.Add(reader.ReadLine());
            }

            return lines;
        }
    }
}