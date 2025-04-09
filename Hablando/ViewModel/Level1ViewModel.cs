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
         
         public ObservableCollection<string> SerbianWords { get; set; }
         public ObservableCollection<string> SpanishWords { get; set; }
        */
        public ObservableCollection<SelectableWord> SerbianWords { get; set; }
        public ObservableCollection<SelectableWord> SpanishWords { get; set; }
        public ObservableCollection<WordPair> WordPairs { get; set; }
        private SelectableWord _selectedSerbianWord;
        private SelectableWord _selectedSpanishWord;

        public Dictionary<string, WordPair> WordMap { get; set; } = new Dictionary<string, WordPair>();
        private List<WordPair> AvailableWordPairs { get; set; }
        private const int InitialWordCount = 5;
        public ICommand RestartCommand { get; }

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
            Points = 0;
            _mainWindow = mainWindow;
            SerbianWords = new ObservableCollection<SelectableWord>();
            SpanishWords = new ObservableCollection<SelectableWord>();
            WordPairs = new ObservableCollection<WordPair>();
         //   var dictionary = Application.Current.Resources.MergedDictionaries.FirstOrDefault(d => d.Contains("SerbianSpanishDictionary"));
            LoadWords();

           
            SelectWordCommand = new RelayCommand<SelectableWord>(SelectWord);
            RestartCommand = new RelayCommandWithoutParameters(RestartGame);

            TimeRemaining = TimeSpan.FromMinutes(2);
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += TimerTick;
            _timer.Start();

        }
        public void RestartGame()
        {
            Points = 0;
            SerbianWords.Clear();
            SpanishWords.Clear();
            WordPairs.Clear();
            WordMap.Clear();
            TimeRemaining = TimeSpan.FromMinutes(2);
            _timer.Stop();
            _timer.Start();
            LoadWords();
        }


        private void LoadWords()
        {
            var dictionary = Application.Current.Resources.MergedDictionaries
                             .FirstOrDefault(d => d.Contains("Recnik"));

            if (dictionary != null)
            {
                var reci = dictionary["Recnik"] as string[];
                AvailableWordPairs = reci.Select(r =>
                {
                    var parts = r.Split(',');
                    return new WordPair(parts[0], parts[1]);
                }).OrderBy(_ => Guid.NewGuid()).ToList();

                /*
                WordPairs = new ObservableCollection<WordPair>();
                SerbianWords = new ObservableCollection<SelectableWord>();
                SpanishWords = new ObservableCollection<SelectableWord>();
                */

                LoadNextBatch();
            }
        }

        private void LoadNextBatch()
        {
            if (AvailableWordPairs.Count == 0) return;
            var nextBatch = AvailableWordPairs.Take(InitialWordCount).ToList();
            AvailableWordPairs.RemoveRange(0, nextBatch.Count);

            WordPairs.Clear();
            SerbianWords.Clear();
            SpanishWords.Clear();
            WordMap.Clear();

            foreach (var pair in nextBatch)
            {
                WordPairs.Add(pair);
                WordMap[pair.SerbianWord.Text] = pair;
                WordMap[pair.SpanishWord.Text] = pair;
            }

            var shuffledSerbian = nextBatch.Select(x => x.SerbianWord).OrderBy(_ => Guid.NewGuid()).ToList();
            var shuffledSpanish = nextBatch.Select(x => x.SpanishWord).OrderBy(_ => Guid.NewGuid()).ToList();

            foreach (var word in shuffledSerbian) SerbianWords.Add(word);
            foreach (var word in shuffledSpanish) SpanishWords.Add(word);
        }
        private void SelectWord(object obj)
        {
            if (!(obj is SelectableWord selected)) return;

            if (SerbianWords.Contains(selected)) _selectedSerbianWord = selected;
            else if (SpanishWords.Contains(selected)) _selectedSpanishWord = selected;

            if (_selectedSerbianWord != null && _selectedSpanishWord != null)
            {
                var pair = WordPairs.FirstOrDefault(p =>
                    p.SerbianWord == _selectedSerbianWord &&
                    p.SpanishWord == _selectedSpanishWord);

                if (pair != null)
                {
                    _selectedSerbianWord.IsCorrect = 1;
                    _selectedSpanishWord.IsCorrect = 1;
                    Points++;

                    Application.Current.Dispatcher.Invoke(async () =>
                    {
                        await Task.Delay(500);
                        WordPairs.Remove(pair);
                        SerbianWords.Remove(pair.SerbianWord);
                        SpanishWords.Remove(pair.SpanishWord);
                        if (!WordPairs.Any() && AvailableWordPairs.Any()) LoadNextBatch();
                    });
                }
                else
                {
                    _selectedSerbianWord.IsCorrect = 2;
                    _selectedSpanishWord.IsCorrect = 2;

                    var wrongSerbian = _selectedSerbianWord;
                    var wrongSpanish = _selectedSpanishWord;

                    Application.Current.Dispatcher.Invoke(async () =>
                    {
                        await Task.Delay(500);
                        wrongSerbian.IsCorrect = 0;
                        wrongSpanish.IsCorrect = 0;
                    });
                }

                _selectedSerbianWord = null;
                _selectedSpanishWord = null;
            }
        
        }

        private void TimerTick(object sender, EventArgs e)
        {
            
            
            if (TimeRemaining.TotalSeconds > 0 && WordPairs.Any())
            {
                    TimeRemaining = TimeRemaining.Subtract(TimeSpan.FromSeconds(1));
                    OnPropertyChanged(nameof(TimeRemaining));
                
            }
            else
            {
                string message = "";
               if(!WordPairs.Any())
            
                    message = "Čestitam! Pogodili ste sve kombinacije.";
                else
                    message = "Vrijeme isteklo!";
                _timer.Stop();
                _mainWindow.MainViewModel.Points += Points;

              //  string message = "Vrijeme isteklo!";
                GameoverWindow dialog2 = new GameoverWindow(Points, true, message);
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
