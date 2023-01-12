using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Minesweeper
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static DispatcherTimer Timer { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            StartTimer();
        }

    private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
                SetFlag(sender);
            if (e.MiddleButton == MouseButtonState.Pressed)
                ((CellCollectionViewModel)DataContext)
                    .OpenNearCells((CellViewModel)((Button)sender).DataContext);
        }

        private void SetFlag(object sender)
        {
            CellViewModel cell = (CellViewModel)((Button)sender).DataContext;
            if (cell.IsOpen)
                return;
            CellCollectionViewModel ccvm = ((CellCollectionViewModel)DataContext);
            if (cell.IsFlag == Visibility.Visible)
            {
                cell.IsFlag = Visibility.Hidden;
                ccvm.BombsCount++;
            }
            else if (cell.IsFlag == Visibility.Hidden)
            {
                cell.IsFlag = Visibility.Visible;
                ccvm.BombsCount--;
            }
            bombs.Text = ccvm.BombsCount.ToString();
        }

        public void StartTimer()
        {
            Timer = new DispatcherTimer();
            Timer.IsEnabled = true;
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        public void Timer_Tick(object sender, EventArgs e) => 
            ((CellCollectionViewModel)DataContext).Duration++;
    }
}
