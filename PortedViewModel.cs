using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;


namespace Co2SensorTester
{
    public class PortedViewModel : ViewModelBase
    {
        private bool _IsSelectedTab = false;
        public bool IsSelectedTab
        {
            get
            {
                return _IsSelectedTab;
            }
            set
            {
                if (_IsSelectedTab != value)
                {
                    _IsSelectedTab = value;
                }

                if(_IsSelectedTab == false)
                {
                    Run = false;
                }
            }
        }


        public bool Run { get; set; } = false;
        public string PortName { get; set; }
        public ObservableCollection<string> AllPorts { get; set; } = new ObservableCollection<string>(SerialPort.GetPortNames().OrderBy(s => int.Parse(s.Replace("COM", ""))));

        public bool IsConnected { get; set; } = false;



        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
        public bool ShowError { get; set; }

        public RelayCommand CloseError { get; set; }



        protected SerialPort comm;
        protected Modbus.Device.ModbusSerialMaster master;
        protected readonly object mLock = new object();
        protected bool PortInUse = false;

        protected int Baud = 9600;

        protected void OpenCommPort()
        {
            if (comm == null)
            {
                comm = new SerialPort(PortName, Baud, Parity.None, 8, StopBits.One);
            }

            if (comm.IsOpen == false)
            {
                comm.Open();
                lock (mLock)
                {
                    master = Modbus.Device.ModbusSerialMaster.CreateRtu(comm);

                    master.Transport.ReadTimeout = 1000;
                    master.Transport.WriteTimeout = 1000;
                }
            }
        }

        protected void ErrorOccured(Exception ex)
        {
            Debug.WriteLine($"{ex}");

            ErrorMessage = ex.Message;
            ErrorDetails = ex.ToString();
            ShowError = true;

            IsConnected = false;

            CloseComm();

            Run = false;
        }

        protected void CloseComm()
        {
            lock (mLock)
            {
                if (master != null)
                {
                    master.Dispose();
                    master = null;
                }

                if (comm != null)
                {
                    comm.Close();
                    comm = null;
                }
            }
        }


        public PortedViewModel()
        {
            if (AllPorts.Count > 0)
            {
                PortName = AllPorts.First();
            }

            CloseError = new RelayCommand((o) => { ShowError = false; });
        }


    }
}
