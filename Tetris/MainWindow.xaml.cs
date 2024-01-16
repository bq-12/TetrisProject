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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]// массив содержащий изображения плиток
        {
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative))
        };

        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative))
        };

        private readonly Image[,] imageControls;// массив элементов управления изображением
        private readonly int maxDelay = 1000;
        private readonly int minDelay = 75;
        private readonly int delayDecrease = 25;

        private GameState gameState = new GameState();// обьект состояния игры

        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.GameGrid);// вызываем массив элементов управления изображением
        }

        private Image[,] SetupGameCanvas(GameGrid grid)// метод для правельной настроки элементов управления изображением
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image//Для каждой позиции мы создаем новый элемент управления изображением с ишриной и высотой 25 пикселей
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }

            return imageControls;
            // теперь есть двумерный массив с одним изоброжением для каждой ячейки
        }

        private void DrawGrid(GameGrid grid)// рисуем игровую сетку
        {// прокручиваем все позиции
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];// для каждой позиции получаем начальный идентификатор
                    imageControls[r, c].Opacity = 1;
                    imageControls[r, c].Source = tileImages[id];// устонавливаем источник изображения в этой позиции используя идентификатор
                }
            }
        }

        private void DrawBlock(Block block)// рисование текущего блока
        {
            foreach (Position p in block.TilePositions())
            {// перебрать позиции плитки и обновить источники изображения
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
            }
        }

        private void DrawNextBlock(BlockQueue blockQueue)// просмотр след.блока
        {
            Block next = blockQueue.NextBlock;
            NextImage.Source = blockImages[next.Id];
        }

        

        private void DrawGhostBlock(Block block)// призрачный блок показывающий где упадет текущий блок
        {
            int dropDistance = gameState.BlockDropDistance();

            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
            }
        }

        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);// ресуем сетку
            DrawGhostBlock(gameState.CurrentBlock);// ресуем  текущий блок
            DrawBlock(gameState.CurrentBlock);//рисуем призрачный блок
            DrawNextBlock(gameState.BlockQueue);//рисуем след.блок
             
            ScoreText.Text = $"Score: {gameState.Score}";//текст очков
        }

        private async Task GameLoop()// игровой цикл
        {
            Draw(gameState);

            while (!gameState.GameOver)
            {
                int delay = Math.Max(minDelay, maxDelay - (gameState.Score * delayDecrease));
                await Task.Delay(delay);
                gameState.MoveBlockDown();
                Draw(gameState);
            }

            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Score: {gameState.Score}";// окнчательный счет очков
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)// обрабатываем нажатия клавиш
        {
            if (gameState.GameOver)// не реагирование клавиш при завершении игры
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    gameState.MoveBlockLeft();// движение блока влево
                    break;
                case Key.Right:
                    gameState.MoveBlockRight();//движение блока вправо
                    break;
                case Key.Down:
                    gameState.MoveBlockDown();// ускорение падения блока
                    break;
                case Key.Up:
                    gameState.RotateBlockCW();//поворот блока по часовой
                    break;
                case Key.Z:
                    gameState.RotateBlockCCW();//поврот блока проив часовой
                    break;
                case Key.C:
                    gameState.HoldBlock();//удержание блока
                    break;
                case Key.Space:
                    gameState.DropBlock();//сброс блока вниз
                    break;
                default:
                    return;
            }

            Draw(gameState);
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {//скроем меню игры и перезапустим цикл игры
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }
    }
}
