using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfSaimmodTwo.Models.Core;
using WpfSaimmodTwo.Models;

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
        #endregion

        #region commands
        private InteractCommand _initializeCommand;
        public InteractCommand InitializeCommand => _initializeCommand ??
                    (_initializeCommand = new InteractCommand(stack =>
                    {
                        Initialize(_multiplier, _initialValue, _divider);
                    }));
        private void Initialize(uint multiplier, uint initialValue, uint divider)
        {
            RunCore(out AppModel md, multiplier, initialValue, divider,
                out IEnumerable<double> normalizedSequence,
                out double estimation, out int period, out int aperiodicity);
        }

   

        private void RunCore(out AppModel md, uint multiplier, uint initialValue, uint divider,
            out IEnumerable<double> normalizedSequence, out double estimation,
            out int period, out int aperiodicity)
        {
            md = new AppModel(
                new NormalizedUniformDistribution(),
                new LehmerGenerator(multiplier, initialValue, divider));
            IEnumerable<uint> seq = md.GenerateSequence(500_000);
            normalizedSequence = md.Normalize(seq, divider); // [0,1]
            estimation = md.CalculateIndirectEstimation(normalizedSequence);
            var (length, start) = md.FindCycle(multiplier, initialValue, divider);
            period = length;
            aperiodicity = start + period;
        }

        private void UpdateView(AppModel md, IEnumerable<double> normalizedSequence, object stack)
        {
            void UpdateOutput(double expectedValueParam, double varianceParam,
                double standardDeviationParam)
            {
                ExpectedValue = expectedValueParam.ToString();
                Variance = varianceParam.ToString();
                StandardDeviation = standardDeviationParam.ToString();
            }

            (double expectedValue, double variance, double standardDeviation) = md.GetStatistics(normalizedSequence);
            IEnumerable<int> bars = md.GetDistribution(normalizedSequence, 0.0, 1.0, 20);
            UpdateOutput(expectedValue, variance, standardDeviation);
            ViewUpdater.DrawBarChart(stack, bars);
        }
     
        #endregion

        #region input -unused

        private uint _val;
        public string Val
        {
            get => _val.ToString();
            set
            {
                if (!uint.TryParse(value, out _))
                {
                    return;
                }
                _val = Convert.ToUInt32(value);
                OnPropertyChanged();
            }
        }

        #endregion

        #region statistics labels

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

        #endregion        
    }
}
