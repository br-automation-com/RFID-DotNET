using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace BR_RFID_DRIVER
{
    public class BR_RFID
    {
        // ------------------------------------------------------------------------
        // Private declarations
        private static int _timeout = 200;
        private static int _refresh = 500;
        private static int _key_min_length = 15;
        private static int _port_cur = -1;
        private static int _port_max = 10;
        private static bool _connected = false;
        private static string _id;
        private static string _id_old;
        private static bool _id_found;
        private System.IO.Ports.SerialPort serRFID;
        internal Timer RefreshTimer;
        internal Timer TimeoutTimer;
        internal string _data_ser;
        internal int _data_empty_cnt = 0;
        internal int _retrys = 0;

        /// <summary>Constant for reader exceptions.</summary>
        public const byte excReaderNotFound = 1;
        public const byte excResponseTimeout = 2;
        public const byte excResponseSize = 3;
        public const byte excDisconnected = 4;

        // ------------------------------------------------------------------------
        /// <summary>Response data event. This event is called when new data arrives</summary>
        public delegate void ResponseData(string id);
        /// <summary>Response data event. This event is called when new data arrives</summary>
        public event ResponseData OnResponseData;
        /// <summary>Exception data event. This event is called when the data is incorrect</summary>
        public delegate void ExceptionData(byte exception);
        /// <summary>Exception data event. This event is called when the data is incorrect</summary>
        public event ExceptionData OnException;

        // ------------------------------------------------------------------------
        /// <summary>Response timeout. If the receiver does not answers within in this time an exception is called.</summary>
        /// <value>The default value is 500ms.</value>
        public int timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        // ------------------------------------------------------------------------
        /// <summary>Minimum RFID key code length.</summary>
        /// <value>The default value is 15 characters.</value>
        public int key_min_length
        {
            get { return _key_min_length; }
            set { _key_min_length = value; }
        }

        // ------------------------------------------------------------------------
        /// <summary>Refresh timer for RFID polling in X ms.</summary>
        /// <value>The default value is 500ms.</value>
        public int refresh
        {
            get { return _refresh; }
            set { _refresh = value; }
        }

        // ------------------------------------------------------------------------
        /// <summary>Returns the raw data string from RFID reader.</summary>
        public string id
        {
            get { return _id; }
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
        public void disconnect()
        {
            try
            {
                serRFID.Close();
                serRFID.Dispose();
                serRFID = null;
                RefreshTimer.Dispose();
                TimeoutTimer.Dispose();
            }
            catch(Exception ex){ }
        }

        // ------------------------------------------------------------------------
        /// <summary>Start connection to RFID reader.</summary>
            public void connect()
        {
            serRFID = new System.IO.Ports.SerialPort();
            serRFID.ReadTimeout = _timeout;
            serRFID.WriteTimeout = _timeout;
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
                        serRFID.WriteLine("010a0003041800260000");
                        Thread.Sleep(_timeout);
                        _data_ser = serRFID.ReadExisting();
                        // Found RFID reader
                        if (_data_ser.StartsWith("[]") == true)
                        {
                            // Send configuration to reader
                            serRFID.WriteLine("010C00030410002101000000");
                            _data_ser = _data_ser + serRFID.ReadExisting();
                            serRFID.WriteLine("0109000304F0000000");
                            _data_ser = _data_ser + serRFID.ReadExisting();
                            serRFID.WriteLine("0109000304F1FF0000");
                            Thread.Sleep(200);
                            _data_ser = _data_ser + serRFID.ReadExisting();
                            // Initiate refresh and timeout timer
                            RefreshTimer = new Timer(new TimerCallback(RefreshTimer_Elapsed), this, 0, _refresh);
                            TimeoutTimer = new Timer(new TimerCallback(TimeoutTimer_Elapsed), this, -1, _timeout);
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
                serRFID.BaudRate = 115200;
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
            Debug.WriteLine(DateTime.Now + " " + DateTime.Now.Millisecond + " Read response");
            // Append data until end is reached
            _data_ser = _data_ser + serRFID.ReadExisting();
            TimeoutTimer.Change(_timeout, _timeout);
            if (_data_ser.EndsWith("\r\n"))
            {
                // Split data and send respond
                try
                {
                    // Wait until all 16 slots are filled
                    _data_ser = _data_ser.Replace("\r", "").Replace("[", "").Replace("]", "");
                    string[] data = _data_ser.Split('\n');
                    // Extract IDs from data slots
                    _id_found = false;
                    for (int i = 0; i < data.GetUpperBound(0); i++)
                    {
                        if ((data[i] != ",40") && (OnResponseData != null))
                        {
                            if (data[i].LastIndexOf(',') != -1)
                            {
                                _id = data[i].Substring(0, data[i].LastIndexOf(','));
                                if (_id_old != _id)
                                {
                                    if (_id.Length >= _key_min_length) OnResponseData(id);
                                    else if (OnException != null) OnException(excResponseSize);
                                }
                                _id_old = _id;
                                _data_empty_cnt = 0;
                            }
                        }
                        else _data_empty_cnt++;
                    }
                    if (_data_empty_cnt > 15) _id_old = "";
                    TimeoutTimer.Change(-1, 0);
                    _retrys = 0;
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                _data_ser = "";
            }
        }

        // ------------------------------------------------------------------------
        // Refresh data telegram
        internal void RefreshTimer_Elapsed(object state)
        {
            Debug.WriteLine(DateTime.Now + " " + DateTime.Now.Millisecond + " Read timer");
            if(!serRFID.IsOpen)
            {
                _connected = false;
                try
                {
                    disconnect();
                }
                catch (Exception ex)
                {
                }
                if (OnException != null) OnException(excDisconnected);
                return;
            }
            if ((_data_ser == "") && (serRFID != null))
            {
                try
                {
                    serRFID.DiscardInBuffer();
                    serRFID.WriteLine("010B000304140401000000");
                    TimeoutTimer.Change(_timeout, _timeout);
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }

        // ------------------------------------------------------------------------
        // Response timeout timer
        internal void TimeoutTimer_Elapsed(object state)
        {
            Debug.WriteLine(DateTime.Now + " " + DateTime.Now.Millisecond + " Timeout timer");

            _retrys++;
            if (_retrys > 10)
            {
                _connected = false;
                try
                {
                    disconnect();
                }
                catch (Exception ex)
                {
                }
                if (OnException != null) OnException(excResponseTimeout);
            }
        }
    }
}
