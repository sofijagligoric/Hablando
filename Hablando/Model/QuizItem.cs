using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hablando.Model
{
    public class QuizItem : INotifyPropertyChanged
    {
        public string SerbianWord { get; set; }
        public List<SelectableWord> SpanishOptions { get; set; } // 3 opcije
        public string CorrectSpanish { get; set; }

        private SelectableWord _selectedAnswer;
        public SelectableWord SelectedAnswer
        {
            get => _selectedAnswer;
            set
            {
                _selectedAnswer = value;
                OnPropertyChanged(nameof(SelectedAnswer));
            }
        }

        public QuizItem (string serbianWord, string correctSpanish,List<string> spanishWords)
        {
            SerbianWord = serbianWord;
            CorrectSpanish = correctSpanish;
            SpanishOptions = new List<SelectableWord>();
            foreach (string pom in spanishWords)
            {
                SpanishOptions.Add(new SelectableWord { Text = pom });
            }
        }

        public bool? IsCorrect => SelectedAnswer == null ? (bool?)null : SelectedAnswer.Text == CorrectSpanish;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
