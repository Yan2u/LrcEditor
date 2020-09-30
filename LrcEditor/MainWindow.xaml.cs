using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using CSCore.SoundOut;
using System.Threading;
using System.Resources;
using System.Drawing.Imaging;

namespace LrcEditor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public LyricCollection lc;
        public FileInfo nowFile;
        public PlayerModule pm;
        public ObservableCollection<LSearchResult> LResults;
        Thread LrcDisplayThread;
        Lsearch lsearch;
        LTranlate ltranslate;
        LTranslator ltranslator;

        bool StopInvoke = false;

        public void OnPositionChanged(object sender)
        {
            try
            {
                MusicGroupBox.Dispatcher.Invoke(new Action(delegate
                {
                    mPlaySpan = pm.Position;
                    if (!StopInvoke) mPlayValue = pm.TimePercent;
                    mVolume = pm.Volume;
                    micon = (pm.PlaybackState == PlaybackState.Playing) ? (PackIconKind.Pause) : (PackIconKind.Play);
                }));
            }
            catch (Exception ex)
            {
                if (ex.Message == "正在中止线程。") return;
                if (pm.PlaybackState != PlaybackState.Stopped) pm.Stop();
                try { if (LrcDisplayThread.IsAlive) LrcDisplayThread.Abort(); }
                catch { }
                pm.Dispose();
                MessageBoxResult MR = MessageBox.Show(ex.Message);
                Environment.Exit(0);
            }
        }

        public double mVolume
        {
            get => pm.Volume;
            set { pm.Volume = value; OnPropertyChanged(); }
        }

        PackIconKind icon = PackIconKind.Play;

        public PackIconKind micon
        {
            get => icon;
            set { icon = value; OnPropertyChanged(); }
        }

        double PlayValue;

        public double mPlayValue
        {
            get => PlayValue;
            set { PlayValue = value; OnPropertyChanged(); }
        }

        TimeSpan PlaySpan;

        public TimeSpan mPlaySpan
        {
            get => PlaySpan;
            set { PlaySpan = value; OnPropertyChanged(); }
        }

        void OnTransResultUpdate(Lyric newlrc)
        {
            lrcGrid.Dispatcher.Invoke(new Action(() =>
            {
                lc.mLrcList.Add(newlrc);
                ReSort();
            }));
        }

        public MainWindow()
        {
            InitializeComponent();
            AlbumImg.Source = PlayerModule.ByteToImg(Properties.Resources._5);
            lc = new LyricCollection();
            pm = new PlayerModule();
            mMessageBar.MessageQueue = new SnackbarMessageQueue();
            pm.PositionChangedEvent += OnPositionChanged;
            ltranslator = new LTranslator();
            ltranslator.OnTransLrcGet += OnTransResultUpdate;
            lrcGrid.ItemsSource = lc.mLrcList;
            LastIndexes = new List<int>();
            if (File.Exists(Environment.CurrentDirectory + "\\theme.resx") == false)
            {
                ResourceWriter RW = new ResourceWriter(Environment.CurrentDirectory + "\\theme.resx");
                ITheme theme = Application.Current.Resources.GetTheme();
                RW.AddResource("ThemePrimaryColor", Encoding.Default.GetBytes(LTheme.ThemePrimaryColor[0].ToArgb().ToString()));
                RW.AddResource("ThemeSecondaryColor", Encoding.Default.GetBytes(LTheme.ThemeSecondaryColor[0].ToArgb().ToString()));
                RW.Generate();
                RW.Close(); RW.Dispose();
            }
            System.Drawing.Color pcolor = new System.Drawing.Color();
            System.Drawing.Color scolor = new System.Drawing.Color();
            ResourceReader RR = new ResourceReader(Environment.CurrentDirectory + "\\theme.resx");
            var iter = RR.GetEnumerator();
            while (iter.MoveNext())
            {
                if ((iter.Key as string) == "ThemePrimaryColor") pcolor = System.Drawing.Color.FromArgb(int.Parse(Encoding.Default.GetString(iter.Value as byte[])));
                if ((iter.Key as string) == "ThemeSecondaryColor") scolor = System.Drawing.Color.FromArgb(int.Parse(Encoding.Default.GetString(iter.Value as byte[])));
            }
            RR.Close(); RR.Dispose();
            SetTheme(pcolor, scolor);
            DataContext = this;
        }

        async void lrcOperation()
        {
            if (lc.mLrcList.Count == 0) return;
            mLrcOperation mlo = new mLrcOperation();
            await mHost.ShowDialog(mlo);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string c_infomation = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(c_infomation));
        }

        public void ReSort()
        {
            List<Lyric> list = lc.mLrcList.ToList();
            list.Sort();
            lc.mLrcList = new ObservableCollection<Lyric>(list);
            lrcGrid.ItemsSource = lc.mLrcList;
        }

        public async void Show_Edit(Lyric mLyric, int indexer, int total, int isadd = 0)
        {
            if (isadd == 2) pm.Pause();
            var mEdit = new mEditLRC(mLyric, indexer, total, isadd);
            await mHost.ShowDialog(mEdit);
            if (mEdit.newLRC != null)
            {
                if (isadd > 0)
                {
                    lc.mLrcList.Add(mEdit.newLRC);
                    ReSort();
                }
                else
                {
                    mLyric.Timeline = mEdit.newLRC.timeline.ToString();
                    mLyric.Word = mEdit.newLRC.Word;
                }
            }
            if (isadd == 2) pm.Play();
        }

        private void DeleteLrc_Click(object sender, RoutedEventArgs e)
        {
            if (lrcGrid.SelectedItems.Count == 0) { mShowMessage("请先至少选择一行"); return; }
            ObservableCollection<Lyric> myItems = new ObservableCollection<Lyric>();
            foreach (var item in lrcGrid.SelectedItems) myItems.Add(item as Lyric);
            foreach (var item in myItems) lc.mLrcList.Remove(item);

        }

        private void mShowMessage(string MessageContent)
        {
            mMessageBar.MessageQueue.Enqueue(MessageContent, null, null, null, false, false, TimeSpan.FromSeconds(0.5));
        }

        private void EditLrc_Click(object sender, RoutedEventArgs e)
        {
            if (lrcGrid.SelectedItems.Count == 0 || lrcGrid.SelectedItems.Count > 1) { mShowMessage("请先选择一行"); return; }
            Lyric lrc = lrcGrid.SelectedItem as Lyric;
            int indexer = lc.mLrcList.IndexOf(lrc) + 1;
            int total = lc.mLrcList.Count;
            Show_Edit(lrc, indexer, total);
        }

        private void AddLrc_Click(object sender, RoutedEventArgs e)
        {
            if (nowFile == null || File.Exists(nowFile.FullName) == false) { mShowMessage("请先打开或新建一个歌词文件"); return; }
            int total = lc.mLrcList.Count;
            Show_Edit(null, total, total, 1);
        }

        void ReSort(object sender, RoutedEventArgs e)
        {
            List<Lyric> list = lc.mLrcList.ToList();
            list.Sort();
            lc.mLrcList = new ObservableCollection<Lyric>(list);
            lrcGrid.ItemsSource = lc.mLrcList;
        }

        public void OpenNewFile(string FileName)
        {
            if (!File.Exists(FileName))
            {
                using (StreamWriter sw = new StreamWriter(FileName, false, Encoding.UTF8))
                {
                    sw.Write("");
                    sw.Close();
                    sw.Dispose();
                }
            }
            nowFile = new FileInfo(FileName);
            lc.ImportLyrics(nowFile);
            nowFileCaption.Header = nowFile.Name;
            lrcGrid.ItemsSource = lc.mLrcList;
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "lrc文件|*.lrc";
            if ((bool)op.ShowDialog())
            {
                OpenNewFile(op.FileName);
            }
        }

        private void ReopenFile_Click(object sender, RoutedEventArgs e)
        {
            if (nowFile == null || File.Exists(nowFile.FullName) == false) { return; }
            OpenNewFile(nowFile.FullName);
        }

        private void ViewSource_Click(object sender, RoutedEventArgs e)
        {
            if (nowFile == null || File.Exists(nowFile.FullName) == false) { return; }
            Process.Start(nowFile.FullName);
        }

        public void ImportMusic(string FileName)
        {
            pm.Open(FileName);
            OpenNewFile(System.IO.Path.GetDirectoryName(FileName) + "\\" + System.IO.Path.GetFileNameWithoutExtension(FileName) + ".lrc");
            Thread.Sleep(200);
            LName.Text = pm.SongName;
            LArt.Text = pm.SongArt;
            MusicTotal.Text = string.Format("{0:mm\\:ss\\.ff}", pm.Length);
            AlbumImg.UpdateLayout();
            AlbumImg.Source = pm.SongImg;
            AlbumImg.UpdateLayout();
        }

        private void ImportMusic_Click(object sender, RoutedEventArgs e)
        {
            if (pm.PlaybackState != PlaybackState.Stopped) pm.Stop();
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "声音文件|*.mp3;*.wav;*.flac;*wma;*.m4a;*.ogg";
            if ((bool)op.ShowDialog())
            {
                ImportMusic(op.FileName);
            }
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            pm.Stop();
        }

        private void MusicPaceBar_MouseLeave(object sender, MouseEventArgs e)
        {
            pm.TimePercent = MusicPaceBar.Value;
        }

        private void MusicPaceBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StopInvoke = true;
            pm.Pause();
        }

        private void MusicPaceBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pm.TimePercent = MusicPaceBar.Value;
            pm.Play(); StopInvoke = false;
        }

        private void MusicBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (pm.PlaybackState == PlaybackState.Playing) pm.Pause();
                else pm.Play();
            }
            catch { }
        }

        private async void ShowAddLotLrc()
        {
            var obj = new AddLotLrc();
            await mHost.ShowDialog(obj);
        }

        private void Button_Click_Forward(object sender, RoutedEventArgs e)
        {
            TimeSpan p = pm.Position;
            pm.Position = p + TimeSpan.FromSeconds(5);
        }

        private void Button_Click_Backward(object sender, RoutedEventArgs e)
        {
            TimeSpan p = pm.Position;
            pm.Position = p - TimeSpan.FromSeconds(5);
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {

        }
        List<int> LastIndexes;
        private void OnMatchLrc()
        {
            if (lc.mLrcList.Count == 0) return;
            double nowTime = pm.Position.TotalSeconds;
            List<int> index = new List<int>();
            for (int i = 0; i < lc.mLrcList.Count; i++)
            {
                if (lc.mLrcList[i].timeline.ToSeconds() > nowTime) { index.Add(i - 1); break; }
            }
            if (nowTime > lc.mLrcList[lc.mLrcList.Count - 1].timeline.ToSeconds()) index.Add(lc.mLrcList.Count - 1);
            if (index.Count == 0 || index[0] == -1) { return; }
            nowTime = lc.mLrcList[index[0]].timeline.ToSeconds();
            for (int i = index[0] - 1; i >= 0; i--)
            {
                if (lc.mLrcList[i].timeline.ToSeconds() == nowTime) index.Add(i);
                else break;
            }
            index.Sort();
            if (index.All(LastIndexes.Contains) && index.Count == LastIndexes.Count) return;
            DataGridRow row;
            try
            {
                lrcGrid.Dispatcher.Invoke(new Action(delegate
                {
                    lrcGrid.UnselectAll();
                    foreach (int ViewIndex in index)
                    {
                        lrcGrid.UpdateLayout();
                        lrcGrid.ScrollIntoView(lrcGrid.Items[ViewIndex]);
                        row = (DataGridRow)lrcGrid.ItemContainerGenerator.ContainerFromIndex(ViewIndex);
                        while (row == null)
                        {
                            lrcGrid.UpdateLayout();
                            lrcGrid.ScrollIntoView(lrcGrid.Items[ViewIndex]);
                            row = (DataGridRow)lrcGrid.ItemContainerGenerator.ContainerFromIndex(ViewIndex);
                        }
                        row.IsSelected = true;
                    }
                }));
                LastIndexes = index;
            }
            catch
            {
                LrcDisplayThread.Abort();
            }
        }

        private void OnThreadWork()
        {
            while (true)
            {
                OnMatchLrc();
                Thread.Sleep(100);
            }
        }

        private void MenuItemDisplay_Click(object sender, RoutedEventArgs e)
        {
            if (LrcDisplayThread == null || !LrcDisplayThread.IsAlive)
            {
                if (pm == null || pm.SongName == null || pm.SongName == "" || nowFile == null || lc.mLrcList.Count == 0)
                {
                    mShowMessage("导入音乐文件和歌词文件后，才能开启歌词预览");
                    return;
                }
                LrcDisplayThread = new Thread(new ThreadStart(OnThreadWork));
                mShowMessage("歌词预览已开启");

                //foreach (MenuItem item in lrcGridMenu.Items) item.IsEnabled = false;
                foreach (var item in nowFileCaption.Items)
                {
                    try
                    {
                        MenuItem mItem = item as MenuItem;
                        mItem.IsEnabled = false;
                    }
                    catch { }
                }
                //ClearLrcBtn.IsEnabled = false;
                ImportMusicBtn.IsEnabled = false;
                CloseMusicBtn.IsEnabled = false;
                //LrcOperationBtn.IsEnabled = false;
                //InsertLrcBtn.IsEnabled = false;

                LrcDisplayThread.Start();
            }
            else
            {
                LrcDisplayThread.Abort();
                mShowMessage("歌词预览已关闭");

                //foreach (MenuItem item in lrcGridMenu.Items) item.IsEnabled = true;
                foreach (var item in nowFileCaption.Items)
                {
                    try
                    {
                        MenuItem mItem = item as MenuItem;
                        mItem.IsEnabled = true;
                    }
                    catch { }
                }
                //ClearLrcBtn.IsEnabled = true;
                ImportMusicBtn.IsEnabled = true;
                CloseMusicBtn.IsEnabled = true;
                //LrcOperationBtn.IsEnabled = true;
            }
        }

        private void MenuItem_LrcClear_Click(object sender, RoutedEventArgs e)
        {
            ShowDialogMessage("提示", "确定要清空歌词吗？", new DialogClosingEventHandler((object src, DialogClosingEventArgs arg)=>
            {
                if ((bool)arg.Parameter) lc.mLrcList.Clear();
            }));
        }

        private void PlayerInsertLrc_Click(object sender, RoutedEventArgs e)
        {
            if (pm.PlaybackState != PlaybackState.Playing && pm.PlaybackState != PlaybackState.Paused)
            {
                mShowMessage("播放时才可添加歌词");
                return;
            }
            string Tdes = pm.Position.ToString(@"mm\:ss\.ff");
            Lyric mlyric = new Lyric(Tdes, "");
            Show_Edit(mlyric, 0, 0, 2);
        }

        private void MenuItem_LrcOperation_Click(object sender, RoutedEventArgs e)
        {
            lrcOperation();
        }

        private void FileNewItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Title = "选择新的歌词文件存放的位置";
            sf.Filter = "lrc 歌词文件|*.lrc";
            if (sf.ShowDialog() == true)
            {
                OpenNewFile(sf.FileName);
            }
        }

        private void FileSaveItem_Click(object sender, RoutedEventArgs e)
        {
            if (nowFile == null || File.Exists(nowFile.FullName) == false) return;
            lc.SaveLyrics(nowFile);
            mShowMessage("已保存至: " + nowFile.FullName);
        }

        private void RemoveBlank_Click(object sender, RoutedEventArgs e)
        {
            for (int i = lc.mLrcList.Count - 1; i >= 0; i--)
            {
                if (lc.mLrcList[i].Word.Trim() == "") lc.mLrcList.RemoveAt(i);
            }
        }

        private void LrcGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lrcGrid.SelectedItems.Count != 1) return;
            Lyric lrc = lrcGrid.SelectedItem as Lyric;
            int indexer = lc.mLrcList.IndexOf(lrc) + 1;
            int total = lc.mLrcList.Count;
            Show_Edit(lrc, indexer, total);
        }

        void SetTheme(System.Drawing.Color PColor, System.Drawing.Color SColor)
        {
            ITheme theme = Application.Current.Resources.GetTheme();
            System.Windows.Media.Color MPColor = System.Windows.Media.Color.FromArgb(PColor.A, PColor.R, PColor.G, PColor.B);
            System.Windows.Media.Color MSColor = System.Windows.Media.Color.FromArgb(SColor.A, SColor.R, SColor.G, SColor.B);
            theme.SetPrimaryColor(MPColor); theme.SetSecondaryColor(MSColor);
            Application.Current.Resources.SetTheme(theme);
            ResourceWriter RW = new ResourceWriter(Environment.CurrentDirectory + "\\theme.resx");
            RW.AddResource("ThemePrimaryColor", Encoding.Default.GetBytes(PColor.ToArgb().ToString()));
            RW.AddResource("ThemeSecondaryColor", Encoding.Default.GetBytes(SColor.ToArgb().ToString()));
            RW.Generate();
            RW.Close(); RW.Dispose();
        }

        private void ThemeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            try
            {
                System.Drawing.Color PColor = LTheme.ThemePrimaryColor[int.Parse(item.Name.Replace("Theme", "")) - 1];
                System.Drawing.Color SColor = LTheme.ThemeSecondaryColor[int.Parse(item.Name.Replace("Theme", "")) - 1];
                SetTheme(PColor, SColor);
            }
            catch { }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (lsearch != null) lsearch.Close();
        }

        private void SongSearchItem_Click(object sender, RoutedEventArgs e)
        {
            try { lsearch.Show(); }
            catch { lsearch = new Lsearch(); lsearch.Show(); }
        }

        private void SongTransItem_Click(object sender, RoutedEventArgs e)
        {
            try { ltranslate.Show(); }
            catch { ltranslate = new LTranlate(); ltranslate.Show(); }
        }

        private void GotoCurTimeItem_Click(object sender, RoutedEventArgs e)
        {
            if (lrcGrid.SelectedItems.Count != 1)
            {
                mShowMessage("请选中一行");
                return;
            }
            if (pm.PlaybackState != PlaybackState.Playing && pm.PlaybackState != PlaybackState.Paused)
            {
                mShowMessage("播放时才可以使用");
                return;
            }
            Lyric lrc = lrcGrid.SelectedItem as Lyric;
            pm.Position = TimeSpan.FromSeconds(lrc.timeline.ToSeconds());
            if (pm.PlaybackState == PlaybackState.Paused) pm.Play();
        }

        private void CopyAllItem_Click(object sender, RoutedEventArgs e)
        {
            if (lrcGrid.SelectedItems.Count == 0) return;
            string content = "";
            Lyric lrc = null;
            foreach (object item in lrcGrid.SelectedItems)
            {
                lrc = (Lyric)item;
                content += $"[{lrc.Timeline}]{lrc.Word}\n";
            }
            Clipboard.SetText(content);
            mShowMessage("已复制到剪贴板");
        }

        private void AddLrcItem_Click(object sender, RoutedEventArgs e)
        {
            if (nowFile == null || File.Exists(nowFile.FullName) == false) { mShowMessage("请先打开或新建一个歌词文件"); return; }
            ShowAddLotLrc();
        }

        async void ShowAbout()
        {
            About about = new About();
            await mHost.ShowDialog(about);
        }

        private void AboutItem_Click(object sender, RoutedEventArgs e)
        {
            ShowAbout();
        }

        private void CopyImg_Click(object sender, RoutedEventArgs e)
        {
            if (AlbumImg.Source != null) Clipboard.SetImage((BitmapSource)AlbumImg.Source);
        }

        private void CloseCur_Click(object sender, RoutedEventArgs e)
        {
            lc.mLrcList.Clear();
            nowFile = null;
            nowFileCaption.Header = "文件";
        }

        private void CloseMusicBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pm.SongName == null) return;
            AlbumImg.Source = PlayerModule.ByteToImg(Properties.Resources._5);
            pm.Dispose();
            LName.Text = "Unknown";
            LArt.Text = "Unknown";
        }

        private void TranslateItem_Click(object sender, RoutedEventArgs e)
        {
            if (lrcGrid.SelectedItems.Count == 0) return;
            if (ltranslator.CanTranslate == false)
            {
                mShowMessage("翻译模块正在初始化……");
                return;
            }
            LyricCollection lyricList = new LyricCollection();
            foreach (object item in lrcGrid.SelectedItems) lyricList.mLrcList.Add((Lyric)item);
            ltranslator.StartTranlateLrc(lyricList);
        }

        DialogClosingEventHandler lastHandler = null;
        private void ShowDialogMessage(string title, string message, DialogClosingEventHandler OnDialogClosing)
        {
            if (lastHandler != null) MainWin_Dialog.DialogClosing -= lastHandler;
            if (OnDialogClosing != null) MainWin_Dialog.DialogClosing += OnDialogClosing;
            lastHandler = OnDialogClosing;
            MainWin_DialogTitle.Text = title;
            MainWin_DialogText.Text = message;
            MainWin_Dialog.IsOpen = true;
        }
    }
}
