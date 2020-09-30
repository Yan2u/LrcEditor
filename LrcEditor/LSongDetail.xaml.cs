using System;
using System.Windows;
using System.Windows.Controls;
using System.Net;
using System.ComponentModel;
using TagLib;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using MaterialDesignThemes.Wpf;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace LrcEditor
{
    public delegate void LDownloadFinishEventHandler();
    /// <summary>
    /// LSongDetail.xaml 的交互逻辑
    /// </summary>
    public partial class LSongDetail : UserControl , INotifyPropertyChanged
    {
        LSearchResult _result;
        public LSearchResult result
        {
            get => _result;
            set { _result = value; OnPropertyChanged(); }
        }
        public WebClient Downloader;
        int DownloadPercentage;
        public int mPercentage
        {
            get => DownloadPercentage;
            set { DownloadPercentage = value; OnPropertyChanged(); }
        }
        Thread PicDownloader;
        byte[] PicData;
        LDownloadFinishEventHandler DownloadFisishHandler;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void PicDownloadFinish()
        {
            AlbumImg.Dispatcher.Invoke(new Action(delegate
            {
                try { AlbumImg.Source = PlayerModule.ByteToImg(PicData); }
                catch { }
            }));
        }

        MemoryStream StreamToMemoryStream(Stream instream)
        {
            MemoryStream outstream = new MemoryStream();
            const int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int count = 0;
            while ((count = instream.Read(buffer, 0, bufferLen)) > 0)
            {
                outstream.Write(buffer, 0, count);
            }
            instream.Close();
            return outstream;
        }

        void BeginDownloadPic()
        {
            if (result.b_PicUrl.ToString().StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) =>
                {
                    return true;
                });
            }
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(result.b_PicUrl);
                MemoryStream stream = StreamToMemoryStream(request.GetResponse().GetResponseStream());
                PicData = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(PicData, 0, PicData.Length);
                stream.Close();
            }
            catch { }
            DownloadFisishHandler.Invoke();
        }

        string SaveName;
        static string SaveDirectory = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)).FullName + "\\Downloads\\LrcEditor_Download";

        public LSongDetail(LSearchResult res)
        {
            InitializeComponent();
            AlbumImg.Source = PlayerModule.ByteToImg(Properties.Resources._5);
            SnakeB.MessageQueue = new SnackbarMessageQueue();
            result = res;
            DownloadFisishHandler += PicDownloadFinish;
            Downloader = new WebClient();
            Downloader.DownloadProgressChanged += OnDownloadProgressChanged;
            Downloader.DownloadFileCompleted += OnComplete;
            PicDownloader = new Thread(new ThreadStart(BeginDownloadPic));
            PicDownloader.Start();
            DataContext = this;
            SaveName = res.b_SongArtist + " - " + res.b_SongName;
            SaveName = SaveName.Replace("\\", "").Replace("/", "");
        }

        void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            mPercentage = e.ProgressPercentage;
        }

        void OnComplete(object sender, AsyncCompletedEventArgs e)
        {
            string SavePath = string.Format("{0}\\{1}{2}", SaveDirectory, SaveName, (result.Src == LInterface.LSearchChoice.QQMusic ? ".m4a" : ".mp3"));
            try
            {
                DownloadDialog.IsOpen = false;
                TagLib.File tf = TagLib.File.Create(SavePath);
                tf.Tag.Title = result.b_SongName;
                tf.Tag.Performers = new string[] { result.b_SongArtist };
                tf.Tag.Album = result.b_SongAlbum;
                tf.Tag.Pictures = new IPicture[] { new Picture(new ByteVector(PicData, PicData.Length)) };
                tf.Save();
                mShowMessage("文件已保存至: " + SavePath);
            }
            catch
            {
                mShowMessage("下载文件出错，尝试更换搜索源");
                if (System.IO.File.Exists(SavePath)) System.IO.File.Delete(SavePath);
            }
            
        }

        void mShowMessage(string content)
        {
            SnakeB.MessageQueue.Enqueue(content, null, null, null, false, false, TimeSpan.FromSeconds(0.5));
        }

        private void OnlineMusic_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(result.b_FileUrl.ToString());
        }

        private void OnlinePage_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(result.b_OnlinePlayUrl.ToString());
        }

        private void DownFile_Click(object sender, RoutedEventArgs e)
        {
            string SavePath = string.Format("{0}\\{1}{2}", SaveDirectory, SaveName, (result.Src == LInterface.LSearchChoice.QQMusic ? ".m4a" : ".mp3"));
            try
            {
                Downloader.DownloadFileAsync(result.b_FileUrl, SavePath);
                DownloadDialog.IsOpen = true;
            }
            catch
            {
                mShowMessage("下载资源时出错，可能是地址已失效");
            }
            
        }

        private void AddLrc_Click(object sender, RoutedEventArgs e)
        {
            string SavePath = string.Format("{0}\\{1}{2}", SaveDirectory, SaveName, ".lrc");
            if (result.b_SongLyric == "")
            {
                mShowMessage("这首歌好像没有歌词");
                return;
            }
            MainWindow curMain = (MainWindow)Application.Current.MainWindow;
            if(curMain.nowFile==null || System.IO.File.Exists(curMain.nowFile.FullName) == false)
            {
                if (MessageBox.Show("是否创建新的歌词文件，然后加入歌词？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    using(StreamWriter sw=new StreamWriter(SavePath, false, System.Text.Encoding.UTF8))
                    {
                        sw.Write("");sw.Close();
                    }
                    mShowMessage("已创建: " + SavePath);
                    curMain.OpenNewFile(SavePath);
                }
                else return;
            }
            string SavePath2 = string.Format("{0}\\{1}{2}", SaveDirectory, SaveName, (result.Src == LInterface.LSearchChoice.QQMusic ? ".m4a" : ".mp3"));
            if (System.IO.File.Exists(SavePath) == false)
            {
                curMain.lc.ImportLyrics(result.b_SongLyric, true);
                curMain.ReSort();
                curMain.lc.SaveLyrics(new FileInfo(SavePath));
            }
            else
            {
                curMain.ImportMusic(SavePath2);
                curMain.lc.ImportLyrics(result.b_SongLyric, true);
                curMain.ReSort();
                curMain.lc.SaveLyrics(new FileInfo(SavePath));
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            if(Downloader.IsBusy || PicDownloader.IsAlive)
            {
                if (MessageBox.Show("有正在进行中的任务，是否还要退出？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel) return;
            }
            if (Downloader.IsBusy) Downloader.CancelAsync();
            if (PicDownloader.IsAlive) PicDownloader.Abort();
            CloseBtn.Command = DialogHost.CloseDialogCommand;
        }

        private void Folder_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(SaveDirectory);
        }

        private void ImgCopy_Click(object sender, RoutedEventArgs e)
        {
            if (AlbumImg.Source != null) { Clipboard.SetImage((BitmapSource)AlbumImg.Source); }
        }

        private void CancelDownloadBtn_Click(object sender, RoutedEventArgs e)
        {
            Downloader.CancelAsync();
            string SavePath = string.Format("{0}\\{1}{2}", SaveDirectory, SaveName, (result.Src == LInterface.LSearchChoice.QQMusic ? ".m4a" : ".mp3"));
            if (System.IO.File.Exists(SavePath)) System.IO.File.Delete(SavePath);
            mShowMessage("下载已经被取消");
        }
    }
}
