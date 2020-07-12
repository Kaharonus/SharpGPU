using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace SharpGPU {
    public class GPU {
        private byte[] _frameBuffer;
        private float[] _depthBuffer;
        
        private IntPtr _fbAdd;
        private GCHandle _fbHandle;
        public GpuController Controller { get; private set; }
        
        public int FbHeight { get; private set; }
        public int FbWidth { get; private set; }


        private GPU() {
            _fbAdd = IntPtr.Zero;
            _frameBuffer = new byte[4];
        }

        ~GPU() {
            _fbHandle.Free();
        }

        public static GPU Create(int width, int height, GpuController controller) {
            var gpu = new GPU();
            gpu.InitFrameBuffer(width, height);
            gpu.Controller = controller;
            gpu.Controller.UseGpu(gpu);
            return gpu;
        }

        
        private void SetPixelColor(int x, int y, byte r, byte g, byte b) {
            int idx = (y * FbWidth + x) * 4;
            _frameBuffer[idx] = r;
            _frameBuffer[idx + 1] = g;
            _frameBuffer[idx + 2] = b;
            _frameBuffer[idx + 3] = 255;
        }

        public IntPtr GetFrameBufferAddr() {
            if (_fbAdd == IntPtr.Zero) {
                _fbHandle = GCHandle.Alloc(_frameBuffer, GCHandleType.Pinned);
                _fbAdd = _fbHandle.AddrOfPinnedObject();
            }

            return _fbAdd;
        }

        public void InitFrameBuffer(int width, int height) {
            FbWidth = width;
            FbHeight = height;
            _frameBuffer = new byte[width * height * 4];
            for (int i = 0; i < width * height * 4; i++) {
                _frameBuffer[i] = 255;
            }
        }

        public void ResizeFrameBuffer(int width, int height) {
            FbWidth = width;
            FbHeight = height;
            Array.Resize(ref _frameBuffer, width * height * 4);
            if (_fbHandle.IsAllocated) {
                _fbHandle.Free();
                _fbAdd = IntPtr.Zero;
            }
        }

        public void DrawTest() {
            var widthInc = 128.0 / FbWidth;
            var heightInc = 128.0 / FbHeight;
            double color = 0;
            for (int x = 0; x < FbWidth; x++) {
                color += widthInc;
                for (int y = 0; y < FbHeight; y++) {
                    color += heightInc;
                    byte colorByte = (byte) color;
                    SetPixelColor(x, y, colorByte, colorByte, colorByte);
                }

                color -= 128;
            }
        }
    }
}