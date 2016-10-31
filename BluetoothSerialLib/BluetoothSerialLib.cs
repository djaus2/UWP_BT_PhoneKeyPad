using System;
using Windows.Devices.Enumeration;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Networking.Sockets;
using System.Collections.ObjectModel;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using Windows.Devices.Bluetooth;

namespace BluetoothSerialLib
{
    /// <summary>
    /// The Main Page for the app
    /// </summary>
    public class BluetoothSerial 
    {
        private Windows.Devices.Bluetooth.Rfcomm.RfcommDeviceService _service;
        public StreamSocket _socket { get; private set; }
        private DataWriter dataWriterObject;
        private DataReader dataReaderObject;
        private CancellationTokenSource ReadCancellationTokenSource;

        public KeypadUWPLib.Keypad Keypad { get; internal set; }
        public BluetoothSerial()
        {

            //InitializeRfcommDeviceService();
            Keypad = new KeypadUWPLib.Keypad();
            ////Specific handlers
            //Keypad.KeyDown += Keypad_KeyDown;
            //Keypad.KeyUp += Keypad_KeyUp;
            //Keypad.KeyHolding += Keypad_KeyHolding;
            ////Common handlers
            //Keypad.KeyDown += Keypad_Keys;
            //Keypad.KeyUp += Keypad_Keys;
            //Keypad.KeyHolding += Keypad_Keys;
        }

        /*
        private void Keypad_Keys(object sender, KeypadUWPLib.KeypadEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Keys " + e.Key + " " + e.Action.ToString());
        }

        private void Keypad_KeyHolding(object sender, KeypadUWPLib.KeypadEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("holding   " + e.Key + " " + e.Action.ToString());
        }

        private void Keypad_KeyUp(object sender, KeypadUWPLib.KeypadEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("up   " + e.Key + " " + e.Action.ToString());
        }

        private void Keypad_KeyDown(object sender, KeypadUWPLib.KeypadEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("down " + e.Key + " " + e.Action.ToString());
        }
        */


        private DeviceInformation DeviceInfo = null;

        async public Task<bool> ConnectDevice(PairedDeviceInfo pairedDevice)//object sender, RoutedEventArgs e)
        {
            DeviceInfo = pairedDevice.DeviceInfo;

            bool success = true;
            try
            {
                _service = await RfcommDeviceService.FromIdAsync(DeviceInfo.Id);

                if (_socket != null)
                {
                    // Disposing the socket with close it and release all resources associated with the socket
                    _socket.Dispose();
                }

                _socket = new StreamSocket();
                try
                {
                    // Note: If either parameter is null or empty, the call will throw an exception
                    await _socket.ConnectAsync(_service.ConnectionHostName, _service.ConnectionServiceName);
                }
                catch (Exception ex)
                {
                    success = false;
                    System.Diagnostics.Debug.WriteLine("Connect:" + ex.Message);
                }
                // If the connection was successful, the RemoteAddress field will be populated
                if (success)
                {
                    string msg = String.Format("Connected to {0}!", _socket.Information.RemoteAddress.DisplayName);
                    System.Diagnostics.Debug.WriteLine(msg);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Overall Connect: " + ex.Message);
                _socket.Dispose();
                _socket = null;
                success = false;
            }
            return success;
        }

        private void Disconnect()
        {
            if (DeviceInfo != null)
            {

                if (_socket != null)
                {
                    // Disposing the socket with close it and release all resources associated with the socket
                    _socket.Dispose();
                }
            }
            DeviceInfo = null;
        }


        public async Task button_Click(string cmd, string param)
        {

            if (cmd != null)
            {
                switch (cmd)
                {
                    case "Disconnect":
                        if (IsRunning)
                            CancelReadTask();
                        if (_socket != null)
                        {
                            _socket.Dispose();
                            _socket = null;
                        }
                        break;
                    case "Send":
                        await Send(param);
                        break;
                    case "Clear Recv":
                        Keypad.RaiseEvent(this, new KeypadUWPLib.KeypadEventArgs( KeypadUWPLib.KeypadActions.Down, '1'));
                        break;
                    case "Start Recv":
                        await Listen();
                        break;
                    case "Stop Recv":
                        if (IsRunning)
                            CancelReadTask();
                        break;
                    case "Refresh":
                        break;
                    case "Exit":
                        if (_socket != null)
                        {
                            _socket.Dispose();
                            _socket = null;
                        }
                        break;
                }
            }
        }


        private async Task Send(string msg)
        {
            try
            {
                if (_socket.OutputStream != null)
                {
                    // Create the DataWriter object and attach to OutputStream
                    dataWriterObject = new DataWriter(_socket.OutputStream);

                    //Launch the WriteAsync task to perform the write
                    await WriteAsync(msg);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Send(): " + ex.Message);
            }
            finally
            {
                // Cleanup once complete
                if (dataWriterObject != null)
                {
                    dataWriterObject.DetachStream();
                    dataWriterObject = null;
                }
            }
        }

        /// <summary>
        /// WriteAsync: Task that asynchronously writes data from the input text box 'sendText' to the OutputStream 
        /// </summary>
        /// <returns></returns>
        private async Task WriteAsync(string msg)
        {
            Task<UInt32> storeAsyncTask;

            if (msg == "")
                msg = "none";// sendText.Text;
            if (msg.Length != 0)
            //if (msg.sendText.Text.Length != 0)
            {
                // Load the text from the sendText input text box to the dataWriter object
                dataWriterObject.WriteString(msg);

                // Launch an async task to complete the write operation
                storeAsyncTask = dataWriterObject.StoreAsync().AsTask();

                UInt32 bytesWritten = await storeAsyncTask;
                if (bytesWritten > 0)
                {
                    string status_Text = msg + ", ";
                    status_Text += bytesWritten.ToString();
                    status_Text += " bytes written successfully!";
                    //System.Diagnostics.Debug.WriteLine(status_Text);
                }
            }
            else
            {
                string status_Text2 = "Enter the text you want to write and then click on 'WRITE'";
                System.Diagnostics.Debug.WriteLine(status_Text2);
            }
        }

        private bool IsRunning = false;

        /// <summary>
        /// - Create a DataReader object
        /// - Create an async task to read from the SerialDevice InputStream
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async Task Listen()
        {
            try
            {
                ReadCancellationTokenSource = new CancellationTokenSource();
                if (_socket.InputStream != null)
                {
                    dataReaderObject = new DataReader(_socket.InputStream);
                    //this.buttonStopRecv.IsEnabled = true;
                    //this.buttonDisconnect.IsEnabled = false;
                    // keep reading the serial input
                    recvbytesCntr = 0;
                    recvbytes = new byte[9];
                    while (true)
                    {
                        IsRunning = true;
                        await ReadAsync(ReadCancellationTokenSource.Token);
                    }
                }
            }

            catch (Exception ex)
            {

                if (ex.GetType().Name == "TaskCanceledException")
                {
                    System.Diagnostics.Debug.WriteLine("Listen: Reading task was cancelled, closing device and cleaning up");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Listen: " + ex.Message);
                }
            }
            finally
            {
                IsRunning = false;
                // Cleanup once complete
                if (dataReaderObject != null)
                {
                    dataReaderObject.DetachStream();
                    dataReaderObject = null;
                }
            }
        }
        private int recvbytesCntr = 0;
        private byte[] recvbytes;

        /// <summary>
        /// ReadAsync: Task that waits on data and reads asynchronously from the serial device InputStream
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task ReadAsync(CancellationToken cancellationToken)
        {
            Task<UInt32> loadAsyncTask;

            uint ReadBufferLength = 1024;

            // If task cancellation was requested, comply
            cancellationToken.ThrowIfCancellationRequested();

            // Set InputStreamOptions to complete the asynchronous read operation when one or more bytes is available
            dataReaderObject.InputStreamOptions = InputStreamOptions.Partial;

            // Create a task object to wait for data on the serialPort.InputStream
            loadAsyncTask = dataReaderObject.LoadAsync(ReadBufferLength).AsTask(cancellationToken);

            // Launch the task and wait
            UInt32 bytesRead = await loadAsyncTask;
            if ((bytesRead + recvbytesCntr)> 9)
            {
                string recvdtxt = dataReaderObject.ReadString(bytesRead);
                recvbytesCntr = 0;
            }
            else if (bytesRead > 0)
            {
                try
                {
                    byte[] temp = new byte[bytesRead];

                    dataReaderObject.ReadBytes(temp);
  
                    for (int i=0; i< bytesRead; i++)
                    {
                        recvbytes[recvbytesCntr++] = temp[i];
                        if (recvbytesCntr == 3)
                        {
                            if (recvbytes[2] == 0)
                            {
                                String MyString = Encoding.ASCII.GetString(recvbytes).TrimEnd((Char)0);
  
                                Keypad.RaiseEvent(this, MyString);

                                recvbytesCntr = 0;
                                break;
                            }
                            else
                            {
                                //Errant condition
                                recvbytesCntr = 0;
                                break;
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("ReadAsync: " + ex.Message);
                }

            }
        }

        /// <summary>
        /// CancelReadTask:
        /// - Uses the ReadCancellationTokenSource to cancel read operations
        /// </summary>
        private void CancelReadTask()
        {
            if (ReadCancellationTokenSource != null)
            {
                if (!ReadCancellationTokenSource.IsCancellationRequested)
                {
                    ReadCancellationTokenSource.Cancel();
                }
            }
        }


        /// <summary>
        ///  Class to hold all paired device information
        /// </summary>
        public class PairedDeviceInfo
        {
            public PairedDeviceInfo(DeviceInformation deviceInfo)
            {
                this.DeviceInfo = deviceInfo;
                this.ID = this.DeviceInfo.Id;
                this.Name = this.DeviceInfo.Name;
            }

            public string Name { get; private set; }
            public string ID { get; private set; }
            public DeviceInformation DeviceInfo { get; private set; }
        }
    }
}
