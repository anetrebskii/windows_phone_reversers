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
	    public static DependencyProperty WinnnerProperty;

        [Description()]
	    public Player Winner
	    {
	        get
	        {
                return (Player)GetValue(WinnnerProperty);
	        }
            set
            {
                SetValue(WinnnerProperty, value);
            }
	    }

        static GameOverControl()
        {
            //var metadata = new PropertyMetadata(Player.Free);
            WinnnerProperty = DependencyProperty.Register("Winner", typeof(Player),
                typeof(GameOverControl), new PropertyMetadata(Player.Free, winPlayerChanged));
        }

        public static void winPlayerChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = (GameOverControl) sender;
            switch ((Player)e.NewValue)
            {                    
                case Player.Black:
                    VisualStateManager.GoToState(control.leftChip, "Black", false);
                    VisualStateManager.GoToState(control.rightChip, "Black", false);
                    control.leftChip.Visibility = Visibility.Visible;
                    control.rightChip.Visibility = Visibility.Visible;
                    control.txtWinText.Text = Strings.rsWin;
                    break;
                case Player.White:
                    control.leftChip.Visibility = Visibility.Visible;
                    control.rightChip.Visibility = Visibility.Visible;
                    VisualStateManager.GoToState(control.leftChip, "White", true);
                    VisualStateManager.GoToState(control.rightChip, "White", true);
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