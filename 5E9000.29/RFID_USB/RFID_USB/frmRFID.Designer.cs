namespace RFID_USB
{
    partial class frmRFID
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRFID));
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.chkSendKeys = new System.Windows.Forms.CheckBox();
            this.chkAutoConnect = new System.Windows.Forms.CheckBox();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.chkSendBallon = new System.Windows.Forms.CheckBox();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(10, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(106, 43);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect Reader";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.BackColor = System.Drawing.SystemColors.Control;
            this.txtOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOutput.Location = new System.Drawing.Point(13, 68);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(409, 232);
            this.txtOutput.TabIndex = 3;
            // 
            // chkSendKeys
            // 
            this.chkSendKeys.AutoSize = true;
            this.chkSendKeys.Location = new System.Drawing.Point(282, 12);
            this.chkSendKeys.Name = "chkSendKeys";
            this.chkSendKeys.Size = new System.Drawing.Size(136, 17);
            this.chkSendKeys.TabIndex = 4;
            this.chkSendKeys.Text = "Send ID as key strokes";
            this.chkSendKeys.UseVisualStyleBackColor = true;
            // 
            // chkAutoConnect
            // 
            this.chkAutoConnect.AutoSize = true;
            this.chkAutoConnect.Location = new System.Drawing.Point(136, 12);
            this.chkAutoConnect.Name = "chkAutoConnect";
            this.chkAutoConnect.Size = new System.Drawing.Size(140, 17);
            this.chkAutoConnect.TabIndex = 5;
            this.chkAutoConnect.Text = "Auto connect on startup";
            this.chkAutoConnect.UseVisualStyleBackColor = true;
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuOpen,
            this.toolStripMenuExit});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(104, 48);
            this.contextMenu.Text = "Context";
            // 
            // toolStripMenuOpen
            // 
            this.toolStripMenuOpen.Name = "toolStripMenuOpen";
            this.toolStripMenuOpen.Size = new System.Drawing.Size(103, 22);
            this.toolStripMenuOpen.Text = "Open";
            this.toolStripMenuOpen.Click += new System.EventHandler(this.toolStripMenuOpen_Click);
            // 
            // toolStripMenuExit
            // 
            this.toolStripMenuExit.Name = "toolStripMenuExit";
            this.toolStripMenuExit.Size = new System.Drawing.Size(103, 22);
            this.toolStripMenuExit.Text = "Exit";
            this.toolStripMenuExit.Click += new System.EventHandler(this.toolStripMenuExit_Click);
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.contextMenu;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "Open B&R RFID reader ";
            this.trayIcon.Visible = true;
            this.trayIcon.DoubleClick += new System.EventHandler(this.toolStripMenuOpen_Click);
            // 
            // chkSendBallon
            // 
            this.chkSendBallon.AutoSize = true;
            this.chkSendBallon.Location = new System.Drawing.Point(282, 35);
            this.chkSendBallon.Name = "chkSendBallon";
            this.chkSendBallon.Size = new System.Drawing.Size(124, 17);
            this.chkSendBallon.TabIndex = 7;
            this.chkSendBallon.Text = "Send ID as ballon tip";
            this.chkSendBallon.UseVisualStyleBackColor = true;
            // 
            // frmRFID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 312);
            this.Controls.Add(this.chkSendBallon);
            this.Controls.Add(this.chkAutoConnect);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.chkSendKeys);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(450, 350);
            this.MinimumSize = new System.Drawing.Size(450, 350);
            this.Name = "frmRFID";
            this.Text = "B&R RFID Tester";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmRFID_FormClosed);
            this.Load += new System.EventHandler(this.frmRFID_Load);
            this.Shown += new System.EventHandler(this.frmRFID_Shown);
            this.Resize += new System.EventHandler(this.frmRFID_Resize);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.CheckBox chkSendKeys;
        private System.Windows.Forms.CheckBox chkAutoConnect;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuExit;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuOpen;
        private System.Windows.Forms.CheckBox chkSendBallon;
    }
}

