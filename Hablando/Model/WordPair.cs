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
