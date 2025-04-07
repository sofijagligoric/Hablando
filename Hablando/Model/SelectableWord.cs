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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
