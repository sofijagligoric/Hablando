using Hablando.Util;
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
using System.Windows.Shapes;

namespace Hablando.View
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        private readonly NavigationService _navigationService;

        public StartPage(MainViewModel viewModel, NavigationService navigationService)
        {
            InitializeComponent();
            DataContext = viewModel;
            _navigationService = navigationService;
        }

        private void ImeTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ImeTextBox.Text == "Unesite Vaše ime...")
            {
                ImeTextBox.Text = "";
                ImeTextBox.Foreground = Brushes.Black;
            }
        }

        private void ImeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ImeTextBox.Text))
            {
                ImeTextBox.Text = "Unesite Vaše ime...";
                ImeTextBox.Foreground = Brushes.Gray;
            }
        }

        private void Level1ButtonClick(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateToLevelOne();
        }

        private void Level2ButtonClick(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateToLevelTwo();
        }

        private void Level3ButtonClick(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateToLevelThree();
        }
    }
}
