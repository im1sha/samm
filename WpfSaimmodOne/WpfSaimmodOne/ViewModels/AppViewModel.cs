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
                        Generate(stack);
                    }));
            }
        }
        private void Generate(object stack)
        {         
            var md = new Mediator(
                new UniformDistribution(), 
                new Lehmer(_multiplier, _initialValue, _divider));

            IEnumerable<uint> seq =  md.InitializeSequence(500_000);
            IEnumerable<double> normalizedSequence = SequenceHelper.Normalize(seq, _divider);
            IEnumerable<int> bars = md.GetDistributedValues(normalizedSequence, 0.0, 1.0, 20);
            ViewUpdater.DrawBarChart(stack, bars);

            (double expectedValue, double variance, double standardDeviation)
                = md.GetStatistics(normalizedSequence);

            var estimation = md.CalculateIndirectEstimation(normalizedSequence);

            int period = 0;
            int aperiodicity = 0;
            var periodResults = SequenceHelper.EstimatePeriod(seq);           
            if (periodResults.HasValue)
            {
                period = periodResults.Value.period;
                aperiodicity = periodResults.Value.aperiodicitySegment;
            }

            UpdateOutput(expectedValue, variance, standardDeviation, estimation, period, aperiodicity);
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

        private InteractCommand _autogenerateCommand;
        public InteractCommand AutogenerateCommand
        {
            get
            {
                return _autogenerateCommand ??
                    (_autogenerateCommand = new InteractCommand(stack =>
                    {
                        //AutoGenerate(stack);
                    }));
            }
        }

        //private void AutoGenerate(object stack)
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
    }
}
