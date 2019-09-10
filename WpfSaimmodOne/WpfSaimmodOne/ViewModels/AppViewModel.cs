using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfSaimmodOne.ViewModels
{
    class AppViewModel : INotifyPropertyChanged
    {

        #region INotifyPropertyChanged 

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion

        #region commands
        private InteractCommand _generateCommand;
        public InteractCommand GenerateCommand
        {
            get
            {
                return _generateCommand ??
                    (_generateCommand = new InteractCommand(o =>
                    {
                        
                    }));
            }
        }

        private InteractCommand _autogenerateCommand;
        public InteractCommand AutogenerateCommand
        {
            get
            {
                return _autogenerateCommand ??
                    (_autogenerateCommand = new InteractCommand(o =>
                    {

                    }));
            }
        }

        #endregion

        #region labels

        private double _expectedValue;
        public string ExpectedValue
        {
            get => $"M: {_expectedValue}";
            set
            {
                _expectedValue = Convert.ToDouble(value);
                OnPropertyChanged();               
            }
        }
        private double _variance;
        public string Variance
        {
            get => $"D: {_variance}";
            set
            {
                _variance = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }
        private double _standardDeviation;
        public string StandardDeviation
        {
            get => $"sqrt(D): {_standardDeviation}";
            set
            {
                _standardDeviation = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }
        private double _estimation;
        public string Estimation
        {
            get => $"est: {_estimation}";
            set
            {
                _estimation = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }
        private double _period;
        public string Period
        {
            get => $"P: {_period}";
            set
            {
                _period = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }
        private double _aperiodicity;
        public string Aperiodicity
        {
            get => $"L: {_aperiodicity}";
            set
            {
                _aperiodicity = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        #endregion

        #region textboxes

        private uint _multiplier = 3;
        public string Multiplier
        {
            get => _multiplier.ToString();
            set
            {
                _multiplier = Convert.ToUInt32(value);
                OnPropertyChanged();
            }
        }
        private uint _initialValue = 1;
        public string InitialValue
        {
            get => _initialValue.ToString();
            set
            {
                _initialValue = Convert.ToUInt32(value);
                OnPropertyChanged();
            }
        }
        private uint _divider = 5;
        public string Divider
        {
            get => _divider.ToString();
            set
            {
                _divider = Convert.ToUInt32(value);
                OnPropertyChanged();
            }
        }
        #endregion

        //private void AutoGenerate(object sender, RoutedEventArgs e)
        //{
        //    uint mul;
        //    uint ini;
        //    uint div;
        //    bool correctData;
        //    bool validPeriod;

        //    Mediator md;
        //    do
        //    {
        //        (mul, ini, div) = Lehmer.GenerateRandomParameters();
        //        md = new Mediator(new UniformDistribution(div), new Lehmer(mul, ini, div));
        //        md.Initialize();
        //        (correctData, _) = md.EstimateDistribution();
        //        (validPeriod, _, _) = md.EstimatePeriod();
        //    } while (!correctData || !validPeriod);

        //    _divider.Text = div.ToString();
        //    _initialValue.Text = ini.ToString();
        //    _multiplier.Text = mul.ToString();

        //    RunCore(md);
        //}

        //private void ManualRun(object sender, RoutedEventArgs e)
        //{
        //    #region form data parsing

        //    uint GetUIntContent(string str)
        //    {
        //        if (!uint.TryParse(str, out uint res))
        //        {
        //            return 0;
        //        }
        //        return res;
        //    }
        //    var mul = GetUIntContent(_multiplier.Text);
        //    var ini = GetUIntContent(_initialValue.Text);
        //    var div = GetUIntContent(_divider.Text);

        //    #endregion

        //    var md = new Mediator(new UniformDistribution(div), new Lehmer(mul, ini, div));
        //    md.Initialize();
        //    RunCore(md);
        //}

        //private void RunCore(Mediator md)
        //{
        //    DrawBarChart(_panel, md.GetChart());

        //    (double expectedValue, double variance, double standardDeviation)
        //        = md.GetNormalizedStatistics();

        //    (_, var estimation) = md.EstimateDistribution();
        //    (_, var period, var aperiodicity) = md.EstimatePeriod();

        //    ShowStatistics(expectedValue, variance, standardDeviation, estimation, period, aperiodicity);
        //}

        //private void DrawBarChart(Panel target, IEnumerable<uint> values)
        //{
        //    target.Children.Clear();

        //    double heightCoef = target.ActualHeight / values.Max();
        //    double itemWidth = target.ActualWidth / values.Count();

        //    int totalItems = values.Count();

        //    Style controlStyle = FindResource("BottomStyle") as Style;

        //    int rowLength = 999.ToString().Length;
        //    for (int i = 0; i < totalItems; i++)
        //    {
        //        string content = values.ElementAt(i).ToString();

        //        if (content.Length > rowLength)
        //        {
        //            for (int j = content.Length - rowLength; j > 0; j -= rowLength)
        //            {
        //                content = content.Insert(j, "\n");
        //            }
        //        }

        //        var uiElement = new TextBlock
        //        {
        //            Width = itemWidth,
        //            Height = heightCoef * values.ElementAt(i),
        //            Background = i % 2 == 0 ? Brushes.Black : Brushes.DarkGray,
        //            Foreground = i % 2 != 0 ? Brushes.Black : Brushes.LightGray,
        //            Style = controlStyle,
        //            Text = content,
        //        };

        //        target.Children.Add(uiElement);
        //    }
        //}

        //private void ShowStatistics(double expectedValue, double variance, double standardDeviation,
        //    double estimation, int period, int aperiodicity)
        //{
        //    _expectedValue.Content = "M: " + expectedValue;
        //    _variance.Content = "D: " + variance;
        //    _standardDeviation.Content = "σ: " + standardDeviation;
        //    _estimation.Content = "est: " + estimation;
        //    _period.Content = "P: " + period;
        //    _aperiodicity.Content = "L: " + aperiodicity;
        //}
    
    }
}
