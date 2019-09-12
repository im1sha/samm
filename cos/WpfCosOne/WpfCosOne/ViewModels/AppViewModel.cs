using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfCosOne.ViewModels
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

        #region commands

        private InteractCommand _command;
        public InteractCommand Command
        {
            get
            {
                return _command ??
                    (_command = new InteractCommand(stack =>
                    {

                    }));
            }
        }
        #endregion

        #region form content

        private int _content;
        public string Content
        {
            get => _content.ToString();
            set
            {
                if (!int.TryParse(value, out _))
                {
                    return;
                }
                _content = Convert.ToInt32(value);
                OnPropertyChanged();
            }
        }

        #endregion     
    }
}
