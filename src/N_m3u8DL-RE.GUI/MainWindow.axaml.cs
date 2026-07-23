using Avalonia.Controls;

namespace N_m3u8DL_RE.GUI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}