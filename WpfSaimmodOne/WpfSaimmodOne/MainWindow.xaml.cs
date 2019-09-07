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
            const int TOTAL_INTERVALS = 20;
            const int TOTAL_VALUES = 500000;

            int GetIntContent(string str)
            {
                if (!int.TryParse(str, out int res))
                {
                    return 0;
                }
                return res;
            }
            var mult = GetIntContent(_multiplier.Text);
            var init = GetIntContent(_initialValue.Text);
            var div = GetIntContent(_divider.Text);

            #region generation

            IEnumerable<int> sq = new Algorithm(mult, init, div).GetSequence();

            #region msgbox        
            // var msg = string.Empty;
            //var count = 0;
            //foreach (int item in sq)
            //{
            //    count++;
            //    msg += $"{item}\t";
            //    if (count == 20) break;               
            //}
            //MessageBox.Show(msg);
            #endregion
            IList<int> generatedValues = new List<int>();
            var count = 0;
            foreach (int item in sq)
            {
                if (count == TOTAL_VALUES) break;
                generatedValues.Add(item);
                ++count;
            }

            #endregion

            #region interval calculation

            double intervalLength = (double)div / TOTAL_INTERVALS;

            List<int> results = new List<int>();
            results.AddRange(Enumerable.Repeat(0, TOTAL_INTERVALS));

            foreach (var item in generatedValues)
            {
                results[(int)(item/intervalLength)]++;
            }
            #endregion

            DrawBarChart(_panel, results);
        }

        void DrawBarChart(Panel target, IEnumerable<int> values)
        {
            target.Children.Clear();
            
            double heightCoef = target.ActualHeight / values.Max(); 
            double itemWidth = target.ActualWidth / values.Count();

            int totalItems = values.Count();

            Style controlStyle = FindResource("BottomStyle") as Style;

            for (int i = 0; i < totalItems; i++)
            {
                var uiElement = new TextBlock
                {
                    Width = itemWidth,
                    Height = heightCoef * values.ElementAt(i),
                    Background = i % 2 == 0 ? Brushes.Black: Brushes.Red,
                    Foreground = i % 2 != 0 ? Brushes.Black: Brushes.Red,
                    Style = controlStyle,  
                    Text = values.ElementAt(i).ToString(),
                };


                target.Children.Add(uiElement);
            }           
        }
    }
}
