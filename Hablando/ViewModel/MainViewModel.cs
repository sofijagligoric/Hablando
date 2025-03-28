using GalaSoft.MvvmLight.Command;
using Hablando.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Hablando.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _player;
        private int _points;
        private bool _isInputVisible = true;
        private bool _isGameVisible = false;
        private readonly NavigationService _navigationService;

        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateNextLevelCommand { get; }

        public string Player
        {
            get => _player;
            set { _player = value; OnPropertyChanged(nameof(Player)); }
        }

        public int Points
        {
            get => _points;
            set { _points = value; OnPropertyChanged(nameof(Points)); }
        }

        public bool IsInputVisible
        {
            get => _isInputVisible;
            set { _isInputVisible = value; OnPropertyChanged(nameof(IsInputVisible)); }
        }

        public bool IsGameVisible
        {
            get => _isGameVisible;
            set { _isGameVisible = value; OnPropertyChanged(nameof(IsGameVisible)); }
        }

        public ICommand SavePlayerCommand { get; }
        public ICommand NewPlayerCommand { get; }

        public MainViewModel()
        {
            SavePlayerCommand = new GalaSoft.MvvmLight.Command.RelayCommand(SavePlayer);
            NewPlayerCommand = new GalaSoft.MvvmLight.Command.RelayCommand(NewPlayer);
            Points = 0;
            Player = string.Empty;
        }

        private void SavePlayer()
        {
            if (!string.IsNullOrWhiteSpace(Player))
            {
                IsInputVisible = false;
                IsGameVisible = true;
                Points = 0;
            }
        }

        private void NewPlayer()
        {
            IsInputVisible = true;
            IsGameVisible = false;
            Player = string.Empty;
            Points = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
