using System;
using System.Runtime.InteropServices;

namespace OGLSharpGPU {
    public class GPU {
        private byte[] frameBuffer;
        private IntPtr fbAdd;
        private GCHandle fbHandle;

        public int FBHeight { get; private set; }
        public int FBWidth { get; private set; }


        public GPU() {
            fbAdd = IntPtr.Zero;
            frameBuffer = new byte[4];
        }

        ~GPU() {
            fbHandle.Free();
        }

        public static GPU Create(int width, int height) {
            var gpu = new GPU();
            gpu.InitFrameBuffer(width, height);
            return gpu;
        }

        private void SetPixelColor(int x, int y, byte r, byte g, byte b) {
            int idx = ((y * FBWidth) + x)*4;
            frameBuffer[idx] = r;
            frameBuffer[idx + 1] = g;
            frameBuffer[idx + 2] = b;
            frameBuffer[idx + 3] = 255;

        }

        public IntPtr GetFrameBufferAddr() {
            if (fbAdd == IntPtr.Zero) {
                fbHandle = GCHandle.Alloc(frameBuffer, GCHandleType.Pinned);
                fbAdd = fbHandle.AddrOfPinnedObject();
            }
            return fbAdd;
        }

        public void InitFrameBuffer(int width, int height) {
            FBWidth = width;
            FBHeight = height;
            frameBuffer = new byte[width * height * 4];
            for (int i = 0; i < width * height * 4; i++) {
                frameBuffer[i] = 255;
            }
        }

        public void ResizeFrameBuffer(int width, int height) {
            FBWidth = width;
            FBHeight = height;
            Array.Resize(ref frameBuffer, width*height*4);
            if (fbHandle.IsAllocated) {
                fbHandle.Free();
                fbAdd = IntPtr.Zero;
            }
        } 

        public void DrawRandom() {
            var r = new Random();
            var bytes = new byte[]{0,0,0};
            for (int x = 0; x < FBWidth; x++) {
                for (int y = 0; y < FBHeight; y++) {
                    r.NextBytes(bytes);
                    SetPixelColor(x, y, bytes[0], bytes[1], bytes[2]);
                }
            }
        }
    }
}