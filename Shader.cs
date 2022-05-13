using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Renderer
{
    public class Shader
    {
        public readonly int id;
        private readonly List<VariableInfo> cache;

        public Shader()
        {
            id = GL.CreateProgram();
            if (id == 0) throw new Exception("Failed to create shader program");

            cache = new List<VariableInfo>();
        }

        public void Render()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            GL.UseProgram(id);

            int activeTexture = 0;
            int vertices = -1;

            foreach (VariableInfo variable in cache)
            {
                if (variable.isAttribute == false)
                {
                    if (variable.type.Equals(ActiveUniformType.FloatMat4))
                    {
                        GL.UniformMatrix4(variable.location, false, ref variable.matrix4Value);
                    }
                    else if (variable.type.Equals(ActiveUniformType.FloatVec2))
                    {
                        GL.Uniform2(variable.location, variable.vector2Value);
                    }
                    else if (variable.type.Equals(ActiveUniformType.FloatVec3))
                    {
                        GL.Uniform3(variable.location, variable.vector3Value);
                    }
                    else if (variable.type.Equals(ActiveUniformType.FloatVec4))
                    {
                        GL.Uniform4(variable.location, variable.vector4Value);
                    }
                    else if (variable.type.Equals(ActiveUniformType.Sampler2D))
                    {
                        GL.ActiveTexture(TextureUnit.Texture0 + activeTexture);
                        GL.BindTexture(TextureTarget.Texture2D, variable.textureValue.GetId());
                        GL.Uniform1(variable.location, activeTexture);
                        activeTexture++;
                    }
                    else if (variable.type.Equals(ActiveUniformType.Float))
                    {
                        GL.Uniform1(variable.location, variable.floatValue);
                    }
                }
                else
                {
                    int size = 0;

                    if (variable.type == ActiveUniformType.Float) size = 1;
                    else if (variable.type == ActiveUniformType.FloatVec2) size = 2;
                    else if (variable.type == ActiveUniformType.FloatVec3) size = 3;
                    else if (variable.type == ActiveUniformType.FloatVec4) size = 4;
                    else throw new ArgumentException("Invalid buffer type");

                    GL.BindBuffer(BufferTarget.ArrayBuffer, variable.bufferValue.GetId());

                    GL.VertexAttribPointer(variable.location, size, VertexAttribPointerType.Float, false, 0, 0);
                    GL.EnableVertexAttribArray(variable.location);

                    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

                    size = variable.bufferValue.GetSize();

                    if (vertices == -1) vertices = size;
                    if (vertices != size) throw new Exception("Attribute streams are of different sizes");
                }
            }

            if (vertices == -1) throw new Exception("No vertices were submitted for drawing");

            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices);
            GL.UseProgram(0);
        }

        public void Render(RenderTexture target)
        {
            // @R - Untested
            int[] viewport = new int[4];
            GL.GetInteger(GetPName.Viewport, out viewport[0]);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, target.GetRboId());

            GL.Viewport(0, 0, target.GetWidth(), target.GetHeight());

            Render();

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            GL.Viewport(viewport[0], viewport[1], viewport[2], viewport[3]);
        }

        public void SetFloat(string variable, float value) => GetVariableInfo(variable, ActiveUniformType.Float, false).floatValue = value;

        public void SetMatrix4(string variable, Matrix4 value) => GetVariableInfo(variable, ActiveUniformType.FloatMat4, false).matrix4Value = value;

        public void SetVector2(string variable, Vector2 value) => GetVariableInfo(variable, ActiveUniformType.FloatVec2, false).vector2Value = value;

        public void SetVector3(string variable, Vector3 value) => GetVariableInfo(variable, ActiveUniformType.FloatVec3, false).vector3Value = value;

        public void SetVector4(string variable, Vector4 value) => GetVariableInfo(variable, ActiveUniformType.FloatVec4, false).vector4Value = value;

        public void SetSampler(string variable, Texture value) => GetVariableInfo(variable, ActiveUniformType.Sampler2D, false).textureValue = value;

        public void SetAttribute(string variable, Buffer value) => GetVariableInfo(variable, value.type, true).bufferValue = value;

        public void SetMesh(Mesh value)
        {
            value.buffers.ForEach(b => SetAttribute(b.name, b.buffer));
            value.textures.ForEach(t => SetSampler(t.name, t.texture));
        }

        private VariableInfo GetVariableInfo(string name, ActiveUniformType type, bool isAttribute)
        {
            if (cache.TryGetValue(v => v.name.Equals(name), out VariableInfo variable))
            {
                if (variable.type == type && variable.isAttribute == isAttribute) { return variable; }
                else throw new ArgumentException($"Variable type of {name} does not match");
            }
            else
            {
                variable = new VariableInfo
                {
                    name = name,
                    type = type,
                    isAttribute = isAttribute,
                    location = VariableInfo.GetLocation(id, name, isAttribute)
                };

                cache.Add(variable);

                return variable;
            }
        }

        public void Parse(string vertexPath, string fragmentPath)
        {
            cache.Clear();

            string vertexSrc = File.ReadAllText(vertexPath);
            int vertexId = GL.CreateShader(ShaderType.VertexShader);

            GL.ShaderSource(vertexId, vertexSrc);

            GL.CompileShader(vertexId);

            GL.GetShader(vertexId, ShaderParameter.CompileStatus, out int code);
            GL.GetShaderInfoLog(vertexId, out string info);
            if (code != (int)All.True) throw new Exception(info);

            string fragmentSrc = File.ReadAllText(fragmentPath);
            int fragmentId = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(fragmentId, fragmentSrc);

            GL.CompileShader(fragmentId);

            GL.GetShader(fragmentId, ShaderParameter.CompileStatus, out code);
            GL.GetShaderInfoLog(fragmentId, out info);
            if (code != (int)All.True) throw new Exception(info);

            GL.AttachShader(id, vertexId);
            GL.AttachShader(id, fragmentId);
            GL.LinkProgram(id);

            GL.GetProgram(id, GetProgramParameterName.LinkStatus, out code);
            GL.GetShaderInfoLog(id, out info);
            if (code != (int)All.True) throw new Exception(info);

            GL.DetachShader(id, vertexId);
            GL.DetachShader(id, fragmentId);
            GL.DeleteShader(vertexId);
            GL.DeleteShader(fragmentId);
        }
    }
}