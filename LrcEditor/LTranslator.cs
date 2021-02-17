using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSScriptControl;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace LrcEditor
{
    delegate void TranslateUpdateHandlerLrc(Lyric newlrc);
    delegate void TransalteResultHandler(string content);
    class LTranslator
    {
        public Thread GetTransResultThread;
        public Thread GetTKKThread;
        public TransalteResultHandler OnTranslationGet;
        public TranslateUpdateHandlerLrc OnTransLrcGet;
        public static string GoogleTransBaseUrl = "http://translate.google.cn/translate_a/single?client=gtx&dt=t&dj=1&ie=UTF-8&sl=auto&tl=zh_CN&q=";
        string[] content;
        public bool CanTranslate;

        string GetTransResult(string contents)
        {
            string html = "";
            var webRequest = WebRequest.Create(GoogleTransBaseUrl + contents) as HttpWebRequest;
            webRequest.Method = "GET";
            webRequest.Timeout = 20000;
            webRequest.Headers.Add("X-Requested-With:XMLHttpRequest");
            webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36";
            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                using (var reader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
                {

                    html = reader.ReadToEnd();
                    reader.Close();
                    webResponse.Close();
                }
            }
            int index = html.IndexOf("\"trans\":") + 9;
            char ch = html[index];
            string result = "";
            while (ch != '\"')
            {
                result += ch;
                index++; ch = html[index];
            }

            return result;
        }

        void TranslateWork()
        {
            try
            {
                foreach (string str in content)
                {
                    if (str == "") continue;
                    string res = GetTransResult(str);
                    OnTranslationGet?.Invoke(res);
                }
            }
            catch
            {
                OnTranslationGet?.Invoke("Error");
            }
        }

        void TranslateLrcWork(object olc)
        {
            LyricCollection lc = (LyricCollection)olc;
            foreach(Lyric lrc in lc.mLrcList)
            {
                Lyric newlrc = new Lyric() { Word = lrc.Word, Timeline = lrc.Timeline };
                try
                {
                    newlrc.Word = GetTransResult(newlrc.Word);
                }
                catch
                {
                    newlrc.Word = "Error";
                    return;
                }
                OnTransLrcGet?.Invoke(newlrc);
            }
        }

        public LTranslator()
        {
            CanTranslate = true;
        }
        
        public void StartTranslate(string _content)
        {
            if (GetTransResultThread != null && GetTransResultThread.IsAlive) return;
            content = Regex.Split(_content, "\r|\n");
            GetTransResultThread = new Thread(new ThreadStart(TranslateWork));
            GetTransResultThread.Start();
        }

        public void StartTranlateLrc(LyricCollection lc)
        {
            if (GetTransResultThread != null && GetTransResultThread.IsAlive) return;
            GetTransResultThread = new Thread(new ParameterizedThreadStart(TranslateLrcWork));
            GetTransResultThread.Start(lc);
        }
    }
}
