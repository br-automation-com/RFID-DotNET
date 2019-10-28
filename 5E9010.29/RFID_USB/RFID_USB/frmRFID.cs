using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BR_RFID_DRIVER;
using cs_IniHandlerDevelop;

namespace RFID_USB
{
    public partial class frmRFID : Form
    {
        BR_RFID_DRIVER.BR_RFID RFID_USB;
        IniStructure ini;
        int ResponseTimeout;
        int RefreshTimer;

        public frmRFID()
        {
            InitializeComponent();
        }

        // ------------------------------------------------------------------------
        // Load application
        private void frmRFID_Load(object sender, EventArgs e)
        {
            // --------------------------------------------------------------------
            // Load software parameter
            ini = IniStructure.ReadIni(System.Windows.Forms.Application.StartupPath + "\\RFID.ini");
            if (ini == null)
            {
                MessageBox.Show("Configuration is missing or corrupt!" + System.Windows.Forms.Application.StartupPath, "Error");
                System.Windows.Forms.Application.Exit();
                return;
            }
            // Load configuration
            ResponseTimeout = Convert.ToInt16(ini.GetValue("RFID", "ResponseTimeout"));
            RefreshTimer = Convert.ToInt16(ini.GetValue("RFID", "RefreshTimer"));
            txtKeyLength.Text = ini.GetValue("RFID", "KeyMinLength");

            chkAutoConnect.Checked = Convert.ToBoolean(ini.GetValue("CHK", "AutoConnect"));
            chkSendKeys.Checked = Convert.ToBoolean(ini.GetValue("CHK", "SendKeys"));
            chkSendKeysEnter.Checked = Convert.ToBoolean(ini.GetValue("CHK", "SendKeysEnter"));
            chkSendBallon.Checked = Convert.ToBoolean(ini.GetValue("CHK", "SendBallon"));
            if (chkAutoConnect.Checked) btnConnect_Click(null, null);
        }

        // ------------------------------------------------------------------------
        // Close application
        private void frmRFID_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ini == null) return;
            // Save current options
            ini.ModifyValue("RFID", "KeyMinLength", txtKeyLength.Text);
            ini.ModifyValue("CHK", "AutoConnect", chkAutoConnect.Checked.ToString());
            ini.ModifyValue("CHK", "SendKeys", chkSendKeys.Checked.ToString());
            ini.ModifyValue("CHK", "SendKeysEnter", chkSendKeysEnter.Checked.ToString());
            ini.ModifyValue("CHK", "SendBallon", chkSendBallon.Checked.ToString());
            IniStructure.WriteIni(ini, System.Windows.Forms.Application.StartupPath + "\\RFID.ini");
        }

        // ------------------------------------------------------------------------
        // Connect to RFID reader
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                btnConnect.Enabled = false;
                // Create new RFID driver instance and try to connect
                RFID_USB = new BR_RFID();
                RFID_USB.timeout = ResponseTimeout;
                RFID_USB.refresh = RefreshTimer;
                RFID_USB.key_min_length = Convert.ToInt16(txtKeyLength.Text);
                RFID_USB.OnException += new BR_RFID.ExceptionData(RFID_USB_OnException);
                RFID_USB.OnResponseData += new BR_RFID.ResponseData(RFID_USB_OnResponseData);
                RFID_USB.connect();

                if (RFID_USB.port != -1) txtOutput.AppendText(DateTime.Now.TimeOfDay + " RFID receiver found on port " + RFID_USB.port + "\r\n");
                if (chkSendBallon.Checked)
                {
                    trayIcon.BalloonTipText = " RFID receiver found on port " + RFID_USB.port;
                    trayIcon.ShowBalloonTip(3);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed because " + ex.Message);
                btnConnect.Enabled = true;
            }
        }

        // ------------------------------------------------------------------------
        // RFID driver exception exception
        void RFID_USB_OnException(byte exception)
        {
            // ------------------------------------------------------------------
            // Seperate calling threads
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new BR_RFID.ExceptionData(RFID_USB_OnException), new object[] { exception });
                return;
            }

            switch(exception)
            {
                case BR_RFID.excReaderNotFound:
                    MessageBox.Show("No RFID USB reader found!", "B&R RFID Reader", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnConnect.Enabled = true;
                    break;
                case BR_RFID.excResponseTimeout:
                    MessageBox.Show("RFID reader timeout!", "B&R RFID Reader", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnConnect.Enabled = true;
                    break;
                case BR_RFID.excResponseSize:
                    txtOutput.AppendText(DateTime.Now.TimeOfDay + " False key reading\r\n");
                    if (chkSendBallon.Checked)
                    {
                        trayIcon.BalloonTipText = " False key reading!";
                        trayIcon.ShowBalloonTip(3);
                    }
                    break;
            }
        }

        // ------------------------------------------------------------------------
        // New data from RFID reader
        void RFID_USB_OnResponseData(string id)
        {
            // ------------------------------------------------------------------
            // Seperate calling threads
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new BR_RFID.ResponseData(RFID_USB_OnResponseData), new object[] { id });
                return;
            }

            if (chkSendKeys.Checked) SendKeys.Send(id);
            if (chkSendKeysEnter.Checked) SendKeys.Send("\r");
            if (chkSendBallon.Checked)
            {
                trayIcon.BalloonTipText = "TAG ID: " + id;
                trayIcon.ShowBalloonTip(1);
            }
            txtOutput.AppendText(DateTime.Now.TimeOfDay + " ID:" + id + "\r\n");
            txtOutput.ScrollToCaret();
        }

        // ------------------------------------------------------------------------
        // System tray handling
        private void toolStripMenuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
            WindowState = FormWindowState.Normal;
        }
        private void toolStripMenuOpen_Click(object sender, EventArgs e)
        {
            toolStripMenuOpen_Click(null, null);
        }
        private void frmRFID_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                trayIcon.Visible = true;
            }
        }
        private void toolStripMenuOpen_Click(object sender, MouseEventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
            trayIcon.Visible = false;
        }
        private void frmRFID_Shown(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void chkSendKeys_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkSendKeys.Checked) chkSendKeysEnter.Checked = false;
        }  
    }
}
