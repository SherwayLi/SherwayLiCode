namespace ImageTry
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            btnAddFile = new Button();
            lstFileList1 = new ListBox();
            cmbProcessing = new ComboBox();
            lblFileList = new Label();
            btnRemoveFile = new Button();
            btnViewResult = new Button();
            btnStart = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // btnAddFile
            // 
            btnAddFile.BackColor = Color.DarkSeaGreen;
            btnAddFile.Font = new Font("宋体", 11F);
            btnAddFile.Location = new Point(364, 98);
            btnAddFile.Margin = new Padding(4);
            btnAddFile.Name = "btnAddFile";
            btnAddFile.Size = new Size(218, 64);
            btnAddFile.TabIndex = 0;
            btnAddFile.Text = "+ 添加图像";
            btnAddFile.UseVisualStyleBackColor = false;
            btnAddFile.Click += btnAddFile_Click;
            // 
            // lstFileList1
            // 
            lstFileList1.FormattingEnabled = true;
            lstFileList1.Location = new Point(60, 186);
            lstFileList1.Margin = new Padding(4);
            lstFileList1.Name = "lstFileList1";
            lstFileList1.SelectionMode = SelectionMode.MultiExtended;
            lstFileList1.Size = new Size(1267, 376);
            lstFileList1.TabIndex = 1;
            // 
            // cmbProcessing
            // 
            cmbProcessing.BackColor = SystemColors.GradientActiveCaption;
            cmbProcessing.FormattingEnabled = true;
            cmbProcessing.Items.AddRange(new object[] { "灰度", "放大", "缩小", "顺时针旋转90°", "逆时针旋转90°", "图像反转", "模糊", "边缘提取" });
            cmbProcessing.Location = new Point(60, 617);
            cmbProcessing.Margin = new Padding(4);
            cmbProcessing.Name = "cmbProcessing";
            cmbProcessing.Size = new Size(287, 39);
            cmbProcessing.TabIndex = 2;
            // 
            // lblFileList
            // 
            lblFileList.AutoSize = true;
            lblFileList.Font = new Font("宋体", 12F);
            lblFileList.Location = new Point(93, 113);
            lblFileList.Margin = new Padding(4, 0, 4, 0);
            lblFileList.Name = "lblFileList";
            lblFileList.Size = new Size(175, 33);
            lblFileList.TabIndex = 6;
            lblFileList.Text = "文件列表：";
            // 
            // btnRemoveFile
            // 
            btnRemoveFile.BackColor = Color.LemonChiffon;
            btnRemoveFile.Font = new Font("宋体", 12F);
            btnRemoveFile.ForeColor = SystemColors.ActiveCaptionText;
            btnRemoveFile.Location = new Point(1378, 233);
            btnRemoveFile.Margin = new Padding(4);
            btnRemoveFile.Name = "btnRemoveFile";
            btnRemoveFile.Size = new Size(325, 106);
            btnRemoveFile.TabIndex = 7;
            btnRemoveFile.Text = "删除选中的文件";
            btnRemoveFile.UseVisualStyleBackColor = false;
            btnRemoveFile.Click += btnRemoveFile_Click;
            // 
            // btnViewResult
            // 
            btnViewResult.BackColor = Color.LightSteelBlue;
            btnViewResult.Font = new Font("宋体", 10F);
            btnViewResult.Location = new Point(1378, 406);
            btnViewResult.Margin = new Padding(4);
            btnViewResult.Name = "btnViewResult";
            btnViewResult.Size = new Size(325, 101);
            btnViewResult.TabIndex = 8;
            btnViewResult.Text = "查看选中文件的处理结果";
            btnViewResult.UseVisualStyleBackColor = false;
            btnViewResult.Click += btnViewResult_Click;
            // 
            // btnStart
            // 
            btnStart.BackColor = Color.LightSteelBlue;
            btnStart.Font = new Font("宋体", 10F);
            btnStart.Location = new Point(407, 595);
            btnStart.Margin = new Padding(4);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(175, 80);
            btnStart.TabIndex = 9;
            btnStart.Text = "开始处理";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.LemonChiffon;
            btnCancel.Font = new Font("宋体", 10F);
            btnCancel.Location = new Point(630, 595);
            btnCancel.Margin = new Padding(4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(175, 80);
            btnCancel.TabIndex = 10;
            btnCancel.Text = "取消处理";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1800, 818);
            Controls.Add(btnCancel);
            Controls.Add(btnStart);
            Controls.Add(btnViewResult);
            Controls.Add(btnRemoveFile);
            Controls.Add(lblFileList);
            Controls.Add(cmbProcessing);
            Controls.Add(lstFileList1);
            Controls.Add(btnAddFile);
            Margin = new Padding(4);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnAddFile;
        private System.Windows.Forms.ListBox lstFileList1;
        private System.Windows.Forms.ComboBox cmbProcessing;
        private System.Windows.Forms.Label lblFileList;
        private System.Windows.Forms.Button btnRemoveFile;
        private System.Windows.Forms.Button btnViewResult;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCancel;
    }
}

