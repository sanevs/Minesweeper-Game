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
    /// Логика взаимодействия для OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        public OptionsWindow() => InitializeComponent();

        private void ChangeVisibility()
        {
            if (rbCustom is null)
                return;
            LabelWidth.IsEnabled = LabelHeight.IsEnabled = LabelMines.IsEnabled =
                TextBoxWidth.IsEnabled = TextBoxHeight.IsEnabled = TextBoxMines.IsEnabled = (bool)rbCustom.IsChecked;
        }

        private void RadioButtonCustom_Checked(object sender, RoutedEventArgs e) => ChangeVisibility();

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            int height, width, minesCount;

            if ((bool)rbEasy.IsChecked)
                Set(9, 9, 10, Difficulty.Easy);
            else if ((bool)rbMedium.IsChecked)
                Set(16, 16, 40, Difficulty.Medium);
            else if ((bool)rbHard.IsChecked)
                Set(30, 30, 99, Difficulty.Hard);
            else if ((bool)rbCustom.IsChecked)
            {
                if (!int.TryParse(TextBoxHeight.Text, out height) ||
                    !int.TryParse(TextBoxWidth.Text, out width) || 
                    !int.TryParse(TextBoxMines.Text, out minesCount))
                {
                    MessageBox.Show("Неправильный формат ввода", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Set(height, width, minesCount, Difficulty.Hard);
            }
            Close();
        }

        private void Set(int height, int width, int minesCount, Difficulty difficulty)
        {
            CellCollectionViewModel.CellsHeight = height;
            CellCollectionViewModel.CellsWidth = width;
            CellCollectionViewModel.MinesCount = minesCount;
            CellCollectionViewModel.CanOpenCells = height * width - minesCount;
            Game.Difficulty = difficulty;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}
