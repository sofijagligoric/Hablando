using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hablando.Model
{
    /*
    public class WordPair : INotifyPropertyChanged
    {
        public string Serbian { get; set; }
        public string Spanish { get; set; }

        public WordPair(string serbian, string spanish) { 
            Serbian = serbian;
            Spanish = spanish;
            IsCorrect = 0;
        }

        public WordPair() { 
            IsCorrect = 0;
        
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        private int _isCorrect;
        public int IsCorrect
        {
            get => _isCorrect;
            set
            {
                _isCorrect = value;
                OnPropertyChanged(nameof(IsCorrect));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    */

    public class WordPair : INotifyPropertyChanged
    {
        public SelectableWord SerbianWord { get; set; }
        public SelectableWord SpanishWord { get; set; }

        public WordPair(string serbian, string spanish)
        {
            SerbianWord = new SelectableWord { Text = serbian };
            SpanishWord = new SelectableWord { Text = spanish };
        }

        public WordPair() { }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
