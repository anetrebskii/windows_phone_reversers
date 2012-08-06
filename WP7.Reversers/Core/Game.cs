using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using resx = WP7.Reversers.Resources.Strings;

namespace Reversi
{      
    public class Game : INotifyPropertyChanged
    {
        public delegate void CellController(int x, int y, bool style);

        public event EventHandler ChangeActivePlayer;
        CellController _controller;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        void RaisePropertyChanged(string propName)
        {
            // since we work in non-ui thread, all data binding should be dispatched to ui thread properly
            if (PropertyChanged != null)
                Deployment.Current.Dispatcher.BeginInvoke(PropertyChanged, this, new PropertyChangedEventArgs(propName));
        }

        void RaiseChangeActivePlayer()
        {
            if (ChangeActivePlayer != null)
                Deployment.Current.Dispatcher.BeginInvoke(ChangeActivePlayer, this, EventArgs.Empty);
        }

        #endregion

        #region Status Property - Text message for player

        string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            private set
            {
                if (_status == value) return;
                _status = value;
                RaisePropertyChanged("Status");
            }
        }

        Player _winPlayer;
        public Player WinPlayer
        {
            get
            {
                return _winPlayer;
            }
            private set
            {
                _winPlayer = value;
                RaisePropertyChanged("WinPlayer");
            }
        }

        #endregion

        #region GameOver Property - Is Game Ended

        bool _gameOver;
        public bool GameOver
        {
            get { return _gameOver; }
            private set
            {
                if (_gameOver == value) return;
                _gameOver = value;
                RaisePropertyChanged("GameOver");
            }
        }

        #endregion

        #region Whites Property - Whites Score

        int _whites;
        public int Whites
        {
            get { return _whites; }
            private set
            {
                if (value == _whites) return;
                _whites = value;
                RaisePropertyChanged("Whites");
            }
        }

        #endregion

        #region Blacks Property - Blacks Score

        int _blacks;
        public int Blacks
        {
            get { return _blacks; }
            private set
            {
                if (value == _blacks) return;
                _blacks = value;
                RaisePropertyChanged("Blacks");
            }
        }

        #endregion

        Player[,] _cells = new Player[8, 8];
        int _timeout;

        public Game(bool fastAi, CellController controller, GamePlayer white, GamePlayer black)
        {
            if (white == null || black == null)
                throw new ArgumentNullException();

            _timeout = fastAi ? 500 : 1500;

            _white = white;
            _white.Game = this;
            _white.Player = Player.White;
            _black = black;
            _black.Game = this;
            _black.Player = Player.Black;

            _controller = controller;

            SetCell(new Position(3, 3), Player.White);
            SetCell(new Position(4, 4), Player.White);
            SetCell(new Position(3, 4), Player.Black);
            SetCell(new Position(4, 3), Player.Black);

            new Thread(Run) { IsBackground = true }.Start();
        }

        /// <summary>
        /// Convenient access to board info
        /// Удобный доступ к игровому полю
        /// </summary>
        public Player this[Position p]
        {
            get
            {
                return p.Valid() ? _cells[p.X, p.Y] : Player.Free;
            }
        }

        /// <summary>
        /// Performs specified move and returns number of winning cells
        /// Выполняем ход и вычисляем выигрыш
        /// </summary>
        int MakeMove(Position target, Player player)
        {
            if (!target.Valid())
                return -1;

            if (_cells[target.X, target.Y] != Player.Free)
                return -1;

            var result = CheckScoreFor(target, player);

            if (result == 0)
                return result;

            Reverse(target, player);

            return result;
        }

        IEnumerable<Position> GetPositionsDirection(Position start, int offsetX, int offsetY)
        {
            while (true)
            {
                start.Offset(offsetX, offsetY);

                if (start.Valid())
                    yield return start;
                else
                    yield break;
            }
        }

        IEnumerable<Player> GetCellsDirection(Position start, int offsetX, int offsetY)
        {
            var p = start;

            while (true)
            {
                p.Offset(offsetX, offsetY);

                if (p.Valid())
                    yield return _cells[p.X, p.Y];
                else
                    yield break;
            }
        }

        /// <summary>
        /// Calculates score in specified by offsets direction
        /// Считаем потенциальный выигрыш в указанном направлении
        /// </summary>
        int CheckScoreFor(Position start, Player player, int offsetX, int offsetY)
        {
            var score = 0;

            foreach (var c in GetCellsDirection(start, offsetX, offsetY))
            {
                if (c == Player.Free)
                    break;

                if (c == player)
                    return score;

                score++;
            }

            return 0;
        }

        /// <summary>
        /// Determines whether this move is allowed, returns number of winning cells
        /// Определяем, разрешен ли ход в указанную ячейку и если да - возвращаем выигрыш
        /// </summary>
        internal int CheckScoreFor(Position target, Player player)
        {
            // if cell is already taken - makes no sense to calc further
            // если ячейка уже занята - о каком счете идет речь?
            if (_cells[target.X, target.Y] != Player.Free)
                return 0;

            return
              CheckScoreFor(target, player, 0, -1) + // N
              CheckScoreFor(target, player, 0, 1) +  // S
              CheckScoreFor(target, player, -1, 0) + // W 
              CheckScoreFor(target, player, 1, 0) +  // E
              CheckScoreFor(target, player, -1, -1) + // NW
              CheckScoreFor(target, player, 1, 1) +   // SE
              CheckScoreFor(target, player, 1, -1) +  // NE
              CheckScoreFor(target, player, -1, 1);   // SW
        }

        bool HasScoreFor(Position target, Player player)
        {
            return
              CheckScoreFor(target, player, 0, -1) > 0 ||// N
              CheckScoreFor(target, player, 0, 1) > 0 ||  // S
              CheckScoreFor(target, player, -1, 0) > 0 || // W 
              CheckScoreFor(target, player, 1, 0) > 0 ||  // E
              CheckScoreFor(target, player, -1, -1) > 0 || // NW
              CheckScoreFor(target, player, 1, 1) > 0 ||   // SE
              CheckScoreFor(target, player, 1, -1) > 0 ||  // NE
              CheckScoreFor(target, player, -1, 1) > 0;   // SW
        }

        public IEnumerable<Position> GetFreeCells()
        {
            for (var x = 0; x < 8; x++)
                for (var y = 0; y < 8; y++)
                    if (_cells[x, y] == Player.Free)
                        yield return new Position(x, y);
        }

        /// <summary>
        /// Calculates whether valid move exists or not
        /// Вычисляем наличие ходов
        /// </summary>
        internal bool HasMove(Player player)
        {
            return !_gameOver && GetFreeCells().Any(i => HasScoreFor(i, player));
        }

        /// <summary>
        /// Reverses opposite cells in specified by offsets direction
        /// Завоевываем вражеские фишки в указанном направлении
        /// </summary>
        void Reverse(Position start, Player player, int offsetX, int offsetY)
        {
            if (CheckScoreFor(start, player, offsetX, offsetY) > 0)
                foreach (var p in GetPositionsDirection(start, offsetX, offsetY))
                {
                    var c = _cells[p.X, p.Y];

                    if (c == Player.Free || c == player)
                        break;

                    SetCell(p, player);
                }
        }

        /// <summary>
        /// Reverses opposite cells  
        /// Инвертируем вражеские фишки
        /// </summary>
        void Reverse(Position target, Player player)
        {
            Reverse(target, player, 0, -1);
            Reverse(target, player, 0, 1);
            Reverse(target, player, -1, 0);
            Reverse(target, player, 1, 0);
            Reverse(target, player, -1, -1);
            Reverse(target, player, 1, 1);
            Reverse(target, player, 1, -1);
            Reverse(target, player, -1, 1);

            SetCell(target, player);
        }

        /// <summary>
        /// Register cell ownership, performs score bookkeeping, delegates changes to UI
        /// Завоевываем ячейку, обновляем счет, делегируем изменения в UI
        /// </summary>
        void SetCell(Position target, Player player)
        {
            var oldBullet = _cells[target.X, target.Y];
            if (oldBullet == player) return;

            switch (oldBullet)
            {
                case Player.White: Whites--; break;
                case Player.Black: Blacks--; break;
            }

            switch (player)
            {
                case Player.White: Whites++; break;
                case Player.Black: Blacks++; break;
            }

            _cells[target.X, target.Y] = player;

            if (_controller != null)
                Deployment.Current.Dispatcher.BeginInvoke(_controller, target.X, target.Y, player == Player.White);
        }

        GamePlayer _white, _black;

        private GamePlayer _activePlayer;
        public GamePlayer ActivePlayer
        {
            get { return _activePlayer; }
            private set
            {
                _activePlayer = value;
                RaiseChangeActivePlayer();
            }
        }

        void Run()
        {                      
            var whiteReady = _white.NextMoveReady;
            var blackReady = _black.NextMoveReady;
            var whiteDown = false;
            var blackDown = false;

            while (!GameOver)
            {
                whiteDown = !HasMove(Player.White);

                if (!whiteDown)
                {
                    ActivePlayer = _white;
                    Status = resx.rsWhiteMove;
                    do
                    {
                        if (whiteReady != null)
                            whiteReady.WaitOne();
                        else
                            Thread.Sleep(_timeout);
                    }
                    while (MakeMove(_white.NextMove, Player.White) <= 0);
                }

                if (whiteDown && blackDown)
                    GameOver = true;

                blackDown = !HasMove(Player.Black);
                if (!blackDown)
                {
                    ActivePlayer = _black;
                    Status = resx.rsBlackMove;

                    do
                    {
                        if (blackReady != null)
                            blackReady.WaitOne();
                        else
                            Thread.Sleep(_timeout);
                    }
                    while (MakeMove(_black.NextMove, Player.Black) <= 0);
                }

                if (whiteDown && blackDown)
                    GameOver = true;
            }

            Status = (_blacks < _whites ? resx.rsWhitesWon : (_blacks > _whites ? resx.rsBlacksWon : resx.rsTie));
            WinPlayer = (_blacks < _whites ? Player.White : (_blacks > _whites ? Player.Black : Player.Free));
        }

        public void Stop()
        {
            _controller = null;
            GameOver = true;
        }
    }
}
