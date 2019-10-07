using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using WpfSaimmodThree.Models;

namespace WpfSaimmodThree.ViewModels
{
    internal class AppViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged 

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion

        #region localization

        private static readonly CultureInfo APP_LOCALIZATION = new CultureInfo("en-US");

        #endregion

        #region input 

        private double _probability1 = 0.5;
        public string Probability1
        {
            get => _probability1.ToString(APP_LOCALIZATION);
            set
            {
                if (!double.TryParse(value, out _))
                {
                    return;
                }
                _probability1 = Convert.ToDouble(value, APP_LOCALIZATION);
                OnPropertyChanged();
            }
        }

        private double _probability2 = 0.5;
        public string Probability2
        {
            get => _probability2.ToString(APP_LOCALIZATION);
            set
            {
                if (!double.TryParse(value, out _))
                {
                    return;
                }
                _probability2 = Convert.ToDouble(value, APP_LOCALIZATION);
                OnPropertyChanged();
            }
        }

        #endregion

        #region labels

        private double _failProbability;
        public string FailProbability
        {
            get => $"P(fail): {_failProbability}";
            set
            {
                _failProbability = Convert.ToDouble(value, APP_LOCALIZATION);
                OnPropertyChanged();
            }
        }

        private double _bandwidth;
        public string Bandwidth
        {
            get => $"A: {_bandwidth}";
            set
            {
                _bandwidth = Convert.ToDouble(value, APP_LOCALIZATION);
                OnPropertyChanged();
            }
        }

        private double _queueLength;
        public string QueueLength
        {
            get => $"L(queue): {_queueLength}";
            set
            {
                _queueLength = Convert.ToDouble(value, APP_LOCALIZATION);
                OnPropertyChanged();
            }
        }

        #endregion

        #region commands

        private InteractCommand _initializeCommand;
        public InteractCommand InitializeCommand => _initializeCommand ??
            (_initializeCommand = new InteractCommand((o) =>
            {
                var model = new AppModel(_probability1, _probability2);

                // 
                //
                //
            }));

        #endregion
    }
}
