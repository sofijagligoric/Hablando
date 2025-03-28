using Hablando.View;
using Hablando.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Hablando.Util
{
    public class NavigationService
    {
        private readonly Frame _mainFrame;
        private readonly StartPage _startPage;
        private readonly MainViewModel _mainViewModel;

        public NavigationService(Frame mainFrame, StartPage startPage, MainViewModel mainViewModel)
        {
            _mainFrame = mainFrame;
            _startPage = startPage;
            _mainViewModel = mainViewModel;
        }

        public void NavigateToStartPage()
        {
            _mainFrame.Content = _startPage;
        }

        public void NavigateToLevelOne()
        {
            _mainFrame.Content = new LevelOnePage(this, _mainViewModel);
        }

        public void NavigateToLevelTwo()
        {
            _mainFrame.Content = new LevelTwoPage(this, _mainViewModel);
        }

        public void NavigateToLevelThree()
        {
            _mainFrame.Content = new LevelThreePage(this, _mainViewModel);
        }


        public void NavigateToNextLevel(Page currentLevel)
        {
            if (currentLevel is LevelOnePage)
                _mainFrame.Content = new LevelTwoPage(this, _mainViewModel); // Dodati parametri
            else if (currentLevel is LevelTwoPage)
                _mainFrame.Content = new LevelThreePage(this, _mainViewModel);
        }
    }
}
