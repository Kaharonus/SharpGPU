using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SharpGPU {
    public class Buffer {
        private Mutex _mutex;
        private int[] _buffer;
        public int Size { get; }

        public Buffer(int size) {
            Size = size;
            _mutex = new Mutex();
            _buffer = new int[size];
        }

        public bool WriteData(int index, int data) {
            if (index >= _buffer.Length) {
                return false;
            }
            _mutex.WaitOne();
            _buffer[index] = data;
            _mutex.ReleaseMutex();
            return true;
        }

        public bool WriteData(int offset, IEnumerable<int> data) {
            var enumerable = data as int[] ?? data.ToArray();
            if (enumerable.Length > Size + offset) {
                return false;
            }
            _mutex.WaitOne();
            var counter = 0;
            foreach (var item in enumerable) {
                _buffer[offset + counter] = item;
                counter++;
            }
            _mutex.ReleaseMutex();
            return true;
        }

        public bool ReadData(int offset, int count, ref int[] data) {
            _mutex.WaitOne();

            if (data.Length < count) {
                _mutex.ReleaseMutex();
                return false;
            }

            for (var i = 0; i < count; i++) {
                data[i] = _buffer[i + offset];
            }

            _mutex.ReleaseMutex();
            return true;
        }
    }
}