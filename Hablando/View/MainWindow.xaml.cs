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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel MainViewModel { get; }
        private readonly NavigationService _navigationService;
        private readonly StartPage _startPage;

        public MainWindow()
        {
            InitializeComponent();

            MainViewModel = new MainViewModel();
            _startPage = new StartPage(MainViewModel);
            _navigationService = new NavigationService(MainFrame, _startPage, MainViewModel);

            DataContext = MainViewModel;
            MainFrame.Content = _startPage;
        }

    }
}
