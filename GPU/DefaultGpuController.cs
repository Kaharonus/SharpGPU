using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace SharpGPU {
    public class DefaultGpuController : GpuController {
        private int _bufferId = 0;
        private int _counter = 0;
        public override void VertexShader(ref OutVertex outVertex, InVertex inVertex, Uniforms uniforms) {
            if(inVertex.VertexId == 0)
                outVertex.Position = new Vector4(-1,-1,0,1);
            if(inVertex.VertexId == 1)
                outVertex.Position = new Vector4(+1,-1,0,1);
            if(inVertex.VertexId == 2)
                outVertex.Position = new Vector4(-1,+1,0,1);
        }

        public override void FragmentShader(ref OutFragment outFragment, InFragment inFragment, Uniforms uniforms) {
            outFragment.Color = new Vector4(1,1,1,1);
        }

        public override void Init() {
            Buffer buffer = new Buffer(100);
            buffer.WriteData(0, new List<int>() {0,1,2,3});
            _bufferId = Gpu.AssignBuffer(buffer);
        }

        public override void Draw(Matrix4x4 proj, Matrix4x4 view, Matrix4x4 light, Matrix4x4 camera) {
            _counter++;
            Gpu.GetBuffer(_bufferId).WriteData(0, _counter);
        }
    }
}