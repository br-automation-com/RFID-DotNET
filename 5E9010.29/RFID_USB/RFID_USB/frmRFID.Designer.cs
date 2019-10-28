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
            this.label1 = new System.Windows.Forms.Label();
            this.txtKeyLength = new System.Windows.Forms.TextBox();
            this.chkSendKeysEnter = new System.Windows.Forms.CheckBox();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(13, 15);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(141, 81);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect Reader";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.BackColor = System.Drawing.SystemColors.Control;
            this.txtOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOutput.Location = new System.Drawing.Point(17, 103);
            this.txtOutput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(545, 265);
            this.txtOutput.TabIndex = 3;
            // 
            // chkSendKeys
            // 
            this.chkSendKeys.AutoSize = true;
            this.chkSendKeys.Location = new System.Drawing.Point(376, 46);
            this.chkSendKeys.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkSendKeys.Name = "chkSendKeys";
            this.chkSendKeys.Size = new System.Drawing.Size(175, 21);
            this.chkSendKeys.TabIndex = 4;
            this.chkSendKeys.Text = "Send ID as key strokes";
            this.chkSendKeys.UseVisualStyleBackColor = true;
            this.chkSendKeys.CheckedChanged += new System.EventHandler(this.chkSendKeys_CheckedChanged);
            // 
            // chkAutoConnect
            // 
            this.chkAutoConnect.AutoSize = true;
            this.chkAutoConnect.Location = new System.Drawing.Point(163, 15);
            this.chkAutoConnect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkAutoConnect.Name = "chkAutoConnect";
            this.chkAutoConnect.Size = new System.Drawing.Size(181, 21);
            this.chkAutoConnect.TabIndex = 5;
            this.chkAutoConnect.Text = "Auto connect on startup";
            this.chkAutoConnect.UseVisualStyleBackColor = true;
            // 
            // contextMenu
            // 
            this.contextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuOpen,
            this.toolStripMenuExit});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(115, 52);
            this.contextMenu.Text = "Context";
            // 
            // toolStripMenuOpen
            // 
            this.toolStripMenuOpen.Name = "toolStripMenuOpen";
            this.toolStripMenuOpen.Size = new System.Drawing.Size(114, 24);
            this.toolStripMenuOpen.Text = "Open";
            this.toolStripMenuOpen.Click += new System.EventHandler(this.toolStripMenuOpen_Click);
            // 
            // toolStripMenuExit
            // 
            this.toolStripMenuExit.Name = "toolStripMenuExit";
            this.toolStripMenuExit.Size = new System.Drawing.Size(114, 24);
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
            this.chkSendBallon.Location = new System.Drawing.Point(376, 17);
            this.chkSendBallon.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkSendBallon.Name = "chkSendBallon";
            this.chkSendBallon.Size = new System.Drawing.Size(160, 21);
            this.chkSendBallon.TabIndex = 7;
            this.chkSendBallon.Text = "Send ID as ballon tip";
            this.chkSendBallon.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(163, 39);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.MinimumSize = new System.Drawing.Size(2, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 24);
            this.label1.TabIndex = 8;
            this.label1.Text = "Min Key Length";
            // 
            // txtKeyLength
            // 
            this.txtKeyLength.Location = new System.Drawing.Point(281, 39);
            this.txtKeyLength.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtKeyLength.Name = "txtKeyLength";
            this.txtKeyLength.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtKeyLength.Size = new System.Drawing.Size(53, 22);
            this.txtKeyLength.TabIndex = 9;
            this.txtKeyLength.Text = "1";
            // 
            // chkSendKeysEnter
            // 
            this.chkSendKeysEnter.AutoSize = true;
            this.chkSendKeysEnter.Location = new System.Drawing.Point(396, 74);
            this.chkSendKeysEnter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkSendKeysEnter.Name = "chkSendKeysEnter";
            this.chkSendKeysEnter.Size = new System.Drawing.Size(92, 21);
            this.chkSendKeysEnter.TabIndex = 10;
            this.chkSendKeysEnter.Text = "Add enter";
            this.chkSendKeysEnter.UseVisualStyleBackColor = true;
            // 
            // frmRFID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 373);
            this.Controls.Add(this.chkSendKeysEnter);
            this.Controls.Add(this.txtKeyLength);
            this.Controls.Add(this.chkSendBallon);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkAutoConnect);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.chkSendKeys);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(594, 420);
            this.MinimumSize = new System.Drawing.Size(594, 420);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtKeyLength;
        private System.Windows.Forms.CheckBox chkSendKeysEnter;
    }
}

