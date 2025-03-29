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

        
        public ObservableCollection<WordPair> WordPairs { get; set; }
        public ObservableCollection<string> SerbianWords { get; set; }
        public ObservableCollection<string> SpanishWords { get; set; }
        private List<WordPair> AvailableWordPairs { get; set; }
        private const int InitialWordCount = 5;
        private string _selectedSerbianWord;
        private string _selectedSpanishWord;

        /*
        private WordPair _selectedSerbianWord;
        private WordPair _selectedSpanishWord;
        */


        /*

        public ObservableCollection<WordPair> WordPairs { get; set; }
        public ObservableCollection<WordPair> SerbianWords { get; set; }
        public ObservableCollection<WordPair> SpanishWords { get; set; }
        */


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

            
            TimeRemaining = TimeSpan.FromMinutes(2);
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += TimerTick;
            _timer.Start();

        }
        /*

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
        */
        /*
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
                    return new WordPair( parts[0],parts[1] );
                }).OrderBy(x => Guid.NewGuid()).ToList();

                WordPairs = new ObservableCollection<WordPair>(AvailableWordPairs.Take(InitialWordCount));
                AvailableWordPairs.RemoveRange(0, InitialWordCount);

                SerbianWords = new ObservableCollection<WordPair>(WordPairs.OrderBy(x => Guid.NewGuid()));
                SpanishWords = new ObservableCollection<WordPair>(WordPairs.OrderBy(x => Guid.NewGuid()));

                OnPropertyChanged(nameof(SerbianWords));
                OnPropertyChanged(nameof(SpanishWords));
            }
        }
        */
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
                }).OrderBy(x => Guid.NewGuid()).ToList();

                WordPairs = new ObservableCollection<WordPair>();
                SerbianWords = new ObservableCollection<string>();
                SpanishWords = new ObservableCollection<string>();

                LoadNextBatch();
            }
        }

        private void LoadNextBatch()
        {
            /*
            if (AvailableWordPairs.Count == 0) return;

            var nextBatch = AvailableWordPairs.Take(InitialWordCount).ToList();
            AvailableWordPairs.RemoveRange(0, nextBatch.Count);

            foreach (var pair in nextBatch)
            {
                WordPairs.Add(pair);
                SerbianWords.Add(pair.Serbian);
                SpanishWords.Add(pair.Spanish);
            }

            OnPropertyChanged(nameof(SerbianWords));
            OnPropertyChanged(nameof(SpanishWords));
            */

            if (AvailableWordPairs.Count == 0) return;

            var nextBatch = AvailableWordPairs.Take(InitialWordCount).ToList();
            AvailableWordPairs.RemoveRange(0, nextBatch.Count);

            foreach (var pair in nextBatch)
            {
                WordPairs.Add(pair); // VAŽNO: Dodajemo u WordPairs kako bi SelectWord radio
            }

            // Nasumično mešamo srpske i španske reči pre nego što ih dodamo
            var shuffledSerbian = WordPairs.Select(x => x.Serbian).OrderBy(x => Guid.NewGuid()).ToList();
            var shuffledSpanish = WordPairs.Select(x => x.Spanish).OrderBy(x => Guid.NewGuid()).ToList();

            SerbianWords.Clear();
            SpanishWords.Clear();

            foreach (var word in shuffledSerbian) SerbianWords.Add(word);
            foreach (var word in shuffledSpanish) SpanishWords.Add(word);

            OnPropertyChanged(nameof(SerbianWords));
            OnPropertyChanged(nameof(SpanishWords));
        }

        private void SelectWord(string word)
        {
            /*
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

                /*
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
                */

            /*

            if (matchingPair != null)
            {
                Points++;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    SerbianWords.Remove(matchingPair);
                    SpanishWords.Remove(matchingPair);
                });

                // Ako su pogođena 2 para, dodaj novi par
                if (AvailableWordPairs.Any())
                {
                    var newPair = AvailableWordPairs.First();
                    AvailableWordPairs.RemoveAt(0);

                    SerbianWords.Add(newPair);
                    SpanishWords.Add(newPair);
                }
            }
            */

            /*

                  if (matchingPair != null)
                  {
                      matchingPair.IsCorrect = 1; // Obeležavanje tačnih odgovora
                      Points++;

                      Application.Current.Dispatcher.Invoke(async () =>
                      {
                          await Task.Delay(1000); 

                          SerbianWords.Remove(matchingPair);
                          SpanishWords.Remove(matchingPair);

                          matchingPair.IsCorrect = 0;

                          // Dodavanje novog para ako ih još ima
                          if (AvailableWordPairs.Any())
                          {
                              var newPair = AvailableWordPairs.First();
                              AvailableWordPairs.RemoveAt(0);

                              SerbianWords.Add(newPair);
                              SpanishWords.Add(newPair);
                          }
                      });
                  }
                  else
                  {
                      // Pogrešan par - privremeno označi crvenom
                      var wrongSerbian = SerbianWords.FirstOrDefault(w => w.Serbian == _selectedSerbianWord);
                      var wrongSpanish = SpanishWords.FirstOrDefault(w => w.Spanish == _selectedSpanishWord);

                      if (wrongSerbian != null) wrongSerbian.IsCorrect = 2;
                      if (wrongSpanish != null) wrongSpanish.IsCorrect = 2;

                      Application.Current.Dispatcher.Invoke(async () =>
                      {
                          await Task.Delay(1000); 
                          if (wrongSerbian != null) wrongSerbian.IsCorrect = 0;
                          if (wrongSpanish != null) wrongSpanish.IsCorrect = 0;
                      });
                  }


                  _selectedSerbianWord = null;
                  _selectedSpanishWord = null;

              }

              */
            Debug.WriteLine($"-----> Kliknuto! {word}");

            if (SerbianWords.Contains(word))
            {
                _selectedSerbianWord = word;
            }
            else if (SpanishWords.Contains(word))
            {
                _selectedSpanishWord = word;
            }

            if (!string.IsNullOrEmpty(_selectedSerbianWord) && !string.IsNullOrEmpty(_selectedSpanishWord))
            {
                var matchingPair = WordPairs.FirstOrDefault(w => w.Serbian == _selectedSerbianWord && w.Spanish == _selectedSpanishWord);

                if (matchingPair != null)
                {
                    matchingPair.IsCorrect = 1;
                    Points++;

                    Application.Current.Dispatcher.Invoke(async () =>
                    {
                        await Task.Delay(1000);

                      //  SerbianWords.Remove(_selectedSerbianWord);
                       // SpanishWords.Remove(_selectedSpanishWord);

                        SerbianWords.Remove(matchingPair.Serbian);
                        SpanishWords.Remove(matchingPair.Spanish);

                        WordPairs.Remove(matchingPair);

                        
                        if (!WordPairs.Any())
                        {
                            
                            if (AvailableWordPairs.Any())
                            {
                                LoadNextBatch();
                            }
                        }
                    });
                }
                else
                {
                    // Ako je pogrešan izbor, označimo ga i poništimo izbor posle kratkog vremena
                    var wrongPair = WordPairs.FirstOrDefault(w => w.Serbian == _selectedSerbianWord || w.Spanish == _selectedSpanishWord);
                    if (wrongPair != null) wrongPair.IsCorrect = 2; // Pogrešan par (crvena boja)

                    Application.Current.Dispatcher.Invoke(async () =>
                    {
                        await Task.Delay(1000);
                        if (wrongPair != null) wrongPair.IsCorrect = 0; // Reset boje
                    });
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
