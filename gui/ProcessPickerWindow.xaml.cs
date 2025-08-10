using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace netstrum_gui
{
    public partial class ProcessPickerWindow : Window
    {
        public Process? SelectedProcess { get; private set; }

        public ProcessPickerWindow()
        {
            InitializeComponent();
            ProcessList.ItemsSource = Process.GetProcesses().OrderBy(p => p.ProcessName);
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            SelectedProcess = ProcessList.SelectedItem as Process;
            DialogResult = SelectedProcess != null;
        }
    }
}
