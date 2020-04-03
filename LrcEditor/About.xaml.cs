using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LrcEditor
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();
        }

        private void UIBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit");
        }

        private void PlayerModuleBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/bob1996w/LRCEditor");
        }

        private void CSCoreBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/filoe/cscore");
        }

        private void TrnsBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.cnblogs.com/marso/p/google_translate_api.html");
        }

        private void MusicUIBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/messoer");
        }
    }
}
