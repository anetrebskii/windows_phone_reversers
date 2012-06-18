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
    public partial class SelectLevelPage : PhoneApplicationPage
    {
        public SelectLevelPage()
        {
            InitializeComponent();
        }

        private void btnSimpleLevel_Click(object sender, RoutedEventArgs e)
        {
           NavigationService.Navigate(new Uri(String.Format("/GamePlay.xaml?{0}={1}&{2}={3}", Consts.PARAM_WHO_PLAY, Consts.USER_VS_AI, Consts.PARAM_SELECT_LEVEL,Consts.SIMPLE_LEVEL), UriKind.Relative));
        }

        private void btnHardLevel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri(String.Format("/GamePlay.xaml?{0}={1}&{2}={3}", Consts.PARAM_WHO_PLAY, Consts.USER_VS_AI,Consts.PARAM_SELECT_LEVEL, Consts.HARD_LEVEL), UriKind.Relative));
        }

        private void btnMediumLevel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri(String.Format("/GamePlay.xaml?{0}={1}&{2}={3}", Consts.PARAM_WHO_PLAY, Consts.USER_VS_AI, Consts.PARAM_SELECT_LEVEL, Consts.MEDIUM_LEVEL), UriKind.Relative));
        }
    }
}