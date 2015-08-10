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

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

using YoutubeExtractor;

namespace ytDownloader_GUI
{
    enum type
    {
        MP3,
        MP4,
        None,
    }
    enum ColumnData
    {
        channel = 0,
        Title = 1,
        Link = 2,
        Type = 3,
        dataCount = 4 ,
    }
    struct linkInfo
    {
        public string link;
        public type downloadType;
    }

    public partial class printingBox : Form
    {
        private string downloadDir = "Download/";
        private YouTubeService youtubeService;
        private string ytLink = "https://www.youtube.com/watch?v=";
        public printingBox()
        {
            InitializeComponent();
            try
            {
                StreamReader config = File.OpenText("config.cfg");
                while (!config.EndOfStream)
                {
                    string line = config.ReadLine();
                    if (line.Contains("downloadPath="))
                    {
                        int start = line.IndexOf("\"") + 1;
                        int end = line.LastIndexOf("\"");
                        string downloadPath = line.Substring(start, end - start);
                        downloadDir = downloadPath;
                        label_downloaddir.Text = downloadDir;
                    }
                }
                config.Close();
            }
            catch (FileNotFoundException)
            {
                StreamWriter configWrite = File.CreateText("config.cfg");
                MessageBox.Show("Please select a download path!");
                save_download.ShowDialog();
                label_downloaddir.Text = save_download.SelectedPath;
                downloadDir = save_download.SelectedPath;
                configWrite.Write("downloadPath=" + "\" " + downloadDir + "\"");
                configWrite.Close();
            }

            button_download.Enabled = false;

            youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyDzqeoR5mWrtJJ1Qt1SL_DLRDJvVKStdSo",
                ApplicationName = this.GetType().ToString()
            });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private async void button_mp3_Click(object sender, EventArgs e)
        {
            string link = txtBox_link.Text;
            txtBox_link.Clear();
            var items = list_queue.Items;
            ListViewItem item = null;

            string prefix = comboBox_pick.SelectedItem.ToString();

            if (prefix == "mp3" || prefix == "mp4")
            {
                string compoundString = comboBox_pick.SelectedItem + " " + txtBox_link.Text;

                if (link.Contains("playlist"))
                {
                    string playlistId = link.Substring(link.LastIndexOf("=") + 1);
                    await loadPlayList(playlistId, toType(prefix));
                    button_download.Enabled = true;
                    return;
                }
                else if (link.Contains("user"))
                {
                    int indexstart = link.IndexOf("user/") + "user/".Length;
                    string username = link.Substring(indexstart, link.IndexOf("/", indexstart + 1) - indexstart);
                    await loadChannel(username, toType(prefix), false);
                    button_download.Enabled = true;

                    return;
                }
                else if (link.Contains("channel"))
                {
                    int indexStart = link.IndexOf("channel") + "channel/".Length;
                    int end = link.IndexOf("/", indexStart);
                    if (end == -1)
                    {
                        end = link.Length;
                    }
                    string channelID = link.Substring(indexStart,end - indexStart);
                    await loadChannel(channelID, toType(prefix), true);
                    button_download.Enabled = true;
                }
                if (!isFound(compoundString, items))
                {
                    var videoRequest = youtubeService.Videos.List("contentDetails, id, snippet");

                    string videoID = link.Substring(link.LastIndexOf("=")+1);
                    videoRequest.Id = videoID;

                    var videoResponse = await videoRequest.ExecuteAsync();
                    foreach (var video in videoResponse.Items)
                    {
                        string[] str = new string[4];
                        str[0] = video.Snippet.ChannelTitle;
                        str[1] = video.Snippet.Title;
                        str[2] = prefix;
                        str[3] = link;
                        item = list_queue.Items.Add(new ListViewItem(str));
                    }
                    
                }
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
            label_Items.Text = list_queue.Items.Count.ToString();
            button_download.Enabled = true;
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

        private void button_clearlist(object sender, EventArgs e)
        {
            list_queue.Items.Clear();
            label_Items.Text = list_queue.Items.Count.ToString();
            button_download.Enabled = false;
            downloader.CancelAsync();
        }

        private void button_startDownload(object sender, EventArgs e)
        {
            var items = list_queue.Items;

            List<ListViewItem> listItems = new List<ListViewItem>();
            for (int i = 0; i < items.Count; i++)
            {
                listItems.Add(items[i]);
            }

            button_download.Enabled = false;
            if (!downloader.IsBusy)
            {
                downloader.RunWorkerAsync(listItems);
            }
            
        }

        private void button_downloadDir(object sender, EventArgs e)
        {
            save_download.ShowDialog();
            label_downloaddir.Text = save_download.SelectedPath;
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

        private void download_progress_Click(object sender, EventArgs e)
        {

        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            var items = list_queue.SelectedItems;
            int count = items.Count;
            while (items.Count > 0 && items[0] != null)
            {
                list_queue.Items.Remove(items[0]);
            }
            label_Items.Text = list_queue.Items.Count.ToString();
        }

        private void downloader_DoWork(object sender, DoWorkEventArgs e)
        {
            var downloader = sender as BackgroundWorker;
            downloader.ProgressChanged += downloader_ProgressChanged;
            downloader.WorkerSupportsCancellation = true;
            List<ListViewItem> items = (List<ListViewItem>)e.Argument;
            var cnt = items.Count;
            downloader.WorkerReportsProgress = true;
            while (items.Count > 0 && !items[0].Equals(null))
            {
                linkInfo link;
                link.link = items[0].SubItems[3].Text;
                link.downloadType = toType(items[0].SubItems[2].Text);
                Download(link, downloadDir);
                items.RemoveAt(0);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void downloader_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            download_progress.Value = e.ProgressPercentage;
            var items = list_queue.Items;
            if (items.Count > 0 && download_progress.Value > 99)
            {
                
                items.RemoveAt(0);
                label_Items.Text = items.Count.ToString();
            }
        }
        private void downloader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }
        /*
        * Converts string to type
        */
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
        static string typeToString(type type)
        {
            switch (type)
            {
                case type.MP3:
                    return "mp3";
                    break;
                case type.MP4:
                    return "mp4";
                    break;
                case type.None:
                    return "none";
                    break;
                default:
                    return " ";
                    break;
            }
        }
        /*
        * Downloads the link to the dir
        */
        private void Download(linkInfo info, string dir)
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
        private void DownloadMP3(string link, string downloadDir)
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
                downloader.ReportProgress((int)argss.ProgressPercentage);

            };
            audioDownloader.DownloadFinished += (sender, argss) =>
            {
                

            };

            audioDownloader.Execute();
        }
        private void DownloadMP4(string link, string downloadDir)
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
                downloader.ReportProgress((int)argss.ProgressPercentage);

            };

            videoDownloader.Execute();
        }
        /*
        * Removes illegall path characters
        */
        private static string RemoveIllegalPathCharacters(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(path, "");
        }

        async Task loadPlayList(string playlistID, type type)
        {
            
            var pageToken = "";
            while (pageToken != null)
            {
                var playlistRequest = youtubeService.PlaylistItems.List("contentDetails, id, snippet");
                playlistRequest.PlaylistId = playlistID;
                playlistRequest.PageToken = pageToken;

                playlistRequest.MaxResults = 20;

                var playlistResponse = await playlistRequest.ExecuteAsync();
                foreach (var item in playlistResponse.Items)
                {
                    string[] str = new string[4];
                    str[0] = item.Snippet.ChannelTitle;
                    str[1] = item.Snippet.Title;
                    str[2] = typeToString(type);
                    str[3] = ytLink + item.Snippet.ResourceId.VideoId;
                    list_queue.Items.Add(new ListViewItem(str));

                }
                label_Items.Text = list_queue.Items.Count.ToString();
                pageToken = playlistResponse.NextPageToken;
            }
            
            
        }


        async Task loadChannel(string userName, type type,bool isID)
        {
            var channelItemsListRequest = youtubeService.Channels.List("contentDetails, snippet, id, statistics");
            if (!isID)
                channelItemsListRequest.ForUsername = userName;
            else
                channelItemsListRequest.Id = userName;

            var channelItemsListResponse = await channelItemsListRequest.ExecuteAsync();
            foreach (var channel in channelItemsListResponse.Items)
            {
                var pageToken = "";
                var uploadListID = channel.ContentDetails.RelatedPlaylists.Uploads;
                while (pageToken != null)
                {
                    var playlistItemsRequest = youtubeService.PlaylistItems.List("snippet");
                    playlistItemsRequest.PlaylistId = uploadListID;
                    playlistItemsRequest.MaxResults = 20;
                    playlistItemsRequest.PageToken = pageToken;
                    var playlistItemsResponse = await playlistItemsRequest.ExecuteAsync();
                    foreach (var video in playlistItemsResponse.Items)
                    {
                        string contentDeta = ytLink + video.Snippet.ResourceId.VideoId;
                        
                        
                        string[] str = new string[4];
                        str[1] = video.Snippet.Title;
                        str[0] = video.Snippet.ChannelTitle;
                        str[3] = contentDeta;
                        str[2] = typeToString(type);
                        ListViewItem item = new ListViewItem(str);
                        list_queue.Items.Add(item);
                    }
                    pageToken = playlistItemsResponse.NextPageToken;
                    label_Items.Text = list_queue.Items.Count.ToString();
                }
            }
        }
        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label_Items_Click(object sender, EventArgs e)
        {

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
