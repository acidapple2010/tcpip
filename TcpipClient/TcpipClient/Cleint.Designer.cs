namespace TcpipClient
{
    partial class Cleint
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.connectionGroup = new System.Windows.Forms.GroupBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbServerIP = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.tbServerPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBoxLst = new System.Windows.Forms.GroupBox();
            this.tbConsoleOut = new System.Windows.Forms.RichTextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnClean = new System.Windows.Forms.Button();
            this.btnLocking = new System.Windows.Forms.Button();
            this.actionsGroup = new System.Windows.Forms.GroupBox();
            this.connectionGroup.SuspendLayout();
            this.groupBoxLst.SuspendLayout();
            this.actionsGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // connectionGroup
            // 
            this.connectionGroup.Controls.Add(this.tbName);
            this.connectionGroup.Controls.Add(this.label3);
            this.connectionGroup.Controls.Add(this.tbServerIP);
            this.connectionGroup.Controls.Add(this.btnConnect);
            this.connectionGroup.Controls.Add(this.tbServerPort);
            this.connectionGroup.Controls.Add(this.label1);
            this.connectionGroup.Controls.Add(this.label2);
            this.connectionGroup.Location = new System.Drawing.Point(12, 12);
            this.connectionGroup.Name = "connectionGroup";
            this.connectionGroup.Size = new System.Drawing.Size(287, 116);
            this.connectionGroup.TabIndex = 10;
            this.connectionGroup.TabStop = false;
            this.connectionGroup.Text = "Connection";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(95, 19);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(181, 20);
            this.tbName.TabIndex = 7;
            this.tbName.Text = "somebody";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Your Name: ";
            // 
            // tbServerIP
            // 
            this.tbServerIP.Location = new System.Drawing.Point(95, 51);
            this.tbServerIP.Name = "tbServerIP";
            this.tbServerIP.Size = new System.Drawing.Size(181, 20);
            this.tbServerIP.TabIndex = 1;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(177, 74);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(99, 36);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // tbServerPort
            // 
            this.tbServerPort.Location = new System.Drawing.Point(95, 81);
            this.tbServerPort.Name = "tbServerPort";
            this.tbServerPort.Size = new System.Drawing.Size(76, 20);
            this.tbServerPort.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Server IP: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Server Port: ";
            // 
            // groupBoxLst
            // 
            this.groupBoxLst.Controls.Add(this.tbConsoleOut);
            this.groupBoxLst.Location = new System.Drawing.Point(305, 12);
            this.groupBoxLst.Name = "groupBoxLst";
            this.groupBoxLst.Size = new System.Drawing.Size(303, 298);
            this.groupBoxLst.TabIndex = 11;
            this.groupBoxLst.TabStop = false;
            this.groupBoxLst.Text = "Listen";
            // 
            // tbConsoleOut
            // 
            this.tbConsoleOut.Location = new System.Drawing.Point(6, 19);
            this.tbConsoleOut.Name = "tbConsoleOut";
            this.tbConsoleOut.Size = new System.Drawing.Size(291, 273);
            this.tbConsoleOut.TabIndex = 2;
            this.tbConsoleOut.Text = "";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(9, 91);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(99, 30);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnClean
            // 
            this.btnClean.Location = new System.Drawing.Point(9, 55);
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(99, 30);
            this.btnClean.TabIndex = 2;
            this.btnClean.Text = "Change request";
            this.btnClean.UseVisualStyleBackColor = true;
            this.btnClean.Click += new System.EventHandler(this.btnChange_Request_Click);
            // 
            // btnLocking
            // 
            this.btnLocking.Location = new System.Drawing.Point(9, 19);
            this.btnLocking.Name = "btnLocking";
            this.btnLocking.Size = new System.Drawing.Size(99, 30);
            this.btnLocking.TabIndex = 12;
            this.btnLocking.Text = "Locking";
            this.btnLocking.UseVisualStyleBackColor = true;
            this.btnLocking.Click += new System.EventHandler(this.btnLocking_Click);
            // 
            // actionsGroup
            // 
            this.actionsGroup.Controls.Add(this.btnLocking);
            this.actionsGroup.Controls.Add(this.btnSend);
            this.actionsGroup.Controls.Add(this.btnClean);
            this.actionsGroup.Location = new System.Drawing.Point(12, 134);
            this.actionsGroup.Name = "actionsGroup";
            this.actionsGroup.Size = new System.Drawing.Size(287, 176);
            this.actionsGroup.TabIndex = 13;
            this.actionsGroup.TabStop = false;
            this.actionsGroup.Text = "Actions";
			this.actionsGroup.Enabled = false;
            // 
            // Cleint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 314);
            this.Controls.Add(this.actionsGroup);
            this.Controls.Add(this.groupBoxLst);
            this.Controls.Add(this.connectionGroup);
            this.Name = "Cleint";
            this.Text = "Client";
            this.Click += new System.EventHandler(this.Client_Load);
            this.connectionGroup.ResumeLayout(false);
            this.connectionGroup.PerformLayout();
            this.groupBoxLst.ResumeLayout(false);
            this.actionsGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox connectionGroup;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbServerIP;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox tbServerPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBoxLst;
        private System.Windows.Forms.RichTextBox tbConsoleOut;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnClean;
        private System.Windows.Forms.Button btnLocking;
        private System.Windows.Forms.GroupBox actionsGroup;
    }
}

