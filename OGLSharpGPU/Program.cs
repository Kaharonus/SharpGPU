using System;
using System.Collections.ObjectModel;
using OpenToolkit;
using OpenToolkit.Graphics.ES11;
using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Desktop;

namespace OGLSharpGPU {
    class Program {



        static void Main(string[] args) {
            
           Window w = new Window(800, 480, "GPUSharp");
           //Window w = new Window();
           w.Run();

           Console.WriteLine("Hello World!");
        }
    }
}