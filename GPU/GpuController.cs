using System;
using System.Numerics;

namespace SharpGPU {
    public abstract class GpuController {

        public GPU Gpu { get; private set; }

        public abstract void VertexShader(ref OutVertex outVertex, InVertex inVertex, Uniforms uniforms);

        public abstract void FragmentShader(ref OutFragment outFragment, InFragment inFragment, Uniforms uniforms);

        public abstract void Init();
        
        public abstract void Draw(Matrix4x4 proj, Matrix4x4 view, Matrix4x4 light, Matrix4x4 camera);

        public void UseGpu(GPU gpu) {
            Gpu = gpu;
        }
    }
}