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
//
// additions
// - added async/awaits/trys/catches in places where they should have been in the first place, allows for better download handling
// - added download progress bar [requested by ElementalTree]
// - added feature to automatically retry a download 3 times if the download completes with a <1MB file
// - added option to have notification when downloads complete
//

namespace SpotDown_V2
{
    public partial class main : Form
    {
        SpotifyLocalAPIClass spotify;
        SpotifyMusicHandler mh;
        SpotifyEventHandler eh;
        int tryNumber = 0;
        string lastDownloaded = string.Empty;
        WebClient client = new WebClient();
        ToolTip tt = new ToolTip();
        int version = 1;

        public main()
        {
            InitializeComponent();
            updateCheck();

            spotify = new SpotifyLocalAPIClass();

            if (!SpotifyLocalAPIClass.IsSpotifyRunning())
            {
                spotify.RunSpotify();
                Thread.Sleep(5000);
                if (!SpotifyLocalAPIClass.IsSpotifyRunning())
                {
                    MessageBox.Show("Spotify didn't open after 5 seconds, exiting");
                    Environment.Exit(1);
                }
            }
            if (!SpotifyLocalAPIClass.IsSpotifyWebHelperRunning())
            {
                spotify.RunSpotifyWebHelper();
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
        }

        private async Task executeDownload(string artist, string name)
        {
            startdownload:
            string term = artist + " - " + name;

            if (string.IsNullOrWhiteSpace(artist))
                return;
            if (term.Substring(term.Length - 15).ToLower() == " - original mix")
                term = term.Substring(0, term.Length - 15);

            if (!File.Exists(browseForFolderBox.Text + term + ".mp3"))
            {
                addToLog("Searching MP3Clan for term \"" + term + "\"", logBox);
                string pageSource = client.DownloadString(new Uri(string.Format("http://mp3clan.com/mp3_source.php?q={0}", term.Replace(" ", "+"))));

                Match trackId = new Regex("<div class=\"mp3list-table\" id=\"(.+?)\">").Match(pageSource);

                if (trackId.Success == false || string.IsNullOrWhiteSpace(trackId.Groups[1].Value))
                {
                    addToLog("Could not find TrackID, skipping download", logBox);
                    return;
                }

                lastDownloaded = term;

                addToLog("TrackId: " + trackId.Groups[1].Value, logBox);

                string dlLink = string.Format("http://mp3clan.com/app/get.php?mp3={0}", trackId.Groups[1].Value);
                addToLog("Downloading from link: " + dlLink, logBox);

                // old code before I used better await/async placement
                //client.DownloadFileCompleted += client_DownloadFileCompleted;
                //client.DownloadFileCompleted += (object sender, AsyncCompletedEventArgs e) => addToLog("Download completed", logBox);
                //client.DownloadFileAsync(new Uri(dlLink), string.Format("{0}\\{1}.mp3", browseForFolderBox.Text, term));
                await Task.WhenAll(downloadFileAsync(dlLink, term));
                // stuff to do after download finishes, I don't have anything to put here though
            }

            while (client.IsBusy) { }
            FileInfo fileInfo = new FileInfo(browseForFolderBox.Text + "\\" + term + ".mp3");
            if (fileInfo.Length < 1000000 && retryIfUnder1Mb.Checked)
            {
                if (tryNumber < 3)
                {
                    tryNumber++;
                    if (File.Exists(browseForFolderBox.Text + "\\" + term + ".mp3"))
                        File.Delete(browseForFolderBox.Text + "\\" + term + ".mp3");
                    addToLog("File downloaded was under 1MB, redownloading try " + tryNumber + "/3", logBox);
                    goto startdownload;
                }
                else
                {
                    addToLog(term + " failed to download every try, skipping", logBox);
                    if (Settings.Default.DownloadNotifications)
                        notifyIcon.ShowBalloonTip(3000, "Download error", "The download for " + term + " failed to download.", ToolTipIcon.Error);
                }
            }
        }
        private async Task downloadFileAsync(string url, string term)
        {
            try
            {
                await client.DownloadFileTaskAsync(new Uri(url), string.Format(browseForFolderBox.Text + "\\" + term + ".mp3"));
            }
            catch (Exception e)
            {
                addToLog("Error downloading file: " + e.ToString(), logBox);
            }
        }
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.TotalBytesToReceive > 0)
            {
                downloadProgress.Maximum = (int)e.TotalBytesToReceive;
                downloadProgress.Value = (int)e.BytesReceived;
            }
        }
        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            int fileSize = (int)new FileInfo(browseForFolderBox.Text + "\\" + lastDownloaded + ".mp3").Length;
            int fileSizeKb = fileSize / 1000;
            addToLog("Download completed for \"" + lastDownloaded + "\" with " + fileSizeKb + "KB", logBox);
            if (Settings.Default.DownloadNotifications && !string.IsNullOrWhiteSpace(lastDownloaded))
            {
                if (fileSize > 1000000)
                {
                    notifyIcon.ShowBalloonTip(3000, "Download completed", "The download for " + lastDownloaded + " completed.", ToolTipIcon.Info);
                }
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
                logBox.AppendText("\n[" + DateTime.Now.ToString("h:mm:ss tt") + "] " + info);
            }
        }
        void updateCheck()
        {
            int latest = Convert.ToInt32(client.DownloadString("https://github.com/Scarsz/SpotDown/raw/master/version").Trim());
            if (latest > version)
            {
                MessageBox.Show("This version of SpotDown is outdated!");
                Process.Start("https://github.com/Scarsz/SpotDown/releases");
            }
        }

        private async void main_Load(object sender, EventArgs e)
        {
            this.Text = "SpotDown v2." + version;
            addToLog("Initialization of SpotDown v2." + version + " complete, enjoy!", logBox);

            spotify.Update();
            currentBar.Maximum = mh.GetCurrentTrack().GetLength() * 100;
            adPlaying.Text = "AdPlaying: " + mh.IsAdRunning();
            songPlaying.Text = "Playing: " + mh.IsPlaying();
            if (!mh.IsAdRunning())
                currentPicture.Image = await spotify.GetMusicHandler().GetCurrentTrack().GetAlbumArtAsync(AlbumArtSize.SIZE_160);

            artistLinkLabel.Text = mh.GetCurrentTrack().GetArtistName();
            artistLinkLabel.LinkClicked += (senderTwo, args) => Process.Start(mh.GetCurrentTrack().GetArtistURI());
            songLinkLabel.Text = mh.GetCurrentTrack().GetTrackName();
            songLinkLabel.LinkClicked += (senderTwo, args) => Process.Start(mh.GetCurrentTrack().GetTrackURI());
            albumLinkLabel.Text = mh.GetCurrentTrack().GetAlbumName();
            albumLinkLabel.LinkClicked += (senderTwo, args) => Process.Start(mh.GetCurrentTrack().GetAlbumURI());
            creatorLabel.LinkClicked += (senderTwo, args) => Process.Start("http://www.hackforums.net/showthread.php?tid=4819899");
            downloadersourceLabel.LinkClicked += (senderTwo, args) => Process.Start("http://www.hackforums.net/showthread.php?tid=4497329");
            ogcreatorLabel.LinkClicked += (senderTwo, args) => Process.Start("http://www.hackforums.net/showthread.php?tid=4506105");

            client.DownloadProgressChanged += client_DownloadProgressChanged;
            client.DownloadFileCompleted += client_DownloadFileCompleted;

            eh.OnTrackChange += new SpotifyEventHandler.TrackChangeEventHandler(trackChange);
            eh.OnTrackTimeChange += new SpotifyEventHandler.TrackTimeChangeEventHandler(timeChange);
            eh.OnPlayStateChange += new SpotifyEventHandler.PlayStateEventHandler(playstateChange);
            eh.OnVolumeChange += new SpotifyEventHandler.VolumeChangeEventHandler(volumeChange);
            eh.SetSynchronizingObject(this);
            eh.ListenForEvents(true);

            if (string.IsNullOrWhiteSpace(Settings.Default.DownloadDestination))
            {
                browseForFolderBox.Text = System.AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                browseForFolderBox.Text = Settings.Default.DownloadDestination;
            }
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
                currentPicture.Image = await e.new_track.GetAlbumArtAsync(AlbumArtSize.SIZE_160);
            if (Settings.Default.DownloadAutomatically)
                await executeDownload(mh.GetCurrentTrack().GetArtistName(), mh.GetCurrentTrack().GetTrackName());
        }
        private void timeChange(TrackTimeChangeEventArgs e)
        {
            currentBar.Value = (int)e.track_time * 100;
        }
        private void playstateChange(PlayStateEventArgs e)
        {
            songPlaying.Text = "Playing: " + mh.IsPlaying();
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
            await executeDownload(mh.GetCurrentTrack().GetArtistName(), mh.GetCurrentTrack().GetTrackName());
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
            if (!Directory.Exists(browseForFolderBox.Text))
            {
                addToLog("Selected folder doesn't exist, reverting to default", logBox);
                browseForFolderBox.Text = System.AppDomain.CurrentDomain.BaseDirectory;
            }

            if (browseForFolderBox.Text.Substring(browseForFolderBox.Text.Length - 1, 1) != "\\")
                browseForFolderBox.Text = browseForFolderBox.Text + "\\";

            Settings.Default.DownloadDestination = browseForFolderBox.Text;
            Settings.Default.Save();
        }
        private void browseForFolderBox_TextChanged(object sender, EventArgs e)
        {
            if (!browseForFolderBox.Focused)
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
        }
        private void browseForFolderPicture_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select the folder for downloaded music to be saved to";
            fbd.ShowDialog();
            browseForFolderBox.Text = fbd.SelectedPath;
        }

        private void main_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon.ShowBalloonTip(1000, "SpotDown is still running", "SpotDown has been minimized to the task tray", ToolTipIcon.Info);
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
