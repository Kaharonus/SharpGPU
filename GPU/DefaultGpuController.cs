using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace SharpGPU {
    public class DefaultGpuController : GpuController{
        

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
            
        }

        public override void Draw(Matrix4x4 proj, Matrix4x4 view, Matrix4x4 light, Matrix4x4 camera) {
        }
    }
}