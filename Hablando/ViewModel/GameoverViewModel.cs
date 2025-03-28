using Hablando.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hablando.ViewModel
{
    public class GameoverViewModel
    {
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateNextLevelCommand { get; }

        public bool HasNextLevel { get; }

        public GameoverViewModel(NavigationService navigationService, Page currentPage, bool hasNextLevel)
        {
            HasNextLevel = hasNextLevel;

            NavigateHomeCommand = new RelayCommand(_ => navigationService.NavigateToStartPage());
            NavigateNextLevelCommand = new RelayCommand(_ =>
            {
                if (HasNextLevel)
                    navigationService.NavigateToNextLevel(currentPage);
            });
        }
    }
}
