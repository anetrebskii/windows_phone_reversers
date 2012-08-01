using System;
using System.Collections.Generic;
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
using Microsoft.Phone.Shell;
using Reversi;
using WP7.Reversers.Core;
using WP7.Reversers.Resources;

namespace WP7.Reversers
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            initializeApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NewGamePrompt();
        }

        private void initializeApplicationBar()
        {
            ApplicationBarMenuItem mnuRestartGame = new ApplicationBarMenuItem(Strings.rsRestartGame);
            mnuRestartGame.Click += new EventHandler(mnuRestartGame_Click);
            ApplicationBar.MenuItems.Add(mnuRestartGame);

            ApplicationBarMenuItem mnuReturnToMainMenu = new ApplicationBarMenuItem(Strings.rsReturnToMainMenu);
            mnuReturnToMainMenu.Click += new EventHandler(mnuReturnToMainMenu_Click);
            ApplicationBar.MenuItems.Add(mnuReturnToMainMenu);
        }

        void mnuReturnToMainMenu_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainMenu.xaml", UriKind.Relative));
        }

        void mnuRestartGame_Click(object sender, EventArgs e)
        {
            NewGamePrompt();
        }

        int _gameMode = 1;
        bool _fastAi = false;


        GamePlayer _black, _white;
        BoardCell[,] _cells;

        void NewGamePrompt()
        {
            if (NavigationContext.QueryString[Consts.PARAM_WHO_PLAY] == Consts.USER_VS_USER)
                _gameMode = 0;

            else
            {
                switch (NavigationContext.QueryString[Consts.PARAM_SELECT_LEVEL])
                {
                    case Consts.SIMPLE_LEVEL:
                        _gameMode = 1;
                        break;
                    case Consts.MEDIUM_LEVEL:
                        _gameMode = 2;
                        break;
                    case Consts.HARD_LEVEL:
                        _gameMode = 3;
                        break;
                }
            }
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

                case 1://pc vs aiSimple
                    _white = new GamePC();
                    _black = new GameAISimple();
                    break;
                case 2://pc vs aiMedium
                    _white = new GamePC();
                    _black = new GameAIMedium();
                    break;
                case 3://pc vs aiHard
                    _white = new GamePC();
                    _black = new GameAIHard();
                    break;
            }

            Game = new Game(_fastAi, SetCell, _white, _black);
            Game.ChangeActivePlayer += new EventHandler(Game_OnChangeActivePlayer);
        }

        void Game_OnChangeActivePlayer(object sender, EventArgs e)
        {            
            updateActivePlayer();
        }

        private void updateActivePlayer()
        {
            while (Game.ActivePlayer == null)
            {
                System.Threading.Thread.Sleep(1);
            }
            switch (Game.ActivePlayer.Player)
            {
                case Player.White:
                    imgWhite.Opacity = 1;
                    imgBlack.Opacity = 0.3;
                    break;
                case Player.Black:
                    imgWhite.Opacity = 0.3;
                    imgBlack.Opacity = 1;
                    break;
            }
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

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void _board_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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