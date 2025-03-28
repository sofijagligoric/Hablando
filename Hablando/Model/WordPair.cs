using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hablando.Model
{
    public class WordPair : INotifyPropertyChanged
    {
        private bool _isCorrect;
        private bool _isIncorrect;

        public string Serbian { get; set; }
        public string Spanish { get; set; }

        public bool IsCorrect
        {
            get => _isCorrect;
            set
            {
                _isCorrect = value;
                OnPropertyChanged(nameof(IsCorrect));
            }
        }

        public bool IsIncorrect
        {
            get => _isIncorrect;
            set
            {
                _isIncorrect = value;
                OnPropertyChanged(nameof(IsIncorrect));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
