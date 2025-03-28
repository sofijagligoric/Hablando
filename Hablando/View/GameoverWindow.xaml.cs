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
    /// Interaction logic for GameoverWindow.xaml
    /// </summary>
    public partial class GameoverWindow : Window
    {
        public GameoverWindow(NavigationService navigationService, Page currentPage, bool hasNextLevel)
        {
            InitializeComponent();
            DataContext = new GameoverViewModel(navigationService, currentPage, hasNextLevel);
        }
    }
}
