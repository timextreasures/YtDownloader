namespace ytDownloader_GUI
{
    partial class printingBox
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
            this.button_add = new System.Windows.Forms.Button();
            this.list_queue = new System.Windows.Forms.ListView();
            this.column_Channel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Undefined = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_link = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtBox_link = new System.Windows.Forms.TextBox();
            this.label_title = new System.Windows.Forms.Label();
            this.button_clear = new System.Windows.Forms.Button();
            this.button_download = new System.Windows.Forms.Button();
            this.button_selectDir = new System.Windows.Forms.Button();
            this.save_download = new System.Windows.Forms.FolderBrowserDialog();
            this.download_progress = new System.Windows.Forms.ProgressBar();
            this.button_delete = new System.Windows.Forms.Button();
            this.downloader = new System.ComponentModel.BackgroundWorker();
            this.comboBox_pick = new System.Windows.Forms.ComboBox();
            this.label_downloaddir = new System.Windows.Forms.Label();
            this.label_Items = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_add
            // 
            this.button_add.Location = new System.Drawing.Point(12, 58);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(77, 23);
            this.button_add.TabIndex = 1;
            this.button_add.Text = "add";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_mp3_Click);
            // 
            // list_queue
            // 
            this.list_queue.AllowColumnReorder = true;
            this.list_queue.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.list_queue.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.column_Channel,
            this.Undefined,
            this.column_type,
            this.column_link});
            this.list_queue.FullRowSelect = true;
            this.list_queue.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.list_queue.HideSelection = false;
            this.list_queue.Location = new System.Drawing.Point(176, 58);
            this.list_queue.Name = "list_queue";
            this.list_queue.Size = new System.Drawing.Size(595, 230);
            this.list_queue.TabIndex = 3;
            this.list_queue.UseCompatibleStateImageBehavior = false;
            this.list_queue.View = System.Windows.Forms.View.Details;
            this.list_queue.SelectedIndexChanged += new System.EventHandler(this.list_queue_SelectedIndexChanged);
            // 
            // column_Channel
            // 
            this.column_Channel.Tag = "channel";
            this.column_Channel.Text = "channel";
            this.column_Channel.Width = 80;
            // 
            // Undefined
            // 
            this.Undefined.Name = "Undefined";
            this.Undefined.Tag = "Name";
            this.Undefined.Text = "Name";
            this.Undefined.Width = 200;
            // 
            // column_type
            // 
            this.column_type.Text = "type";
            this.column_type.Width = 79;
            // 
            // column_link
            // 
            this.column_link.Text = "link";
            this.column_link.Width = 213;
            // 
            // txtBox_link
            // 
            this.txtBox_link.Location = new System.Drawing.Point(143, 12);
            this.txtBox_link.Name = "txtBox_link";
            this.txtBox_link.Size = new System.Drawing.Size(365, 20);
            this.txtBox_link.TabIndex = 4;
            this.txtBox_link.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label_title
            // 
            this.label_title.AutoSize = true;
            this.label_title.Location = new System.Drawing.Point(12, 15);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(125, 13);
            this.label_title.TabIndex = 5;
            this.label_title.Text = "Input a valid youtube link";
            this.label_title.Click += new System.EventHandler(this.label1_Click);
            // 
            // button_clear
            // 
            this.button_clear.Location = new System.Drawing.Point(12, 294);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(158, 23);
            this.button_clear.TabIndex = 6;
            this.button_clear.Text = "Clear";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clearlist);
            // 
            // button_download
            // 
            this.button_download.Location = new System.Drawing.Point(12, 116);
            this.button_download.Name = "button_download";
            this.button_download.Size = new System.Drawing.Size(160, 23);
            this.button_download.TabIndex = 7;
            this.button_download.Text = "Download";
            this.button_download.UseVisualStyleBackColor = true;
            this.button_download.Click += new System.EventHandler(this.button_startDownload);
            // 
            // button_selectDir
            // 
            this.button_selectDir.Location = new System.Drawing.Point(12, 145);
            this.button_selectDir.Name = "button_selectDir";
            this.button_selectDir.Size = new System.Drawing.Size(160, 23);
            this.button_selectDir.TabIndex = 8;
            this.button_selectDir.Text = "Select Download directory";
            this.button_selectDir.UseVisualStyleBackColor = true;
            this.button_selectDir.Click += new System.EventHandler(this.button_downloadDir);
            // 
            // save_download
            // 
            this.save_download.HelpRequest += new System.EventHandler(this.save_download_HelpRequest);
            // 
            // download_progress
            // 
            this.download_progress.Location = new System.Drawing.Point(176, 294);
            this.download_progress.Name = "download_progress";
            this.download_progress.Size = new System.Drawing.Size(595, 23);
            this.download_progress.TabIndex = 9;
            this.download_progress.Click += new System.EventHandler(this.download_progress_Click);
            // 
            // button_delete
            // 
            this.button_delete.Location = new System.Drawing.Point(12, 87);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(160, 23);
            this.button_delete.TabIndex = 11;
            this.button_delete.Text = "delete";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // downloader
            // 
            this.downloader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.downloader_DoWork);
            // 
            // comboBox_pick
            // 
            this.comboBox_pick.FormattingEnabled = true;
            this.comboBox_pick.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboBox_pick.Items.AddRange(new object[] {
            "mp4",
            "mp3"});
            this.comboBox_pick.Location = new System.Drawing.Point(95, 60);
            this.comboBox_pick.Name = "comboBox_pick";
            this.comboBox_pick.Size = new System.Drawing.Size(77, 21);
            this.comboBox_pick.TabIndex = 12;
            this.comboBox_pick.Text = "mp4";
            this.comboBox_pick.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label_downloaddir
            // 
            this.label_downloaddir.AutoSize = true;
            this.label_downloaddir.Location = new System.Drawing.Point(13, 36);
            this.label_downloaddir.Name = "label_downloaddir";
            this.label_downloaddir.Size = new System.Drawing.Size(35, 13);
            this.label_downloaddir.TabIndex = 13;
            this.label_downloaddir.Text = "label1";
            this.label_downloaddir.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // label_Items
            // 
            this.label_Items.AutoSize = true;
            this.label_Items.Location = new System.Drawing.Point(758, 42);
            this.label_Items.Name = "label_Items";
            this.label_Items.Size = new System.Drawing.Size(13, 13);
            this.label_Items.TabIndex = 14;
            this.label_Items.Text = "0";
            this.label_Items.Click += new System.EventHandler(this.label_Items_Click);
            // 
            // printingBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(783, 329);
            this.Controls.Add(this.label_Items);
            this.Controls.Add(this.label_downloaddir);
            this.Controls.Add(this.comboBox_pick);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.download_progress);
            this.Controls.Add(this.button_selectDir);
            this.Controls.Add(this.button_download);
            this.Controls.Add(this.button_clear);
            this.Controls.Add(this.label_title);
            this.Controls.Add(this.txtBox_link);
            this.Controls.Add(this.list_queue);
            this.Controls.Add(this.button_add);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "printingBox";
            this.Text = "ytDownloader";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_add;
        private System.Windows.Forms.ListView list_queue;
        private System.Windows.Forms.TextBox txtBox_link;
        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Button button_download;
        private System.Windows.Forms.Button button_selectDir;
        private System.Windows.Forms.FolderBrowserDialog save_download;
        private System.Windows.Forms.ProgressBar download_progress;
        private System.Windows.Forms.Button button_delete;
        private System.ComponentModel.BackgroundWorker downloader;
        private System.Windows.Forms.ComboBox comboBox_pick;
        private System.Windows.Forms.Label label_downloaddir;
        private System.Windows.Forms.Label label_Items;
        private System.Windows.Forms.ColumnHeader column_name;
        private System.Windows.Forms.ColumnHeader column_Channel;
        private System.Windows.Forms.ColumnHeader Undefined;
        private System.Windows.Forms.ColumnHeader column_type;
        private System.Windows.Forms.ColumnHeader column_link;
    }
}

