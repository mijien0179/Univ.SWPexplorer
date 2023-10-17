namespace explorerClient
{
    partial class detailForm
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
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelDescFileType = new System.Windows.Forms.Label();
            this.labelDescFileLocation = new System.Windows.Forms.Label();
            this.labelDescFileSize = new System.Windows.Forms.Label();
            this.labelDescDayofMade = new System.Windows.Forms.Label();
            this.labelDescDayofModify = new System.Windows.Forms.Label();
            this.labelDescDayofAccess = new System.Windows.Forms.Label();
            this.labelFileType = new System.Windows.Forms.Label();
            this.labelFileLocation = new System.Windows.Forms.Label();
            this.labelFileSize = new System.Windows.Forms.Label();
            this.labelDayofMade = new System.Windows.Forms.Label();
            this.labelDayofModify = new System.Windows.Forms.Label();
            this.labelDayofAccess = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(151, 63);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new System.Drawing.Size(337, 21);
            this.txtFileName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(12, 162);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(503, 1);
            this.label1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(12, 281);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(503, 1);
            this.label2.TabIndex = 3;
            // 
            // labelDescFileType
            // 
            this.labelDescFileType.AutoSize = true;
            this.labelDescFileType.Location = new System.Drawing.Point(12, 194);
            this.labelDescFileType.Name = "labelDescFileType";
            this.labelDescFileType.Size = new System.Drawing.Size(65, 12);
            this.labelDescFileType.TabIndex = 4;
            this.labelDescFileType.Text = "파일 형식: ";
            // 
            // labelDescFileLocation
            // 
            this.labelDescFileLocation.AutoSize = true;
            this.labelDescFileLocation.Location = new System.Drawing.Point(12, 221);
            this.labelDescFileLocation.Name = "labelDescFileLocation";
            this.labelDescFileLocation.Size = new System.Drawing.Size(37, 12);
            this.labelDescFileLocation.TabIndex = 5;
            this.labelDescFileLocation.Text = "위치: ";
            // 
            // labelDescFileSize
            // 
            this.labelDescFileSize.AutoSize = true;
            this.labelDescFileSize.Location = new System.Drawing.Point(10, 248);
            this.labelDescFileSize.Name = "labelDescFileSize";
            this.labelDescFileSize.Size = new System.Drawing.Size(37, 12);
            this.labelDescFileSize.TabIndex = 6;
            this.labelDescFileSize.Text = "크기: ";
            // 
            // labelDescDayofMade
            // 
            this.labelDescDayofMade.AutoSize = true;
            this.labelDescDayofMade.Location = new System.Drawing.Point(10, 302);
            this.labelDescDayofMade.Name = "labelDescDayofMade";
            this.labelDescDayofMade.Size = new System.Drawing.Size(65, 12);
            this.labelDescDayofMade.TabIndex = 7;
            this.labelDescDayofMade.Text = "만든 날짜: ";
            // 
            // labelDescDayofModify
            // 
            this.labelDescDayofModify.AutoSize = true;
            this.labelDescDayofModify.Location = new System.Drawing.Point(10, 329);
            this.labelDescDayofModify.Name = "labelDescDayofModify";
            this.labelDescDayofModify.Size = new System.Drawing.Size(77, 12);
            this.labelDescDayofModify.TabIndex = 8;
            this.labelDescDayofModify.Text = "수정한 날짜: ";
            // 
            // labelDescDayofAccess
            // 
            this.labelDescDayofAccess.AutoSize = true;
            this.labelDescDayofAccess.Location = new System.Drawing.Point(10, 356);
            this.labelDescDayofAccess.Name = "labelDescDayofAccess";
            this.labelDescDayofAccess.Size = new System.Drawing.Size(89, 12);
            this.labelDescDayofAccess.TabIndex = 9;
            this.labelDescDayofAccess.Text = "액세스한 날짜: ";
            // 
            // labelFileType
            // 
            this.labelFileType.AutoSize = true;
            this.labelFileType.Location = new System.Drawing.Point(149, 194);
            this.labelFileType.Name = "labelFileType";
            this.labelFileType.Size = new System.Drawing.Size(81, 12);
            this.labelFileType.TabIndex = 10;
            this.labelFileType.Text = "labelFileType";
            // 
            // labelFileLocation
            // 
            this.labelFileLocation.AutoSize = true;
            this.labelFileLocation.Location = new System.Drawing.Point(149, 221);
            this.labelFileLocation.Name = "labelFileLocation";
            this.labelFileLocation.Size = new System.Drawing.Size(100, 12);
            this.labelFileLocation.TabIndex = 11;
            this.labelFileLocation.Text = "labelFileLocation";
            // 
            // labelFileSize
            // 
            this.labelFileSize.AutoSize = true;
            this.labelFileSize.Location = new System.Drawing.Point(149, 248);
            this.labelFileSize.Name = "labelFileSize";
            this.labelFileSize.Size = new System.Drawing.Size(77, 12);
            this.labelFileSize.TabIndex = 12;
            this.labelFileSize.Text = "labelFileSize";
            // 
            // labelDayofMade
            // 
            this.labelDayofMade.AutoSize = true;
            this.labelDayofMade.Location = new System.Drawing.Point(149, 302);
            this.labelDayofMade.Name = "labelDayofMade";
            this.labelDayofMade.Size = new System.Drawing.Size(96, 12);
            this.labelDayofMade.TabIndex = 13;
            this.labelDayofMade.Text = "labelDayofMade";
            // 
            // labelDayofModify
            // 
            this.labelDayofModify.AutoSize = true;
            this.labelDayofModify.Location = new System.Drawing.Point(149, 329);
            this.labelDayofModify.Name = "labelDayofModify";
            this.labelDayofModify.Size = new System.Drawing.Size(102, 12);
            this.labelDayofModify.TabIndex = 14;
            this.labelDayofModify.Text = "labelDayofModify";
            // 
            // labelDayofAccess
            // 
            this.labelDayofAccess.AutoSize = true;
            this.labelDayofAccess.Location = new System.Drawing.Point(149, 356);
            this.labelDayofAccess.Name = "labelDayofAccess";
            this.labelDayofAccess.Size = new System.Drawing.Size(107, 12);
            this.labelDayofAccess.TabIndex = 15;
            this.labelDayofAccess.Text = "labelDayofAccess";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(414, 386);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(101, 37);
            this.btnOK.TabIndex = 16;
            this.btnOK.Text = "확인";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(114, 134);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // detailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(527, 435);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.labelDayofAccess);
            this.Controls.Add(this.labelDayofModify);
            this.Controls.Add(this.labelDayofMade);
            this.Controls.Add(this.labelFileSize);
            this.Controls.Add(this.labelFileLocation);
            this.Controls.Add(this.labelFileType);
            this.Controls.Add(this.labelDescDayofAccess);
            this.Controls.Add(this.labelDescDayofModify);
            this.Controls.Add(this.labelDescDayofMade);
            this.Controls.Add(this.labelDescFileSize);
            this.Controls.Add(this.labelDescFileLocation);
            this.Controls.Add(this.labelDescFileType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.pictureBox1);
            this.MaximumSize = new System.Drawing.Size(543, 474);
            this.MinimumSize = new System.Drawing.Size(543, 474);
            this.Name = "detailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "detailForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelDescFileType;
        private System.Windows.Forms.Label labelDescFileLocation;
        private System.Windows.Forms.Label labelDescFileSize;
        private System.Windows.Forms.Label labelDescDayofMade;
        private System.Windows.Forms.Label labelDescDayofModify;
        private System.Windows.Forms.Label labelDescDayofAccess;
        private System.Windows.Forms.Label labelFileType;
        private System.Windows.Forms.Label labelFileLocation;
        private System.Windows.Forms.Label labelFileSize;
        private System.Windows.Forms.Label labelDayofMade;
        private System.Windows.Forms.Label labelDayofModify;
        private System.Windows.Forms.Label labelDayofAccess;
        private System.Windows.Forms.Button btnOK;
    }
}