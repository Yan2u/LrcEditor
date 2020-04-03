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
    /// LTranlate.xaml 的交互逻辑
    /// </summary>
    public partial class LTranlate : Window
    {
        LTranslator lt;
        public LTranlate()
        {
            InitializeComponent();
            lt = new LTranslator();
            lt.OnTranslationGet += ResultGet;
        }

        void ResultGet(string content)
        {
            OutputBox.Dispatcher.Invoke(new Action(delegate
            {
                OutputBox.Text += content + "\n";
            }));
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (lt.CanTranslate == false) { mShowMessage("正在初始化，请稍等..."); return; }
            if (InputBox.Text == "") { mShowMessage("请输入内容"); return; }
            if (lt.GetTransResultThread!=null && lt.GetTransResultThread.IsAlive) { mShowMessage("正在进行翻译"); return; }
            lt.StartTranslate(InputBox.Text);
            OutputBox.Clear();
        }

        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            if (OutputBox.Text == "") { mShowMessage("没有内容可以复制"); return; }
            Clipboard.SetText(OutputBox.Text);
            mShowMessage("已复制到剪贴板");
        }

        void mShowMessage(string content)
        {
            SnakeBarMessage.Content = content;
            SnakeB.IsActive = true;
        }

        private void SnakeBarMessage_ActionClick(object sender, RoutedEventArgs e)
        {
            SnakeB.IsActive = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (lt.GetTransResultThread != null && lt.GetTransResultThread.IsAlive) lt.GetTransResultThread.Abort();
            if (lt.GetTKKThread != null && lt.GetTKKThread.IsAlive) lt.GetTKKThread.Abort();
        }
    }
}
