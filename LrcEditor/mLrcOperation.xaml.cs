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

namespace LrcEditor
{
    /// <summary>
    /// mLrcOperation.xaml 的交互逻辑
    /// </summary>
    public partial class mLrcOperation : UserControl
    {
        public mLrcOperation()
        {
            InitializeComponent();
            mWordMatch.IsChecked = true;
            mTimeOperation.SelectedIndex = 0;
            mIgnoreUpperLower.IsChecked = true;
        }

        private void MTimeAdjust_Click(object sender, RoutedEventArgs e)
        {
            if (mMin.Text == "" || mSec.Text == "" || mMultiSec.Text == "") return;
            if (mTimeOperation.Text == "+")
            {
                foreach (Lyric lrc in ((MainWindow)Application.Current.MainWindow).lc.mLrcList)
                {
                    TimeLine tl = lrc.timeline;
                    tl += new TimeLine(int.Parse(mMin.Text), int.Parse(mSec.Text), int.Parse(mMultiSec.Text));
                    lrc.Timeline = tl.ToString();
                }
            }
            else
            {
                foreach (Lyric lrc in ((MainWindow)Application.Current.MainWindow).lc.mLrcList)
                {
                    TimeLine tl = lrc.timeline;
                    tl -= new TimeLine(int.Parse(mMin.Text), int.Parse(mSec.Text), int.Parse(mMultiSec.Text));
                    lrc.Timeline = tl.ToString();
                }
            }
        }

        private void MLrcAdjust_Click(object sender, RoutedEventArgs e)
        {
            if (mSearchBox.Text == "") return;
            if (mWholeMatch.IsChecked == true)
            {
                foreach (Lyric lrc in ((MainWindow)Application.Current.MainWindow).lc.mLrcList)
                {
                    if (mIgnoreUpperLower.IsChecked == true && lrc.Word.ToLower() == mSearchBox.Text.ToLower()) lrc.Word = mReplaceBox.Text;
                    else if (lrc.Word == mSearchBox.Text) lrc.Word = mReplaceBox.Text;
                }
            }
            else
            {
                foreach (Lyric lrc in ((MainWindow)Application.Current.MainWindow).lc.mLrcList)
                {
                    if (mIgnoreUpperLower.IsChecked == true && lrc.Word.ToLower().IndexOf(mSearchBox.Text.ToLower()) >= 0)
                    {
                        string replacer = lrc.Word.Substring(lrc.Word.ToLower().IndexOf(mSearchBox.Text.ToLower()), mSearchBox.Text.Length);
                        lrc.Word = lrc.Word.Replace(replacer, mReplaceBox.Text);
                    }
                    else if (lrc.Word.IndexOf(mSearchBox.Text) >= 0) lrc.Word = lrc.Word.Replace(mSearchBox.Text, mReplaceBox.Text);
                }
            }
        }
    }
}
