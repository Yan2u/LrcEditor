using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;

namespace LrcEditor
{
    /// <summary>
    /// mEditTheme.xaml 的交互逻辑
    /// </summary>
    public partial class mEditTheme : UserControl
    {
        public LThemeCollcetion ThemeSet;
        public LColorCollection ColorSet;

        void LoadXml()
        {
            XmlSerializer xmls = new XmlSerializer(typeof(LThemeCollcetion));
            StreamReader sr = new StreamReader(Environment.CurrentDirectory + "\\ThemeConfig.xml", Encoding.UTF8);
            string content = sr.ReadToEnd();
            MemoryStream stream = new MemoryStream();
            StreamWriter sw = new StreamWriter(stream);
            sw.Write(content);
            sw.Flush();
            stream.Position = 0;
            ThemeSet = (LThemeCollcetion)xmls.Deserialize(stream);
            sw.Close();
            stream.Dispose();
            sr = new StreamReader(Environment.CurrentDirectory + "\\ColorConfig.xml", Encoding.UTF8);
            content = sr.ReadToEnd();
            stream = new MemoryStream();
            sw = new StreamWriter(stream);
            sw.Write(content);
            sw.Flush();
            stream.Position = 0;
            ColorSet =(LColorCollection) xmls.Deserialize(stream);
            sw.Close();
            sw.Dispose();
            stream.Dispose();
            mThemeList.ItemsSource = ThemeSet.ThemeSet;
        }

        public mEditTheme()
        {
            InitializeComponent();
            if (File.Exists(Environment.CurrentDirectory + "\\ThemeConfig.xml") == false)
            {
                StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + "\\ThemeConfig.xml", false, Encoding.UTF8);
                sw.Write(Properties.Resources.ThemeConfig);
                sw.Close();
            }

            if (File.Exists(Environment.CurrentDirectory + "\\ColorConfig.xml") == false)
            {
                StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + "\\ColorConfig.xml", false, Encoding.UTF8);
                sw.Write(Properties.Resources.ColorConfig);
                sw.Close();
            }
            LoadXml();
        }
    }
}
