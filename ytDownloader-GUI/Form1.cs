using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using ytDownloader_GUI.Properties;
using YoutubeExtractor;

namespace ytDownloader_GUI
{
    public enum Type
    {
        Mp3,
        Mp4,
        None
    }

    public enum Quality
    {
        Low,
        Medium,
        High,
        SuperHigh

    }
    enum ColumnData
    {
        Channel = 0,
        Title = 1,
        Link = 2,
        Type = 3,
        Quality = 4,
        DataCount = 5
    }
    struct LinkInfo
    {
        LinkInfo(string link, Type type, Quality info)
        {
            Link = link;
            DownloadType = type;
            QualityInfo = info;
        }
        public string Link;
        public Type DownloadType;
        public Quality QualityInfo;
    }

    public partial class PrintingBox : Form
    {
        private string _downloadDir = "Download/";
        private YouTubeService _youtubeService;
        private const string YtLink = "https://www.youtube.com/watch?v=";
        public PrintingBox()
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
                        _downloadDir = downloadPath;
                        label_downloaddir.Text = _downloadDir;
                    }
                }
                config.Close();
            }
            catch (FileNotFoundException)
            {
                StreamWriter configWrite = File.CreateText("config.cfg");
                MessageBox.Show(Resources.PrintingBox_PrintingBox_Please_select_a_download_path_);
                save_download.ShowDialog();
                label_downloaddir.Text = save_download.SelectedPath;
                _downloadDir = save_download.SelectedPath;
                configWrite.Write("downloadPath=" + "\" " + _downloadDir + "\"");
                configWrite.Close();
            }

            button_download.Enabled = false;

            _youtubeService = new YouTubeService(new BaseClientService.Initializer()
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
                    await LoadPlayList(playlistId, toType(prefix));
                    button_download.Enabled = true;
                    return;
                }
                else if (link.Contains("user"))
                {
                    int indexstart = link.IndexOf("user/") + "user/".Length;
                    string username = link.Substring(indexstart, link.IndexOf("/", indexstart + 1) - indexstart);
                    await LoadChannel(username, toType(prefix), false);
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
                    string channelID = link.Substring(indexStart, end - indexStart);
                    await LoadChannel(channelID, toType(prefix), true);
                    button_download.Enabled = true;
                }
                if (!isFound(compoundString, items))
                {
                    var videoRequest = _youtubeService.Videos.List("contentDetails, id, snippet");

                    string videoID = link.Substring(link.LastIndexOf("=") + 1);
                    videoRequest.Id = videoID;

                    var videoResponse = await videoRequest.ExecuteAsync();
                    foreach (var video in videoResponse.Items)
                    {
                        string[] str = new string[5];
                        str[0] = video.Snippet.ChannelTitle;
                        str[1] = video.Snippet.Title;
                        str[2] = prefix;
                        str[3] = link;
                        str[4] = cmbQualitySetting.Text;
                        if (cmbQualitySetting.Text == "")
                        {
                            str[4] = "Low";
                        }


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
            _downloadDir = save_download.SelectedPath;
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
                LinkInfo link;
                link.Link = items[0].SubItems[3].Text;
                link.DownloadType = toType(items[0].SubItems[2].Text);
                link.QualityInfo = ToQualityType(items[0].SubItems[4].Text);
                Download(link, _downloadDir);
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
        static Type toType(string input)
        {
            if (input.ToLower().Equals("mp3"))
            {
                return Type.Mp3;
            }
            else if (input.ToLower().Equals("mp4"))
            {
                return Type.Mp4;
            }
            return Type.None;
        }
        static string typeToString(Type type)
        {
            switch (type)
            {
                case Type.Mp3:
                    return "mp3";
                    break;
                case Type.Mp4:
                    return "mp4";
                    break;
                case Type.None:
                    return "none";
                    break;
                default:
                    return " ";
                    break;
            }
        }

        private Quality ToQualityType(string input)
        {
            Quality info = Quality.Low;

            if (input == "Low")
            {
                info = Quality.Low;
            }
            else if (input == "Medium")
            {
                info = Quality.Medium;
            }
            else if (input == "High")
            {
                info = Quality.High;
            }
            else if (input == "Super High")
            {
                info = Quality.SuperHigh;
            }

            return info;
        }

        private string QualityTypeToString(Quality input)
        {
            string output;
            switch (input)
            {
                case Quality.Low:
                    output = "Low";
                    break;
                case Quality.Medium:
                    output = "Medium";
                    break;
                case Quality.High:
                    output = "High";
                    break;
                case Quality.SuperHigh:
                    output = "SuperHigh";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(input), input, null);
            }
            return output;
        }
        /*
        * Downloads the link to the dir
        */
        private void Download(LinkInfo info, string dir)
        {
            switch (info.DownloadType)
            {
                case Type.Mp3:
                    DownloadMp3(info, dir);
                    break;
                case Type.Mp4:
                    DownloadMp4(info, dir);
                    break;
                case Type.None:
                    break;
                default:
                    break;
            }
        }
        private void DownloadMp3(LinkInfo link, string downloadDir)
        {
            Directory.CreateDirectory(downloadDir);
            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link.Link);

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
        private void DownloadMp4(LinkInfo link, string downloadDir)
        {
            Directory.CreateDirectory(downloadDir);

            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link.Link);

            VideoInfo video;
            switch (link.QualityInfo)
            {
                case Quality.Low:
                    video = videoInfos.Where(info => info.Resolution < 480).OrderByDescending(info => info.Resolution).First();
                    break;
                case Quality.Medium:
                    video = videoInfos.Where(info => info.Resolution < 720).OrderByDescending(info => info.Resolution).First();
                    break;
                case Quality.High:
                    video = videoInfos.Where(info => info.Resolution < 1080).OrderByDescending(info => info.Resolution).First();
                    break;
                case Quality.SuperHigh:
                    video = videoInfos.OrderByDescending(info => info.Resolution).First();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }

            var videoDownloader = new VideoDownloader(video, Path.Combine(downloadDir, RemoveIllegalPathCharacters(video.Title) + "-" + video.Resolution + video.VideoExtension));
            videoDownloader.DownloadStarted += (sender, argss) => Console.WriteLine("Started downloading " + video.Title + "-" + video.Resolution + video.VideoExtension);

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

        async Task LoadPlayList(string playlistID, Type type)
        {

            var pageToken = "";
            while (pageToken != null)
            {
                var playlistRequest = _youtubeService.PlaylistItems.List("contentDetails, id, snippet");
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
                    str[3] = YtLink + item.Snippet.ResourceId.VideoId;
                    list_queue.Items.Add(new ListViewItem(str));

                }
                label_Items.Text = list_queue.Items.Count.ToString();
                pageToken = playlistResponse.NextPageToken;
            }


        }


        async Task LoadChannel(string userName, Type type, bool isID)
        {
            var channelItemsListRequest = _youtubeService.Channels.List("contentDetails, snippet, id, statistics");
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
                    var playlistItemsRequest = _youtubeService.PlaylistItems.List("snippet");
                    playlistItemsRequest.PlaylistId = uploadListID;
                    playlistItemsRequest.MaxResults = 20;
                    playlistItemsRequest.PageToken = pageToken;
                    var playlistItemsResponse = await playlistItemsRequest.ExecuteAsync();
                    foreach (var video in playlistItemsResponse.Items)
                    {
                        string contentDeta = YtLink + video.Snippet.ResourceId.VideoId;


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

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            button_add.Enabled = true;
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
