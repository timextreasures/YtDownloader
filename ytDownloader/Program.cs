using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        static void Main(string[] args)
        {

            StreamReader config = File.OpenText("config.cfg");
            if (config.EndOfStream)
            {
                Console.WriteLine("Failed to open config.cfg");
                Console.ReadKey();
                return;
            }

            string downloadDirectory="";
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
                catch (Exception e)
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
                    Console.Write("Available commands: \n \t - download_mp4 \n \t - download_mp3\n Usage: \"download_mp4 [insert link here] \"\n");
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
                    string playlistFile = input.Substring(input.IndexOf(command) + command.Length+1);

                    StreamReader tmpFile = File.OpenText(playlistFile);
                    string fileName = playlistFile.Substring(0,playlistFile.Length -4);

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

                if (directDownload == true)
                {
                    switch (downloadType)
                    {
                        case type.MP3:
                            Program.DownloadMP3(link, downloadDirectory);
                            break;
                        case type.MP4:
                            Program.DownloadMP4(link, downloadDirectory);
                            break;
                        case type.None:
                            break;
                        default:
                            break;
                    }
                }

               

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
            audioDownloader.DownloadProgressChanged += (sender, argss) =>
            {

                    Console.WriteLine(argss.ProgressPercentage);
            };

            Console.WriteLine("Started audio extraction and download of " + video.Title + video.AudioExtension);
            audioDownloader.Execute();
            Console.WriteLine("Finished audio extraction and download of " + video.Title + video.AudioExtension);
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
            videoDownloader.DownloadProgressChanged += (sender, argss) =>
            {
                    Console.WriteLine(argss.ProgressPercentage);
                
            };
            videoDownloader.DownloadFinished += (sender, argss) => Console.WriteLine("Finished downloading " + video.Title + video.VideoExtension);

            videoDownloader.Execute();
        }
    }
}
