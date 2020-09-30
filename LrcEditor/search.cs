using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Security;
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Security.Cryptography;
using System.Windows;

namespace LrcEditor
{
    public delegate void SearchResultUpdater(LSearchResult SearchResult, bool finished = false);

    /// <summary>
    /// 提供接口，包括网址，发送Request，反序列化
    /// </summary>
    public sealed class LInterface
    {
        public enum LSearchChoice
        {
            NetEase,
            QQMusic,
            KuGouMusic
        }

        static readonly string table = "abcdefghijklmnopqrstuvwxyz1234567890";

        static T[] RandChoose<T>(T[] In, int count)
        {
            T[] result = new T[count];
            int len = In.Length;
            if (len <= count) return In;
            Random rd = new Random();
            for (int i = 1; i <= count; i++)
            {
                int index = rd.Next(0, len - 1);
                result[i - 1] = In[index];
                In[index] = In[len - 1];
                len--;
            }
            return result;
        }

        public static string GetKGMid()
        {
            char[] key = RandChoose(table.ToCharArray(), 4);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            return BitConverter.ToString(result);
        }

        /// <summary>
        /// 网易云音乐搜索接口
        /// </summary>
        static readonly string NetEaseMusicSearch = "http://music.163.com/api/search/get?&limit=35&type=1&offset={0}&s={1}";
        static readonly string NetEaseMusicLyric = "http://music.163.com/api/song/lyric?os=pc&lv=-1&kv=-1&tv=-1&id={0}";
        static readonly string NetEaseMusicAlbum = "https://api.imjad.cn/cloudmusic/?id={0}&type=album";
        static readonly string NetEaseMusicOnline = "https://music.163.com/#/song?id={0}";
        static readonly string NetEaseMusicPlay = "https://music.163.com/#/song?id={0}";
        static readonly string NetEaseMusicDownload = "https://api.imjad.cn/cloudmusic/?id={0}&type=song";

        public static string NetEaseMusic_Search(int page, string word)
        {
            return string.Format(NetEaseMusicSearch, 35 * (page - 1), word);
        }

        public static string NetEaseMusic_Lyric(int id)
        {

            return string.Format(NetEaseMusicLyric, id);
        }

        public static string NetEaseMusic_Album(int id)
        {
            return string.Format(NetEaseMusicAlbum, id);
        }

        public static string NetEaseMusic_Online(int id)
        {
            return string.Format(NetEaseMusicOnline, id);
        }

        public static string NetEaseMusic_Download(int id)
        {
            return string.Format(NetEaseMusicDownload, id);
        }

        public static string NetEaseMusic_Play(int id)
        {
            return string.Format(NetEaseMusicPlay, id);
        }

        /// <summary>
        /// QQ音乐搜索接口
        /// </summary>
        static readonly string QQMusicSearch = "https://c.y.qq.com/soso/fcgi-bin/client_search_cp?aggr=1&cr=1&flag_qc=0&p={0}&n=35&w={1}";
        static readonly string QQMusicAlbum = "http://imgcache.qq.com/music/photo/album_500/17/500_albumpic_{0}_0.jpg";
        static readonly string QQMusicToken = "https://c.y.qq.com/base/fcgi-bin/fcg_music_express_mobile3.fcg?format=json205361747&platform=yqq&cid=205361747&songmid={0}&filename={1}&guid=126548448";
        static readonly string QQMusicDownload = "http://ws.stream.qqmusic.qq.com/{0}?fromtag=0&guid=126548448&vkey={1}";
        static readonly string QQMusicLyric = "https://c.y.qq.com/lyric/fcgi-bin/fcg_query_lyric_new.fcg?-=MusicJsonCallback_lrc&songmid={0}&g_tk=758816886&format=json&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq.json&needNewCode=0";
        static readonly string QQMusicPlay = "https://y.qq.com/n/yqq/song/{0}.html";

        public static string QQMusic_Search(int page, string word)
        {
            return string.Format(QQMusicSearch, page, word);
        }

        public static string QQMusic_Album(int album_mid)
        {
            return string.Format(QQMusicAlbum, album_mid);
        }

        public static string QQMusic_Token(string song_mid)
        {
            return string.Format(QQMusicToken, song_mid, "C400" + song_mid + ".m4a");
        }

        public static string QQMusic_Download(string song_mid, string vkey)
        {
            return string.Format(QQMusicDownload, "C400" + song_mid + ".m4a", vkey);
        }

        public static string QQMusic_Lyric(string song_mid)
        {
            return string.Format(QQMusicLyric, song_mid);
        }

        public static string QQMusic_Play(string song_mid)
        {
            return string.Format(QQMusicPlay, song_mid);
        }

        /// <summary>
        /// 酷狗音乐搜索接口
        /// 鸣谢：MessAPI
        /// https://github.com/messoer
        /// </summary>
        static readonly string KuGouMusicSearch = "http://mobilecdn.kugou.com/api/v3/search/song?format=json&keyword={1}&page={0}&pagesize=35&showtype=1";
        static readonly string KuGouMusicDetail = "https://www.kugou.com/yy/index.php?r=play/getdata&hash={0}";
        static readonly string KuGouOnlineAddr = "https://www.kugou.com/song/#hash={0}";

        public static string KuGouMusic_Search(int page, string word)
        {
            return string.Format(KuGouMusicSearch, page, word);
        }

        public static string KuGouMusic_Detail(string hash)
        {
            return string.Format(KuGouMusicDetail, hash);
        }

        public static string KuGouMusic_OnlineAddr(string hash)
        {
            return string.Format(KuGouOnlineAddr, hash);
        }

        public static string GetResponse(string weburl, bool isqq = false)
        {
            // 对 Https 访问的安全性指定
            if (weburl.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) =>
                {
                    return true;
                });
            }
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(new Uri(weburl));
            HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
            string res = "";
            using (Stream stream = Response.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                res = sr.ReadToEnd();
                stream.Close();
            }
            if (isqq)
            {
                res = res.Remove(0, 9);
                res = res.Remove(res.Length - 1, 1);
            }
            return res;
        }

        // 这个感觉并没有什么用
        public static T Deserialize<T>(string jsonstr)
        {
            T res = JsonConvert.DeserializeObject<T>(jsonstr);
            return res;
        }
    }

    // 网易歌词搜索部分

    public struct NetEaseReturn_Song
    {
        public int code;
        public NERS_Songs result;
    }

    public struct NERS_Artist { public string name; }

    public struct NERS_Single
    {
        public int id, duration;
        public string name;
        public List<NERS_Artist> artists;
        public NERS_Album album;
    }

    public struct NERS_Album { public string name; public int id; }

    public struct NERS_Songs
    {
        public int songCount;
        public List<NERS_Single> songs;
    }

    public struct NERL_Lyric { public string lyric; }

    public struct NetEaseReturn_Lyric
    {
        public int code;
        public NERL_Lyric lrc, tlyric;
    }

    public struct NERD_Data { public string url; }

    public struct NetEaseReturn_Download
    {
        public List<NERD_Data> data;
    }

    public struct NERP_Al { public string picUrl; }

    public struct NERP_Data { public NERP_Al al; }

    public struct NetEaseReturn_Pic
    {
        public List<NERP_Data> songs;
    }

    // QQ音乐搜索部分

    public struct QMRS_Artist
    {
        public string name;
    }

    public struct QMRS_Single
    {
        public string songname, songmid, albumname;
        public int albumid, interval;
        public List<QMRS_Artist> singer;
    }

    public struct QMRS_Data
    {
        public List<QMRS_Single> list;
        public int totalnum;
    }

    public struct QMRS_Songs { public QMRS_Data song; }

    public struct QQMusicReturn_Song
    {
        public QMRS_Songs data;
        public int code;
    }

    public struct QQMusicReturn_Lyric
    {
        public string lyric, trans;
    }

    public struct QMRST_Data { public string vkey; }

    public struct QMRST_Single { public List<QMRST_Data> items; }

    public struct QQMusicReturn_Token { public QMRST_Single data; }

    // 酷狗音乐搜索部分 

    public struct KGRS_Single
    {
        public string songname, singername, album_name, album_id;
        public string hash;
        public int duration;
    }

    public struct KGRS_Detail_Data
    {
        public string img;
        public string play_url;
        public string lyrics;
    }

    public struct KGRS_Detail
    {
        public KGRS_Detail_Data data;
    }

    public struct KGRS_Data
    {
        public List<KGRS_Single> info;
    }

    public struct KGMusicReturn_Song
    {
        public int status;
        public KGRS_Data data;
    }

    /// <summary>
    /// 搜索结果，对不同搜索引擎返回的数据格式进行统一，此类被绑定到前端显示
    /// 此类被绑定到前端，为了实现 Bingding 的实时更新，需要实现 INotifyPropertyChanged 接口 
    /// </summary>
    public class LSearchResult : INotifyPropertyChanged
    {
        // 源数据
        string SongName;
        string SongArtist;
        string SongAlbum;
        string SongLyric;
        public string QMid;
        Uri PicUrl;
        Uri FileUrl;
        Uri OnlinePlayUrl;
        int albumID; // netease
        int SongID;
        int SongDuration;
        public event PropertyChangedEventHandler PropertyChanged;
        static readonly Uri DefaultUrl = new Uri("https://www.easyicon.net/api/resizeApi.php?id=1229001&size=128");
        static readonly Uri DefaultFile = new Uri("https://www.baidu.com");

        // 前端绑定的数据必须具有 get 和 set 方法
        public string b_SongName { get { return SongName; } set { UpdateProperty(ref SongName, value); } }
        public string b_SongArtist { get { return SongArtist; } set { UpdateProperty(ref SongArtist, value); } }
        public string b_SongAlbum { get { return SongAlbum; } set { UpdateProperty(ref SongAlbum, value); } }
        public Uri b_PicUrl { get { return PicUrl; } set { UpdateProperty(ref PicUrl, value); } }
        public Uri b_FileUrl { get { return FileUrl; } set { UpdateProperty(ref FileUrl, value); } }
        public Uri b_OnlinePlayUrl { get { return OnlinePlayUrl; } set { UpdateProperty(ref OnlinePlayUrl, value); } }
        public string b_SongDuration
        {
            get
            {
                return string.Format("{0:d2} : {1:d2}", SongDuration / 60, SongDuration % 60);
            }
            set { SongDuration = int.Parse(value); OnPropertyChanged("b_Duration"); }
        }
        public string b_SongLyric { get { return SongLyric; } set { s_SongLyric = value; } }
        public string s_SongLyric { get { return "Click to View"; } set { UpdateProperty(ref SongLyric, value); } }

        public string s_PicUrl { get { return "Click to Open"; } set { b_PicUrl = new Uri(value); } }
        public string s_FileUrl { get { return "Click to Open"; } set { b_FileUrl = new Uri(value); } }
        public string s_OnlinePlayUrl { get { return "Click to Open"; } set { b_OnlinePlayUrl = new Uri(value); } }

        /// <summary>
        /// 表示搜索结果的搜索源
        /// </summary>
        public LInterface.LSearchChoice Src;

        // 实现接口 INotifyPropertyChanged
        void UpdateProperty<T>(ref T properVal, T newVal, [CallerMemberName] string propertyName = "")
        {
            if (Equals(properVal, newVal)) return;
            properVal = newVal;
            OnPropertyChanged(propertyName); // 触发 PropertyChanged
        }

        void OnPropertyChanged([CallerMemberName] string c_infomation = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(c_infomation));
            // 由 PropertyChangedEventHandler 调用委托触发 PropertyChanged 事件
        }

        public void SpecifyAsync(object obj /*callback*/)
        {
            if (Src == LInterface.LSearchChoice.NetEase)
            {
                int SongID = int.Parse(QMid);
                NetEaseReturn_Lyric nerl = LInterface.Deserialize<NetEaseReturn_Lyric>(LInterface.GetResponse(LInterface.NetEaseMusic_Lyric(SongID)));
                b_SongLyric = nerl.lrc.lyric + nerl.tlyric.lyric;
                NetEaseReturn_Download nerd = LInterface.Deserialize<NetEaseReturn_Download>(LInterface.GetResponse(LInterface.NetEaseMusic_Download(SongID)));
                b_FileUrl = new Uri(nerd.data[0].url);
                NetEaseReturn_Pic nerp = LInterface.Deserialize<NetEaseReturn_Pic>(LInterface.GetResponse(LInterface.NetEaseMusic_Album(albumID)));
                b_PicUrl = new Uri(nerp.songs[0].al.picUrl);

            }
            else if (Src == LInterface.LSearchChoice.QQMusic)
            {
                // 获取Token，再获取下载地址
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri(LInterface.QQMusic_Token(QMid)));
                req.Headers.Add("Cookie", "skey=@Y3TD47qBo");
                req.Referer = "https://y.qq.com/portal/player.html";
                QQMusicReturn_Token qmrt;
                using (Stream stream = req.GetResponse().GetResponseStream())
                {
                    StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                    qmrt = LInterface.Deserialize<QQMusicReturn_Token>(sr.ReadToEnd());
                    sr.Close();
                    stream.Close();
                }

                b_FileUrl = new Uri(LInterface.QQMusic_Download(QMid, qmrt.data.items[0].vkey));
                //获取歌词
                QQMusicReturn_Lyric qmrl;
                req = (HttpWebRequest)WebRequest.Create(new Uri(LInterface.QQMusic_Token(QMid)));
                req.Headers.Add("Cookie", "skey=@Y3TD47qBo");
                req.Referer = "https://y.qq.com/portal/player.html";
                using (Stream stream = req.GetResponse().GetResponseStream())
                {
                    StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                    qmrl = LInterface.Deserialize<QQMusicReturn_Lyric>(sr.ReadToEnd());
                    sr.Close();
                    stream.Close();
                }
                if (qmrl.lyric == null) b_SongLyric = "";
                else SongLyric = Encoding.UTF8.GetString(Convert.FromBase64String(qmrl.lyric));
                if (qmrl.trans != null) b_SongLyric += Encoding.UTF8.GetString(Convert.FromBase64String(qmrl.trans));
                b_OnlinePlayUrl = new Uri(LInterface.QQMusic_Play(QMid));
            }
            else
            {
                // 取得详细信息
                KGRS_Detail kgrs;
                CookieContainer cookie = new CookieContainer();
                cookie.Add(new Cookie("kg_mid", LInterface.GetKGMid(), "/", "kugou.com"));
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(LInterface.KuGouMusic_Detail(QMid));
                req.CookieContainer = cookie;
                using (Stream stream = req.GetResponse().GetResponseStream())
                {
                    StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                    kgrs = LInterface.Deserialize<KGRS_Detail>(sr.ReadToEnd());
                    sr.Close(); sr.Dispose();
                }
                b_FileUrl = new Uri(kgrs.data.play_url);
                b_PicUrl = new Uri(kgrs.data.img);
                b_OnlinePlayUrl = new Uri(LInterface.KuGouMusic_OnlineAddr(QMid));
                b_SongLyric = kgrs.data.lyrics;
            }
            ((Action)obj).Invoke();
        }

        /// <summary>
        /// 提供歌词搜索结果，自动搜索专辑封面，歌词，网易版
        /// </summary>
        /// <param name="nerls">网易云歌词搜索结果集合</param>
        public LSearchResult(NERS_Single nerls)
        {
            Src = LInterface.LSearchChoice.NetEase;
            b_SongName = nerls.name;
            b_SongArtist = nerls.artists[0].name;
            b_SongAlbum = nerls.album.name;
            b_SongDuration = (nerls.duration / 1000).ToString();
            b_OnlinePlayUrl = new Uri(LInterface.NetEaseMusic_Play(nerls.id));
            QMid = nerls.id.ToString();
            albumID = nerls.album.id;
        }

        /// <summary>
        /// 提供歌词搜索结果，自动搜索专辑封面，歌词，QQ版
        /// </summary>
        /// <param name="qmrss">QQ音乐歌词搜索结果</param>
        public LSearchResult(QMRS_Single qmrss)
        {
            Src = LInterface.LSearchChoice.QQMusic;
            // SongID 作为专辑 mid
            b_SongName = qmrss.songname;
            b_SongAlbum = qmrss.albumname;
            b_SongDuration = qmrss.interval.ToString();
            b_SongArtist = qmrss.singer[0].name;
            SongID = qmrss.albumid;
            QMid = qmrss.songmid;
            b_PicUrl = new Uri(LInterface.QQMusic_Album(SongID));
        }

        /// <summary>
        /// 提供歌词搜索结果，自动搜索专辑封面，歌词，酷狗版
        /// </summary>
        /// <param name="ners">酷狗音乐歌词搜索结果</param>
        public LSearchResult(KGRS_Single kgrss)
        {
            Src = LInterface.LSearchChoice.KuGouMusic;
            // QMid 作为 hash
            b_SongName = kgrss.songname;
            b_SongAlbum = kgrss.album_name;
            b_SongArtist = kgrss.singername;
            b_SongDuration = kgrss.duration.ToString();
            QMid = kgrss.hash;
        }
    }

    public class LSearchEngine
    {
        public Thread UpdateSearchResultThread;
        public SearchResultUpdater SearchResultUpdateHandler;
        bool isSeachCanceled;
        LInterface.LSearchChoice mSearchChoice;
        string mKeyWord;

        /// <summary>
        /// 搜索引擎的默构造方法
        /// </summary>
        /// <param name="SearchUpdater">用于实现搜索结果更新的委托函数</param>
        public LSearchEngine(SearchResultUpdater SearchUpdater)
        {
            SearchResultUpdateHandler += SearchUpdater;
            isSeachCanceled = false;
        }

        /// <summary>
        /// 开始搜索，并在出现新的搜索结果时调用委托函数进行更新，每一页返回 15 个结果
        /// </summary>
        /// <param name="KeyWord">要搜索的关键字</param>
        /// <param name="searchchoice">搜索引擎的选择</param>
        public void BeginSearch(string KeyWord, LInterface.LSearchChoice searchchoice)
        {
            isSeachCanceled = false;
            mSearchChoice = searchchoice;
            mKeyWord = KeyWord;
            UpdateSearchResultThread = new Thread(new ThreadStart(OnThreadWork));
            UpdateSearchResultThread.Start();

        }

        /// <summary>
        /// 取消正在进行的搜索
        /// </summary>
        public void CancelSearch()
        {
            if (UpdateSearchResultThread.IsAlive) isSeachCanceled = true;
        }

        void OnThreadWork()
        {
            if (mSearchChoice == LInterface.LSearchChoice.NetEase)
            {
                NetEaseReturn_Song ners = LInterface.Deserialize<NetEaseReturn_Song>(LInterface.GetResponse(LInterface.NetEaseMusic_Search(1, mKeyWord)));
                if (isSeachCanceled) { SearchResultUpdateHandler?.Invoke(null, true); return; }
                foreach (NERS_Single nerss in ners.result.songs)
                {
                    if (isSeachCanceled) { SearchResultUpdateHandler?.Invoke(null, true); return; }
                    try { SearchResultUpdateHandler?.Invoke(new LSearchResult(nerss)); }
                    catch { continue; }
                }
            }
            else if (mSearchChoice == LInterface.LSearchChoice.QQMusic)
            {
                QQMusicReturn_Song qmrs = LInterface.Deserialize<QQMusicReturn_Song>(LInterface.GetResponse(LInterface.QQMusic_Search(1, mKeyWord), true));
                if (isSeachCanceled) return;
                foreach (QMRS_Single qmrss in qmrs.data.song.list)
                {
                    if (isSeachCanceled) { SearchResultUpdateHandler?.Invoke(null, true); return; }
                    try { SearchResultUpdateHandler?.Invoke(new LSearchResult(qmrss)); }
                    catch { continue; }
                }
            }
            else
            {
                KGMusicReturn_Song kgrs = LInterface.Deserialize<KGMusicReturn_Song>(LInterface.GetResponse(LInterface.KuGouMusic_Search(1, mKeyWord)));
                if (isSeachCanceled) return;
                foreach (KGRS_Single kgrss in kgrs.data.info)
                {
                    if (isSeachCanceled) { SearchResultUpdateHandler?.Invoke(null, true); return; }
                    try { SearchResultUpdateHandler?.Invoke(new LSearchResult(kgrss)); }
                    catch { continue; }
                }

            }
            SearchResultUpdateHandler?.Invoke(null, true);
        }
    }
}
