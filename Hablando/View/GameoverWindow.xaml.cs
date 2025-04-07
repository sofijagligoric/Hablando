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
    /// Interaction logic for GameoverWindow.xaml
    /// </summary>
    public partial class GameoverWindow : Window
    {

         public int Points { get; set; }
        public bool HasNextLevel { get; }
        public string Message { get; set; }
        // private MainWindow _mainWindow;

        public GameoverWindow(int points, bool hasNextLevel, string message)
        {
            InitializeComponent();
            Points = points;
            DataContext = this;
            HasNextLevel = hasNextLevel;
            Message = message;
           // _mainWindow = mainWindow;
        }

        private void HomeButtonClicked(object sender, RoutedEventArgs e)
        {
            // _mainWindow.MainFrame.Content = _mainWindow.StartPage;
            DialogResult = false;
            Close();
        }

        private void NextLevelClicked(object sender, RoutedEventArgs e)
        {
            //_mainWindow.MainFrame.Content = new LevelTwoPage(_mainWindow);
            DialogResult = true;
            Close();
        }
    }
}
