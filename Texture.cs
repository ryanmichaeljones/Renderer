using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace Renderer
{
    public class Texture
    {
        public readonly int id;
        private BitmapData data;
        private bool dirty;

        public int Width { get => data.Width; }
        public int Height { get => data.Height; }

        public Texture()
        {
            GL.GenTextures(1, out id);
        }

        public int GetId()
        {
            if (dirty)
            {
                GL.BindTexture(TextureTarget.Texture2D, id);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

                GL.BindTexture(TextureTarget.Texture2D, 0);

                dirty = false;
            }

            return id;
        }

        public void Parse(string path)
        {
            var image = new Bitmap(path);
            var rect = new Rectangle(0, 0, image.Width, image.Height);

            image.RotateFlip(RotateFlipType.RotateNoneFlipY);

            data = image.LockBits(rect, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            dirty = true;
        }
    }
}