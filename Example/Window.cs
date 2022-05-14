using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Renderer.Example
{
    public class Window : GameWindow
    {
        private readonly TriangleExample triangleExample;
        private readonly TextureExample textureExample;
        private readonly MeshExample meshExample;
        private readonly ModelExample modelExample;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) 
            : base(gameWindowSettings, nativeWindowSettings)
        {
            triangleExample = new TriangleExample();
            textureExample = new TextureExample();
            meshExample = new MeshExample();
            modelExample = new ModelExample();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);

            //triangleExample.OnLoad();
            textureExample.OnLoad();
            //meshExample.OnLoad();
            //modelExample.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //triangleExample.OnRenderFrame();
            textureExample.OnRenderFrame(args.Time);
            //meshExample.OnRenderFrame();
            //modelExample.OnRenderFrame();

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
        }
    }
}
