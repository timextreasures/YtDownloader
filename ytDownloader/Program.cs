using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using YoutubeExtractor;


namespace ytDownloader
{

    class Program
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

        static private string downloadDirectory = "";
        static private string ytLink = "https://www.youtube.com/watch?v=";
        static private YouTubeService youtubeService = new YouTubeService();
        static void Main(string[] args)
        {

            try
            {
                Run();
            }
            catch (Exception xe)
            {
                Console.WriteLine(xe.Message);
                
            }
            Console.WriteLine("Press a key to continue...");
            Console.ReadKey();
        }

        static void Run()
        {
            StreamReader config = File.OpenText("config.cfg");
            if (config.EndOfStream)
            {
                Console.WriteLine("Failed to open config.cfg");
                Console.ReadKey();
                return;
            }

            while (!config.EndOfStream)
            {
                string extractedLine = config.ReadLine();
                if (extractedLine.Contains("download_path"))
                {
                    int startIndex = extractedLine.IndexOf("=") + 2;
                    int endIndex = extractedLine.IndexOf("\"", startIndex);
                    downloadDirectory = extractedLine.Substring(startIndex, endIndex - startIndex);
                }
            }
            config.Close();

            List<linkInfo> m_Links = new List<linkInfo>();



            youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyDzqeoR5mWrtJJ1Qt1SL_DLRDJvVKStdSo",
                ApplicationName = new Program().GetType().ToString()
            });
            /*
            * Main loop
            */
            while (true)
            {
                Console.Write("$ ");
                string input = Console.ReadLine();
                string link = "";
                type downloadType = type.None;
                string command;
                try
                {
                    command = input.Substring(0, input.IndexOf(" "));
                }
                catch (Exception)
                {
                    command = input;
                }

                bool directDownload = false;
                if (command.ToLower().Contains("download_mp4") || input.Contains("d-mp4"))
                {

                    int id = input.IndexOf(command) + command.Length;
                    link = input.Substring(id);
                    downloadType = type.MP4;
                    directDownload = true;
                }
                else if (command.ToLower().Contains("download_mp3") || command.Contains("d-mp3"))
                {
                    int id = input.IndexOf(command) + command.Length;
                    link = input.Substring(id);
                    downloadType = type.MP3;
                    directDownload = true;
                }
                else if (command.ToLower().Equals("help"))
                {
                    StreamReader reader = File.OpenText("README.txt");
                    while (!reader.EndOfStream)
                    {
                        Console.WriteLine(reader.ReadLine());
                    }
                    reader.Close();
                }
                else if (command.ToLower().Contains("add_mp3"))
                {
                    int id = input.IndexOf(command) + command.Length;
                    string tmpLink = input.Substring(id);
                    linkInfo tmpInfo;
                    tmpInfo.link = tmpLink;
                    tmpInfo.downloadType = type.MP3;
                    m_Links.Add(tmpInfo);

                }
                else if (command.ToLower().Contains("add_mp4"))
                {
                    int id = input.IndexOf(command) + command.Length;
                    string tmpLink = input.Substring(id);
                    linkInfo tmpInfo;
                    tmpInfo.link = tmpLink;
                    tmpInfo.downloadType = type.MP4;
                    m_Links.Add(tmpInfo);

                }
                else if (command.ToLower().Equals("list"))
                {
                    foreach (var item in m_Links)
                    {
                        IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(item.link);

                        Console.WriteLine(videoInfos.First().Title);
                    }
                }
                else if (command.ToLower().Equals("start"))
                {
                    DownloadPlaylist(m_Links, downloadDirectory);
                }
                else if (command.Contains("startPlaylist"))
                {
                    List<linkInfo> playlist = new List<linkInfo>();
                    string playlistFile = input.Substring(input.IndexOf(command) + command.Length + 1);

                    StreamReader tmpFile = File.OpenText(playlistFile);
                    string fileName = playlistFile.Substring(0, playlistFile.Length - 4);

                    while (!tmpFile.EndOfStream)
                    {
                        string line = tmpFile.ReadLine();
                        linkInfo tmpInfo = new linkInfo();
                        string tmpType = line.Substring(0, line.IndexOf(" "));
                        if (tmpType.ToLower() == "mp3")
                        {
                            tmpInfo.downloadType = type.MP3;
                        }
                        else if (tmpType.ToLower() == "mp4")
                        {
                            tmpInfo.downloadType = type.MP4;
                        }
                        int endOfType = line.IndexOf(" ");
                        string tmpLink = line.Substring(endOfType + 1);
                        tmpInfo.link = tmpLink;
                        playlist.Add(tmpInfo);
                    }
                    DownloadPlaylist(playlist, downloadDirectory + "/" + fileName);
                    Console.WriteLine("Finished downloading " + fileName);
                }
                else if (command.Equals("downloadChannel"))
                {
                    int firstSpace = input.IndexOf(" ");
                    int secondSpace = input.IndexOf(" ", firstSpace + 1);
                    string strtype = input.Substring(firstSpace + 1, secondSpace - firstSpace - 1);

                    type tmpType = type.None;
                    if (strtype.ToLower().Equals("mp3"))
                    {
                        tmpType = type.MP3;
                    }
                    else if (strtype.ToLower() == "mp4")
                    {
                        tmpType = type.MP4;
                    }

                    string channelname = input.Substring(secondSpace + 1);
                    try
                    {
                        new Program().downloadChannel(channelname, tmpType).Wait();
                    }
                    catch (AggregateException e)
                    {
                        foreach (var item in e.InnerExceptions)
                        {
                            Console.WriteLine(item.Message);
                        }

                    }

                }
                else if (command.Equals("downloadPlaylist"))
                {
                    int firstSpace = input.IndexOf(" ");
                    int secondSpace = input.IndexOf(" ", firstSpace + 1);
                    string strtype = input.Substring(firstSpace + 1, secondSpace - firstSpace - 1);

                    type tmpType = type.None;
                    if (strtype.ToLower().Equals("mp3"))
                    {
                        tmpType = type.MP3;
                    }
                    else if (strtype.ToLower() == "mp4")
                    {
                        tmpType = type.MP4;
                    }

                    string playlist = input.Substring(secondSpace + 1);
                    try
                    {
                        new Program().downloadPlaylist(playlist, tmpType).Wait();
                    }
                    catch (AggregateException e)
                    {
                        foreach (var item in e.InnerExceptions)
                        {
                            Console.WriteLine(item.Message);
                        }

                    }

                }

                if (directDownload == true)
                {
                    linkInfo info = new linkInfo();
                    info.link = link;
                    info.downloadType = downloadType;
                    Download(info, downloadDirectory);
                }

            }
        }
        static void Download(linkInfo info,string dir)
        {
            switch (info.downloadType)
            {
                case type.MP3:
                    Program.DownloadMP3(info.link, dir);
                    break;
                case type.MP4:
                    Program.DownloadMP4(info.link, dir);
                    break;
                case type.None:
                    break;
                default:
                    break;
            }
        }

        /*
        * Downloads all the uploads of 1 channel!
        */
        private async Task downloadChannel(string channelname, type type)
        {

            var channelListRequest = youtubeService.Channels.List("contentDetails, statistics, snippet");
            channelListRequest.ForUsername = channelname;

            var channelListResponse = await channelListRequest.ExecuteAsync();
            foreach (var channel in channelListResponse.Items)
            {
                var uploadListId = channel.ContentDetails.RelatedPlaylists.Uploads;
                var nextToken = "";
                while (nextToken != null)
                {
                    var playlistrequest = youtubeService.PlaylistItems.List("snippet");
                    playlistrequest.PlaylistId = uploadListId;
                    playlistrequest.MaxResults = 50;
                    playlistrequest.PageToken = nextToken;
                    var playlistItemsListResponse = await playlistrequest.ExecuteAsync();
                    foreach (var video in playlistItemsListResponse.Items)
                    {
                        Console.WriteLine("Download " + video.Snippet.Title);
                        switch (type)
                        {
                            case type.MP3:
                                DownloadMP3(ytLink + video.Snippet.ResourceId.VideoId,downloadDirectory);
                                break;
                            case type.MP4:
                                DownloadMP4(ytLink + video.Snippet.ResourceId.VideoId,downloadDirectory);
                                break;
                            case type.None:
                                break;
                            default:
                                break;
                        }
                    }

                    nextToken = channelListResponse.NextPageToken;
                }
            }
        }
        /*
        * Downloads a playlist
        */ 
        private async Task downloadPlaylist(string playlistID, type type)
        {

            var playlistRequest = youtubeService.PlaylistItems.List("contentDetails,id,snippet");
            playlistRequest.PlaylistId = playlistID;

            var playlistResponse = await playlistRequest.ExecuteAsync();
            foreach (var video in playlistResponse.Items)
            {
                Console.WriteLine("Downloading " + video.Snippet.Title);
                linkInfo info;
                info.downloadType = type;
                info.link = ytLink + video.Snippet.ResourceId.VideoId;
                Download(info, downloadDirectory);
            }
        }
        static void DownloadPlaylist(List<linkInfo> list, string dir)
        {
            foreach (var item in list)
            {
                switch (item.downloadType)
                {
                    case type.MP3:
                        Program.DownloadMP3(item.link, dir);
                        break;
                    case type.MP4:
                        Program.DownloadMP4(item.link, dir);
                        break;
                    case type.None:
                        break;
                    default:
                        break;
                }
            }
        }
        private static string RemoveIllegalPathCharacters(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(path, "");
        }
        static void DownloadMP3(string link, string downloadDir)
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
            int ticks = 0;
            audioDownloader.DownloadProgressChanged += (sender, argss) =>
            {
                ticks++;

                if (ticks > 1000)
                {
                    Console.Write("#");
                    ticks -= 1000;
                }
                
            };
            audioDownloader.DownloadFinished += (sender, argss) =>
            {
                
                Console.WriteLine("\nFinished download " + video.Title + video.AudioExtension);
            };

            audioDownloader.Execute();
        }
        static void DownloadMP4(string link, string downloadDir)
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

            int ticks = 0;
            videoDownloader.DownloadProgressChanged += (sender, argss) =>
            {
                ticks++;

                if (ticks > 1000)
                {
                    Console.Write("#");
                    ticks -= 1000;
                }

            };
            videoDownloader.DownloadFinished += (sender, argss) => Console.WriteLine("Finished downloading " + video.Title + video.VideoExtension);

            videoDownloader.Execute();
        }
    }
}
