using System.Numerics;
using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Mathematics;
using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Desktop;

namespace SharpGPU.OpenGL {
    public class OpenGLWindow : GameWindow {
        public GPU Gpu { get; private set; }

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

        private OpenGlShader _openGlShader;

        // For documentation on this, check Texture.cs
        private OpenGlTexture _openGlTexture;
        
        public OpenGLWindow(int width, int height, string title, GPU gpu) : base(GameWindowSettings.Default,
            new NativeWindowSettings() {
                Size = new Vector2i(width, height),
                Title = title,
            }) {
            this.Gpu = gpu;
        }

        

        protected override void OnResize(ResizeEventArgs e) {
            GL.Viewport(0, 0, e.Width, e.Height);
            Gpu.ResizeFrameBuffer(e.Width,e.Height);
            base.OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs args) {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Gpu.Controller.Draw(new Matrix4x4(), new Matrix4x4(), new Matrix4x4(), new Matrix4x4());
            _openGlShader.Use();
            GL.DeleteTexture(_openGlTexture.Handle);
            _openGlTexture = new OpenGlTexture(Gpu.FbWidth, Gpu.FbHeight, Gpu.GetFrameBufferAddr());
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
            base.OnUpdateFrame(args);
        }
        
        
        protected override void OnLoad() {
            Gpu.Controller.Init();
            
            GL.ClearColor(0f,0f,0f,1.0f);
            VertexBufferObject = GL.GenBuffer();
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
            _openGlTexture = new OpenGlTexture(Gpu.FbWidth, Gpu.FbHeight, Gpu.GetFrameBufferAddr());

            _openGlShader = new OpenGlShader("OpenGL/shader.vert", "OpenGL/shader.frag");
            _openGlShader.Use();
            
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            
            var vertexLocation = _openGlShader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            
            var texCoordLocation = _openGlShader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            
            base.OnLoad();

        }
        
        protected override void OnUnload() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);
            _openGlShader.Dispose();
            base.OnUnload();
        }
    }
}