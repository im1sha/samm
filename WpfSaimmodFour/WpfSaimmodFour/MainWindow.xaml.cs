using System.Windows;
using WpfSaimmodFour.ViewModels;

namespace WpfSaimmodFour
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new AppViewModel();
        }
    }
}
