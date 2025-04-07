using GalaSoft.MvvmLight;
using Hablando.Model;
using Hablando.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Hablando.ViewModel
{
    internal class Level3ViewModel : ViewModelBase, INotifyPropertyChanged
    {

        private MainWindow _mainWindow;


        public ObservableCollection<WordPair> SRSPDictionary { get; set; }
        public ObservableCollection<string> SerbianWords { get; set; }
        public ObservableCollection<string> SpanishWords { get; set; }
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
            // Učitavanje reči iz ResourceDictionary
            Points = 0;
            _mainWindow = mainWindow;
            var dictionary = Application.Current.Resources.MergedDictionaries
                            .FirstOrDefault(d => d.Contains("SerbianSpanishDictionary"));

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

        private void TimerTick(object sender, EventArgs e)
        {

            Points += 1;
            if (TimeRemaining.TotalSeconds > 0)
            {
                TimeRemaining = TimeRemaining.Subtract(TimeSpan.FromSeconds(1));
                OnPropertyChanged(nameof(TimeRemaining));
            }
            else
            {
                _timer.Stop();
                _mainWindow.MainViewModel.Points += Points;
                string message = "Vrijeme isteklo!";
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
