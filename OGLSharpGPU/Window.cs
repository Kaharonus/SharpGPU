using System;
using OpenToolkit.Core;
using OpenToolkit.Windowing.Desktop;
using OpenToolkit.Graphics;
using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Mathematics;
using OpenToolkit.Windowing.Common;

namespace OGLSharpGPU {
    public class Window : GameWindow {

        private GPU gpu;

        public Window() : base(GameWindowSettings.Default, NativeWindowSettings.Default) { }

        public Window(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() {
            Size = new Vector2i(width, height),
            Title = title
        }) { }

        private void InitGPU(int width, int height) {
            gpu = new GPU();
            gpu.InitFrameBuffer(width, height);
        }

        protected override void OnResize(ResizeEventArgs e) {
            //gpu.ResizeFrameBuffer(e.Width,e.Height);
            base.OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs args) {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //GL.ReadPixels(0,0, gpu.FBWidth, gpu.FBHeight, PixelFormat.Rgb, PixelType.UnsignedByte, gpu.GetFrameBufferAddr());
            gpu.DrawRandom();
            GL.DrawPixels(gpu.FBWidth, gpu.FBHeight, PixelFormat.Rgb, PixelType.UnsignedByte, gpu.GetFrameBufferAddr());
            SwapBuffers();
            base.OnUpdateFrame(args);
        }
        
        protected override void OnLoad() {
            InitGPU(Size.X, Size.Y);
            GL.ClearColor(0.2f,0.2f,0.2f,1.0f);
            base.OnLoad();

        }
    }
}