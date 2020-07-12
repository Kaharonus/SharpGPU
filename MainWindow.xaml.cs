using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SharpGPU.OpenGL;


namespace SharpGPU {
    public class MainWindow : Window {
        
        private bool _isWindowOpened = false;
        private OpenGLWindow _renderWindow;
        private readonly Semaphore _openWindowSem;

        private Slider RSlider;
        private Slider GSlider;
        private Slider BSlider;
        
        public MainWindow() {
            _openWindowSem = new Semaphore(0,1);
            var t = new Thread(RenderWindow);
            t.Start();
            InitializeComponent();
        }

        ~MainWindow() {
            _openWindowSem.Dispose();
        }


        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
            RSlider = this.FindControl<Slider>("RSlider");
            GSlider = this.FindControl<Slider>("GSlider"); 
            BSlider = this.FindControl<Slider>("BSlider");
        }

        private void RenderWindow() {
            while (true) {
                _openWindowSem.WaitOne();
                _renderWindow = new OpenGLWindow(500, 500, "GPUSharp Preview Window");
                _renderWindow.Run();
                _renderWindow.Dispose();
                _renderWindow = null;
                if (!_isWindowOpened) {
                    break;
                }
                _isWindowOpened = false;
            }
        }

        private void OpenWindowClick(object sender, RoutedEventArgs args) {
            if (_isWindowOpened) {
                return;
            }

            _openWindowSem.Release();
            _isWindowOpened = true;
        }

        private void CreateNoiseClick(object sender, RoutedEventArgs args) {
            _renderWindow.Gpu.DrawRandom();
        }

        private void MainWindowClosing(object sender, CancelEventArgs args) {
            _isWindowOpened = false;
            _renderWindow?.Close();
        }

        private void SliderMoved(object sender, AvaloniaPropertyChangedEventArgs args) {
            if (args.Property.Name != "Value") {
                return;
            }
            _renderWindow.Gpu.DrawSolidColor((byte)RSlider.Value, (byte)GSlider.Value, (byte)BSlider.Value);
            
        }
    }
}