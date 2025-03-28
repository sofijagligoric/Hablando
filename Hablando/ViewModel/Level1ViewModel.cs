using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using Hablando.Model;
using Hablando.Util;
using Hablando.View;

namespace Hablando.ViewModel
{
    public class Level1ViewModel : ViewModelBase, INotifyPropertyChanged
    {

        private MainWindow _mainWindow;

        /*
        public ObservableCollection<WordPair> SRSPDictionary { get; set; }
        public ObservableCollection<string> SerbianWords { get; set; }
        public ObservableCollection<string> SpanishWords { get; set; }
        */

        /*
        private WordPair _selectedSerbianWord;
        private WordPair _selectedSpanishWord;
        */
        private string _selectedSerbianWord;
        private string _selectedSpanishWord;

        public ObservableCollection<WordPair> WordPairs { get; set; }
        public ObservableCollection<WordPair> SerbianWords { get; set; }
        public ObservableCollection<WordPair> SpanishWords { get; set; }
        public ICommand SelectWordCommand { get; }
        private int _points;
        public int Points
        {
            get => _points;
            set
            {
                _points = value;
                OnPropertyChanged(nameof(Points));
            }
        }

        private TimeSpan _timeRemaining;
        public TimeSpan TimeRemaining
        {
            get => _timeRemaining;
            set
            {
                _timeRemaining = value;
                OnPropertyChanged(nameof(TimeRemaining));
            }
        }

        private DispatcherTimer _timer;

        public Level1ViewModel(MainWindow mainWindow)
        {
            // Učitavanje reči iz ResourceDictionary
            Points = 0;
            _mainWindow = mainWindow;
            var dictionary = Application.Current.Resources.MergedDictionaries
                            .FirstOrDefault(d => d.Contains("SerbianSpanishDictionary"));
            LoadWords();

            //     SelectWordCommand = new RelayCommand<WordPair>(SelectWord);
            SelectWordCommand = new RelayCommand<String>(SelectWord);

            /*
            if (dictionary != null)
            {
                var reci = dictionary["SerbianSpanishDictionary"] as string[];

                SRSPDictionary = new ObservableCollection<WordPair>(
                    reci.Select(r =>
                    {
                        var parts = r.Split(',');
                        return new WordPair { Serbian = parts[0], Spanish = parts[1] };
                    })
                );

                SerbianWords = new ObservableCollection<string>(SRSPDictionary.Select(r => r.Serbian).OrderBy(x => Guid.NewGuid()));
                SpanishWords = new ObservableCollection<string>(SRSPDictionary.Select(r => r.Spanish).OrderBy(x => Guid.NewGuid()));
            }
            */
            TimeRemaining = TimeSpan.FromMinutes(2);
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += TimerTick;
            _timer.Start();

        }

        private void LoadWords()
        {
            var dictionary = Application.Current.Resources.MergedDictionaries
                             .FirstOrDefault(d => d.Contains("Recnik"));

            if (dictionary != null)
            {
                var reci = dictionary["Recnik"] as string[];
                var shuffledPairs = reci.Select(r =>
                {
                    var parts = r.Split(',');
                    return new WordPair { Serbian = parts[0], Spanish = parts[1] };
                }).OrderBy(x => Guid.NewGuid()).ToList();

                WordPairs = new ObservableCollection<WordPair>(shuffledPairs);
                SerbianWords = new ObservableCollection<WordPair>(WordPairs.OrderBy(x => Guid.NewGuid()));
                SpanishWords = new ObservableCollection<WordPair>(WordPairs.OrderBy(x => Guid.NewGuid()));

                OnPropertyChanged(nameof(SerbianWords));
                OnPropertyChanged(nameof(SpanishWords));
            }
        }

        /*
        private async void SelectWord(WordPair word)
        {
            Debug.WriteLine($"-----> Kliknuto! { word}");
            if (SerbianWords.Contains(word))
            {
                _selectedSerbianWord = word;
            }
            else if (SpanishWords.Contains(word))
            {
                _selectedSpanishWord = word;
            }

            if (_selectedSerbianWord != null && _selectedSpanishWord != null)
            {
                bool isMatch = _selectedSerbianWord.Spanish == _selectedSpanishWord.Spanish;

                if (isMatch)
                {
                    _selectedSerbianWord.IsCorrect = true;
                    _selectedSpanishWord.IsCorrect = true;
                    Points++;

                    await Task.Delay(1000);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        SerbianWords.Remove(_selectedSerbianWord);
                        SpanishWords.Remove(_selectedSpanishWord);

                        // Dodajemo nove reči ako ih ima
                        if (WordPairs.Count > 0)
                        {
                            var newPair = WordPairs.First();
                            WordPairs.Remove(newPair);
                            SerbianWords.Add(newPair);
                            SpanishWords.Add(newPair);
                        }
                    });
                }
                else
                {
                    _selectedSerbianWord.IsIncorrect = true;
                    _selectedSpanishWord.IsIncorrect = true;

                    await Task.Delay(1000);

                    _selectedSerbianWord.IsIncorrect = false;
                    _selectedSpanishWord.IsIncorrect = false;
                }

                _selectedSerbianWord = null;
                _selectedSpanishWord = null;
            }
        }
        */
        private void SelectWord(string word)
        {
            Debug.WriteLine($"-----> Kliknuto! {word}");

            // Ako je reč na srpskom, setuj je
            if (SerbianWords.Any(w => w.Serbian == word))
            {
                _selectedSerbianWord = word;
            }
            // Ako je reč na španskom, setuj je
            else if (SpanishWords.Any(w => w.Spanish == word))
            {
                _selectedSpanishWord = word;
            }

            // Kada su oba selektovana, proveravamo da li su par
            if (!string.IsNullOrEmpty(_selectedSerbianWord) && !string.IsNullOrEmpty(_selectedSpanishWord))
            {
                var matchingPair = WordPairs.FirstOrDefault(w => w.Serbian == _selectedSerbianWord && w.Spanish == _selectedSpanishWord);

                if (matchingPair != null)
                {
                    Debug.WriteLine("✔ Tačan par!");

                    Points++;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        SerbianWords.Remove(matchingPair);
                        SpanishWords.Remove(matchingPair);
                    });
                }
                else
                {
                    Debug.WriteLine("❌ Pogrešan par!");
                }

                _selectedSerbianWord = null;
                _selectedSpanishWord = null;
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            
            
            if (TimeRemaining.TotalSeconds > 0)
            {
                TimeRemaining = TimeRemaining.Subtract(TimeSpan.FromSeconds(1));
                OnPropertyChanged(nameof(TimeRemaining));
            }
            else
            {
                _timer.Stop();
                _mainWindow.MainViewModel.Points += Points;
               
                GameoverWindow dialog2 = new GameoverWindow(Points, true);
                bool? dialogResult2 = dialog2.ShowDialog();
                if ((bool)dialogResult2)
                {
                    _mainWindow.MainFrame.Content = new LevelTwoPage(_mainWindow);
                }
                else
                {
                    _mainWindow.MainFrame.Content = _mainWindow.StartPage;
                }
            }
        }

        public void StopTimer()
        {
            if (_timer != null && _timer.IsEnabled)
            {
                _timer.Stop();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
