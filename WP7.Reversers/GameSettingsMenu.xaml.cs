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
using Microsoft.Phone.Controls;
using WP7.Reversers.Core;

namespace WP7.Reversers
{
    public partial class GameSettingsMenu : PhoneApplicationPage
    {
        public GameSettingsMenu()
        {
            InitializeComponent();
        }

        private void btnUserVsUser_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri(String.Format("/GamePlay.xaml?{0}={1}", Consts.PARAM_WHO_PLAY, Consts.USER_VS_USER), UriKind.Relative));
        }

        private void btnUserVsAI_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri(String.Format("/GamePlay.xaml?{0}={1}", Consts.PARAM_WHO_PLAY, Consts.USER_VS_AI), UriKind.Relative));
        }
    }
}