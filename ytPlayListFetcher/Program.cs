using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

using YoutubeExtractor;


namespace ytPlayListFetcher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Youtube Data API: PlayList!");

            try
            {
                new Program().Run().Wait();
            }
            catch (AggregateException ex)
            {

                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        private async Task Run()
        {
            //UserCredential credential;
            //using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            //{
            //    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            //        GoogleClientSecrets.Load(stream).Secrets,
            //        // This OAuth 2.0 access scope allows for full read/write access to the
            //        // authenticated user's account.
            //        new[] { YouTubeService.Scope.Youtube },
            //        "user",
            //        CancellationToken.None,
            //        new FileDataStore(this.GetType().ToString())
            //    );
            //}

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyDzqeoR5mWrtJJ1Qt1SL_DLRDJvVKStdSo",
                ApplicationName = this.GetType().ToString()
            });

            var channelsListRequest = youtubeService.Channels.List("contentDetails, statistics, snippet");
            channelsListRequest.ForUsername = "MonstercatMedia";

            var channelsListResponse = await channelsListRequest.ExecuteAsync();

            foreach (var channel in channelsListResponse.Items)
            {
                var uploadListId = channel.ContentDetails.RelatedPlaylists.Uploads;
                Console.WriteLine("Videos in list {0}", uploadListId);

                Console.WriteLine("Videos: " + channel.Statistics.VideoCount + " Views:" + channel.Statistics.ViewCount + " Subscribers:" + channel.Statistics.SubscriberCount);
                Console.WriteLine(channel.Snippet.Description);
                Console.WriteLine(" ");
                    
                var nextPageToken = "";
                
                while (nextPageToken != null)
                {
                    var playlistItemsListRequest = youtubeService.PlaylistItems.List("snippet");
                    playlistItemsListRequest.PlaylistId = uploadListId;
                    playlistItemsListRequest.MaxResults = 50;
                    playlistItemsListRequest.PageToken = nextPageToken;

                    // Retrieve the list of videos uploaded to the authenticated user's channel.
                    var playlistItemsListResponse = await playlistItemsListRequest.ExecuteAsync();

                    foreach (var playlistItem in playlistItemsListResponse.Items)
                    {
                        // Print information about each video.
                        Console.WriteLine("{0} ({1})", playlistItem.Snippet.Title, playlistItem.Snippet.ResourceId.VideoId);

                        IEnumerable<string> files = Directory.EnumerateFiles("Downloads/");
                        bool found = false;
                        foreach (var file in files)
                        {
                            if (file.Substring(0,file.LastIndexOf('.')) == "Downloads/" + playlistItem.Snippet.Title )
                            {
                                found = true;
                            }
                        }
                        if (!found && !(playlistItem.Snippet.Title.ToLower().Contains("podcast")))
                        {
                            DownloadMusic(playlistItem.Snippet.ResourceId.VideoId);
                        }
 
                    }

                    nextPageToken = playlistItemsListResponse.NextPageToken;
                }
            }


        }
        static void DownloadMusic(string videoID)
        {
            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls("https://www.youtube.com/watch?v="+videoID);

            VideoInfo video = videoInfos.Where(info => info.AudioType == AudioType.Mp3 && info.CanExtractAudio ).OrderByDescending(info => info.AudioBitrate).First();
            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }
            var audioDownload = new AudioDownloader(video, Path.Combine("Downloads/", RemoveIllegalPathCharacters(video.Title) + video.AudioExtension));
            audioDownload.DownloadStarted += (sender, args) =>
            {
                Console.WriteLine("Downloading " + video.Title + video.AudioExtension);
            };
            int ticks = 0;

            audioDownload.DownloadProgressChanged += (sender, args) =>
            {
                ticks++;
                if (ticks > 300)
                {
                    ticks -= 300;
                    Console.Write("|");
                }
                
            };
            audioDownload.DownloadFinished += (sender, args) =>
            {
                Console.WriteLine("Finished " + video.Title + video.AudioExtension);
            };
            audioDownload.Execute();
        }
        static async void DownloadVideo(string videoID)
        {
            Directory.CreateDirectory("Downloads/");

            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls("https://www.youtube.com/watch?v="+videoID);

            VideoInfo video = videoInfos.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 720);

            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }

            var videoDownloader = new VideoDownloader(video, Path.Combine("Downloads/", RemoveIllegalPathCharacters(video.Title) + video.VideoExtension));
            videoDownloader.DownloadStarted += (sender, argss) => Console.WriteLine("Started downloading " + video.Title + video.VideoExtension);
            videoDownloader.DownloadProgressChanged += (sender, argss) =>
            {
                Console.WriteLine(argss.ProgressPercentage);

            };
            videoDownloader.DownloadFinished += (sender, argss) => Console.WriteLine("Finished downloading " + video.Title + video.VideoExtension);

            videoDownloader.Execute();
        }
        private static string RemoveIllegalPathCharacters(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(path, "");
        }
    }
}
