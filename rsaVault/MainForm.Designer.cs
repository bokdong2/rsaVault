namespace rsaVault
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnLoadPublic = new System.Windows.Forms.Button();
            this.btnLoadPrivate = new System.Windows.Forms.Button();
            this.btnSavePublic = new System.Windows.Forms.Button();
            this.btnSavePrivate = new System.Windows.Forms.Button();
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.linklabel = new System.Windows.Forms.LinkLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(12, 12);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(187, 23);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "코드 발급";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnLoadPublic
            // 
            this.btnLoadPublic.Location = new System.Drawing.Point(12, 41);
            this.btnLoadPublic.Name = "btnLoadPublic";
            this.btnLoadPublic.Size = new System.Drawing.Size(187, 23);
            this.btnLoadPublic.TabIndex = 1;
            this.btnLoadPublic.Text = "암호화 코드 불러오기";
            this.btnLoadPublic.UseVisualStyleBackColor = true;
            this.btnLoadPublic.Click += new System.EventHandler(this.btnLoadPublic_Click);
            // 
            // btnLoadPrivate
            // 
            this.btnLoadPrivate.Location = new System.Drawing.Point(12, 70);
            this.btnLoadPrivate.Name = "btnLoadPrivate";
            this.btnLoadPrivate.Size = new System.Drawing.Size(187, 23);
            this.btnLoadPrivate.TabIndex = 2;
            this.btnLoadPrivate.Text = "복호화 코드 불러오기";
            this.btnLoadPrivate.UseVisualStyleBackColor = true;
            this.btnLoadPrivate.Click += new System.EventHandler(this.btnLoadPrivate_Click);
            // 
            // btnSavePublic
            // 
            this.btnSavePublic.Location = new System.Drawing.Point(205, 12);
            this.btnSavePublic.Name = "btnSavePublic";
            this.btnSavePublic.Size = new System.Drawing.Size(187, 23);
            this.btnSavePublic.TabIndex = 3;
            this.btnSavePublic.Text = "암호화 코드 저장";
            this.btnSavePublic.UseVisualStyleBackColor = true;
            this.btnSavePublic.Click += new System.EventHandler(this.btnSavePublic_Click);
            // 
            // btnSavePrivate
            // 
            this.btnSavePrivate.Location = new System.Drawing.Point(398, 12);
            this.btnSavePrivate.Name = "btnSavePrivate";
            this.btnSavePrivate.Size = new System.Drawing.Size(187, 23);
            this.btnSavePrivate.TabIndex = 4;
            this.btnSavePrivate.Text = "복호화 코드 저장";
            this.btnSavePrivate.UseVisualStyleBackColor = true;
            this.btnSavePrivate.Click += new System.EventHandler(this.btnSavePrivate_Click);
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(205, 41);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(187, 52);
            this.btnEncrypt.TabIndex = 5;
            this.btnEncrypt.Text = "암호화";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Location = new System.Drawing.Point(398, 41);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(187, 52);
            this.btnDecrypt.TabIndex = 6;
            this.btnDecrypt.Text = "복호화";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 99);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(488, 23);
            this.progressBar.TabIndex = 7;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(506, 99);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(79, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 145);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(611, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(48, 17);
            this.lblStatus.Text = "Ready...";
            // 
            // linklabel
            // 
            this.linklabel.AutoSize = true;
            this.linklabel.Location = new System.Drawing.Point(27, 125);
            this.linklabel.Name = "linklabel";
            this.linklabel.Size = new System.Drawing.Size(535, 12);
            this.linklabel.TabIndex = 10;
            this.linklabel.TabStop = true;
            this.linklabel.Text = "Coded By PhilipMo@TeamH4C \\\\ https://facebook.com/teamh4c \\\\ White Hacking Team";
            this.linklabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linklabel_LinkClicked);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 167);
            this.Controls.Add(this.linklabel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnDecrypt);
            this.Controls.Add(this.btnEncrypt);
            this.Controls.Add(this.btnSavePrivate);
            this.Controls.Add(this.btnSavePublic);
            this.Controls.Add(this.btnLoadPrivate);
            this.Controls.Add(this.btnLoadPublic);
            this.Controls.Add(this.btnGenerate);
            this.Name = "MainForm";
            this.Text = "RSA VAULT";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnLoadPublic;
        private System.Windows.Forms.Button btnLoadPrivate;
        private System.Windows.Forms.Button btnSavePublic;
        private System.Windows.Forms.Button btnSavePrivate;
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.LinkLabel linklabel;
    }
}

