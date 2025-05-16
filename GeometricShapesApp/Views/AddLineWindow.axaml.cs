using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GeometricShapesApp.Views
{
    public partial class AddLineWindow : Window
    {
        public AddLineWindow()
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