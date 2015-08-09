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
            this.button_addmp3 = new System.Windows.Forms.Button();
            this.button_addmp4 = new System.Windows.Forms.Button();
            this.list_queue = new System.Windows.Forms.ListView();
            this.txtBox_link = new System.Windows.Forms.TextBox();
            this.label_title = new System.Windows.Forms.Label();
            this.button_clear = new System.Windows.Forms.Button();
            this.button_download = new System.Windows.Forms.Button();
            this.button_selectDir = new System.Windows.Forms.Button();
            this.save_download = new System.Windows.Forms.FolderBrowserDialog();
            this.download_progress = new System.Windows.Forms.ProgressBar();
            this.progress_label = new System.Windows.Forms.Label();
            this.button_delete = new System.Windows.Forms.Button();
            this.downloader = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // button_addmp3
            // 
            this.button_addmp3.Location = new System.Drawing.Point(10, 48);
            this.button_addmp3.Name = "button_addmp3";
            this.button_addmp3.Size = new System.Drawing.Size(77, 23);
            this.button_addmp3.TabIndex = 1;
            this.button_addmp3.Text = "add mp3";
            this.button_addmp3.UseVisualStyleBackColor = true;
            this.button_addmp3.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_addmp4
            // 
            this.button_addmp4.Location = new System.Drawing.Point(93, 48);
            this.button_addmp4.Name = "button_addmp4";
            this.button_addmp4.Size = new System.Drawing.Size(77, 23);
            this.button_addmp4.TabIndex = 2;
            this.button_addmp4.Text = "add mp4";
            this.button_addmp4.UseVisualStyleBackColor = true;
            this.button_addmp4.Click += new System.EventHandler(this.button_addmp4_Click);
            // 
            // list_queue
            // 
            this.list_queue.Location = new System.Drawing.Point(176, 48);
            this.list_queue.Name = "list_queue";
            this.list_queue.Size = new System.Drawing.Size(332, 240);
            this.list_queue.TabIndex = 3;
            this.list_queue.UseCompatibleStateImageBehavior = false;
            this.list_queue.View = System.Windows.Forms.View.List;
            this.list_queue.SelectedIndexChanged += new System.EventHandler(this.list_queue_SelectedIndexChanged);
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
            this.button_clear.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button_download
            // 
            this.button_download.Location = new System.Drawing.Point(10, 106);
            this.button_download.Name = "button_download";
            this.button_download.Size = new System.Drawing.Size(160, 23);
            this.button_download.TabIndex = 7;
            this.button_download.Text = "Download";
            this.button_download.UseVisualStyleBackColor = true;
            this.button_download.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // button_selectDir
            // 
            this.button_selectDir.Location = new System.Drawing.Point(10, 135);
            this.button_selectDir.Name = "button_selectDir";
            this.button_selectDir.Size = new System.Drawing.Size(160, 23);
            this.button_selectDir.TabIndex = 8;
            this.button_selectDir.Text = "Select Download directory";
            this.button_selectDir.UseVisualStyleBackColor = true;
            this.button_selectDir.Click += new System.EventHandler(this.button1_Click_3);
            // 
            // save_download
            // 
            this.save_download.HelpRequest += new System.EventHandler(this.save_download_HelpRequest);
            // 
            // download_progress
            // 
            this.download_progress.Location = new System.Drawing.Point(176, 294);
            this.download_progress.Name = "download_progress";
            this.download_progress.Size = new System.Drawing.Size(305, 23);
            this.download_progress.TabIndex = 9;
            this.download_progress.Click += new System.EventHandler(this.download_progress_Click);
            // 
            // progress_label
            // 
            this.progress_label.AutoSize = true;
            this.progress_label.Location = new System.Drawing.Point(484, 299);
            this.progress_label.Name = "progress_label";
            this.progress_label.Size = new System.Drawing.Size(24, 13);
            this.progress_label.TabIndex = 10;
            this.progress_label.Text = "0/0";
            this.progress_label.Visible = false;
            // 
            // button_delete
            // 
            this.button_delete.Location = new System.Drawing.Point(10, 77);
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
            // printingBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 329);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.progress_label);
            this.Controls.Add(this.download_progress);
            this.Controls.Add(this.button_selectDir);
            this.Controls.Add(this.button_download);
            this.Controls.Add(this.button_clear);
            this.Controls.Add(this.label_title);
            this.Controls.Add(this.txtBox_link);
            this.Controls.Add(this.list_queue);
            this.Controls.Add(this.button_addmp4);
            this.Controls.Add(this.button_addmp3);
            this.Name = "printingBox";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_addmp3;
        private System.Windows.Forms.Button button_addmp4;
        private System.Windows.Forms.ListView list_queue;
        private System.Windows.Forms.TextBox txtBox_link;
        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Button button_download;
        private System.Windows.Forms.Button button_selectDir;
        private System.Windows.Forms.FolderBrowserDialog save_download;
        private System.Windows.Forms.ProgressBar download_progress;
        private System.Windows.Forms.Label progress_label;
        private System.Windows.Forms.Button button_delete;
        private System.ComponentModel.BackgroundWorker downloader;
    }
}

