using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;

namespace Renderer
{
    public class RenderTexture
    {
        private readonly int id;
        private readonly int fboId;
        private readonly int rboId;
        private bool dirty;
        private Vector2i size;

        public RenderTexture() 
        {
            GL.GenTextures(1, out id);
            GL.GenFramebuffers(1, out fboId);
            GL.GenRenderbuffers(1, out rboId);

            SetSize(256, 256);
        }

        public void Clear()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, GetRboId());
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public int GetTextureId() => id;

        public int GetRboId()
        {
            if (dirty)
            {
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, fboId);

                GL.BindTexture(TextureTarget.Texture2D, id);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, size.X, size.Y, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, id, 0);

                GL.BindTexture(TextureTarget.Texture2D, 0);
                GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rboId);
                GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, size.X, size.Y);
                GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, rboId);
                GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

                if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete) 
                    throw new ArgumentException("Frame buffer is incomplete");

                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

                dirty = false;
            }

            return fboId;
        }

        public Vector2i GetSize() => size;

        public int GetWidth() => size.X;

        public int GetHeight() => size.Y;

        public void SetSize(int width, int height)
        {
            size = new Vector2i(width, height);
            dirty = true;
        }
    }
}