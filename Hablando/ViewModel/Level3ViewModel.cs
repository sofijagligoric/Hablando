using GalaSoft.MvvmLight;
using Hablando.Model;
using Hablando.Util;
using Hablando.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Hablando.ViewModel
{
    public class Level3ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private MainWindow _mainWindow;


        public QuizItem CurrentQuizItem { get; set; }
        private List<WordPair> AvailableWordPairs { get; set; }
        private WordPair _currentWordPair;
        private string _userInput;
        public WordPair CurrentWordPair
        {
            get => _currentWordPair;
            set
            {
                _currentWordPair = value;
                OnPropertyChanged(nameof(CurrentWordPair));
            }
        }

        public string UserInput
        {
            get => _userInput;
            set
            {
                _userInput = value;
                OnPropertyChanged(nameof(UserInput));
            }
        }
        public ICommand RestartCommand { get; }
        public ICommand CheckAnswerCommand { get; }
        public ICommand NextWordCommand { get; }

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

        public Level3ViewModel(MainWindow mainWindow)
        {

            Points = 0;
            _mainWindow = mainWindow;
            CheckAnswerCommand = new RelayCommandWithoutParameters(CheckAnswer);
            NextWordCommand = new RelayCommandWithoutParameters(NextWord);
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

                NextWord();
            }
        }



        private void NextWord()
        {
            UserInput = string.Empty;

            if (AvailableWordPairs.Count == 0)
            {
                string message = "Kraj igre!";
                _timer.Stop();
                _mainWindow.MainViewModel.Points += Points;


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

            CurrentWordPair = AvailableWordPairs[0];
            AvailableWordPairs.RemoveAt(0);
        }

        private void CheckAnswer()
        {
            if (string.IsNullOrWhiteSpace(UserInput)) return;

            if (string.Equals(UserInput.Trim(), CurrentWordPair.SpanishWord.Text.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                CurrentWordPair.SpanishWord.IsCorrect = 1;
                Points++;
                Application.Current.Dispatcher.Invoke(async () =>
                {
                    await Task.Delay(800);
                    AvailableWordPairs.Remove(CurrentWordPair);
                    NextWord();
                });
               
            }
            else
            {
                CurrentWordPair.SpanishWord.IsCorrect = 2;
                Application.Current.Dispatcher.Invoke(async () =>
                {
                    await Task.Delay(800);
                    CurrentWordPair.SpanishWord.IsCorrect = 0;
                });
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
                string message = "";
                message = "Vrijeme isteklo!";
                _timer.Stop();
                _mainWindow.MainViewModel.Points += Points;


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
