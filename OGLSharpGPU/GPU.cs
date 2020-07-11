using System;
using System.Runtime.InteropServices;

namespace OGLSharpGPU {
    public class GPU {
        private byte[] FrameBuffer { get; set; }

        private IntPtr fbAdd;
        private GCHandle fbHandle;

        public int FBHeight { get; private set; }
        public int FBWidth { get; private set; }


        public GPU() {
            fbAdd = IntPtr.Zero;
            FrameBuffer = new byte[3];
        }

        ~GPU() {
            fbHandle.Free();
        }

        private void SetPixelColor(int x, int y, byte r, byte g, byte b) {
            int idx = ((y * FBWidth) + x)*3;
            FrameBuffer[idx] = r;
            FrameBuffer[idx + 1] = g;
            FrameBuffer[idx + 2] = b;
        }

        public IntPtr GetFrameBufferAddr() {
            if (fbAdd == IntPtr.Zero) {
                fbHandle = GCHandle.Alloc(FrameBuffer, GCHandleType.Pinned);
                fbAdd = fbHandle.AddrOfPinnedObject();
            }
            return fbAdd;
        }

        public void InitFrameBuffer(int width, int height) {
            FBWidth = width;
            FBHeight = height;
            FrameBuffer = new byte[width * height * 3];
            for (int i = 0; i < width * height * 3; i++) {
                FrameBuffer[i] = 255;
            }
        }

        public void ResizeFrameBuffer(int width, int height) {
            FBWidth = width;
            FBHeight = height;
        } 

        public void DrawRandom() {
            var r = new Random();
            var bytes = new byte[3]{255,255,255};
            for (int x = 0; x < FBWidth; x++) {
                for (int y = 0; y < FBHeight; y++) {
                    //r.NextBytes(bytes);
                    SetPixelColor(x, y, bytes[0], bytes[1], bytes[2]);
                }
            }
        }
    }
}