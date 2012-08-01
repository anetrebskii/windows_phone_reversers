using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Reversi;
using WP7.Reversers.Resources;

namespace WP7.Reversers
{
	public partial class GameOverControl : UserControl
	{
	    public static DependencyProperty WinPlayerProperty;

        [Description]
	    public Player WinPlayer
	    {
	        get
	        {
	            return (Player)GetValue(WinPlayerProperty);
	        }
            set
            {                
                SetValue(WinPlayerProperty, value);
            }
	    }

        static GameOverControl()
        {
            WinPlayerProperty = DependencyProperty.Register("WinPlayer", typeof(Player),
                typeof(GameOverControl), new PropertyMetadata(winPlayerChanged));
        }

        static void winPlayerChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = (GameOverControl) sender;
            switch ((Player)e.NewValue)
            {                    
                case Player.Black:
                    VisualStateManager.GoToState(control.leftChip, "Black", false);
                    control.leftChip.Visibility = Visibility.Visible;
                    control.rightChip.Visibility = Visibility.Visible;
                    control.txtWinText.Text = Strings.rsWin;
                    break;
                case Player.White:
                    control.leftChip.Visibility = Visibility.Visible;
                    control.rightChip.Visibility = Visibility.Visible;

                    VisualStateManager.GoToState(control.leftChip, "White", true);
                    control.txtWinText.Text = Strings.rsWin;
                    break;
                case Player.Free:
                    control.leftChip.Visibility = Visibility.Collapsed;
                    control.rightChip.Visibility = Visibility.Collapsed;
                    control.txtWinText.Text = Strings.rsTie;
                    break;
            }
        }

		public GameOverControl()
		{            
			// Required to initialize variables
			InitializeComponent();            
		}
	}
}