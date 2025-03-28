using Hablando.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hablando.View
{
    /// <summary>
    /// Interaction logic for LevelTwoPage.xaml
    /// </summary>
    public partial class LevelTwoPage : Page
    {
        private MainWindow _mainWindow;

        public LevelTwoPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            DataContext = new Level2ViewModel(_mainWindow);
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is Level2ViewModel viewModel)
            {
                viewModel.StopTimer();
                _mainWindow.MainViewModel.Points += viewModel.Points;
            }
            _mainWindow.MainFrame.Content = _mainWindow.StartPage;
        }


    }
}
