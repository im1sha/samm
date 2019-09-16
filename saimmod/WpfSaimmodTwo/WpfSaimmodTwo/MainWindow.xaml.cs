using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using WpfSaimmodTwo.ViewModels;

namespace WpfSaimmodTwo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AppViewModel _vm;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _vm = new AppViewModel();
            _vm.InitializeCommand.Execute(null);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
