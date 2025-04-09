using GalaSoft.MvvmLight;
using Hablando.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using Hablando.View;
using System.Windows.Input;
using Hablando.Util;

namespace Hablando.ViewModel
{
    public class Level2ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private MainWindow _mainWindow;


        public QuizItem CurrentQuizItem { get; set; }
        private List<WordPair> AvailableWordPairs { get; set; }
        public ICommand AnswerCommand { get; }
        public ICommand RestartCommand { get; }
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

        public Level2ViewModel(MainWindow mainWindow)
        {
           
            Points = 0;
            _mainWindow = mainWindow;
            var dictionary = Application.Current.Resources.MergedDictionaries
                            .FirstOrDefault(d => d.Contains("SerbianSpanishDictionary"));
            AnswerCommand = new RelayCommand<SelectableWord>(CheckAnswer);
            RestartCommand = new RelayCommandWithoutParameters(RestartGame);
            LoadWords();


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

                GenerateNewQuizItem();
            }
        }

        /*
        private void GenerateNewQuizItem()
        {
            if (AvailableWordPairs.Count < 3) return;

            var correctPair = AvailableWordPairs[0];
            AvailableWordPairs.RemoveAt(0);

            var incorrectOptions = AvailableWordPairs
                .OrderBy(_ => Guid.NewGuid())
                .Take(2)
                .Select(p => p.SpanishWord.Text)
                .ToList();

            var allOptions = incorrectOptions.Append(correctPair.SpanishWord.Text)
                                             .OrderBy(_ => Guid.NewGuid())
                                             .ToList();

            CurrentQuizItem = new QuizItem(correctPair.SerbianWord.Text,correctPair.SpanishWord.Text, allOptions);

            OnPropertyChanged(nameof(CurrentQuizItem));
        }*/

        private void GenerateNewQuizItem()
        {
            if (AvailableWordPairs.Count < 3)
            {
                string message = "Kraj igre!";
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
                return;
            }

            var correctPair = AvailableWordPairs[0];
            AvailableWordPairs.RemoveAt(0);

            var incorrectOptions = AvailableWordPairs
                .OrderBy(_ => Guid.NewGuid())
                .Take(2)
                .Select(p => p.SpanishWord.Text)
                .ToList();

            var allOptions = incorrectOptions.Append(correctPair.SpanishWord.Text)
                                             .OrderBy(_ => Guid.NewGuid())
                                             .ToList();

            CurrentQuizItem = new QuizItem(correctPair.SerbianWord.Text, correctPair.SpanishWord.Text, allOptions);
            OnPropertyChanged(nameof(CurrentQuizItem));
        }

      
        private void CheckAnswer(SelectableWord selected)
        {
            if (selected == null) return;

            CurrentQuizItem.SelectedAnswer = selected;

            if (CurrentQuizItem.IsCorrect == true)
            {
                selected.IsCorrect = 1;
                Points++;
            }
            else
            {
                selected.IsCorrect = 2;
            }

            Task.Delay(500).ContinueWith(_ =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    GenerateNewQuizItem();
                });
            });
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
                string message = "";
             /*   if (!WordPairs.Any())

                    message = "Čestitam! Pogodili ste sve kombinacije.";
                else
             */
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
