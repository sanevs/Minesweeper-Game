using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Threading;
using System.Windows.Media;

namespace Minesweeper
{
    public class CellViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CellModel CellModel { get; set; } = new CellModel();
        public Game Game { get; set; } = new Game();

        public DelegateCommand OnOpen { get; set; }

        public static int OpenCells { get; set; } 

        public CellViewModel(Visibility isMine)
        {
            IsFlag = Visibility.Hidden;
            IsMine = isMine;
            Mines = 0;

            OnOpen = new DelegateCommand(() => Open());
        }

        private Visibility isFlag;
        public Visibility IsFlag
        {
            get => isFlag;
            set
            {
                if (isFlag == value)
                    return;
                isFlag = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFlag)));
                CellModel.IsFlag = isFlag;
            }
        }

        private bool isOpen;
        public bool IsOpen
        {
            get => isOpen;
            set
            {
                if (isOpen == value)
                    return;
                isOpen = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsOpen)));
                CellModel.IsOpen = isOpen;
            }
        }

        private Visibility isMine;
        public Visibility IsMine
        {
            get => isMine;

            set
            {
                if (isMine == value)
                    return;
                isMine = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMine)));
                CellModel.IsMine = isMine;
            }
        }

        private byte? mines;
        public byte? Mines
        {
            get => mines;
            set
            {
                if (mines == value)
                    return;
                mines = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mines)));
                CellModel.Mines = mines;
            }
        }

        private Brush minesColor;
        public Brush MinesColor
        {
            get => minesColor;
            set
            {
                if (minesColor == value)
                    return;
                minesColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MinesColor)));
                CellModel.MinesColor = minesColor;
            }
        }

        private void Open()
        {
            if (IsFlag == Visibility.Visible)
                return;
            if (IsOpen)
                return;
            IsOpen = true;
            if(IsMine == Visibility.Visible)
            {
                Game.End("Вы проиграли!", MessageBoxImage.Error);
                return;
            }
            OpenCells++;
            if (OpenCells == CellCollectionViewModel.CanOpenCells)
                Game.End("Вы выиграли!", MessageBoxImage.Information);
        }

        private SolidColorBrush skin = Brushes.Beige;
        public SolidColorBrush Skin
        {
            get => skin;
            set
            {
                if (skin == value)
                    return;
                skin = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Skin)));
            }
        }
    }
}
