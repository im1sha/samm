using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

            long GetInt64Content(string str)
            {
                if (!long.TryParse(str, out long res))
                {
                    return 0;
                }
                return res;
            }
            var mul = GetInt64Content(_multiplier.Text);
            var ini = GetInt64Content(_initialValue.Text);
            var div = GetInt64Content(_divider.Text);

            #region generation

            IEnumerable<long> sq = new Algorithm(mul, ini, div).GetSequenceOfInt64();    
            IList<long> generatedValues = new List<long>();
            var count = 0;
            foreach (var item in sq)
            {
                if (count == TOTAL_VALUES) { break; }
                generatedValues.Add(item);
                ++count;
            }

            #endregion

            #region interval calculation

            double intervalLength = (double)div / TOTAL_INTERVALS;

            var results = new List<long>();
            results.AddRange(Enumerable.Repeat<long>(0, TOTAL_INTERVALS));

            foreach (var item in generatedValues)
            {
                results[(int)(item / intervalLength)]++;
            }
            #endregion

            DrawBarChart(_panel, results);
        }

        private void DrawBarChart(Panel target, IEnumerable<long> values)
        {
            target.Children.Clear();

            double heightCoef = target.ActualHeight / values.Max();
            double itemWidth = target.ActualWidth / values.Count();

            int totalItems = values.Count();

            Style controlStyle = FindResource("BottomStyle") as Style;

            int rowLength = 999.ToString().Length;
            for (int i = 0; i < totalItems; i++)
            {
                string content = values.ElementAt(i).ToString();

                if (content.Length > rowLength)
                {
                    for (int j = content.Length - rowLength; j > 0; j -= rowLength)
                    {
                        content = content.Insert(j, "\n");
                    }
                }

                var uiElement = new TextBlock
                {
                    Width = itemWidth,
                    Height = heightCoef * values.ElementAt(i),
                    Background = i % 2 == 0 ? Brushes.Black : Brushes.DarkGray,
                    Foreground = i % 2 != 0 ? Brushes.Black : Brushes.LightGray,
                    Style = controlStyle,
                    Text = content,
                };

                target.Children.Add(uiElement);
            }
        }
    }
}
