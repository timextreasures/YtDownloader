using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeExtractor;

namespace ytDownloader_GUI
{
    enum type
    {
        MP3,
        MP4,
        None,
    }
    struct linkInfo
    {
        public string link;
        public type downloadType;
    }

    public partial class printingBox : Form
    {
        private string downloadDir = "Download/";
        
        public printingBox()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string link = txtBox_link.Text;
            

            var items = list_queue.Items;
            ListViewItem item = null;
            string compoundString = "mp3 " + txtBox_link.Text;
            
            if (!isFound(compoundString,items))
            {
                item = list_queue.Items.Add(compoundString);
            }
            try
            {
                using (var client = new MyClient())
                {
                    client.HeadOnly = true;
                    string s1 = client.DownloadString(link);
                }
            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.Message);
                list_queue.Items.Remove(item);
                return;
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void list_queue_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button_addmp4_Click(object sender, EventArgs e)
        {
            string link = txtBox_link.Text;
            var items = list_queue.Items;
            ListViewItem item = null;
            string compoundString = "mp4 " + txtBox_link.Text;
            if (!isFound(compoundString, items))
            {
                item = list_queue.Items.Add(compoundString);
            }
            
            try
            {
                using (var client = new MyClient())
                {
                    client.HeadOnly = true;
                    string s1 = client.DownloadString(link);
                }
            }
            catch (Exception ek)
            {
                MessageBox.Show(ek.Message);
                list_queue.Items.Remove(item);
                return;
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            list_queue.Items.Clear();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            var items = list_queue.Items;
            int amount = items.Count;
            progress_label.Visible = true;
            for (int i = 0; i < items.Count; i++)
            {
                string input = items[i].Text;
                string type = input.Substring(0, input.IndexOf(" "));
                type actualType = toType(type);

                string link = input.Substring(type.Length+1);

                linkInfo info;
                info.link = link;
                info.downloadType = actualType;
                progress_label.Text = i+1 + "/" + amount;
                progress_label.Update();
                Download(info, downloadDir).Wait();
                
                
            }
            items.Clear();
            progress_label.Visible = false;
        }

        private void button1_Click_3(object sender, EventArgs e)
        {
            save_download.ShowDialog();
            downloadDir = save_download.SelectedPath;
        }

        private void save_download_HelpRequest(object sender, EventArgs e)
        {
            
        }

        bool isFound(string compoundString, ListView.ListViewItemCollection items)
        {
            bool found = false;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Text == compoundString)
                {
                    found = true;
                }
            }
            return found;
        }

        static type toType(string input)
        {
            if (input.ToLower().Equals("mp3"))
            {
                return type.MP3;
            }
            else if (input.ToLower().Equals("mp4"))
            {
                return type.MP4;
            }
            return type.None;
        }

        async Task Download(linkInfo info, string dir)
        {
            switch (info.downloadType)
            {
                case type.MP3:
                    DownloadMP3(info.link, dir);
                    break;
                case type.MP4:
                    DownloadMP4(info.link, dir);
                    break;
                case type.None:
                    break;
                default:
                    break;
            }
        }

        void DownloadMP3(string link, string downloadDir)
        {
            Directory.CreateDirectory(downloadDir);
            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link);

            // Extracting stream with highest quality
            VideoInfo video = videoInfos.Where(info => info.CanExtractAudio).OrderByDescending(info => info.AudioBitrate).First();

            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }

            var audioDownloader = new AudioDownloader(video, Path.Combine(downloadDir, RemoveIllegalPathCharacters(video.Title) + video.AudioExtension));
            audioDownloader.DownloadProgressChanged += (sender, argss) =>
            {
                download_progress.Value = (int)argss.ProgressPercentage;

            };
            audioDownloader.DownloadFinished += (sender, argss) =>
            {
                download_progress.Value = 0;
                Console.WriteLine("\nFinished download " + video.Title + video.AudioExtension);
            };

            audioDownloader.Execute();
        }

        void DownloadMP4(string link, string downloadDir)
        {
            Directory.CreateDirectory(downloadDir);

            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link);

            VideoInfo video = videoInfos.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 720);

            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }

            var videoDownloader = new VideoDownloader(video, Path.Combine(downloadDir, RemoveIllegalPathCharacters(video.Title) + video.VideoExtension));
            videoDownloader.DownloadStarted += (sender, argss) => Console.WriteLine("Started downloading " + video.Title + video.VideoExtension);

            videoDownloader.DownloadProgressChanged += (sender, argss) =>
            {
                download_progress.Value = (int)argss.ProgressPercentage;

            };
            videoDownloader.DownloadFinished += (sender, argss) => download_progress.Value = 0;

            videoDownloader.Execute();
        }
        private static string RemoveIllegalPathCharacters(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(path, "");
        }

        private void download_progress_Click(object sender, EventArgs e)
        {
            
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            var items = list_queue.SelectedItems;
            ListView.ListViewItemCollection list = list_queue.Items;
            list.Clear();
            int count = items.Count;
            for (int i = 0; i < count; i++)
            {
                list.Add(items[i]);
            }
            for (int i = 0; i < list.Count; i++)
            {
                list_queue.Items.Remove(list[i]);
            }
        }

        private void downloader_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

        }
    }

    class MyClient : WebClient
    {
        public bool HeadOnly { get; set; }
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest req = base.GetWebRequest(address);
            if (HeadOnly && req.Method == "GET")
            {
                req.Method = "HEAD";
            }
            return req;
        }
    }
}
