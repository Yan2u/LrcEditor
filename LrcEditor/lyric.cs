using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LrcEditor
{
    public class TimeLine : IComparable
    {
        public int sec, multisec, minute;

        public TimeLine() { sec = 0; multisec = 0; minute = 0; }

        public TimeLine(int A, int B, int C) { minute = A; sec = B; multisec = C; }

        public TimeLine(string sformat)
        {
            string[] _sformat = sformat.Split(':');
            string[] sformat_ = _sformat[1].Split('.');
            minute = int.Parse(_sformat[0]);
            sec = int.Parse(sformat_[0]);
            while (sformat_[1].Length > 2)
            {
                if (sformat_[1][sformat_[1].Length - 1] != '0') break;
                sformat_[1] = sformat_[1].Remove(sformat_[1].Length - 1, 1);
            }
            multisec = int.Parse(sformat_[1]);
            minute %= 60;
            sec %= 60;
            multisec %= 100;
        }

        public int CompareTo(object x)
        {
            TimeLine tx = x as TimeLine;
            if (minute != tx.minute) return minute > tx.minute ? 1 : -1;
            if (sec != tx.sec) return sec > tx.sec ? 1 : -1;
            if (multisec != tx.multisec) return multisec > tx.multisec ? 1 : -1;
            return 0;
        }

        public static bool operator <(TimeLine t1, TimeLine t2)
        {
            if (t1.minute != t2.minute) return t1.minute > t2.minute ? false : true;
            if (t1.sec != t2.sec) return t1.sec > t2.sec ? false : true;
            if (t1.multisec != t2.multisec) return t1.multisec > t2.multisec ? false : true;
            return false;
        }

        public static bool operator >(TimeLine t1, TimeLine t2)
        {
            if (t1.minute != t2.minute) return t1.minute < t2.minute ? false : true;
            if (t1.sec != t2.sec) return t1.sec < t2.sec ? false : true;
            if (t1.multisec != t2.multisec) return t1.multisec < t2.multisec ? false : true;
            return false;
        }

        public static bool operator ==(TimeLine t1, TimeLine t2)
        {
            return t1.sec == t2.sec && t1.multisec == t2.multisec && t1.multisec == t2.minute;
        }

        public static bool operator !=(TimeLine t1, TimeLine t2)
        {
            return t1.sec != t2.sec || t1.multisec != t2.multisec || t1.multisec != t2.minute;
        }

        public static TimeLine operator +(TimeLine t1, TimeLine t2)
        {
            TimeLine tres = new TimeLine();
            tres.multisec = t1.multisec + t2.multisec;
            tres.sec += tres.multisec / 100; tres.multisec %= 100;
            tres.sec += t1.sec + t2.sec;
            tres.minute += tres.sec / 60; tres.sec %= 60;
            tres.minute += t1.minute + t2.minute;
            return tres;
        }

        public static TimeLine operator -(TimeLine t1, TimeLine t2)
        {
            if (t1 < t2 || t1 == t2) return new TimeLine(0, 0, 0);
            TimeLine tres = new TimeLine();
            tres.multisec = t1.multisec - t2.multisec;
            if (tres.multisec < 0) { tres.multisec += 100; tres.sec--; }
            tres.sec += t1.sec - t2.sec;
            if (tres.sec < 0) { tres.sec += 60; tres.minute--; }
            tres.minute += t1.minute - t2.minute;
            return tres;
        }

        public override string ToString()
        {
            return string.Format("{0:D2}:{1:D2}.{2:D2}", minute, sec, multisec);
        }

        public double ToSeconds()
        {
            return (double)minute * 60 + (double)sec + (double)multisec / 100.0;
        }
    }

    public class Lyric : IComparable, INotifyPropertyChanged
    {
        public TimeLine timeline;
        public event PropertyChangedEventHandler PropertyChanged;
        string word;

        public Lyric() { timeline = new TimeLine(); word = ""; }

        public Lyric(string Tdescription, string _content = ""){
            Timeline = Tdescription;
            Word = _content.Trim();
        }

        public string Timeline
        {
            get { return timeline.ToString(); }
            set { UpdateProperty(ref timeline, new TimeLine(value)); }
        }
        public string Word
        {
            get { return word; }
            set { UpdateProperty(ref word, value.Trim()); }
        }

        public int CompareTo(object x)
        {
            Lyric lx = x as Lyric;
            if (timeline != lx.timeline) return timeline < lx.timeline ? -1 : 1;
            if (Word != lx.Word) return string.Compare(Word, lx.Word);
            return 0;
        }

        void UpdateProperty<T>(ref T properVal, T newVal, [CallerMemberName] string propertyName = "")
        {
            if (Equals(properVal, newVal)) return;
            properVal = newVal;
            OnPropertyChanged(propertyName); // 触发 PropertyChanged
        }

        void OnPropertyChanged([CallerMemberName] string c_infomation = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(c_infomation));
        }
    }

    public class LyricCollection : INotifyPropertyChanged
    {
        ObservableCollection<Lyric> lrcCollection;
        public event PropertyChangedEventHandler PropertyChanged;
        void UpdateProperty<T>(ref T properVal, T newVal, [CallerMemberName] string propertyName = "")
        {
            if (Equals(properVal, newVal)) return;
            properVal = newVal;
            OnPropertyChanged(propertyName); // 触发 PropertyChanged
        }
        void OnPropertyChanged([CallerMemberName] string c_infomation = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(c_infomation));
        }

        public ObservableCollection<Lyric> mLrcList
        {
            get { return lrcCollection; }
            set { UpdateProperty(ref lrcCollection, value); }
        }
        public LyricCollection()
        {
            mLrcList = new ObservableCollection<Lyric>();
        }

        /// <summary>
        /// 从给定的歌词文本中取得歌词
        /// </summary>
        /// <param name="LrcStr">歌词文本</param>
        /// <param name="isAddLrc">为false则清空当前歌词，为true则追加到当前歌词中</param>
        public void ImportLyrics(string LrcStr, bool isAddLrc=false)
        {
            if (!isAddLrc) mLrcList.Clear();
            int nowPos = 0;
            int nowLeft = 0, nowRight = 0, nxtLeft = 0;
            while (nowPos < LrcStr.Length)
            {
                nowLeft = LrcStr.IndexOf('[', nowPos);
                if (nowLeft < 0) break;
                nowRight = LrcStr.IndexOf(']', nowPos);
                try { TimeLine tl = new TimeLine(LrcStr.Substring(nowLeft + 1, nowRight - nowLeft - 1)); }
                catch { nowPos = nowRight + 1; continue; }
                nxtLeft = LrcStr.IndexOf('[', nowRight);
                if (nxtLeft < 0) nxtLeft = LrcStr.Length;
                mLrcList.Add(new Lyric(LrcStr.Substring(nowLeft + 1, nowRight - nowLeft - 1),
                    LrcStr.Substring(nowRight + 1, nxtLeft - nowRight - 1)));
                nowPos = nowRight + 1;
            }
            
        }

        /// <summary>
        /// 从给定的文件中读取歌词
        /// </summary>
        /// <param name="fi">给定文件的 fileinfo </param>
        public void ImportLyrics(FileInfo fi)
        {
            StreamReader sr;
            while (true)
            {
                try {  sr = new StreamReader(fi.FullName, Encoding.UTF8); break; }
                catch { }
            }
            string content = sr.ReadToEnd();
            sr.Close();
            if (content == "") { mLrcList.Clear(); return; }
            if (content.IndexOf("�") >= 0)
            {
                sr = new StreamReader(fi.FullName, Encoding.Default);
                content = sr.ReadToEnd();
                sr.Close();
            }
            ImportLyrics(content);
        }

        /// <summary>
        /// 以 UTF-8 的编码存储歌词文件
        /// </summary>
        /// <param name="fi">给定文件的 fileinfo </param>
        public void SaveLyrics(FileInfo fi)
        {
            StreamWriter sw = new StreamWriter(fi.FullName, false, Encoding.UTF8);
            foreach(Lyric lrc in mLrcList)
            {
                sw.WriteLine(string.Format("[{0}]{1}", lrc.timeline.ToString(), lrc.Word));
            }
            sw.Close(); sw.Dispose();
        }
        
    }
}
