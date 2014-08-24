﻿namespace PhotoNamer
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.goButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.deleteOriginalsYes = new System.Windows.Forms.RadioButton();
            this.deleteOriginalsNo = new System.Windows.Forms.RadioButton();
            this.deleteOriginalsLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.formatStringTextBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // goButton
            // 
            this.goButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.goButton.Location = new System.Drawing.Point(445, 138);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(75, 23);
            this.goButton.TabIndex = 0;
            this.goButton.Text = "Go";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.GoButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Path";
            // 
            // pathTextBox
            // 
            this.pathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathTextBox.Location = new System.Drawing.Point(105, 12);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(334, 20);
            this.pathTextBox.TabIndex = 2;
            this.pathTextBox.Text = "C:\\Temp";
            // 
            // browseButton
            // 
            this.browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseButton.Location = new System.Drawing.Point(445, 10);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 3;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.folderBrowserDialog.SelectedPath = "C:\\Temp";
            // 
            // deleteOriginalsYes
            // 
            this.deleteOriginalsYes.AutoSize = true;
            this.deleteOriginalsYes.Location = new System.Drawing.Point(105, 51);
            this.deleteOriginalsYes.Name = "deleteOriginalsYes";
            this.deleteOriginalsYes.Size = new System.Drawing.Size(43, 17);
            this.deleteOriginalsYes.TabIndex = 4;
            this.deleteOriginalsYes.Text = "Yes";
            this.deleteOriginalsYes.UseVisualStyleBackColor = true;
            // 
            // deleteOriginalsNo
            // 
            this.deleteOriginalsNo.AutoSize = true;
            this.deleteOriginalsNo.Checked = true;
            this.deleteOriginalsNo.Location = new System.Drawing.Point(154, 51);
            this.deleteOriginalsNo.Name = "deleteOriginalsNo";
            this.deleteOriginalsNo.Size = new System.Drawing.Size(39, 17);
            this.deleteOriginalsNo.TabIndex = 5;
            this.deleteOriginalsNo.TabStop = true;
            this.deleteOriginalsNo.Text = "No";
            this.deleteOriginalsNo.UseVisualStyleBackColor = true;
            // 
            // deleteOriginalsLabel
            // 
            this.deleteOriginalsLabel.AutoSize = true;
            this.deleteOriginalsLabel.Location = new System.Drawing.Point(12, 53);
            this.deleteOriginalsLabel.Name = "deleteOriginalsLabel";
            this.deleteOriginalsLabel.Size = new System.Drawing.Size(87, 13);
            this.deleteOriginalsLabel.TabIndex = 6;
            this.deleteOriginalsLabel.Text = "Delete Originals?";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Format string";
            // 
            // formatStringTextBox
            // 
            this.formatStringTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formatStringTextBox.Location = new System.Drawing.Point(105, 87);
            this.formatStringTextBox.Name = "formatStringTextBox";
            this.formatStringTextBox.Size = new System.Drawing.Size(415, 20);
            this.formatStringTextBox.TabIndex = 8;
            this.formatStringTextBox.Text = "100 {0}.jpg";
            this.formatStringTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.deleteOriginalsNo);
            this.panel1.Controls.Add(this.browseButton);
            this.panel1.Controls.Add(this.pathTextBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.goButton);
            this.panel1.Controls.Add(this.deleteOriginalsYes);
            this.panel1.Controls.Add(this.deleteOriginalsLabel);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.formatStringTextBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(532, 178);
            this.panel1.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.logTextBox);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 178);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(532, 237);
            this.panel2.TabIndex = 10;
            // 
            // logTextBox
            // 
            this.logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox.Location = new System.Drawing.Point(0, 0);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox.Size = new System.Drawing.Size(532, 237);
            this.logTextBox.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 415);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "PhotoNamer";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox pathTextBox;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.RadioButton deleteOriginalsYes;
        private System.Windows.Forms.RadioButton deleteOriginalsNo;
        private System.Windows.Forms.Label deleteOriginalsLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox formatStringTextBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox logTextBox;
    }
}