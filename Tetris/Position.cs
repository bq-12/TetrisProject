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
using System.Windows.Navigation;
using System.Windows.Shapes;    
namespace Tetris
{
    public class Position//класс хранит в себе строку и столбец
    {
        public int Row { get; set; }
        public int Column { get; set; }
        //задаем простой конструктор
        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
