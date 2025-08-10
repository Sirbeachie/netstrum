using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace netstrum_gui
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Packet> Packets { get; } = new();
        private Packet? _selectedPacket;
        private int? _selectedProcessId;
        private readonly TcpListenerService _listener = new(9000);

        public Packet? SelectedPacket
        {
            get => _selectedPacket;
            set { _selectedPacket = value; OnPropertyChanged(nameof(SelectedPacket)); }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _listener.PacketReceived += OnPacketReceived;
        }

        private void OnPacketReceived(string data)
        {
            Dispatcher.Invoke(() => Packets.Add(new Packet { Data = data, Timestamp = DateTime.Now }));
        }

        private void SelectProcess_Click(object sender, RoutedEventArgs e)
        {
            var picker = new ProcessPickerWindow();
            if (picker.ShowDialog() == true && picker.SelectedProcess != null)
            {
                _selectedProcessId = picker.SelectedProcess.Id;
            }
        }

        private void Attach_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProcessId.HasValue)
            {
                MessageBox.Show($"Attached to process {_selectedProcessId}");
            }
        }

        private void Detach_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Detached");
        }

        private void Capture_Click(object sender, RoutedEventArgs e)
        {
            _ = _listener.StartAsync();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            _listener.Stop();
        }

        private void SavePacket_Click(object sender, RoutedEventArgs e)
        {
            // Binding updates data automatically
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
