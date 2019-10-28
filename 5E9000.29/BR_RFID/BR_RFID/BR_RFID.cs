using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace BR_RFID_DRIVER
{
    public class BR_RFID
    {
        // ------------------------------------------------------------------------
        // Private declarations
        private static ushort _timeout = 500;
        private static ushort _refresh = 500;
        private static int _port_cur = -1;
        private static ushort _port_max = 10;
        private static bool _connected = false;
        private static string _data_raw;
        private System.IO.Ports.SerialPort serRFID;
        internal Timer RefreshTimer;
        internal Timer TimeoutTimer;
        internal string _data_ser;

        /// <summary>Constant for reader exceptions.</summary>
        public const byte excReaderNotFound = 1;
        public const byte excResponseTimeout = 2;

        // ------------------------------------------------------------------------
        /// <summary>Response data event. This event is called when new data arrives</summary>
        public delegate void ResponseData(int type, string id);
        /// <summary>Response data event. This event is called when new data arrives</summary>
        public event ResponseData OnResponseData;
        /// <summary>Exception data event. This event is called when the data is incorrect</summary>
        public delegate void ExceptionData(byte exception);
        /// <summary>Exception data event. This event is called when the data is incorrect</summary>
        public event ExceptionData OnException;

        // ------------------------------------------------------------------------
        /// <summary>Response timeout. If the receiver does not answers within in this time an exception is called.</summary>
        /// <value>The default value is 500ms.</value>
        public ushort timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        // ------------------------------------------------------------------------
        /// <summary>Refresh timer for RFID polling in X ms.</summary>
        /// <value>The default value is 500ms.</value>
        public ushort refresh
        {
            get { return _refresh; }
            set { _refresh = value; }
        }

        // ------------------------------------------------------------------------
        /// <summary>Returns the raw data string from RFID reader.</summary>
        public string raw_data
        {
            get { return _data_raw; }
        }

        // ------------------------------------------------------------------------
        /// <summary>Shows if a connection is active.</summary>
        public bool connected
        {
            get { return _connected; }
        }

        // ------------------------------------------------------------------------
        /// <summary>Shows the port number where RFID was found.</summary>
        public int port
        {
            get { return _port_cur; }
        }

        // ------------------------------------------------------------------------
        /// <summary>Create master instance without parameters.</summary>
        public BR_RFID()
        {
        }

        // ------------------------------------------------------------------------
        /// <summary>Start connection to RFID reader.</summary>
        public void connect()
        {
            serRFID = new System.IO.Ports.SerialPort();
            _port_cur = 1;

            // Serach for RFID reader
            while (_port_cur < _port_max)
            {
                // Find open COM port
                _port_cur = findCOMport(_port_cur);
                if (_port_cur < _port_max)
                {
                    try
                    {
                        // Send test data and check response
                        serRFID.WriteLine("tag,0,0,#crc" + "\r");
                        Thread.Sleep(_timeout);
                        _data_ser = serRFID.ReadExisting();
                        // Found RFID reader
                        if (_data_ser.StartsWith("nak") == true)
                        {
                            RefreshTimer = new Timer(new TimerCallback(RefreshTimer_Elapsed), this, 0, _refresh);
                            TimeoutTimer = new Timer(new TimerCallback(TimeoutTimer_Elapsed), this, _timeout, _timeout);
                            serRFID.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serRFID_DataReceived);
                            _connected = true;
                            _data_ser = "";
                            return;
                        }
                        else
                        {
                            serRFID.Close();
                            _port_cur++;
                        }
                    }
                    catch (Exception ex)
                    {
                        serRFID.Close();
                        _port_cur++;
                    }
                }
            }

            // Send exception when RFID reader was not found
            if (_port_cur == _port_max)
            {
                _port_cur = -1;
                if (OnException != null) OnException(excReaderNotFound);
            }
        }

        // ------------------------------------------------------------------------
        // Find valid COM port
        private int findCOMport(int cur_port)
        {
            int port = cur_port;
            // Try to open port, proceed with next port if not successfull until max port is reached
            try
            {
                serRFID.PortName = "COM" + cur_port.ToString();
                serRFID.BaudRate = 9600;
                serRFID.Open();
                return port;
            }
            catch (Exception ex)
            {
                if (cur_port < _port_max) port = findCOMport(cur_port + 1);
                return port;
            }
        }

        // ------------------------------------------------------------------------
        // Return data from RFID reader
        void serRFID_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            // Append data until end is reached
            _data_ser = _data_ser + serRFID.ReadExisting();
            TimeoutTimer.Change(_timeout, _timeout);
            if (_data_ser.EndsWith("\r\n"))
            {
                // Dont repeat data
                if (_data_ser != _data_raw)
                {
                    _data_raw = _data_ser;
                    // Split data and send respond
                    try
                    {
                        string[] data = _data_ser.Split(',');
                        if ((data[2] != "3") && (OnResponseData != null)) OnResponseData(Convert.ToInt32(data[2]), data[3]);
                        if (_data_ser.StartsWith("nak") == false)
                        {
                            string id = _data_ser;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                }
                _data_ser = "";
            }
        }

        // ------------------------------------------------------------------------
        // Refresh data telegram
        internal void RefreshTimer_Elapsed(object state)
        {
            if ((_data_ser == "") && (serRFID != null) && (serRFID.IsOpen))
            {
                try
                {
                    serRFID.WriteLine("tag,0,0,#crc" + "\r");
                    TimeoutTimer.Change(_timeout, _timeout);
                }
                catch (Exception ex)
                {
                }
            }
        }

        // ------------------------------------------------------------------------
        // Response timeout timer
        internal void TimeoutTimer_Elapsed(object state)
        {
            _connected = false;
            try
            {
                serRFID.Close();
                serRFID.Dispose();
                serRFID = null;
                RefreshTimer.Dispose();
                TimeoutTimer.Dispose();
            }
            catch (Exception ex)
            {
            }
            if (OnException != null) OnException(excResponseTimeout);
        }

    }
}
