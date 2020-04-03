using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Lsearch.xaml 的交互逻辑
    /// </summary>
    public partial class Lsearch : Window
    {
        public Lsearch()
        {
            InitializeComponent();
            lresult = new ObservableCollection<LSearchResult>();
            ResultGrid.ItemsSource = lresult;
            ChoiceBox.SelectedIndex = 0;
            le = new LSearchEngine(UpdateResult);
        }
        public ObservableCollection<LSearchResult> lresult;
        public LSearchEngine le;
        LSearchResult onShowResult;

        void UpdateResult(LSearchResult newResult, bool finished = false)
        {
            ResultGrid.Dispatcher.Invoke(new Action(delegate {
                PCircle.IsIndeterminate = !finished;
                if (finished) return;
                lresult.Add(newResult);
            }));
        }

        void mShowMessage(string content)
        {
            SnakeBarMessage.Content = content;
            SnakeB.IsActive = true;
        }

        void mySearch()
        {
            if (SearchBox.Text == "") return;
            if (le.UpdateSearchResultThread != null && le.UpdateSearchResultThread.IsAlive)
            {
                mShowMessage("搜索还没有完成");
                return;
            }
            lresult.Clear();
            LInterface.LSearchChoice choice = (LInterface.LSearchChoice)ChoiceBox.SelectedIndex;
            le.BeginSearch(SearchBox.Text, choice);
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            mySearch();
        }

        private void SnakeBarMessage_ActionClick(object sender, RoutedEventArgs e)
        {
            SnakeB.IsActive = false;
        }

        async void ShowDetail()
        {
            onShowResult.Specify();
            var detail = new LSongDetail(onShowResult);
            await mHost.ShowDialog(detail);
        }

        private void ResultGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            onShowResult = ResultGrid.SelectedItem as LSearchResult;
            ShowDetail();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (le.UpdateSearchResultThread != null && le.UpdateSearchResultThread.IsAlive) le.CancelSearch();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (le.UpdateSearchResultThread != null && le.UpdateSearchResultThread.IsAlive) le.UpdateSearchResultThread.Abort();

        }

        private void SearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) mySearch();
        }

        private void InfoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (ResultGrid.SelectedItems.Count != 1)
            {
                mShowMessage("请选择一行");
                return;
            }
            onShowResult = ResultGrid.SelectedItem as LSearchResult;
            ShowDetail();
        }
    }
}
