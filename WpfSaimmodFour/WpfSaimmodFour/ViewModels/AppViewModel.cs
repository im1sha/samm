using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using WpfSaimmodFour.Models;

namespace WpfSaimmodFour.ViewModels
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

        //private double _probability1 = 0.5;
        //public string Probability1
        //{
        //    get => _probability1.ToString(APP_LOCALIZATION);
        //    set
        //    {
        //        if (!double.TryParse(value, out _))
        //        {
        //            return;
        //        }
        //        _probability1 = Convert.ToDouble(value, APP_LOCALIZATION);
        //        OnPropertyChanged();
        //    }
        //}



        #endregion

        #region labels

        //private double _failProbability;
        //public string FailProbability
        //{
        //    get => $"P(fail): {_failProbability}";
        //    set
        //    {
        //        _failProbability = Convert.ToDouble(value, APP_LOCALIZATION);
        //        OnPropertyChanged();
        //    }
        //}
       
        #endregion

        #region list

        //public ObservableCollection<ProbabilityItem> States { get; set; }
        //    = new ObservableCollection<ProbabilityItem>();

        #endregion

        #region commands

        private InteractCommand _calculateCommand;
        public InteractCommand CalculateCommand => _calculateCommand ??
            (_calculateCommand = new InteractCommand((o) =>
            {
                //AppModel model = new AppModel(_probability1, _probability2);

                //model.Run();

                //Bandwidth = model.GetBandwidth().ToString(APP_LOCALIZATION);
                //QueueLength = model.GetAverageQueueLength().ToString(APP_LOCALIZATION);
                //FailProbability = model.GetFailureProbability().ToString(APP_LOCALIZATION);

                //IEnumerable<ProbabilityItem> newItems = model.StatesProbabilities;
                //States.Clear();
                //foreach (var item in newItems)
                //{
                //    States.Add(item);
                //}
            }));

        #endregion
    }
}
