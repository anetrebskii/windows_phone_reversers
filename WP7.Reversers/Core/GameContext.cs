using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Reversi;

namespace WP7.Reversers.Core
{
    public class GameContext : INotifyPropertyChanged
    {
        private static GameContext _instance = new GameContext();

        public GameContext Instance
        {
            get { return _instance; }
        }


        private Game _currentGame;
        public Game CurrentGame
        {
            get { return _currentGame; }
            private set
            {
                _currentGame = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentGame"));
                }
            }
        }

        public void RestartGame()
        {
            CurrentGame = new Game();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
