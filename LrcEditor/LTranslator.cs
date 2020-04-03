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
        CookieContainer container;
        public static string GoogleTransBaseUrl = "https://translate.google.cn/";
        string TKK;
        string[] content;
        public bool CanTranslate;

        string GetTransResult(string contents)
        {
            string tk = CalculateTK(contents);
            string googleTransUrl = string.Format("https://translate.google.cn/translate_a/single?client=webapp&sl={0}&tl=zh-CN&hl=zh-CN&dt=at&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t&source=bh&ssel=0&tsel=0&kc=1&tk={1}&q={2}", "auto", tk, contents);
            string ResultHtml = GetResultHtml(googleTransUrl, container, "https://translate.google.cn/");
            dynamic TempResult = Newtonsoft.Json.JsonConvert.DeserializeObject(ResultHtml);
            return TempResult[0][0][0];
        }

        void TranslateWork()
        {
            try
            {
                foreach (string str in content)
                {
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

        public static string ExecuteScript(string sExpression, string sCode)
        {
            MSScriptControl.ScriptControl scriptControl = new MSScriptControl.ScriptControl();
            scriptControl.UseSafeSubset = true;
            scriptControl.Language = "JScript";
            scriptControl.AddCode(sCode);
            try
            {
                string str = scriptControl.Eval(sExpression).ToString();
                return str;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return null;
        }
        string CalculateTK(string text)
        {
            string tk = ExecuteScript("tk(\"" + text + "\",\"" + TKK + "\")", Properties.Resources.JSCode);
            return tk;
        }
        string GetResultHtml(string url, CookieContainer cookie, string referer)
        {
            var html = "";
            var webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = "GET";
            webRequest.CookieContainer = cookie;
            webRequest.Referer = referer;
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
            return html;
        }

        void GetTkk()
        {
            // 获得 TKK 值
            CookieContainer cc = new CookieContainer();
            string BaseResultHtml = GetResultHtml(GoogleTransBaseUrl, cc, "");
            int st_index = BaseResultHtml.IndexOf("tkk:'");
            int cnt = 0; string res = "";
            for (int i = st_index; i <= BaseResultHtml.Length - 1; i++)
            {
                if (BaseResultHtml[i] != '\'' && cnt >= 1) res += BaseResultHtml[i];
                if (BaseResultHtml[i] == '\'') cnt++;
                if (cnt >= 2) break;
            }
            TKK = res; container = cc;
            CanTranslate = true;
        }

        public LTranslator()
        {
            GetTKKThread = new Thread(new ThreadStart(GetTkk));
            GetTKKThread.Start();
        }
        
        public void StartTranslate(string _content)
        {
            if (GetTransResultThread != null && GetTransResultThread.IsAlive) return;
            content = Regex.Split(_content, "\r\n");
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
