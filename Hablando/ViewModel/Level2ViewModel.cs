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

namespace Hablando.ViewModel
{
    public class Level2ViewModel : ViewModelBase, INotifyPropertyChanged
    {
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

        public Level2ViewModel()
        {
            // Učitavanje reči iz ResourceDictionary
            Points = 0;
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

            Debug.WriteLine("-----------> Timer startovan.");

        }

        private void TimerTick(object sender, EventArgs e)
        {
            Debug.WriteLine($"Preostalo vreme: {TimeRemaining.TotalSeconds} sekundi");
            if (TimeRemaining.TotalSeconds > 0)
            {
                TimeRemaining = TimeRemaining.Subtract(TimeSpan.FromSeconds(1));
                OnPropertyChanged(nameof(TimeRemaining));
            }
            else
            {
                _timer.Stop();
                MessageBox.Show($"Vreme je isteklo!\nOsvojeni bodovi: {Points}", "Kraj igre", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void StopTimer()
        {
            if (_timer != null && _timer.IsEnabled)
            {
                _timer.Stop();
                Debug.WriteLine("-----------> Timer zaustavljen.");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
