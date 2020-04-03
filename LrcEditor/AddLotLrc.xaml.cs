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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
namespace LrcEditor
{
    /// <summary>
    /// AddLotLrc.xaml 的交互逻辑
    /// </summary>
    public partial class AddLotLrc : UserControl
    {
        public AddLotLrc()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (InputBox.Text == "") return;
            MainWindow curMain = (MainWindow)Application.Current.MainWindow;
            curMain.lc.ImportLyrics(InputBox.Text, true);
            curMain.ReSort();
            SureBtn.Command = DialogHost.CloseDialogCommand;
        }

        private void BtnHelper_Click(object sender, RoutedEventArgs e)
        {
            ShowDialogMessage("提示", "歌词文本的格式：\n" +
                "[分 : 秒 . 毫秒] + 歌词的内容\n" +
                "三个时间值需要为 2 位整数，不足在前面补0");
        }

        private void ShowDialogMessage(string title, string message)
        {
            MsgDialog_DialogTitle.Text = title;
            MsgDialog_DialogText.Text = message;
            MsgDialog.IsOpen = true;
        }

        private void MsgDialogCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            MsgDialog.IsOpen = false;
        }
    }
}
