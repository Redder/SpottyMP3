namespace SpottyMP3
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.currentBar = new System.Windows.Forms.ProgressBar();
            this.currentBox = new System.Windows.Forms.GroupBox();
            this.downloadButton = new System.Windows.Forms.Button();
            this.previousButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.currentPicture = new System.Windows.Forms.PictureBox();
            this.playButton = new System.Windows.Forms.Button();
            this.albumLabel = new System.Windows.Forms.Label();
            this.albumLinkLabel = new System.Windows.Forms.LinkLabel();
            this.songLabel = new System.Windows.Forms.Label();
            this.artistLabel = new System.Windows.Forms.Label();
            this.artistLinkLabel = new System.Windows.Forms.LinkLabel();
            this.songLinkLabel = new System.Windows.Forms.LinkLabel();
            this.optionsBox = new System.Windows.Forms.GroupBox();
            this.showDownloadNotification = new System.Windows.Forms.CheckBox();
            this.retryIfUnder1Mb = new System.Windows.Forms.CheckBox();
            this.browseForFolderPicture = new System.Windows.Forms.PictureBox();
            this.browseForFolderBox = new System.Windows.Forms.TextBox();
            this.liveDownloads = new System.Windows.Forms.CheckBox();
            this.debugBox = new System.Windows.Forms.GroupBox();
            this.songPlaying = new System.Windows.Forms.Label();
            this.adPlaying = new System.Windows.Forms.Label();
            this.logBox = new System.Windows.Forms.RichTextBox();
            this.creatorLabel = new System.Windows.Forms.LinkLabel();
            this.downloadersourceLabel = new System.Windows.Forms.LinkLabel();
            this.ogcreatorLabel = new System.Windows.Forms.LinkLabel();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.downloadProgress = new System.Windows.Forms.ProgressBar();
            this.downloadSpeedLabel = new System.Windows.Forms.Label();
            this.downloadBytesLabel = new System.Windows.Forms.Label();
            this.creditsBox = new System.Windows.Forms.GroupBox();
            this.redderLabel = new System.Windows.Forms.LinkLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.currentBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currentPicture)).BeginInit();
            this.optionsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.browseForFolderPicture)).BeginInit();
            this.debugBox.SuspendLayout();
            this.creditsBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // currentBar
            // 
            this.currentBar.Location = new System.Drawing.Point(6, 101);
            this.currentBar.Name = "currentBar";
            this.currentBar.Size = new System.Drawing.Size(234, 26);
            this.currentBar.TabIndex = 0;
            // 
            // currentBox
            // 
            this.currentBox.Controls.Add(this.downloadButton);
            this.currentBox.Controls.Add(this.previousButton);
            this.currentBox.Controls.Add(this.nextButton);
            this.currentBox.Controls.Add(this.pauseButton);
            this.currentBox.Controls.Add(this.currentPicture);
            this.currentBox.Controls.Add(this.playButton);
            this.currentBox.Controls.Add(this.albumLabel);
            this.currentBox.Controls.Add(this.albumLinkLabel);
            this.currentBox.Controls.Add(this.songLabel);
            this.currentBox.Controls.Add(this.artistLabel);
            this.currentBox.Controls.Add(this.artistLinkLabel);
            this.currentBox.Controls.Add(this.songLinkLabel);
            this.currentBox.Controls.Add(this.currentBar);
            this.currentBox.Location = new System.Drawing.Point(12, 12);
            this.currentBox.Name = "currentBox";
            this.currentBox.Size = new System.Drawing.Size(246, 133);
            this.currentBox.TabIndex = 1;
            this.currentBox.TabStop = false;
            this.currentBox.Text = "Currently Playing";
            // 
            // downloadButton
            // 
            this.downloadButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadButton.Location = new System.Drawing.Point(121, 76);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(20, 20);
            this.downloadButton.TabIndex = 16;
            this.downloadButton.Text = "▽";
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // previousButton
            // 
            this.previousButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.previousButton.Location = new System.Drawing.Point(14, 76);
            this.previousButton.Name = "previousButton";
            this.previousButton.Size = new System.Drawing.Size(20, 20);
            this.previousButton.TabIndex = 15;
            this.previousButton.Text = "<";
            this.previousButton.UseVisualStyleBackColor = true;
            this.previousButton.Click += new System.EventHandler(this.previousButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextButton.Location = new System.Drawing.Point(95, 76);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(20, 20);
            this.nextButton.TabIndex = 14;
            this.nextButton.Text = ">";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // pauseButton
            // 
            this.pauseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pauseButton.Location = new System.Drawing.Point(66, 76);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(23, 20);
            this.pauseButton.TabIndex = 13;
            this.pauseButton.Text = "II";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // currentPicture
            // 
            this.currentPicture.Location = new System.Drawing.Point(157, 12);
            this.currentPicture.Name = "currentPicture";
            this.currentPicture.Size = new System.Drawing.Size(83, 83);
            this.currentPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.currentPicture.TabIndex = 5;
            this.currentPicture.TabStop = false;
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(40, 76);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(20, 20);
            this.playButton.TabIndex = 12;
            this.playButton.Text = "▶";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // albumLabel
            // 
            this.albumLabel.AutoSize = true;
            this.albumLabel.Location = new System.Drawing.Point(6, 60);
            this.albumLabel.Name = "albumLabel";
            this.albumLabel.Size = new System.Drawing.Size(36, 13);
            this.albumLabel.TabIndex = 11;
            this.albumLabel.Text = "Album";
            // 
            // albumLinkLabel
            // 
            this.albumLinkLabel.AutoSize = true;
            this.albumLinkLabel.Location = new System.Drawing.Point(47, 60);
            this.albumLinkLabel.MaximumSize = new System.Drawing.Size(103, 13);
            this.albumLinkLabel.Name = "albumLinkLabel";
            this.albumLinkLabel.Size = new System.Drawing.Size(64, 13);
            this.albumLinkLabel.TabIndex = 10;
            this.albumLinkLabel.TabStop = true;
            this.albumLinkLabel.Text = "AlbumName";
            // 
            // songLabel
            // 
            this.songLabel.AutoSize = true;
            this.songLabel.Location = new System.Drawing.Point(6, 16);
            this.songLabel.Name = "songLabel";
            this.songLabel.Size = new System.Drawing.Size(32, 13);
            this.songLabel.TabIndex = 9;
            this.songLabel.Text = "Song";
            // 
            // artistLabel
            // 
            this.artistLabel.AutoSize = true;
            this.artistLabel.Location = new System.Drawing.Point(6, 38);
            this.artistLabel.Name = "artistLabel";
            this.artistLabel.Size = new System.Drawing.Size(30, 13);
            this.artistLabel.TabIndex = 8;
            this.artistLabel.Text = "Artist";
            // 
            // artistLinkLabel
            // 
            this.artistLinkLabel.AutoSize = true;
            this.artistLinkLabel.Location = new System.Drawing.Point(47, 38);
            this.artistLinkLabel.MaximumSize = new System.Drawing.Size(103, 13);
            this.artistLinkLabel.Name = "artistLinkLabel";
            this.artistLinkLabel.Size = new System.Drawing.Size(58, 13);
            this.artistLinkLabel.TabIndex = 7;
            this.artistLinkLabel.TabStop = true;
            this.artistLinkLabel.Text = "ArtistName";
            // 
            // songLinkLabel
            // 
            this.songLinkLabel.AutoSize = true;
            this.songLinkLabel.Location = new System.Drawing.Point(47, 16);
            this.songLinkLabel.MaximumSize = new System.Drawing.Size(103, 13);
            this.songLinkLabel.Name = "songLinkLabel";
            this.songLinkLabel.Size = new System.Drawing.Size(60, 13);
            this.songLinkLabel.TabIndex = 6;
            this.songLinkLabel.TabStop = true;
            this.songLinkLabel.Text = "SongName";
            // 
            // optionsBox
            // 
            this.optionsBox.Controls.Add(this.showDownloadNotification);
            this.optionsBox.Controls.Add(this.retryIfUnder1Mb);
            this.optionsBox.Controls.Add(this.browseForFolderPicture);
            this.optionsBox.Controls.Add(this.browseForFolderBox);
            this.optionsBox.Controls.Add(this.liveDownloads);
            this.optionsBox.Controls.Add(this.debugBox);
            this.optionsBox.Location = new System.Drawing.Point(12, 151);
            this.optionsBox.Name = "optionsBox";
            this.optionsBox.Size = new System.Drawing.Size(246, 138);
            this.optionsBox.TabIndex = 2;
            this.optionsBox.TabStop = false;
            this.optionsBox.Text = "Misc";
            // 
            // showDownloadNotification
            // 
            this.showDownloadNotification.AutoSize = true;
            this.showDownloadNotification.Location = new System.Drawing.Point(30, 42);
            this.showDownloadNotification.Name = "showDownloadNotification";
            this.showDownloadNotification.Size = new System.Drawing.Size(192, 17);
            this.showDownloadNotification.TabIndex = 6;
            this.showDownloadNotification.Text = "Show download status notifications";
            this.showDownloadNotification.UseVisualStyleBackColor = true;
            this.showDownloadNotification.CheckedChanged += new System.EventHandler(this.showDownloadNotification_CheckedChanged);
            // 
            // retryIfUnder1Mb
            // 
            this.retryIfUnder1Mb.AutoSize = true;
            this.retryIfUnder1Mb.Location = new System.Drawing.Point(173, 19);
            this.retryIfUnder1Mb.Name = "retryIfUnder1Mb";
            this.retryIfUnder1Mb.Size = new System.Drawing.Size(67, 17);
            this.retryIfUnder1Mb.TabIndex = 5;
            this.retryIfUnder1Mb.Tag = "";
            this.retryIfUnder1Mb.Text = "<1MB=X";
            this.retryIfUnder1Mb.UseVisualStyleBackColor = true;
            this.retryIfUnder1Mb.CheckedChanged += new System.EventHandler(this.retryIfUnder1Mb_CheckedChanged);
            // 
            // browseForFolderPicture
            // 
            this.browseForFolderPicture.ImageLocation = "http://www.icpsr.umich.edu/icpsrweb/SAMHDA/images/folder_icon.png";
            this.browseForFolderPicture.Location = new System.Drawing.Point(218, 65);
            this.browseForFolderPicture.Name = "browseForFolderPicture";
            this.browseForFolderPicture.Size = new System.Drawing.Size(22, 20);
            this.browseForFolderPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.browseForFolderPicture.TabIndex = 4;
            this.browseForFolderPicture.TabStop = false;
            this.browseForFolderPicture.Click += new System.EventHandler(this.browseForFolderPicture_Click);
            // 
            // browseForFolderBox
            // 
            this.browseForFolderBox.Location = new System.Drawing.Point(6, 65);
            this.browseForFolderBox.Name = "browseForFolderBox";
            this.browseForFolderBox.Size = new System.Drawing.Size(206, 20);
            this.browseForFolderBox.TabIndex = 3;
            this.browseForFolderBox.Leave += new System.EventHandler(this.browseForFolderBox_Leave);
            // 
            // liveDownloads
            // 
            this.liveDownloads.AutoSize = true;
            this.liveDownloads.Location = new System.Drawing.Point(8, 19);
            this.liveDownloads.Name = "liveDownloads";
            this.liveDownloads.Size = new System.Drawing.Size(164, 17);
            this.liveDownloads.TabIndex = 2;
            this.liveDownloads.Text = "Download songs as they play";
            this.liveDownloads.UseVisualStyleBackColor = true;
            this.liveDownloads.CheckedChanged += new System.EventHandler(this.liveDownloads_CheckedChanged);
            // 
            // debugBox
            // 
            this.debugBox.Controls.Add(this.songPlaying);
            this.debugBox.Controls.Add(this.adPlaying);
            this.debugBox.Location = new System.Drawing.Point(6, 90);
            this.debugBox.Name = "debugBox";
            this.debugBox.Size = new System.Drawing.Size(234, 40);
            this.debugBox.TabIndex = 1;
            this.debugBox.TabStop = false;
            this.debugBox.Text = "Debug";
            // 
            // songPlaying
            // 
            this.songPlaying.AutoSize = true;
            this.songPlaying.Location = new System.Drawing.Point(126, 18);
            this.songPlaying.Name = "songPlaying";
            this.songPlaying.Size = new System.Drawing.Size(69, 13);
            this.songPlaying.TabIndex = 1;
            this.songPlaying.Text = "Playing: false";
            // 
            // adPlaying
            // 
            this.adPlaying.AutoSize = true;
            this.adPlaying.Location = new System.Drawing.Point(38, 18);
            this.adPlaying.Name = "adPlaying";
            this.adPlaying.Size = new System.Drawing.Size(82, 13);
            this.adPlaying.TabIndex = 0;
            this.adPlaying.Text = "AdPlaying: false";
            // 
            // logBox
            // 
            this.logBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.logBox.Location = new System.Drawing.Point(264, 16);
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.Size = new System.Drawing.Size(383, 171);
            this.logBox.TabIndex = 3;
            this.logBox.Text = "";
            // 
            // creatorLabel
            // 
            this.creatorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.creatorLabel.AutoSize = true;
            this.creatorLabel.Location = new System.Drawing.Point(6, 16);
            this.creatorLabel.Name = "creatorLabel";
            this.creatorLabel.Size = new System.Drawing.Size(115, 13);
            this.creatorLabel.TabIndex = 4;
            this.creatorLabel.TabStop = true;
            this.creatorLabel.Text = "Creator: @ScarszRawr";
            // 
            // downloadersourceLabel
            // 
            this.downloadersourceLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadersourceLabel.AutoSize = true;
            this.downloadersourceLabel.Location = new System.Drawing.Point(6, 38);
            this.downloadersourceLabel.Name = "downloadersourceLabel";
            this.downloadersourceLabel.Size = new System.Drawing.Size(190, 13);
            this.downloadersourceLabel.TabIndex = 5;
            this.downloadersourceLabel.TabStop = true;
            this.downloadersourceLabel.Text = "Modified downloader Source: privTech";
            // 
            // ogcreatorLabel
            // 
            this.ogcreatorLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ogcreatorLabel.AutoSize = true;
            this.ogcreatorLabel.Location = new System.Drawing.Point(120, 16);
            this.ogcreatorLabel.Name = "ogcreatorLabel";
            this.ogcreatorLabel.Size = new System.Drawing.Size(254, 13);
            this.ogcreatorLabel.TabIndex = 6;
            this.ogcreatorLabel.TabStop = true;
            this.ogcreatorLabel.Text = "Original creator [No source code used]: DarkN3ss61";
            this.ogcreatorLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.Text = "SpotDown";
            this.notifyIcon.Visible = true;
            this.notifyIcon.Click += new System.EventHandler(this.notifyIcon_Click);
            // 
            // downloadProgress
            // 
            this.downloadProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadProgress.Location = new System.Drawing.Point(394, 205);
            this.downloadProgress.Maximum = 1;
            this.downloadProgress.Name = "downloadProgress";
            this.downloadProgress.Size = new System.Drawing.Size(250, 19);
            this.downloadProgress.TabIndex = 7;
            this.downloadProgress.Value = 1;
            // 
            // downloadSpeedLabel
            // 
            this.downloadSpeedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadSpeedLabel.AutoSize = true;
            this.downloadSpeedLabel.Location = new System.Drawing.Point(264, 208);
            this.downloadSpeedLabel.Name = "downloadSpeedLabel";
            this.downloadSpeedLabel.Size = new System.Drawing.Size(49, 13);
            this.downloadSpeedLabel.TabIndex = 18;
            this.downloadSpeedLabel.Text = "512KB/s";
            // 
            // downloadBytesLabel
            // 
            this.downloadBytesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadBytesLabel.AutoSize = true;
            this.downloadBytesLabel.Location = new System.Drawing.Point(316, 208);
            this.downloadBytesLabel.Name = "downloadBytesLabel";
            this.downloadBytesLabel.Size = new System.Drawing.Size(72, 13);
            this.downloadBytesLabel.TabIndex = 19;
            this.downloadBytesLabel.Text = "16000/16000";
            // 
            // creditsBox
            // 
            this.creditsBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.creditsBox.Controls.Add(this.redderLabel);
            this.creditsBox.Controls.Add(this.creatorLabel);
            this.creditsBox.Controls.Add(this.downloadersourceLabel);
            this.creditsBox.Controls.Add(this.ogcreatorLabel);
            this.creditsBox.Location = new System.Drawing.Point(264, 230);
            this.creditsBox.Name = "creditsBox";
            this.creditsBox.Size = new System.Drawing.Size(380, 59);
            this.creditsBox.TabIndex = 21;
            this.creditsBox.TabStop = false;
            this.creditsBox.Text = "Credits";
            // 
            // redderLabel
            // 
            this.redderLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.redderLabel.AutoSize = true;
            this.redderLabel.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.redderLabel.Location = new System.Drawing.Point(229, 38);
            this.redderLabel.Name = "redderLabel";
            this.redderLabel.Size = new System.Drawing.Size(145, 13);
            this.redderLabel.TabIndex = 7;
            this.redderLabel.TabStop = true;
            this.redderLabel.Text = "New Active Editor: Redder04";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 299);
            this.Controls.Add(this.creditsBox);
            this.Controls.Add(this.downloadBytesLabel);
            this.Controls.Add(this.downloadSpeedLabel);
            this.Controls.Add(this.downloadProgress);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.optionsBox);
            this.Controls.Add(this.currentBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(800, 337);
            this.MinimumSize = new System.Drawing.Size(600, 337);
            this.Name = "Main";
            this.Text = "SpotDown v2";
            this.Load += new System.EventHandler(this.main_Load);
            this.Resize += new System.EventHandler(this.main_Resize);
            this.currentBox.ResumeLayout(false);
            this.currentBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currentPicture)).EndInit();
            this.optionsBox.ResumeLayout(false);
            this.optionsBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.browseForFolderPicture)).EndInit();
            this.debugBox.ResumeLayout(false);
            this.debugBox.PerformLayout();
            this.creditsBox.ResumeLayout(false);
            this.creditsBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar currentBar;
        private System.Windows.Forms.GroupBox currentBox;
        private System.Windows.Forms.PictureBox currentPicture;
        private System.Windows.Forms.LinkLabel artistLinkLabel;
        private System.Windows.Forms.LinkLabel songLinkLabel;
        private System.Windows.Forms.Label songLabel;
        private System.Windows.Forms.Label artistLabel;
        private System.Windows.Forms.Label albumLabel;
        private System.Windows.Forms.LinkLabel albumLinkLabel;
        private System.Windows.Forms.GroupBox optionsBox;
        private System.Windows.Forms.GroupBox debugBox;
        private System.Windows.Forms.Label adPlaying;
        private System.Windows.Forms.Label songPlaying;
        private System.Windows.Forms.Button previousButton;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Button downloadButton;
        private System.Windows.Forms.CheckBox liveDownloads;
        private System.Windows.Forms.RichTextBox logBox;
        private System.Windows.Forms.PictureBox browseForFolderPicture;
        private System.Windows.Forms.TextBox browseForFolderBox;
        private System.Windows.Forms.LinkLabel creatorLabel;
        private System.Windows.Forms.LinkLabel downloadersourceLabel;
        private System.Windows.Forms.LinkLabel ogcreatorLabel;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.CheckBox retryIfUnder1Mb;
        private System.Windows.Forms.ProgressBar downloadProgress;
        private System.Windows.Forms.CheckBox showDownloadNotification;
        private System.Windows.Forms.Label downloadSpeedLabel;
        private System.Windows.Forms.Label downloadBytesLabel;
        private System.Windows.Forms.GroupBox creditsBox;
        private System.Windows.Forms.LinkLabel redderLabel;
        private System.Windows.Forms.Timer timer1;
    }
}

