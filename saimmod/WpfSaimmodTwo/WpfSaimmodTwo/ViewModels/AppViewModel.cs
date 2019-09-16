using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using WpfSaimmodTwo.Models;
using WpfSaimmodTwo.Models.Core;
using WpfSaimmodTwo.Models.Distributions;
using WpfSaimmodTwo.Models.Generators;

namespace WpfSaimmodTwo.ViewModels
{
    public class AppViewModel : INotifyPropertyChanged
    {     
        #region INotifyPropertyChanged 

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion

        #region default values

        private readonly uint _multiplier = 2093054237;
        private readonly uint _initialValue = 2667363311;
        private readonly uint _divider = 4286309017;
        private readonly int _totalValues = 25_000;
        private readonly int _totalIntervals = 20;

        #endregion

        private AppModel _model;

        #region commands
        private InteractCommand _initializeCommand;
        public InteractCommand InitializeCommand => _initializeCommand ??
            (_initializeCommand = new InteractCommand((o) =>
            {
                _model = new AppModel(new LehmerGenerator(_multiplier, _initialValue, _divider));
                _model.InitializeModel(_totalValues);
            }));

        private InteractCommand _generateCommand;
        public InteractCommand GenerateCommand => _generateCommand ??
            (_generateCommand = new InteractCommand((stack) =>
            {
                 UniformNormalizedBasedGenerator generator = 
                    CreateDistribution(
                        _selectedDistribution,  
                        _begin, _end, 
                        _expectedValue, _variance,
                        _lambda, _eta);

                if (generator == null)
                {
                    MessageBox.Show("check input data");
                    return;
                }

                var res = _model.Run(generator, _totalValues, _totalIntervals);
                ViewUpdater.DrawBarChart(stack, res.distribution);
                ExpectedValueLabel = res.expectedValue.ToString();
                VarianceLabel = res.variance.ToString();
                StandardDeviationLabel = res.standardDeviation.ToString();
            }));

        #endregion

        #region input 

        private double _begin;
        public string Begin
        {
            get => _begin.ToString();
            set
            {
                if (!double.TryParse(value, out _))
                {
                    return;
                }
                _begin = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        private double _end;
        public string End
        {
            get => _end.ToString();
            set
            {
                if (!double.TryParse(value, out _))
                {
                    return;
                }
                _end = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        private double _variance;
        public string Variance
        {
            get => _variance.ToString();
            set
            {
                if (!double.TryParse(value, out _))
                {
                    return;
                }
                _variance = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        private double _expectedValue;
        public string ExpectedValue
        {
            get => _expectedValue.ToString();
            set
            {
                if (!double.TryParse(value, out _))
                {
                    return;
                }
                _expectedValue = Convert.ToDouble(value);
                OnPropertyChanged();
            }

        }

        private double _lambda;
        public string Lambda
        {
            get => _lambda.ToString();
            set
            {
                if (!double.TryParse(value, out _))
                {
                    return;
                }
                _lambda = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        private int _eta;
        public string Eta
        {
            get => _eta.ToString();
            set
            {
                if (!double.TryParse(value, out _))
                {
                    return;
                }
                _eta = Convert.ToInt32(value);
                OnPropertyChanged();
            }
        }

        #endregion

        #region statistics labels

        private double _expectedValueLabel;
        public string ExpectedValueLabel
        {
            get => $"M: {_expectedValueLabel}";
            set
            {
                _expectedValueLabel = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        private double _varianceLabel;
        public string VarianceLabel
        {
            get => $"D: {_varianceLabel}";
            set
            {
                _varianceLabel = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        private double _standardDeviationLabel;
        public string StandardDeviationLabel
        {
            get => $"sqrt(D): {_standardDeviationLabel}";
            set
            {
                _standardDeviationLabel = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        #endregion

        #region selected distribution

        private string _selectedDistribution = null;

        public ObservableCollection<string> Distributions { get; set; } = new ObservableCollection<string>()
        {
            "Exponential",
            "Gamma",
            "Normal",
            "Simpson",
            "Triangular",
            "Uniform"
        };

        public string SelectedDistribution
        {
            get { return _selectedDistribution; }
            set
            {
                _selectedDistribution = value;
                OnPropertyChanged();
            }
        }

        #endregion

        private UniformNormalizedBasedGenerator CreateDistribution(
            string selectedDistribution,
            double begin, double end,
            double expectedValue, double variance, 
            double lambda, int eta)
        {
            UniformNormalizedBasedGenerator generator = null;
            try
            {
                switch (selectedDistribution)
                {
                    case "Exponential":
                        generator = new ExponentialDistributionGenerator(new ExponentialDistribution(lambda));
                        break;
                    case "Gamma":
                        generator = new GammaDistributionGenerator(new GammaDistribution(eta, lambda));
                        break;
                    case "Normal":
                        generator = new NormalDistributionGenerator(new NormalDistribution(expectedValue, variance));
                        break;
                    case "Simpson":
                        generator = new SimpsonDistributionGenerator(new SimpsonDistribution(begin, end));
                        break;
                    case "Triangular":
                        generator = new TriangularDistributionGenerator(new TriangularDistribution(begin, end));
                        break;
                    case "Uniform":
                        generator = new UniformDistributionGenerator(new UniformDistribution(begin, end));
                        break;
                }
            }
            catch 
            {
                return null;
            }
              
            return generator;
        }
    }
}
