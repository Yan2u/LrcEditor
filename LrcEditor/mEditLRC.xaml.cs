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
    /// mEditLRC.xaml 的交互逻辑
    /// </summary>
    public partial class mEditLRC : UserControl
    {
        
        public Lyric newLRC;
        public mEditLRC()
        {
            InitializeComponent();
        }
        public mEditLRC(Lyric mLyric, int indexer, int total,int isadd=0)
        {
            InitializeComponent();
            if (isadd==0)
            {
                mEditCaption.Text = string.Format("修改歌词 {0} / {1}：", indexer, total);
                mEditMinute.Text = mLyric.timeline.minute.ToString();
                mEditSecond.Text = mLyric.timeline.sec.ToString();
                mEditMultiSecond.Text = mLyric.timeline.multisec.ToString();
                mEditContent.Text = mLyric.Word;
            }
            else if(isadd==1)
            {
                mEditCaption.Text = string.Format("添加歌词：", indexer, total);
                mEditMinute.Text = "";
                mEditSecond.Text = "";
                mEditMultiSecond.Text = "";
                mEditContent.Text = "";
            }
            else
            {
                mEditCaption.Text = string.Format("添加歌词：", indexer, total);
                mEditMinute.Text = mLyric.timeline.minute.ToString();
                mEditSecond.Text = mLyric.timeline.sec.ToString();
                mEditMultiSecond.Text = mLyric.timeline.multisec.ToString();
                mEditContent.Text = "";
            }
            
        }

        private void Button_Click_Sure(object sender, RoutedEventArgs e)
        {
            if (mEditMinute.Text == "" || mEditSecond.Text == "" || mEditMultiSecond.Text == "" || mEditContent.Text == "") return;
            newLRC = new Lyric(string.Format("{0:D2}:{1:D2}.{2:D2}", mEditMinute.Text, mEditSecond.Text, mEditMultiSecond.Text), mEditContent.Text);
            btnSure.Command = DialogHost.CloseDialogCommand;
        }
    }
}
