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
    public class OBlock : Block//квадратный блок
    {
        private readonly Position[][] tiles = new Position[][]//предостовляем одно состояние вращения
        {
            new Position[] { new(0,0), new(0,1), new(1,0), new(1,1) }
        };

        public override int Id => 4;
        protected override Position StartOffset => new Position(0, 4);
        protected override Position[][] Tiles => tiles;
    }
}
