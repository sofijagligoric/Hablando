using GalaSoft.MvvmLight;
using Hablando.Model;
using Hablando.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Hablando.ViewModel
{
    internal class Level3ViewModel : ViewModelBase, INotifyPropertyChanged
    {

        private readonly NavigationService _navigationService;
        private readonly MainViewModel _mainViewModel;

        public ObservableCollection<WordPair> SRSPDictionary { get; set; }
        public ObservableCollection<string> SerbianWords { get; set; }
        public ObservableCollection<string> SpanishWords { get; set; }

        private int _points;
        public int Points
        {
            get => _points;
            set { _points = value; OnPropertyChanged(nameof(Points)); }
        }

        private TimeSpan _timeRemaining;
        public TimeSpan TimeRemaining
        {
            get => _timeRemaining;
            set { _timeRemaining = value; OnPropertyChanged(nameof(TimeRemaining)); }
        }

        private readonly DispatcherTimer _timer;
        public ICommand CancelCommand { get; }

        public Level3ViewModel(NavigationService navigationService, MainViewModel mainViewModel)
        {
            _navigationService = navigationService;
            _mainViewModel = mainViewModel;

            Points = 0;
            TimeRemaining = TimeSpan.FromMinutes(2);
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += TimerTick;
            _timer.Start();

            CancelCommand = new RelayCommand(_ => CancelGame());
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (TimeRemaining.TotalSeconds > 0)
            {
                TimeRemaining = TimeRemaining.Subtract(TimeSpan.FromSeconds(1));
            }
            else
            {
                _timer.Stop();
                // MessageBox.Show($"Vreme je isteklo!\nOsvojeni bodovi: {Points}", "Kraj igre", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void StopTimer()
        {
            if (_timer.IsEnabled)
            {
                _timer.Stop();
            }
        }

        private void CancelGame()
        {
            StopTimer();
            _mainViewModel.Points += Points;
            _navigationService.NavigateToStartPage();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
