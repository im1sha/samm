using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfSaimmodTwo.Models;
using WpfSaimmodTwo.Models.Core;
using WpfSaimmodTwo.Models.Distributions;
using WpfSaimmodTwo.Models.Generators;

namespace WpfSaimmodTwo.ViewModels
{
    public class AppViewModel : INotifyPropertyChanged
    {
        #region autogenerate command -unused
        //private InteractCommand _autogenerateCommand;
        //public InteractCommand AutogenerateCommand => _autogenerateCommand ??
        //            (_autogenerateCommand = new InteractCommand(stack =>
        //            {
        //                AutoGenerate(stack);
        //            }));

        //private void AutoGenerate(object stack)
        //{
        //    uint multiplier, initialValue, divider;
        //    bool correctData, validPeriod; // flags
        //    int period, aperiodicity;
        //    double estimation;
        //    IEnumerable<double> normalizedSequence;
        //    AppModel md;

        //    do
        //    {
        //        (multiplier, initialValue, divider) = AppModel.GenerateRandomParameters();

        //        RunCore(out md, multiplier, initialValue, divider, out normalizedSequence,
        //            out estimation, out period, out aperiodicity);

        //        validPeriod = md.CheckIndirectEstimation(estimation, 0.001);
        //        correctData = period > 50_000;
        //    } while (!correctData || !validPeriod);

        //    UpdateTextboxes(multiplier, initialValue, divider);
        //    UpdateView(md, normalizedSequence, stack, estimation, period, aperiodicity);
        //}
        #endregion

        #region INotifyPropertyChanged 

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion

        #region default values
        private readonly uint _initialValue = 2684222873;
        private readonly uint _divider = 4291343633;
        private readonly uint _multiplier = 2595211397;
        private readonly int _totalValues = 300_000;
        private readonly int _totalIntervals = 20;

        #endregion

        private AppModel _model;

        #region commands
        private InteractCommand _initializeCommand;
        public InteractCommand InitializeCommand => _initializeCommand ??
            (_initializeCommand = new InteractCommand((o) =>
            {
                _model = new AppModel(new NormalizedUniformDistribution(), new LehmerGenerator(_multiplier, _initialValue, _divider));
                _model.InitializeModel(_totalValues);
            }));

        private InteractCommand _generateCommand;
        public InteractCommand GenerateCommand => _generateCommand ??
            (_generateCommand = new InteractCommand((stack) =>
            {
                var dist = new UniformDistribution(-100, 500);
                var generator = new UniformDistributionGenerator(dist);

                var res = _model.Run(generator, dist, _totalValues, _totalIntervals);
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

        private double _eta;
        public string Eta
        {
            get => _eta.ToString();
            set
            {
                if (!double.TryParse(value, out _))
                {
                    return;
                }
                _eta = Convert.ToDouble(value);
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

        public List<string> Distributions { get; set; } = new List<string>()
        {
            "Exponential", "Gamma", "Normal", "Simpson", "Triangular", "Uniform"
        };

    }
}
