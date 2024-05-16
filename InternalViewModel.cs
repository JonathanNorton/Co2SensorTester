using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Co2SensorTester.ViewModels
{
    public class InternalViewModel : PortedViewModel
    {
        public double Pressure { get; set; }
        public double PressureRaw { get; set; }
        public bool PressureStatus { get; set; }


        public double TempInternal { get; set; }
        public double TempHotWater { get; set; }


        public bool TempIntStatus { get; set; }
        public bool TempHotStatus { get; set; }


        public double Ppo2 { get; set; }
        public double Ppo2Raw { get; set; }
        public bool Ppo2Status { get; set; }
        public double Ppo2Span { get; set; }

        public double PpCo2 { get; set; }
        public double PpCo2Raw { get; set; }
        public bool PpCo2Status { get; set; }
        public double PpCo2Span { get; set; }


        public int Span { get; set; }

        public int CalCountdown { get; set; }

        public int PowerMode { get; set; }

        public string PowerModeString { get => PowerMode == 0 ? "Full Power" : "Low Power"; }


        public ICommand SpanCmd1 { get; set; } = new RelayCommand((o) => { (o as InternalViewModel).DoSpan(); });

        public ICommand ZeroCmd1 { get; set; } = new RelayCommand((o) => { (o as InternalViewModel).DoZero(); });

        public ICommand ResetCmd1 { get; set; } = new RelayCommand((o) => { (o as InternalViewModel).DoReset(); });

        private void DoSpan()
        {
            DoCal(5301, (ushort)Span);
        }

        private void DoZero()
        {
            Debug.WriteLine("Do zero");

            DoCal(1301, 5475);
        }

        private void DoReset()
        {
            Debug.WriteLine("Do reset");
            DoCal(8301, 5475);
        }

        private void DoCal(ushort reg, ushort value)
        {
            try
            {
                lock (mLock)
                {
                    master.WriteSingleRegister(99, reg, value);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public InternalViewModel()
        {
            Baud = 8192;

            Task.Run(async () =>
            {
                StreamWriter sw = null;

                while (true)
                {
                    await Task.Delay(500);

                    if (Run)
                    {
                        try
                        {
                            OpenCommPort();

                            if (sw == null)
                            {
                                DateTime dt = DateTime.Now;
                                sw = new StreamWriter($"co2_readings {dt.Year}-{dt.Month}-{dt.Day} {dt.Hour}_{dt.Minute}.csv");
                                sw.WriteLine("Time,Pressure, CO2, CO2 Raw");
                            }

                            byte slaveId = 99;
                            ushort startAddress = 301;
                            ushort numregisters = 29;

                            lock (mLock)
                            {
                                Task.Delay(10);

                                var registers = master.ReadHoldingRegisters(slaveId, startAddress, numregisters);

                                Pressure = ModbusUtils.BuildDouble(ModbusUtils.Get32(registers, 0), 100000);
                                PressureRaw = ModbusUtils.BuildDouble(ModbusUtils.Get32(registers, 2), 100000);

                                TempInternal = ModbusUtils.BuildDouble(ModbusUtils.Get32(registers, 4), 10);
                                TempHotWater = ModbusUtils.BuildDouble(ModbusUtils.Get32(registers, 6), 10);

                                Ppo2 = ModbusUtils.BuildDouble(ModbusUtils.Get32(registers, 8), 10);
                                Ppo2Raw = ModbusUtils.Get32(registers, 10);

                                PpCo2 = ModbusUtils.BuildDouble(ModbusUtils.Get32(registers, 12), 100);
                                PpCo2Raw = ModbusUtils.Get32(registers, 14);


                                PressureStatus = registers[16] != 0;
                                TempIntStatus  = registers[17] != 0;
                                TempHotStatus  = registers[18] != 0;
                                Ppo2Status     = registers[19] != 0;
                                PpCo2Status    = registers[20] != 0;

                                Span = ModbusUtils.Get32(registers, 21);

                                Ppo2Span = ModbusUtils.BuildDouble(ModbusUtils.Get32(registers, 23), 10);
                                PpCo2Span = ModbusUtils.BuildDouble(ModbusUtils.Get32(registers, 25), 100);

                                CalCountdown = registers[27];

                                PowerMode = registers[28];

                                LogReadings(sw, Pressure, PpCo2, PpCo2Raw);

                                IsConnected = true;
                            }


                        }
                        catch (Exception ex)
                        {
                            ErrorOccured(ex);
                        }
                    }
                    else
                    {
                        if (sw != null)
                        {
                            sw.Flush();
                            sw.Close();
                            sw = null;
                        }

                        IsConnected = false;

                        CloseComm();
                    }

                }
            });

        }

        private static void LogReadings(StreamWriter sw, double pressure, double ppCo2, double ppCo2Raw)
        {
            sw.WriteLine($"{DateTime.Now},{pressure}, {ppCo2}, {ppCo2Raw}");
        }
    }
}
