using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace LrcEditor
{
    /// <summary>
    /// LrcDisplay.xaml 的交互逻辑
    /// </summary>
    public partial class LrcDisplay : Window
    {
        public LrcDisplay()
        {
            InitializeComponent();
            tbDisplay.Clear();
        }

        public LrcDisplay(string DisplayString)
        {
            InitializeComponent();
            tbDisplay.Clear();
            tbDisplay.Text = DisplayString;
        }
    }
}
