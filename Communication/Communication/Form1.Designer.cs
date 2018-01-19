namespace Communication
{
    partial class mainForm
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
            this.richTextBoxLogs = new System.Windows.Forms.RichTextBox();
            this.textBoxSend = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.buttonReceive = new System.Windows.Forms.Button();
            this.richTextBoxRecieve = new System.Windows.Forms.RichTextBox();
            this.buttonRecieveAll = new System.Windows.Forms.Button();
            this.textBoxFile = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.buttonSendFile = new System.Windows.Forms.Button();
            this.buttonSimulatePktLoss = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // richTextBoxLogs
            // 
            this.richTextBoxLogs.Location = new System.Drawing.Point(13, 478);
            this.richTextBoxLogs.Name = "richTextBoxLogs";
            this.richTextBoxLogs.Size = new System.Drawing.Size(724, 148);
            this.richTextBoxLogs.TabIndex = 0;
            this.richTextBoxLogs.Text = "";
            // 
            // textBoxSend
            // 
            this.textBoxSend.Location = new System.Drawing.Point(12, 54);
            this.textBoxSend.Name = "textBoxSend";
            this.textBoxSend.Size = new System.Drawing.Size(323, 20);
            this.textBoxSend.TabIndex = 1;
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(372, 54);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(103, 23);
            this.buttonSend.TabIndex = 2;
            this.buttonSend.Text = "Send PKT MQ";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // buttonReceive
            // 
            this.buttonReceive.Location = new System.Drawing.Point(611, 51);
            this.buttonReceive.Name = "buttonReceive";
            this.buttonReceive.Size = new System.Drawing.Size(126, 23);
            this.buttonReceive.TabIndex = 3;
            this.buttonReceive.Text = "Receive 1 PKT MQ";
            this.buttonReceive.UseVisualStyleBackColor = true;
            this.buttonReceive.Click += new System.EventHandler(this.buttonReceive_Click);
            // 
            // richTextBoxRecieve
            // 
            this.richTextBoxRecieve.Location = new System.Drawing.Point(12, 256);
            this.richTextBoxRecieve.Name = "richTextBoxRecieve";
            this.richTextBoxRecieve.Size = new System.Drawing.Size(725, 187);
            this.richTextBoxRecieve.TabIndex = 4;
            this.richTextBoxRecieve.Text = "";
            // 
            // buttonRecieveAll
            // 
            this.buttonRecieveAll.Location = new System.Drawing.Point(611, 95);
            this.buttonRecieveAll.Name = "buttonRecieveAll";
            this.buttonRecieveAll.Size = new System.Drawing.Size(126, 23);
            this.buttonRecieveAll.TabIndex = 5;
            this.buttonRecieveAll.Text = "Recieve All PKT MQ";
            this.buttonRecieveAll.UseVisualStyleBackColor = true;
            this.buttonRecieveAll.Click += new System.EventHandler(this.buttonRecieveAll_Click);
            // 
            // textBoxFile
            // 
            this.textBoxFile.Location = new System.Drawing.Point(108, 112);
            this.textBoxFile.Name = "textBoxFile";
            this.textBoxFile.Size = new System.Drawing.Size(227, 20);
            this.textBoxFile.TabIndex = 7;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk_2);
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Location = new System.Drawing.Point(13, 109);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenFile.TabIndex = 8;
            this.buttonOpenFile.Text = "Open File";
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // buttonSendFile
            // 
            this.buttonSendFile.Location = new System.Drawing.Point(372, 112);
            this.buttonSendFile.Name = "buttonSendFile";
            this.buttonSendFile.Size = new System.Drawing.Size(103, 23);
            this.buttonSendFile.TabIndex = 9;
            this.buttonSendFile.Text = "Send File MQ";
            this.buttonSendFile.UseVisualStyleBackColor = true;
            this.buttonSendFile.Click += new System.EventHandler(this.buttonSendFile_Click);
            // 
            // buttonSimulatePktLoss
            // 
            this.buttonSimulatePktLoss.Location = new System.Drawing.Point(611, 149);
            this.buttonSimulatePktLoss.Name = "buttonSimulatePktLoss";
            this.buttonSimulatePktLoss.Size = new System.Drawing.Size(126, 23);
            this.buttonSimulatePktLoss.TabIndex = 10;
            this.buttonSimulatePktLoss.Text = "Simulate PKT Loss";
            this.buttonSimulatePktLoss.UseVisualStyleBackColor = true;
            this.buttonSimulatePktLoss.Click += new System.EventHandler(this.buttonSimulatePktLoss_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 459);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Log Sent and Activety:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 227);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Log Received:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(611, 205);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Stop RMQ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(372, 173);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 23);
            this.progressBar.TabIndex = 14;
            this.progressBar.Click += new System.EventHandler(this.progressBar_Click);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 638);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonSimulatePktLoss);
            this.Controls.Add(this.buttonSendFile);
            this.Controls.Add(this.buttonOpenFile);
            this.Controls.Add(this.textBoxFile);
            this.Controls.Add(this.buttonRecieveAll);
            this.Controls.Add(this.richTextBoxRecieve);
            this.Controls.Add(this.buttonReceive);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.textBoxSend);
            this.Controls.Add(this.richTextBoxLogs);
            this.Name = "mainForm";
            this.Text = "Communication RabbitMQ Tester";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxLogs;
        private System.Windows.Forms.TextBox textBoxSend;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Button buttonReceive;
        private System.Windows.Forms.RichTextBox richTextBoxRecieve;
        private System.Windows.Forms.Button buttonRecieveAll;
        private System.Windows.Forms.TextBox textBoxFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.Button buttonSendFile;
        private System.Windows.Forms.Button buttonSimulatePktLoss;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}

