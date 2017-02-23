namespace TcpipServer
{
    partial class Server
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
            this.groupBoxLst = new System.Windows.Forms.GroupBox();
            this.tbConsoleOut = new System.Windows.Forms.RichTextBox();
            this.btnStartListening = new System.Windows.Forms.Button();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.tbIPAddress = new System.Windows.Forms.TextBox();
            this.btnClean = new System.Windows.Forms.Button();
            this.connectionGroup = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBoxLst.SuspendLayout();
            this.connectionGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxLst
            // 
            this.groupBoxLst.Controls.Add(this.btnClean);
            this.groupBoxLst.Controls.Add(this.tbConsoleOut);
            this.groupBoxLst.Location = new System.Drawing.Point(12, 106);
            this.groupBoxLst.Name = "groupBoxLst";
            this.groupBoxLst.Size = new System.Drawing.Size(302, 286);
            this.groupBoxLst.TabIndex = 0;
            this.groupBoxLst.TabStop = false;
            this.groupBoxLst.Text = "Listen";
            // 
            // tbConsoleOut
            // 
            this.tbConsoleOut.Location = new System.Drawing.Point(6, 13);
            this.tbConsoleOut.Name = "tbConsoleOut";
            this.tbConsoleOut.Size = new System.Drawing.Size(290, 231);
            this.tbConsoleOut.TabIndex = 1;
            this.tbConsoleOut.Text = "";
            // 
            // btnStartListening
            // 
            this.btnStartListening.Location = new System.Drawing.Point(197, 41);
            this.btnStartListening.Name = "btnStartListening";
            this.btnStartListening.Size = new System.Drawing.Size(99, 32);
            this.btnStartListening.TabIndex = 2;
            this.btnStartListening.Text = "Listen";
            this.btnStartListening.UseVisualStyleBackColor = true;
            this.btnStartListening.Click += new System.EventHandler(this.btnStartListening_Click);
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(67, 48);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(124, 20);
            this.tbPort.TabIndex = 1;
            // 
            // tbIPAddress
            // 
            this.tbIPAddress.Location = new System.Drawing.Point(67, 13);
            this.tbIPAddress.Name = "tbIPAddress";
            this.tbIPAddress.Size = new System.Drawing.Size(229, 20);
            this.tbIPAddress.TabIndex = 0;
            // 
            // btnClean
            // 
            this.btnClean.Location = new System.Drawing.Point(197, 250);
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(99, 30);
            this.btnClean.TabIndex = 2;
            this.btnClean.Text = "Clean";
            this.btnClean.UseVisualStyleBackColor = true;
            this.btnClean.Click += new System.EventHandler(this.btnClean_Click);
            // 
            // connectionGroup
            // 
            this.connectionGroup.Controls.Add(this.btnStartListening);
            this.connectionGroup.Controls.Add(this.tbPort);
            this.connectionGroup.Controls.Add(this.tbIPAddress);
            this.connectionGroup.Controls.Add(this.label2);
            this.connectionGroup.Controls.Add(this.label4);
            this.connectionGroup.Location = new System.Drawing.Point(12, 13);
            this.connectionGroup.Name = "connectionGroup";
            this.connectionGroup.Size = new System.Drawing.Size(302, 87);
            this.connectionGroup.TabIndex = 11;
            this.connectionGroup.TabStop = false;
            this.connectionGroup.Text = "Connection";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Server IP: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Server Port: ";
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 404);
            this.Controls.Add(this.connectionGroup);
            this.Controls.Add(this.groupBoxLst);
            this.Name = "Server";
            this.Text = "Server";
            this.Load += new System.EventHandler(this.Server_Load);
            this.groupBoxLst.ResumeLayout(false);
            this.connectionGroup.ResumeLayout(false);
            this.connectionGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxLst;
        private System.Windows.Forms.Button btnStartListening;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.TextBox tbIPAddress;
        private System.Windows.Forms.Button btnClean;
        private System.Windows.Forms.GroupBox connectionGroup;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox tbConsoleOut;
    }
}

