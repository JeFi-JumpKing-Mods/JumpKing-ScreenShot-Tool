using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ScreenShot
{
    [Serializable]
    public class Preferences : INotifyPropertyChanged
    {
        private bool _DrawRayManWall = true;
        public bool DrawRayManWall
        {
            get => _DrawRayManWall;
            set
            {
                _DrawRayManWall = value;
                OnPropertyChanged();
            }
        }

        private int _startIndex = 0;
        public int StartIndex
        {
            get => _startIndex;
            set
            {
                _startIndex = value;
                OnPropertyChanged();
            }
        }

        private int _endIndex = 168;
        public int EndIndex
        {
            get => _endIndex;
            set
            {
                _endIndex = value;
                OnPropertyChanged();
            }
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
