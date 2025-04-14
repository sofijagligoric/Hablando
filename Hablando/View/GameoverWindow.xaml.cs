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

        public GameoverWindow(int points, bool hasNextLevel, string message)
        {
            InitializeComponent();
            Points = points;
            DataContext = this;
            HasNextLevel = hasNextLevel;
            Message = message;
        }

        private void HomeButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void NextLevelClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
