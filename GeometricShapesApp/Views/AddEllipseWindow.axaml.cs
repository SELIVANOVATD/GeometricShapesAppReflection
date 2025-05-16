using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GeometricShapesApp.Views
{
    public partial class AddEllipseWindow : Window
    {
        public AddEllipseWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}