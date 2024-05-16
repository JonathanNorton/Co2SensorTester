using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Co2SensorTester
{
    /// <summary>
    /// Interaction logic for PressureInfoPanel.xaml
    /// </summary>
    public partial class PressureInfoPanel : UserControl, INotifyPropertyChanged
    {
        public PressureInfoPanel()
        {
            InitializeComponent();
            grid.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged = (s, e) => { };





        public double Pressure
        {
            get { return (double)GetValue(PressureProperty); }
            set { SetValue(PressureProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Pressure.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressureProperty =
            DependencyProperty.Register("Pressure", typeof(double), typeof(PressureInfoPanel), new PropertyMetadata(1.2345));





        public double Raw
        {
            get { return (double)GetValue(RawProperty); }
            set { SetValue(RawProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Raw.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RawProperty =
            DependencyProperty.Register("Raw", typeof(double), typeof(PressureInfoPanel), new PropertyMetadata(2.3456));





        public double Span
        {
            get { return (double)GetValue(SpanProperty); }
            set { SetValue(SpanProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Span.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpanProperty =
            DependencyProperty.Register("Span", typeof(double), typeof(PressureInfoPanel), new PropertyMetadata(1000.0));







        public bool Status
        {
            get { return (bool)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Status.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(bool), typeof(PressureInfoPanel), new PropertyMetadata(false));





        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(PressureInfoPanel), new PropertyMetadata("Label"));








        public ICommand SpanCommand
        {
            get { return (ICommand)GetValue(SpanCommandProperty); }
            set { SetValue(SpanCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SpanCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpanCommandProperty =
            DependencyProperty.Register("SpanCommand", typeof(ICommand), typeof(PressureInfoPanel), new PropertyMetadata(null));







        public ICommand ZeroCommand
        {
            get { return (ICommand)GetValue(ZeroCommandProperty); }
            set { SetValue(ZeroCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ZeroCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZeroCommandProperty =
            DependencyProperty.Register("ZeroCommand", typeof(ICommand), typeof(PressureInfoPanel), new PropertyMetadata(null));







        public ICommand ResetCommand
        {
            get { return (ICommand)GetValue(ResetCommandProperty); }
            set { SetValue(ResetCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ResetCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResetCommandProperty =
            DependencyProperty.Register("ResetCommand", typeof(ICommand), typeof(PressureInfoPanel), new PropertyMetadata(null));








        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(PressureInfoPanel), new PropertyMetadata(null));








        public bool CanCalibrate
        {
            get { return (bool)GetValue(CanCalibrateProperty); }
            set { SetValue(CanCalibrateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanCalibrate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanCalibrateProperty =
            DependencyProperty.Register("CanCalibrate", typeof(bool), typeof(PressureInfoPanel), new PropertyMetadata(true));







        public bool HideRaw
        {
            get { return (bool)GetValue(HideRawProperty); }
            set { SetValue(HideRawProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HideRaw.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HideRawProperty =
            DependencyProperty.Register("HideRaw", typeof(bool), typeof(PressureInfoPanel), new PropertyMetadata(false));







        public double SpanEntry
        {
            get { return (double)GetValue(SpanEntryProperty); }
            set { SetValue(SpanEntryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SpanEntry.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpanEntryProperty =
            DependencyProperty.Register("SpanEntry", typeof(double), typeof(PressureInfoPanel), new PropertyMetadata(0.0));






        public bool ShowSpanEntry
        {
            get { return (bool)GetValue(ShowSpanEntryProperty); }
            set { SetValue(ShowSpanEntryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowSpanEntry.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowSpanEntryProperty =
            DependencyProperty.Register("ShowSpanEntry", typeof(bool), typeof(PressureInfoPanel), new PropertyMetadata(false));


        private void OnClick()
        {
            SpanEntry = Span;
            ShowSpanEntry = true;
            tb_span.Focus();
        }

        private bool mousing_is_clicking = false;

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((sender as TextBlock).IsEnabled && Status)
            {
                mousing_is_clicking = true;
            }
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (mousing_is_clicking == true)
            {
                OnClick();
            }

            mousing_is_clicking = false;
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            mousing_is_clicking = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShowSpanEntry = false;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ShowSpanEntry = false;
        }
    }
}
