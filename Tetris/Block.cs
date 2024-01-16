using System.Collections.Generic;
using System;
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
    public abstract class Block
    {
        protected abstract Position[][] Tiles { get; }//двумерный массив позиций который содержит позиции плитки в четрех состояниях вращения
        protected abstract Position StartOffset { get; }// начальное смещение где блок появляется в сетке
        public abstract int Id { get; }// целочисленный идентификатор для различия блоков

        private int rotationState;//текущее состояние вращения
        private Position offset;//текущее смещение

        public Block()//устонавливаем сещение равное начальному смещению
        {
            offset = new Position(StartOffset.Row, StartOffset.Column);
        }

        public IEnumerable<Position> TilePositions()//  возвращает позиции сетки, занемаемые блоком с учетом текущего поворота и смещения
        {
            foreach (Position p in Tiles[rotationState])
            {
                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);
            }
        }

        public void RotateCW()// поворот блока на 90 градусов по часовой 
        {
            rotationState = (rotationState + 1) % Tiles.Length;
        }

        public void RotateCCW()// поворот блока против часовой
        {
            if (rotationState == 0)
            {
                rotationState = Tiles.Length - 1;
            }
            else
            {
                rotationState--;
            }
        }

        public void Move(int rows, int columns)// перемещение блока с заданым кол-вом строк и столбцов
        {
            offset.Row += rows;
            offset.Column += columns;
        }

        public void Reset()// сбрасывание вращения и положения
        {
            rotationState = 0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;
        }
    }
}
