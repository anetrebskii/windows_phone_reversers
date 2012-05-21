using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using WP7.Reversers.Resources;

namespace WP7.Reversers.Core
{
    public class LocalizedStrings
    {
        public LocalizedStrings()
        {
        }

        private static Strings localizedResources = new Strings();

        public Strings LocalizedResources { get { return localizedResources; } }
    }
}
