using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CSCore;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using MaterialDesignThemes.Wpf;
using TagLib;
using System.Windows.Media.Imaging;
using System.IO;
using System.Resources;
using System.Drawing;

namespace LrcEditor
{
    public class PlayerModule : IDisposable
    {
        private ISoundOut _soundOut;
        private IWaveSource _waveSource;
        private double volumeHandle = 100;  // control volume when _soundOut == null
        public Thread PositionUpdateThread;
        public event EventHandler<PlaybackStoppedEventArgs> PlaybackStopped;
        public string SongName { get; set; }
        public string SongArt { get; set; }
        public BitmapImage SongImg { get; set; }

        public double TimePercent
        {
            get { return Position.TotalMilliseconds / Length.TotalMilliseconds * 100.0; }
            set { Position = TimeSpan.FromMilliseconds(value / 100.0 * Length.TotalMilliseconds); }
        }

        public PlaybackState PlaybackState
        {
            get
            {
                if (_soundOut != null)
                    return _soundOut.PlaybackState;
                return PlaybackState.Stopped;
            }
        }

        public TimeSpan Position
        {
            get
            {
                if (_waveSource != null)
                    return _waveSource.GetPosition();
                return TimeSpan.Zero;
            }
            set
            {
                if (_waveSource != null)
                {
                    if (value <= TimeSpan.Zero)
                        _waveSource.SetPosition(TimeSpan.Zero);
                    else if (value >= Length)
                        _waveSource.SetPosition(Length);
                    else
                        _waveSource.SetPosition(value);

                }

            }
        }

        public TimeSpan Length
        {
            get
            {
                if (_waveSource != null)
                    return _waveSource.GetLength();
                return TimeSpan.Zero;
            }
        }

        public double Volume
        {
            get
            {
                if (_soundOut == null)
                {
                    return volumeHandle;
                }
                else
                {
                    return Math.Min(100, Math.Max((_soundOut.Volume * 100), 0));
                }
            }
            set
            {
                if (_soundOut == null)
                {
                    volumeHandle = value;
                }
                else
                {
                    volumeHandle = value;
                    _soundOut.Volume = Math.Min(1.0f, Math.Max((float)value / 100f, 0f));
                }
            }
        }

        public static BitmapImage ByteToImg(byte[] data)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream(data);
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = stream;
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.EndInit();
            stream.Dispose();
            return img;
        }

        public static BitmapImage BitmapToImg(System.Drawing.Bitmap bitmap)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            BitmapImage img = new BitmapImage();
            bitmap.Save(stream, bitmap.RawFormat);
            img.BeginInit();
            img.StreamSource = stream;
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.EndInit();
            stream.Dispose();
            return img;
        }

        public void Open(string filename)
        {
            CleanupPlayback();
            SongName = System.IO.Path.GetFileName(filename);
            try
            {
                TagLib.File tf = TagLib.File.Create(filename);
                SongArt = tf.Tag.Performers[0];
                SongName = tf.Tag.Title;
                if (tf.Tag.Pictures.Length > 0) SongImg = ByteToImg(tf.Tag.Pictures[0].Data.Data);
                else
                {
                    Random rd = new Random();
                    int index = rd.Next(1, 6);
                    SongImg = ByteToImg(Properties.Resources.ResourceManager.GetObject("_" + index.ToString()) as byte[]);
                }
    }
            catch
            {
                SongName = Path.GetFileNameWithoutExtension(filename);
                SongArt = "Unknown";
                Random rd = new Random();
                int index = rd.Next(1, 6);
                SongImg = ByteToImg(Properties.Resources.ResourceManager.GetObject("_" + index.ToString()) as byte[]);
            }
            try
            {
                _waveSource =
                    CodecFactory.Instance.GetCodec(filename)
                        .ToSampleSource()
                        .ToStereo()
                        .ToWaveSource();
                _soundOut = new WasapiOut() { Latency = 100 };
                _soundOut.Initialize(_waveSource);
                _soundOut.Volume = Math.Min(1.0f, Math.Max((float)volumeHandle / 100f, 0f));
            }
            catch(Exception ex)
            {
                MessageBoxResult MR = MessageBox.Show(ex.Message);
                Environment.Exit(0);
            }
            if (PlaybackStopped != null) _soundOut.Stopped += PlaybackStopped;

            PositionUpdateThread = new Thread(() => {
                while (true)
                {
                    PositionChangedEvent?.Invoke(Position);
                    Thread.Sleep(10);
                }
            });
            PositionUpdateThread.Start();
        }

        public void Play()
        {
            if (_soundOut != null)
                _soundOut.Play();
        }

        public void Pause()
        {
            if (_soundOut != null)
                _soundOut.Pause();
        }

        public void Stop()
        {
            if (_soundOut != null)
                _soundOut.Stop();
            if (_waveSource != null)
                _waveSource.SetPosition(TimeSpan.Zero);
        }

        private void CleanupPlayback()
        {
            if (PositionUpdateThread != null)
            {
                PositionUpdateThread.Abort();
                PositionUpdateThread = null;
            }
            if (_soundOut != null)
            {
                _soundOut.Dispose();
                _soundOut = null;
            }
            if (_waveSource != null)
            {
                _waveSource.Dispose();
                _waveSource = null;
            }
            if (SongName != null)
            {
                SongName = null;
                SongArt = null;
                SongImg = null;
            }
        }

        public void Dispose()
        {
            CleanupPlayback();
        }

        public delegate void PositionChangedEventHandler(object sender);
        public event PositionChangedEventHandler PositionChangedEvent;

    }
}
