using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Reversi;
using WP7.Reversers.Core;

namespace WP7.Reversers
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            String navigationMessage;
            _gameMode = NavigationContext.QueryString[Consts.PARAM_WHO_PLAY] == Consts.USER_VS_USER ? 0 : 1;
            
            NewGame();
        }

        int _gameMode = 1;
        bool _fastAi = false;

        void NewGameClick(object sender, RoutedEventArgs e)
        {
            NewGamePrompt();
        }

        GamePlayer _black, _white;
        BoardCell[,] _cells;

        void NewGamePrompt()
        {
            _gameMode = NavigationContext.QueryString[Consts.PARAM_WHO_PLAY] == Consts.USER_VS_USER ? 0 : 1;
            DataContext = _gameMode;
            NewGame();
        }

        void NewGame()
        {
            CloseGame();

            _cells = new BoardCell[8, 8];

            switch (_gameMode)
            {
                case 0://pc vs pc
                    _white = new GamePC();
                    _black = new GamePC();
                    break;

                case 1://pc vs ai
                    _white = new GamePC();
                    _black = new GameAI();
                    break;

                case 2://ai vs pc
                    _white = new GameAI();
                    _black = new GamePC();
                    break;

                case 3://ai vs ai
                    _white = new GameAI();
                    _black = new GameAI();
                    break;
            }

            Game = new Game(_fastAi, SetCell, _white, _black);
        }

        void CloseGame()
        {
            if (Game != null)
            {
                Game.Stop();
                Game = null;
                _board.Children.Clear();
            }
        }

        public Game Game
        {
            get
            {
                return (Game)DataContext;
            }
            set
            {
                DataContext = value;
            }
        }

        void SetCell(int x, int y, bool value)
        {
            var cell = _cells[x, y];
            if (cell == null)
            {
                cell = new BoardCell();
                cell.SetValue(Grid.ColumnProperty, x);
                cell.SetValue(Grid.RowProperty, y);
                _cells[x, y] = cell;
                _board.Children.Add(cell);
                VisualStateManager.GoToState(cell, value ? "White" : "Black", true);
            }
            else
                VisualStateManager.GoToState(cell, value ? "White" : "Black", true);
        }

        void _board_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Game == null || Game.GameOver)
                return;

            var pc = Game.ActivePlayer as GamePC;

            if (pc == null)
                return;

            var p = e.GetPosition(_board);
            var c = new Position((int)(p.X / (_board.Width / 8)), (int)(p.Y / (_board.Height / 8)));

            pc.UserMove(c);
        }
    }
}