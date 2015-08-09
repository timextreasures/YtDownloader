A basic youtube downloader. This downloader uses the youtubeExtractor library.

The solution is for visual studio 2015.

Command list:
    download_mp4        [insert yt link]        - Downloads a mp4 at 720p
    download_mp3        [insert yt link]        - Downloads a mp3 at highestbitrate
    help                                        - Shows this file
    add_mp3             [yt link]               - Adds a mp3 to the internal queue (not saved when exiting)
    add_mp4             [yt link]               - Adds a mp4 to the internal queue (not saved when exiting)
    list                                        - displays the internal queue
    start                                       - starts downloading the internal queue ( will not resume when crashed)
    startPlaylist       [txt file]              - Reads youtube links from a txt file. Ever line 1 txt file ex. mp3 [link]
    downloadChannel     [type] [channelname]    - Downloads a whole channel in a certain type ( mp3 or mp4)
    downloadPlaylist    [type]  [playlistID]    - Downloads a whole playlist from playlistID
    
