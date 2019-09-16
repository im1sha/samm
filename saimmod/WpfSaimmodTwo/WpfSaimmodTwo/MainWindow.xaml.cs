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
        public MainWindow()
        {
            AppViewModel vm;
            InitializeComponent();
            DataContext = vm = new AppViewModel();
            vm.InitializeCommand.Execute(null);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
