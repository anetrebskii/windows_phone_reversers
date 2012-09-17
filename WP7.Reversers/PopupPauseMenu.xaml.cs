using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WP7.Reversers
{
    public partial class PopupPauseMenu : UserControl
    {
        public PopupPauseMenu()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            closePopup();
        }

        private void closePopup()
        {
            Popup popup = this.Parent as Popup;            
        }
    }
}
