using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Minesweeper
{
    public enum Difficulty { Easy, Medium, Hard }

    public class Game
    {
        public string WinnerName { get; set; }

        public DelegateCommand OnNew { get; set; }
        public DelegateCommand OnExit { get; set; }
        public DelegateCommand OnAbout { get; set; }
        public DelegateCommand OnOptions { get; set; }
        public DelegateCommand OnStatistics { get; set; }
        public DelegateCommand OnChangeSkin { get; set; }

        public static bool IsEnd { get; set; }
        public static Difficulty Difficulty { get; set; } = Difficulty.Easy;
        public static IList<Player> Players { get; set; } = Player.FromXML();

        private Random Random { get; set; } = new Random();
        private SolidColorBrush[] Skins { get; set; } =
            new SolidColorBrush[]
            {
                Brushes.Black, Brushes.Blue, Brushes.Brown, Brushes.Chocolate, 
                Brushes.DarkGray, Brushes.DarkGreen, Brushes.HotPink,
                Brushes.DarkViolet, Brushes.Purple, Brushes.Thistle
            };

        public Game()
        {
            OnNew = new DelegateCommand(() => New());
            OnExit = new DelegateCommand(() => Application.Current.Shutdown());
            OnAbout = new DelegateCommand(() => new AboutWindow().ShowDialog());
            OnOptions = new DelegateCommand(() => SetOptions());
            OnStatistics = new DelegateCommand(() => new StatisticsWindow().ShowDialog());
            OnChangeSkin = new DelegateCommand(() => ChangeSkin());
        }

        private void ChangeSkin()
        {
            SolidColorBrush skin = Skins[Random.Next(Skins.Length)];
            foreach (CellViewModel cell in ((CellCollectionViewModel)Application.Current.MainWindow.DataContext).Cells)
                cell.Skin = skin;
        }

        public void End(string message, MessageBoxImage mboxImage)
        {
            CellViewModel.OpenCells = 0;
            IEnumerable<CellViewModel> mineCells =
                ((CellCollectionViewModel)((MainWindow)Application.Current.MainWindow).DataContext)
                    .Cells
                    .Where(c => c.IsMine == Visibility.Visible);

            foreach (CellViewModel cell in mineCells)
                cell.IsOpen = true;
            MainWindow.Timer.Stop();
            if(message != "")
                MessageBox.Show(message, "Информация", MessageBoxButton.OK, mboxImage);
            if(message == "Вы выиграли!")
                RefreshScores((CellCollectionViewModel)((MainWindow)Application.Current.MainWindow).DataContext);
            ((MainWindow)Application.Current.MainWindow).DataContext = new CellCollectionViewModel();
            MainWindow.Timer.Start();
            IsEnd = true;
        }

        private void RefreshScores(CellCollectionViewModel cellCollectionVM)
        {
            if (cellCollectionVM.Duration < Players[(int)Difficulty].Duration)
            {
                new WinnerNameWindow(this).ShowDialog();
                Players[(int)Difficulty].Name = WinnerName;
                Players[(int)Difficulty].Duration = cellCollectionVM.Duration;
                Players[(int)Difficulty].DateTime = DateTime.Now;
                Player.ToXML(Players);
            }
        }

        private void New() => End("", MessageBoxImage.None);

        private void SetOptions()
        {
            OptionsWindow optionsWindow = new OptionsWindow();
            optionsWindow.ShowDialog();
            New();
        }
    }
}
