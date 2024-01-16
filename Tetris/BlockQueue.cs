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
    public class BlockQueue// класс очереди блоков, отвечающий за выбор следующего блока
    {
        private readonly Block[] blocks = new Block[]// массив содержащий 7 классов каждого блока
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()
        };

        private readonly Random random = new Random();//случайный обьект

        public Block NextBlock { get; private set; }//свойство для следующего блока в очереди

        public BlockQueue()//инициализируем следующий блок случайным блоком
        {
            NextBlock = RandomBlock();
        }

        private Block RandomBlock()// возвращаем случайный блок
        {
            return blocks[random.Next(blocks.Length)];
        }

        public Block GetAndUpdate()//возвращает следующий блок и обновляет свойство
        {
            Block block = NextBlock;

            do
            {
                NextBlock = RandomBlock();
            }
            while (block.Id == NextBlock.Id);

            return block;
        }
    }
}
