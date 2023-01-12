using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Threading;
using System.Windows.Media;

namespace Minesweeper
{
    public class CellCollectionViewModel : BindableBase 
    {
        public static int CellsHeight { get; set; } = 9;
        public static int CellsWidth { get; set; } = 9;
        public static int MinesCount { get; set; } = 10;
        public static int CanOpenCells { get; set; } =
            CellsHeight * CellsWidth - MinesCount;

        private int[,] GameField = new int[CellsWidth, CellsHeight];
        private const int MaxNeighbors = 8;

        private Random Random { get; } = new Random();

        public CellCollectionViewModel()
        {
            Cells = Generate();
            BombsCount = MinesCount;
        }

        private IList<CellViewModel> Generate()
        {
            IList<CellViewModel> cells = new List<CellViewModel>();

            FillMines(cells);
            FillNearMines(cells);
            foreach (CellViewModel cell in cells.Where(c => c.Mines == 0))
                cell.Mines = null;
            PaintCorrespondingColor(cells);

            return cells;
        }

        private void FillMines(IList<CellViewModel> cells)
        {
            int currentMinesCount = MinesCount;
            for (int i = 0; i < CellsHeight; i++)
            {
                for (int j = 0; j < CellsWidth; j++)
                {
                    if (Random.Next(CellsHeight * CellsWidth / MinesCount) == 0 && currentMinesCount > 0)
                    {
                        cells.Add(new CellViewModel(isMine: Visibility.Visible));
                        currentMinesCount--;
                    }
                    else
                        cells.Add(new CellViewModel(isMine: Visibility.Hidden));
                }
                if (i + 1 == CellsWidth && currentMinesCount != 0)
                {
                    cells.Clear();
                    currentMinesCount = MinesCount;
                    i = -1;
                }
            }
        }

        private void FillNearMines(IList<CellViewModel> cells)
        {
            for (int i = 0; i < CellsHeight; i++)
                for (int j = 0; j < CellsWidth; j++)
                    if (cells[i * CellsWidth + j].IsMine == Visibility.Visible)
                        AddMine(cells, i, j);
        }

        private void AddMine (IList<CellViewModel> cells, int i, int j)
        {
            bool midI = 0 < i && i < CellsHeight - 1;
            bool midJ = 0 < j && j < CellsWidth - 1;
            bool lastI = i == CellsHeight - 1;
            bool lastJ = j == CellsWidth - 1;

            if (i == 0 && j == 0) // |``
            {
                cells[i * CellsWidth + (j + 1)].Mines++;
                cells[(i + 1) * CellsWidth + j].Mines++;
                cells[(i + 1) * CellsWidth + (j + 1)].Mines++;
            }
            else if (i == 0 && midJ) // `|`
            {
                cells[i * CellsWidth + (j - 1)].Mines++;
                cells[i * CellsWidth + (j + 1)].Mines++;
                cells[(i + 1) * CellsWidth + (j - 1)].Mines++;
                cells[(i + 1) * CellsWidth + j].Mines++;
                cells[(i + 1) * CellsWidth + (j + 1)].Mines++;
            }
            else if (i == 0 && lastJ) // ``|
            {
                cells[i * CellsWidth + (j - 1)].Mines++;
                cells[(i + 1) * CellsWidth + (j - 1)].Mines++;
                cells[(i + 1) * CellsWidth + j].Mines++;
            }
            else if (midI && j == 0) // |-
            {
                cells[(i - 1) * CellsWidth + j].Mines++;
                cells[(i - 1) * CellsWidth + (j + 1)].Mines++;
                cells[i * CellsWidth + (j + 1)].Mines++;
                cells[(i + 1) * CellsWidth + j].Mines++;
                cells[(i + 1) * CellsWidth + (j + 1)].Mines++;
            }
            else if (midI && midJ) // +
            {
                cells[(i - 1) * CellsWidth + (j - 1)].Mines++;
                cells[(i - 1) * CellsWidth + j].Mines++;
                cells[(i - 1) * CellsWidth + (j + 1)].Mines++;
                cells[i * CellsWidth + j - 1].Mines++;
                cells[i * CellsWidth + j + 1].Mines++;
                cells[(i + 1) * CellsWidth + (j - 1)].Mines++;
                cells[(i + 1) * CellsWidth + j].Mines++;
                cells[(i + 1) * CellsWidth + (j + 1)].Mines++;
            }
            else if (midI && lastJ) // -|
            {
                cells[(i - 1) * CellsWidth + (j - 1)].Mines++;
                cells[(i - 1) * CellsWidth + j].Mines++;
                cells[i * CellsWidth + (j - 1)].Mines++;
                cells[(i + 1) * CellsWidth + (j - 1)].Mines++;
                cells[(i + 1) * CellsWidth + j].Mines++;
            }
            else if (lastI && j == 0) // |__
            {
                cells[(i - 1) * CellsWidth + j].Mines++;
                cells[(i - 1) * CellsWidth + (j + 1)].Mines++;
                cells[i * CellsWidth + (j + 1)].Mines++;
            }
            else if (lastI && midJ) // _|_
            {
                cells[(i - 1) * CellsWidth + (j - 1)].Mines++;
                cells[(i - 1) * CellsWidth + j].Mines++;
                cells[(i - 1) * CellsWidth + (j + 1)].Mines++;
                cells[i * CellsWidth + (j - 1)].Mines++;
                cells[i * CellsWidth + (j + 1)].Mines++;
            }
            else if (lastI && lastJ) // __|
            {
                cells[(i - 1) * CellsWidth + (j - 1)].Mines++;
                cells[(i - 1) * CellsWidth + j].Mines++;
                cells[i * CellsWidth + (j - 1)].Mines++;
            }
        }

        private void PaintCorrespondingColor(IList<CellViewModel> cells)
        {
            SolidColorBrush[] brushes = {
                Brushes.Violet, Brushes.Blue, Brushes.Red, Brushes.Green,
                Brushes.Yellow, Brushes.Brown, Brushes.Orange, Brushes.Coral };

            for (int i = 1; i <= MaxNeighbors; i++)
                foreach (CellViewModel cell in cells.Where(c => c.Mines == i))
                    cell.MinesColor = brushes[i - 1];
        }

        private IList<CellViewModel> cells;
        public IList<CellViewModel> Cells
        {
            get => cells;
            set
            {
                if (cells == value)
                    return;
                SetProperty(ref cells, value);
            }
        }

        public void OpenNearCells(CellViewModel currentCell)
        {
            for (int i = 0; i < CellsHeight; i++)
                for (int j = 0; j < CellsWidth; j++)
                    if (cells[i * CellsWidth + j] == currentCell)
                        OpenNear(cells, i, j);
        }

        private void OpenNear(IList<CellViewModel> cells, int i, int j)
        {
            bool midI = 0 < i && i < CellsHeight - 1;
            bool midJ = 0 < j && j < CellsWidth - 1;
            bool lastI = i == CellsHeight - 1;
            bool lastJ = j == CellsWidth - 1;

            cells[i * CellsWidth + j].OnOpen.Execute();
            if (i == 0 && j == 0) // |``
            {
                cells[i * CellsWidth + j + 1].OnOpen.Execute();
                cells[(i + 1) * CellsWidth + j].OnOpen.Execute();
                cells[(i + 1) * CellsWidth + (j + 1)].OnOpen.Execute();
            }
            else if (i == 0 && midJ) // `|`
            {
                cells[i * CellsWidth + j - 1].OnOpen.Execute();
                cells[i * CellsWidth + j + 1].OnOpen.Execute();
                cells[(i + 1) * CellsWidth + (j - 1)].OnOpen.Execute();
                cells[(i + 1) * CellsWidth + j].OnOpen.Execute();
                cells[(i + 1) * CellsWidth + (j + 1)].OnOpen.Execute();
            }
            else if (i == 0 && lastJ) // ``|
            {
                cells[i * CellsWidth + j - 1].OnOpen.Execute();
                cells[(i + 1) * CellsWidth + (j - 1)].OnOpen.Execute();
                cells[(i + 1) * CellsWidth + j].OnOpen.Execute();
            }
            else if (midI && j == 0) // |-
            {
                cells[(i - 1) * CellsWidth + j].OnOpen.Execute();
                cells[(i - 1) * CellsWidth + (j + 1)].OnOpen.Execute();
                cells[i * CellsWidth + j + 1].OnOpen.Execute();
                cells[(i + 1) * CellsWidth + j].OnOpen.Execute();
                cells[(i + 1) * CellsWidth + (j + 1)].OnOpen.Execute();
            }   
            else if (midI && midJ) // +
            {
                cells[(i - 1) * CellsWidth + (j - 1)].OnOpen.Execute();
                cells[(i - 1) * CellsWidth + j].OnOpen.Execute();
                cells[(i - 1) * CellsWidth + (j + 1)].OnOpen.Execute();
                cells[i * CellsWidth + j - 1].OnOpen.Execute();
                cells[i * CellsWidth + j + 1].OnOpen.Execute();
                cells[(i + 1) * CellsWidth + (j - 1)].OnOpen.Execute();
                cells[(i + 1) * CellsWidth + j].OnOpen.Execute();
                cells[(i + 1) * CellsWidth + (j + 1)].OnOpen.Execute();
            }
            else if (midI && lastJ) // -|
            {
                cells[(i - 1) * CellsWidth + (j - 1)].OnOpen.Execute();
                cells[(i - 1) * CellsWidth + j].OnOpen.Execute();
                cells[i * CellsWidth + j - 1].OnOpen.Execute();
                cells[(i + 1) * CellsWidth + (j - 1)].OnOpen.Execute();
                cells[(i + 1) * CellsWidth + j].OnOpen.Execute();
            }
            else if (lastI && j == 0) // |__
            {
                cells[(i - 1) * CellsWidth + j].OnOpen.Execute();
                cells[(i - 1) * CellsWidth + (j + 1)].OnOpen.Execute();
                cells[i * CellsWidth + j + 1].OnOpen.Execute();
            }
            else if (lastI && midJ) // _|_
            {
                cells[(i - 1) * CellsWidth + (j - 1)].OnOpen.Execute();
                cells[(i - 1) * CellsWidth + j].OnOpen.Execute();
                cells[(i - 1) * CellsWidth + (j + 1)].OnOpen.Execute();
                cells[i * CellsWidth + j - 1].OnOpen.Execute();
                cells[i * CellsWidth + j + 1].OnOpen.Execute();
            }
            else if (lastI && lastJ) // __|
            {
                cells[(i - 1) * CellsWidth + (j - 1)].OnOpen.Execute();
                cells[(i - 1) * CellsWidth + j].OnOpen.Execute();
                cells[i * CellsWidth + j - 1].OnOpen.Execute();
            }
            if (Game.IsEnd)
            {
                CellViewModel.OpenCells = 0;
                Game.IsEnd = false;
            }
        }

        private int bombsCount;
        public int BombsCount
        {
            get => bombsCount;
            set
            {
                if (bombsCount == value)
                    return;
                SetProperty(ref bombsCount, value);
            }
        }

        private int duration;
        public int Duration
        {
            get => duration;
            set
            {
                if (duration == value)
                    return;
                SetProperty(ref duration, value);
            }
        }
    }
}
