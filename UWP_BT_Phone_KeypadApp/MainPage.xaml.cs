using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Enumeration;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Networking.Sockets;
using System.Collections.ObjectModel;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using Windows.Devices.Bluetooth;



namespace BTKeypadUWP
{
    /// <summary>
    /// The Main Page for the app
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string Title = "Modularised Generic Bluetooth Serial Universal Windows App";
        ObservableCollection<BluetoothSerialLib.BluetoothSerial.PairedDeviceInfo> _pairedDevices;
        BluetoothSerialLib.BluetoothSerial SerialPort =null;
        KeypadUWPLib.Keypad Keypad = null;
        KeyPadKeysUWPLib.KeyFunctions keyFunctions = null;


        public MainPage()
        {
            this.InitializeComponent();
            MyTitle.Text = Title;
            SerialPort = new BluetoothSerialLib.BluetoothSerial();
            InitializeRfcommDeviceService();
            Keypad = SerialPort.Keypad;
            //SetupKPadEventHandlers();
            SetupDelegates();
        }      
        private void SetupDelegates()
        { 
            keyFunctions = new KeyPadKeysUWPLib.KeyFunctions(SerialPort.Keypad);
            keyFunctions.Set(HashKey, '#');
            keyFunctions.Set(StarKey, '*');
            keyFunctions.Set(Num0, '0');
            keyFunctions.Set(Num1, '1');
            keyFunctions.Set(Num2, '2');
            keyFunctions.Set(Num3, '3');
            keyFunctions.Set(Num4, '4');
            keyFunctions.Set(Num5, '5');
            keyFunctions.Set(Num6, '6');
            keyFunctions.Set(Num7, '7');
            keyFunctions.Set(Num8, '8');
            keyFunctions.Set(Num9, '9');
        }

        public void HashKey()
        {
            System.Diagnostics.Debug.WriteLine("HashKey invoked");
        }

        public void StarKey()
        {
            System.Diagnostics.Debug.WriteLine("StarKey invoked");
        }

        private void Num0()
        {
            System.Diagnostics.Debug.WriteLine("Num0 invoked");
        }

        private void Num1()
        {
            System.Diagnostics.Debug.WriteLine("Num1 invoked");
        }

        private void Num2()
        {
            System.Diagnostics.Debug.WriteLine("Num2 invoked");
        }

        private void Num3()
        {
            System.Diagnostics.Debug.WriteLine("Num3 invoked");
        }

        private void Num4()
        {
            System.Diagnostics.Debug.WriteLine("Num4 invoked");
        }

        private void Num5()
        {
            System.Diagnostics.Debug.WriteLine("Num5 invoked");
        }

        private void Num6()
        {
            System.Diagnostics.Debug.WriteLine("Num6 invoked");
        }

        private void Num7()
        {
            System.Diagnostics.Debug.WriteLine("Num7 invoked");
        }

        private void Num8()
        {
            System.Diagnostics.Debug.WriteLine("Num8 invoked");
        }

        private void Num9()
        {
            System.Diagnostics.Debug.WriteLine("Num9 invoked");
        }



        async void InitializeRfcommDeviceService()
        {
            try
            {
                DeviceInformationCollection DeviceInfoCollection = await DeviceInformation.FindAllAsync(RfcommDeviceService.GetDeviceSelector(RfcommServiceId.SerialPort));

                var numDevices = DeviceInfoCollection.Count();
                if (numDevices > 0)
                {
                }

                // By clearing the backing data, we are effectively clearing the ListBox
                _pairedDevices = new ObservableCollection<BluetoothSerialLib.BluetoothSerial.PairedDeviceInfo>();
                _pairedDevices.Clear();

                if (numDevices == 0)
                {
                    System.Diagnostics.Debug.WriteLine("InitializeRfcommDeviceService: No paired devices found.");
                }
                else
                {
                    // Found paired devices.
                    foreach (var deviceInfo in DeviceInfoCollection)
                    {
                        _pairedDevices.Add(new BluetoothSerialLib.BluetoothSerial.PairedDeviceInfo(deviceInfo));
                    }
                }
                PairedDevices.Source = _pairedDevices;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("InitializeRfcommDeviceService: " + ex.Message);
            }
        }

        DeviceInformation DeviceInfo = null;

        async private void ConnectDevice_Click(object sender, RoutedEventArgs e)
        {
            BluetoothSerialLib.BluetoothSerial.PairedDeviceInfo pairedDevice = (BluetoothSerialLib.BluetoothSerial.PairedDeviceInfo)ConnectDevices.SelectedItem;
            DeviceInfo = pairedDevice.DeviceInfo;
            bool success = await SerialPort.ConnectDevice(pairedDevice);

            // If the connection was successful, the RemoteAddress field will be populated
            if (success)
            {
                this.buttonDisconnect.IsEnabled = true;
                this.buttonSend.IsEnabled = true;
                this.buttonStartRecv.IsEnabled = true;
                this.buttonStopRecv.IsEnabled = false;
                string msg = String.Format("Connected to {0}!", SerialPort._socket.Information.RemoteAddress.DisplayName);
                System.Diagnostics.Debug.WriteLine(msg);
            }
        }


        private void ConnectDevices_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            BluetoothSerialLib.BluetoothSerial.PairedDeviceInfo pairedDevice = (BluetoothSerialLib.BluetoothSerial.PairedDeviceInfo)ConnectDevices.SelectedItem;
            this.TxtBlock_SelectedID.Text = pairedDevice.ID;
            this.textBlockBTName.Text = pairedDevice.Name;
            ConnectDevice_Click(sender, e);
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            //OutBuff = new Windows.Storage.Streams.Buffer(100);
            Button button = (Button)sender;
            if (button != null)
            {
                string command = (string)button.Content;
                switch (command)
                {
                    case "Disconnect":
                        await SerialPort.button_Click(command, "");
                        this.textBlockBTName.Text = "";
                        this.TxtBlock_SelectedID.Text = "";
                        this.buttonDisconnect.IsEnabled = false;
                        this.buttonSend.IsEnabled = false;
                        this.buttonStartRecv.IsEnabled = false;
                        this.buttonStopRecv.IsEnabled = false;
                        break;
                    case "Send":
                        await SerialPort.button_Click(command, this.textBoxSendText.Text);
                        this.textBoxSendText.Text = "";
                        break;
                    case "Clear Recv":
                        this.textBoxRecvdText.Text = "";
                        this.textBoxSendText.Text = "";
                        break;
                    case "Start Recv":
                        this.buttonStartRecv.IsEnabled = false;
                        this.buttonStopRecv.IsEnabled = true;
                        this.buttonDisconnect.IsEnabled = false; ;
                        await SerialPort.button_Click(command, "");
                        break;
                    case "Stop Recv":
                        this.buttonStartRecv.IsEnabled = true;
                        this.buttonStopRecv.IsEnabled = false;
                        this.buttonDisconnect.IsEnabled = true;
                        await SerialPort.button_Click(command, "");
                        break;
                    case "Refresh":
                        InitializeRfcommDeviceService();
                        break;
                    case "Exit":
                        await SerialPort.button_Click(command, "");
                        Application.Current.Exit();
                        break;
                }
            }
        }


    }
}
