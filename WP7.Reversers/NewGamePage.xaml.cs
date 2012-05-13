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
using System.Windows.Shapes;

namespace WP7.Reversers
{
    public partial class NewGamePage : UserControl
    {
        public NewGamePage()
        {
            InitializeComponent();
        }

        void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //DialogResult = true;
        }

        void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //DialogResult = false;
        }

        public bool FastAI
        {
            get
            {
                return _fastAi.IsChecked.GetValueOrDefault();
            }
            set
            {
                _fastAi.IsChecked = value;
            }
        }

        public int SelectedGameMode
        {
            get
            {
                return _players.SelectedIndex;
            }
            set
            {
                _players.SelectedIndex = value;
            }
        }

        public bool GameInProgress
        {
            get
            {
                return _warning.Visibility == Visibility.Visible;
            }
            set
            {
                _warning.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
