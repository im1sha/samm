using System;
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

        private void Generate(object sender, RoutedEventArgs e)
        {
            (uint mul, uint ini, uint div) = Lehmer.GenerateParameters();
            _divider.Text = div.ToString();
            _initialValue.Text = ini.ToString();
            _multiplier.Text = mul.ToString();
            Run(sender, e);
        }

        private void Run(object sender, RoutedEventArgs e)
        {
            #region form data parsing

            uint GetUIntContent(string str)
            {
                if (!uint.TryParse(str, out uint res))
                {
                    return 0;
                }
                return res;
            }
            var mul = GetUIntContent(_multiplier.Text);
            var ini = GetUIntContent(_initialValue.Text);
            var div = GetUIntContent(_divider.Text);

            #endregion

            var md = new Mediator(new UniformDistribution(div), new Lehmer(mul, ini, div));

            DrawBarChart(_panel, md.GetDistibution());

            (double expectedValue, double variance, double standardDeviation)
                = md.GetNormalizedStatistics();
            ShowStatistics(expectedValue, variance, standardDeviation);
        }

        private void DrawBarChart(Panel target, IEnumerable<uint> values)
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

        private void ShowStatistics(double expectedValue, double variance, double standardDeviation)
        {
            _expectedValue.Content = "M: " + expectedValue;
            _variance.Content = "D: " + variance;
            _standardDeviation.Content = "σ: " + standardDeviation;
        }
    }
}
