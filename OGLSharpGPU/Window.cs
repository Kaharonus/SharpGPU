using System;
using OpenToolkit.Core;
using OpenToolkit.Windowing.Desktop;
using OpenToolkit.Graphics;
using OpenToolkit.Graphics.OpenGL4;
using OpenToolkit.Mathematics;
using OpenToolkit.Windowing.Common;

namespace OGLSharpGPU {
    public class Window : GameWindow {

        private GPU gpu;

        float[] vertices = {
            1.0f,  1.0f, 0.0f, 1.0f, 1.0f, // top right
            1.0f, -1.0f, 0.0f, 1.0f, 0.0f, // bottom right
            -1.0f, -1.0f, 0.0f, 0.0f, 0.0f, // bottom left
            -1.0f,  1.0f, 0.0f, 0.0f, 1.0f  // top left
        };

        private readonly uint[] _indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        private int ElementBufferObject;

        private int VertexBufferObject;

        private int VertexArrayObject;

        private Shader shader;

        // For documentation on this, check Texture.cs
        private Texture texture;

        public Window() : base(GameWindowSettings.Default, NativeWindowSettings.Default) { }

        public Window(int width, int height, string title) : base(new GameWindowSettings() {
            RenderFrequency = 60,
            UpdateFrequency = 60
        }, new NativeWindowSettings() {
            Size = new Vector2i(width, height),
            Title = title,
        }) { }
        

        protected override void OnResize(ResizeEventArgs e) {
            GL.Viewport(0, 0, e.Width, e.Height);
            gpu.ResizeFrameBuffer(e.Width,e.Height);
            base.OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs args) {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            shader.Use();
            gpu.DrawRandom();
            GL.DeleteTexture(texture.Handle);
            texture = new Texture(gpu.FBWidth, gpu.FBHeight, gpu.GetFrameBufferAddr());
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
            base.OnUpdateFrame(args);
        }
        
        
        protected override void OnLoad() {
            gpu = GPU.Create(Size.X, Size.Y);
            
            GL.ClearColor(0f,0f,0f,1.0f);
            VertexBufferObject = GL.GenBuffer();
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
            texture = new Texture(gpu.FBWidth, gpu.FBHeight, gpu.GetFrameBufferAddr());

            shader = new Shader("shader.vert", "shader.frag");
            shader.Use();
            
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            
            var vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            
            var texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            
            base.OnLoad();

        }
        
        protected override void OnUnload() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);
            shader.Dispose();
            base.OnUnload();
        }
    }
}