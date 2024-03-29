﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfSaimmodOne.Models;
using WpfSaimmodOne.Utils;

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
                    (_generateCommand = new InteractCommand(stack =>
                    {
                        Generate(stack, _multiplier, _initialValue, _divider);
                    }));
            }
        }
        private void Generate(object stack, uint multiplier, uint  initialValue, uint divider)
        {
            RunCore(out Mediator md, multiplier, initialValue, divider,
                out IEnumerable<double> normalizedSequence, 
                out double estimation, out int period, out int aperiodicity);

            UpdateView(md, normalizedSequence, stack, estimation, period, aperiodicity);
        }


        private InteractCommand _autogenerateCommand;
        public InteractCommand AutogenerateCommand
        {
            get
            {
                return _autogenerateCommand ??
                    (_autogenerateCommand = new InteractCommand(stack =>
                    {
                        AutoGenerate(stack);
                    }));
            }
        }

        private void AutoGenerate(object stack)
        {
            uint multiplier, initialValue, divider;
            bool correctData, validPeriod;// flags
            int period, aperiodicity;
            double estimation;
            IEnumerable<double> normalizedSequence;
            Mediator md;

            do
            {
                (multiplier, initialValue, divider) = Lehmer.GenerateRandomParameters();

                RunCore(out md, multiplier, initialValue, divider, out normalizedSequence, 
                    out estimation, out period, out aperiodicity);

                validPeriod = md.CheckIndirectEstimation(estimation, 0.001);                
                correctData = period > 50_000;     
            } while (!correctData || !validPeriod);

            UpdateTextboxes(multiplier, initialValue, divider);
            UpdateView(md, normalizedSequence, stack, estimation, period, aperiodicity);
        }

        private void RunCore(out Mediator md, uint multiplier, uint initialValue, uint divider,
            out IEnumerable<double> normalizedSequence, out double estimation,
            out int period, out int aperiodicity)
        {
            md = new Mediator(
               new UniformDistribution(),
               new Lehmer(multiplier, initialValue, divider));
            IEnumerable<uint> seq = md.InitializeSequence(500_000);
            normalizedSequence = SequenceNormalizer.Normalize(seq, divider); // [0,1]
            estimation = md.CalculateIndirectEstimation(normalizedSequence);
            var periodResults = md.FindCycle(multiplier, initialValue, divider);
            period = periodResults.clength;
            aperiodicity = periodResults.cstart + period;
        }

        private void UpdateView(Mediator md, IEnumerable<double> normalizedSequence, object stack, 
            double estimation, int period, int aperiodicity)
        {
            (double expectedValue, double variance, double standardDeviation)
               = md.GetStatistics(normalizedSequence);
            IEnumerable<int> bars = md.GetDistributedValues(normalizedSequence, 0.0, 1.0, 20);
            UpdateOutput(expectedValue, variance, standardDeviation, estimation, period, aperiodicity);
            ViewUpdater.DrawBarChart(stack, bars);
        }

        private void UpdateTextboxes(uint multiplier, uint initialValue, uint divider)
        {
            Multiplier = multiplier.ToString();
            InitialValue = initialValue.ToString();
            Divider = divider.ToString();
        }
        private void UpdateOutput(double expectedValue, double variance, 
            double standardDeviation, double estimation,
            int period, int aperiodicity)
        {
            ExpectedValue = expectedValue.ToString();
            Variance = variance.ToString();
            StandardDeviation = standardDeviation.ToString();
            Estimation = estimation.ToString();
            Period = period.ToString();
            Aperiodicity = aperiodicity.ToString();
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
        private int _period;
        public string Period
        {
            get => $"P: {_period}";
            set
            {
                _period = Convert.ToInt32(value);
                OnPropertyChanged();
            }
        }
        private int _aperiodicity;
        public string Aperiodicity
        {
            get => $"L: {_aperiodicity}";
            set
            {
                _aperiodicity = Convert.ToInt32(value);
                OnPropertyChanged();
            }
        }

        #endregion

        #region textboxes

        private uint _multiplier = 2172724891;
        public string Multiplier
        {
            get => _multiplier.ToString();
            set
            {
                if (!uint.TryParse(value, out _))
                {
                    return;
                }
                _multiplier = Convert.ToUInt32(value);
                OnPropertyChanged();
            }
        }
        private uint _initialValue = 3479695279;
        public string InitialValue
        {
            get => _initialValue.ToString();
            set
            {
                if (!uint.TryParse(value, out _))
                {
                    return;
                }
                _initialValue = Convert.ToUInt32(value);
                OnPropertyChanged();
            }
        }
        private uint _divider = 4290123121;
        public string Divider
        {
            get => _divider.ToString();
            set
            {
                if (!uint.TryParse(value, out _))
                {
                    return;
                }
                _divider = Convert.ToUInt32(value);
                OnPropertyChanged();
            }
        }
        #endregion
    }
}
