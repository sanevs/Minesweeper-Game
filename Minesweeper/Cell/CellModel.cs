using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Minesweeper
{
    public class CellModel 
    {
        public Visibility IsFlag { get; set; } = Visibility.Hidden;
        public bool IsOpen { get; set; }
        public Visibility IsMine { get; set; }
        public byte? Mines { get; set; }
        public Brush MinesColor { get; set; }
    }
}
