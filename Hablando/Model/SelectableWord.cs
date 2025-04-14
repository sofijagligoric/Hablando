using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hablando.Model
{
    public class SelectableWord : INotifyPropertyChanged
    {
        public string Text { get; set; }

        private int _isCorrect;
        public int IsCorrect
        {
            get => _isCorrect;
            set { _isCorrect = value; OnPropertyChanged(nameof(IsCorrect)); }
        }

        private int _isClicked;
        public int IsClicked
        {
            get => _isClicked;
            set { _isClicked = value; OnPropertyChanged(nameof(IsClicked)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
