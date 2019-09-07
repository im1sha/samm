using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace WpfSaimmodOne
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      
        public MainWindow()
        {
            InitializeComponent();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Calculate(object sender, RoutedEventArgs e)
        {            
            string msg = string.Empty;

            var mult = Convert.ToInt32(multiplyer.Text);
            var init = Convert.ToInt32(initialValue.Text);
            var div = Convert.ToInt32(divider.Text);

            var alg = new Algorithm(mult, init, div);
            var sq = alg.GetSequence();
            var count = 0;

            Console.WriteLine("\nVALUES\n");
            foreach (int item in sq)
            {
                count++;
                msg += $"{item}\t";
                if (count == 20)
                {
                    break;
                }
            }

            MessageBox.Show(msg);
        }
    }
}
