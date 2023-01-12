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

namespace Minesweeper
{
    /// <summary>
    /// Логика взаимодействия для WinnerNameWindow.xaml
    /// </summary>
    public partial class WinnerNameWindow : Window
    {
        public Game Game { get; set; }

        public WinnerNameWindow(Game game)
        {
            InitializeComponent();
            Game = game;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Game.WinnerName = textBox.Text;
            Close();
        }
    }
}
