﻿using System;
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
        private static int _key_min_length = 16;
        private static int _port_cur = -1;
        private static int _port_max = 10;
        private static string _revision;
        private static bool _connected = false;
        private static string _id;
        private static string _id_old;
        private System.IO.Ports.SerialPort serRFID;
        internal Timer RefreshTimer;
        internal Timer TimeoutTimer;
        internal string _data_ser;
        internal int _retrys = 0;

        /// <summary>Constant for reader exceptions.</summary>
        public const byte excReaderNotFound = 1;
        public const byte excResponseTimeout = 2;
        public const byte excResponseSize = 3;
        public const byte excDisconnected = 4;
        public const byte excPortGeneric = 10;
        public const byte excPortDoesNotExist = 11;
        public const byte excPortAccesDenied = 12;
        public const byte excPortWrongDevice = 13;

        // ------------------------------------------------------------------------
        /// <summary>Response data event. This event is called when new data arrives</summary>
        public delegate void ResponseData(string id);
        /// <summary>Response data event. This event is called when new data arrives</summary>
        public event ResponseData OnResponseData;
        /// <summary>Exception data event. This event is called when the data is incorrect</summary>
        public delegate void ExceptionData(byte exception, int port);
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
        /// <summary>Revision number for the RFID reader</summary>
        public string revision
        {
            get { return _revision; }
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
            if (_connected) return;

            serRFID = new System.IO.Ports.SerialPort();
            serRFID.ReadTimeout = _timeout;
            serRFID.WriteTimeout = _timeout;
            _port_cur = 1;

            // Search for RFID reader
            while (_port_cur < _port_max)
            {
                // Find open COM port
                _port_cur = findCOMport(_port_cur);
                if (_port_cur < _port_max)
                {
                    try
                    {
                        // Send test data and check response
                        serRFID.WriteLine("Life");
                        Thread.Sleep(_timeout);
                        _data_ser = serRFID.ReadExisting();
                        // Found RFID reader
                        if (_data_ser.Contains("OK") == true)
                        {
                            // Get RFID reader details
                            serRFID.WriteLine("Show_Revision");
                            _revision = serRFID.ReadExisting().Replace("Command show_revision ->", "").Replace("\r\n", " ").Trim();
                            serRFID.WriteLine(_revision);
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
                            if (OnException != null) OnException(excPortWrongDevice, _port_cur);
                            _port_cur++;
                        }
                    }
                    catch (Exception ex)
                    {
                        serRFID.Close();
                        if (OnException != null) OnException(excPortGeneric, _port_cur);
                        _port_cur++;
                    }
                }
            }
            // Send exception when RFID reader was not found
            if (_port_cur == _port_max)
            {
                _port_cur = -1;
                if (OnException != null) OnException(excReaderNotFound, _port_max);
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
            catch (UnauthorizedAccessException ex)
            {
                if (OnException != null) OnException(excPortAccesDenied, cur_port);
            }
            catch (IOException ex)
            {
                if (OnException != null) OnException(excPortDoesNotExist, cur_port);
            }
            catch (Exception ex)
            {
                if (OnException != null) OnException(excPortGeneric, cur_port);
            }
            if (cur_port < _port_max) port = findCOMport(cur_port + 1);
            return port;
        }

        // ------------------------------------------------------------------------
        // Return data from RFID reader
        void serRFID_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            // Append data until end is reached, reset timer
            _data_ser = _data_ser + serRFID.ReadExisting();
            Debug.WriteLine(DateTime.Now + " " + DateTime.Now.Millisecond + " Read response:" + _data_ser);
            TimeoutTimer.Change(-1, 0);
            _retrys = 0;

            if (_data_ser.Contains("Command") || _data_ser.Contains("PiccSelect"))
            {
                // Split data and send respond
                try
                {
                    // Wait until all 16 slots are filled
                     _data_ser = _data_ser.Replace("\r", "");
                    string[] data = _data_ser.Split('\n');
                    // Extract IDs from data slots
                    for (int i = 0; i < data.GetUpperBound(0); i++)
                    {
                        if (data[i].Contains("PiccSelect:") && OnResponseData != null)
                        {
                            _id = data[i].Replace("PiccSelect:", "").Trim();
                            if (_id_old != _id && _id != "")
                            {
                                if (_id.Length >= _key_min_length) OnResponseData(id);
                                else if (OnException != null) OnException(excResponseSize, _port_cur);
                            }
                            _id_old = _id;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
            _data_ser = "";
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
                if (OnException != null) OnException(excDisconnected, port);
                return;
            }
            if ((_data_ser == "") && (serRFID != null))
            {
                try
                {
                    _id_old = "";
                    serRFID.DiscardInBuffer();
                    serRFID.WriteLine("Show_SN");
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
            Debug.WriteLine(DateTime.Now + " " + DateTime.Now.Millisecond + " Timeout timer, retry " + _retrys.ToString());

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
                if (OnException != null) OnException(excResponseTimeout, Convert.ToByte(serRFID.PortName));
                return;
            }
            if(TimeoutTimer != null) TimeoutTimer.Change(-1, 0);
        }
    }
}
