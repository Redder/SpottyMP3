using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using SpotifyAPI.SpotifyLocalAPI;

// Changelog from v2.0 to v2.1
//
// ** now on github **
//
// fixes
// - fixed automatic downloading not redownloading corrupted files
// - fixed artist/song label text's being in each other's spot, don't know how I missed that
// - fixed including "original mix" in search terms which would screw up the download
// - fixed window resizing issues
// - fixed track names with /'s looking like it's another directory instead of just part of the song name
//   - song names with /'s will have the /'s replaced with -'s
//
// additions
// - added async/awaits/trys/catches in places where they should have been in the first place, allows for better download handling
// - added download progress bar [requested by ElementalTree]
// - added feature to automatically retry a download 3 times if the download completes with a <1MB file
// - added option to have notification when downloads complete
//
//
// TODO: Add queueing system for song downloads, probably going to use a List<string> and a foreach loop

/* Changelog from v2.1 to v2.2 (Updated by Redder04)
 * 
 * - Added as much comments as I could (im still a beginner so I don't understand most things)
 * - Added credits box to UI, makes it look more organized, and its "responsive"
 * - Changed InitializeComponent() to the end of the Main funciton instead of beginning.
 * - Modified startup process, more efficient
 * - Fixed Error when Program starts without a track playing in spotify, it crashes
 */

namespace SpottyMP3
{
    public partial class Main : Form
    {
        SpotifyLocalAPIClass spotify;
        SpotifyMusicHandler mh;
        SpotifyEventHandler eh;
        int tryNumber = 0;
        bool downloading = false;
        string lastDownloaded = string.Empty;
        WebClient client = new WebClient();
        ToolTip tt = new ToolTip();
        Stopwatch sw = new Stopwatch();
        int time = 0;
        int version = 2;

        DateTime lastUpdate;
        long lastBytes = 0;

        public Main()
        {
            updateCheck();
            spotify = new SpotifyLocalAPIClass();   //Creating a new instance of Spotify Local API

            if (!SpotifyLocalAPIClass.IsSpotifyRunning()) //If Spotify is not running then
            {
                spotify.RunSpotify(); //Run spotify

                Thread.Sleep(5000);
                if (!SpotifyLocalAPIClass.IsSpotifyRunning())
                {
                    MessageBox.Show("Spotify didn't open after 5 seconds, exiting");
                    Environment.Exit(1);
                }
            }
            if (!SpotifyLocalAPIClass.IsSpotifyWebHelperRunning())
            {
                spotify.RunSpotifyWebHelper(); //Run Spotify Web Helper

                Thread.Sleep(5000);
                if (!SpotifyLocalAPIClass.IsSpotifyWebHelperRunning())
                {
                    MessageBox.Show("Spotify Web Helper didn't open after 5 seconds, exiting");
                    Environment.Exit(2);
                }
            }

            if (!spotify.Connect())
            {
                Boolean retry = true;
                while (retry)
                {
                    if (MessageBox.Show("SLAPI couldn't load!", "Error", MessageBoxButtons.RetryCancel) == System.Windows.Forms.DialogResult.Retry)
                    {
                        if (spotify.Connect())
                            retry = false;
                        else
                            retry = true;
                    }
                    else
                    {
                        this.Close();
                        return;
                    }
                }
            }
            mh = spotify.GetMusicHandler();
            eh = spotify.GetEventHandler();
            InitializeComponent();
        }
        //executeDownload(mh.GetCurrentTrack().GetArtistName(), mh.GetCurrentTrack().GetTrackName(), true);
        private async Task executeDownload(string artist, string name, bool tellIfAlreadyExists = false)
        {
            tryNumber = 0;
            time = 0;

            startdownload:
            try
            {
                if (downloading)
                {
                    addToLog("Already downloading a song, ignoring requested download", logBox);
                    return;
                }

                downloading = true;
                
                string term = artist + " - " + name; //Building search term
                term = new Regex(string.Format("[{0}]", Regex.Escape(new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars())))).Replace(term, "");  //Removing any invalid characters?
                
                if (string.IsNullOrWhiteSpace(artist) || string.IsNullOrWhiteSpace(name) || mh.IsAdRunning())   //If there is no artist or no name or an ad is running return since theres nothing to download
                {
                    downloading = false;
                    return;
                }

                if (term.Length > 16)
                    if (term.Substring(term.Length - 15).ToLower() == " - original mix")
                        term = term.Substring(0, term.Length - 15); //Remove "Original Mix"
                if (term.Contains(" - feat"))
                    term = term.Split(new string[] { " - feat" }, StringSplitOptions.None)[0]; //Remove feat

                lastDownloaded = term;

                if (!File.Exists(browseForFolderBox.Text + lastDownloaded + ".mp3")) //If the song wasent downloaded to the folder already
                {
                    addToLog("Searching MP3Clan for term \"" + term + "\"", logBox);
                    string pageSource = client.DownloadString(new Uri(string.Format("http://mp3clan.com/mp3_source.php?q={0}", term.Replace(" ", "+"))));   //Perform search and get page source
                    //Perfom a search query in your browser and you can view the source to understand the next line
                    Match trackId = new Regex("<div class=\"mp3list-table\" id=\"(.+?)\">").Match(pageSource); //Checks if div exists in the page source //What is (.+?) 

                    if (!trackId.Success || string.IsNullOrWhiteSpace(trackId.Groups[1].Value)) //If there was no match or ...I don't know what is trackId.Groups[1].Value? Is it the ID of the div?
                    {
                        if (tryNumber < 3) //If try number is less than 3, retry download
                        {
                            downloading = false;
                            tryNumber++;
                            addToLog("Could not find TrackID, retrying " + tryNumber + "/3", logBox);
                            goto startdownload;
                        }
                        else  //If try number is greater than 3, then skip the download/give up
                        {
                            addToLog("Could not find TrackID, skipping download", logBox);
                            downloading = false;
                            return;
                        }
                    }

                    addToLog("TrackId: " + trackId.Groups[1].Value, logBox);

                    string dlLink = string.Format("http://mp3clan.com/app/get.php?mp3={0}", trackId.Groups[1].Value);
                    addToLog("Downloading from link: " + dlLink, logBox);

                    sw.Start(); //Start the download stopwatch
                    await Task.WhenAll(downloadFileAsync(dlLink, lastDownloaded)); //Download the track
                }
                else { if (tellIfAlreadyExists) { addToLog("Song already downloaded", logBox); } downloading = false; } //FIle already downloaded

                FileInfo fileInfo = new FileInfo(browseForFolderBox.Text + lastDownloaded + ".mp3");
                if (fileInfo.Length < 1000000 && retryIfUnder1Mb.Checked) //If length of file is less than 1mb and retry under 1mb is checked then retry download
                {
                    if (tryNumber < 3)
                    {
                        downloading = false;
                        tryNumber++;
                        if (File.Exists(browseForFolderBox.Text +  lastDownloaded + ".mp3"))
                            File.Delete(browseForFolderBox.Text +  lastDownloaded + ".mp3");
                        addToLog("File downloaded was under 1MB, redownloading try " + tryNumber + "/3", logBox);
                        goto startdownload;
                    }
                    else
                    {
                        downloading = false;
                        addToLog(term + " failed to download every try, skipping", logBox);
                        if (Settings.Default.DownloadNotifications)
                            notifyIcon.ShowBalloonTip(3000, "Download error", "The download for \"" + lastDownloaded + "\" failed to download.", ToolTipIcon.Error);
                    }
                }
                downloading = false;
            }
            catch (Exception e)
            {
                downloading = false;
                tryNumber++;
                addToLog("Error downloading file, retrying " + tryNumber + "\n" + e.ToString(), logBox);
                goto startdownload;
            }
        }
        private async Task downloadFileAsync(string url, string term)
        {
            try
            {
                await client.DownloadFileTaskAsync(new Uri(url), string.Format(browseForFolderBox.Text + "\\" + term + ".mp3"));
            }
            // System.Net.WebException: An exception occurred during a WebClient request. ---> System.IO.IOException: Unable to read data from the transport connection: The connection was closed.
            catch (WebException)
            {
                addToLog("Error downloading file: connection closed", logBox);
            }
            catch (Exception e)
            {
                addToLog("Error downloading file:\n" + e.ToString(), logBox);
            }
        }
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.TotalBytesToReceive > 0)
            {
                try
                {
                    if (lastBytes == 0)
                    {
                        lastUpdate = DateTime.Now;
                        lastBytes = e.BytesReceived;
                        return;
                    }
                    DateTime now = DateTime.Now;
                    TimeSpan timeSpan = now - lastUpdate;
                    long bytesChange = e.BytesReceived - lastBytes;
                    long bytesPerSecond = 0;
                    if (timeSpan.Seconds > 0) // stupid divide by zero errors
                        bytesPerSecond = bytesChange / timeSpan.Seconds;

                    downloadSpeedLabel.Text = bytesPerSecond / 1000 + "KB/s";
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                
                downloadBytesLabel.Text = e.BytesReceived / 1000 + "/" + e.TotalBytesToReceive / 1000;
                downloadProgress.Maximum = (int)e.TotalBytesToReceive;
                downloadProgress.Value = (int)e.BytesReceived;
            }
        }
        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            time = (int)sw.ElapsedMilliseconds / 1000;
            sw.Reset();
            if (string.IsNullOrWhiteSpace(lastDownloaded))
                return;
            int fileSize = (int)new FileInfo(browseForFolderBox.Text + "\\" + lastDownloaded + ".mp3").Length;
            int fileSizeKb = fileSize / 1000;
            if (fileSize > 1000000)
            {
                if (tryNumber == 0)
                    addToLog("Download completed for \"" + lastDownloaded + "\" with " + fileSizeKb + "KB in " + time + " seconds in " + (tryNumber + 1) + " try", logBox);
                else
                    addToLog("Download completed for \"" + lastDownloaded + "\" with " + fileSizeKb + "KB in " + time + " seconds in " + (tryNumber + 1) + " tries", logBox);
                if (Settings.Default.DownloadNotifications)
                    notifyIcon.ShowBalloonTip(3000, "Download completed", "The download for \"" + lastDownloaded + "\" completed in " + time + " seconds.", ToolTipIcon.Info);
            }
        }

        void addToLog(string info, RichTextBox logBox)
        {
            if (String.IsNullOrWhiteSpace(logBox.Text))
            {
                logBox.AppendText("[" + DateTime.Now.ToString("h:mm:ss tt") + "] " + info);
            }
            else
            {
                //logBox.AppendText("\n[" + DateTime.Now.ToString("h:mm:ss tt") + "] " + info);
                logBox.Text = "[" + DateTime.Now.ToString("h:mm:ss tt") + "] " + info + "\n" + logBox.Text;
            }
        }
        void updateCheck()
        {
            try
            {
                int latest = Convert.ToInt32(client.DownloadString("https://github.com/Scarsz/SpottyMP3/raw/master/version").Trim());
                if (latest > version)
                {
                    MessageBox.Show("This version of SpottyMP3 is outdated!");
                    Process.Start("https://github.com/Scarsz/SpottyMP3/releases");
                }
            }
            catch (Exception)
            {
                addToLog("Update check failed", logBox);
            }
        }

        private async void main_Load(object sender, EventArgs e)
        {
            this.Text = "SpottyMP3 v2." + version;  //Changes text to match version user is using
            addToLog("Initialization of SpottyMP3 v2." + version + " complete, enjoy!", logBox);

            spotify.Update();
            
            adPlaying.Text = "AdPlaying: " + mh.IsAdRunning();  //Is an ad playing right now?
            songPlaying.Text = "Playing: " + mh.IsPlaying();    //Is a song playing right now?

            //If an add is not running then get the tracks image
            if (!mh.IsAdRunning())
                currentPicture.Image = await spotify.GetMusicHandler().GetCurrentTrack().GetAlbumArtAsync(AlbumArtSize.SIZE_160);
            try
            {
                currentBar.Maximum = mh.GetCurrentTrack().GetLength() * 100;    //Gets track length and sets the end of the track as maximum value of progress bar
                artistLinkLabel.Text = mh.GetCurrentTrack().GetArtistName();    //Gets Current tracks artist name
                artistLinkLabel.LinkClicked += (senderTwo, args) => Process.Start(mh.GetCurrentTrack().GetArtistURI());
                songLinkLabel.Text = mh.GetCurrentTrack().GetTrackName();   //Gets Current tracks name
                songLinkLabel.LinkClicked += (senderTwo, args) => Process.Start(mh.GetCurrentTrack().GetTrackURI());
                albumLinkLabel.Text = mh.GetCurrentTrack().GetAlbumName();  //Gets Current tracks album name
                albumLinkLabel.LinkClicked += (senderTwo, args) => Process.Start(mh.GetCurrentTrack().GetAlbumURI());
            }
            catch (NullReferenceException) { }
            
            //Other contributers links, im going to add mine :)
            creatorLabel.LinkClicked += (senderTwo, args) => Process.Start("http://www.hackforums.net/showthread.php?tid=4819899");
            downloadersourceLabel.LinkClicked += (senderTwo, args) => Process.Start("http://www.hackforums.net/showthread.php?tid=4497329");
            ogcreatorLabel.LinkClicked += (senderTwo, args) => Process.Start("http://www.hackforums.net/showthread.php?tid=4506105");
            redderLabel.LinkClicked += (senderTwo, args) => Process.Start("https://leakforums.net/user-49440");

            client.DownloadProgressChanged += client_DownloadProgressChanged;
            client.DownloadFileCompleted += client_DownloadFileCompleted;

            eh.OnTrackChange += new SpotifyEventHandler.TrackChangeEventHandler(trackChange);
            eh.OnTrackTimeChange += new SpotifyEventHandler.TrackTimeChangeEventHandler(timeChange);
            eh.OnPlayStateChange += new SpotifyEventHandler.PlayStateEventHandler(playstateChange);
            eh.OnVolumeChange += new SpotifyEventHandler.VolumeChangeEventHandler(volumeChange);
            eh.SetSynchronizingObject(this);
            eh.ListenForEvents(true);

            if (string.IsNullOrWhiteSpace(Settings.Default.DownloadDestination)) //If there is no default download destination then set it to the program directory, else, set it to default download
                browseForFolderBox.Text = System.AppDomain.CurrentDomain.BaseDirectory;
            else
                browseForFolderBox.Text = Settings.Default.DownloadDestination;

            retryIfUnder1Mb.Checked = Settings.Default.DownloadRetry;
            liveDownloads.Checked = Settings.Default.DownloadAutomatically;
            showDownloadNotification.Checked = Settings.Default.DownloadNotifications;
            
            tt.AutoPopDelay = 5000;
            tt.InitialDelay = 1000;
            tt.ReshowDelay = 500;
            tt.SetToolTip(retryIfUnder1Mb, "Whether or not to retry downloading a song if it finishes downloading smaller than 1MB");

        }

        private async void trackChange(TrackChangeEventArgs e)
        {
            currentBar.Maximum = mh.GetCurrentTrack().GetLength() * 100;
            artistLinkLabel.Text = e.new_track.GetArtistName();
            songLinkLabel.Text = e.new_track.GetTrackName();
            albumLinkLabel.Text = e.new_track.GetAlbumName();
            adPlaying.Text = "AdPlaying: " + mh.IsAdRunning();
            if (!mh.IsAdRunning())
                currentPicture.Image = await e.new_track.GetAlbumArtAsync(AlbumArtSize.SIZE_640);
            if (Settings.Default.DownloadAutomatically)
                await executeDownload(mh.GetCurrentTrack().GetArtistName(), mh.GetCurrentTrack().GetTrackName());
        }
        private void timeChange(TrackTimeChangeEventArgs e)
        {
            if (!mh.IsAdRunning())
            {
                try
                {
                    currentBar.Maximum = (int)mh.GetCurrentTrack().length * 100;
                    currentBar.Value = (int)e.track_time * 100;
                }
                catch
                {
                    currentBar.Maximum = currentBar.Maximum * 100;
                    currentBar.Value = currentBar.Value * 100;
                }
            }
        }
        private void playstateChange(PlayStateEventArgs e)
        {
            songPlaying.Text = "Playing: " + mh.IsPlaying(); //Is a song playing right now?
        }
        private void volumeChange(VolumeChangeEventArgs e)
        {
            
        }

        private void previousButton_Click(object sender, EventArgs e)
        {
            mh.Previous();
        }
        private void playButton_Click(object sender, EventArgs e)
        {
            mh.Play();
        }
        private void pauseButton_Click(object sender, EventArgs e)
        {
            mh.Pause();
        }
        private void nextButton_Click(object sender, EventArgs e)
        {
            mh.Skip();
        }
        private async void downloadButton_Click(object sender, EventArgs e)
        {
            await executeDownload(mh.GetCurrentTrack().GetArtistName(), mh.GetCurrentTrack().GetTrackName(), true);
        }
        private async void liveDownloads_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.DownloadAutomatically = liveDownloads.Checked;
            Settings.Default.Save();

            if (Settings.Default.DownloadAutomatically && !mh.IsAdRunning())
                await executeDownload(mh.GetCurrentTrack().GetArtistName(), mh.GetCurrentTrack().GetTrackName());
        }
        private void retryIfUnder1Mb_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.DownloadRetry = retryIfUnder1Mb.Checked;
            Settings.Default.Save();
        }
        private void showDownloadNotification_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.DownloadNotifications = showDownloadNotification.Checked;
            Settings.Default.Save();
        }
        private void browseForFolderBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(browseForFolderBox.Text))
            {
                browseForFolderBox.Text = Settings.Default.DownloadDestination;
            }
            else if (!Directory.Exists(browseForFolderBox.Text))
            {
                addToLog("Selected folder doesn't exist, reverting to default", logBox);
                browseForFolderBox.Text = System.AppDomain.CurrentDomain.BaseDirectory;
            }

            if (browseForFolderBox.Text.Substring(browseForFolderBox.Text.Length - 1, 1) != "\\")
                browseForFolderBox.Text = browseForFolderBox.Text + "\\";

            Settings.Default.DownloadDestination = browseForFolderBox.Text;
            Settings.Default.Save();
        }
        private void browseForFolderPicture_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select the folder for downloaded music to be saved to";
            fbd.ShowDialog();
            browseForFolderBox.Text = fbd.SelectedPath;
            browseForFolderBox.Focus();
            logBox.Focus();
            browseForFolderBox.Focus();
        }

        private void main_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon.ShowBalloonTip(1000, "SpottyMP3 is still running", "SpottyMP3 has been minimized to the task tray", ToolTipIcon.Info);
                this.ShowInTaskbar = false;
            }
        }
        private void notifyIcon_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }
    }
}
