using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;

namespace Clicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int toggleRunHotkeyId = 1;
        private const int toggleScenarioRecordingHotkeyId = 2;
        private const int recordMouseClickHotkeyId = 3;

        private Scenario currentScenario;
        private bool recording = false;

        private HwndSource source;

        public MainWindow()
        {
            InitializeComponent();

            ScenarioNameTextBox.TextChanged += ScenarioNameTextBox_TextChanged;

            currentScenario = new Scenario(ScenarioNameTextBox.Text);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case InputSimulatorWrapper.HotkeyMessage:
                {
                    switch(wParam.ToInt32())
                    {
                        case toggleRunHotkeyId:
                        {
                            ToggleScenarioExecution();
                        }
                        break;

                        case toggleScenarioRecordingHotkeyId:
                        {
                            ToggleScenarioRecording();
                        }
                        break;

                        case recordMouseClickHotkeyId:
                        {
                            RecordMouseClick();
                        }
                        break;
                    }
                }
                break;
            }

            return IntPtr.Zero;
        }

        private void ToggleScenarioExecution()
        {
            bool isExecuting = currentScenario.IsExecuting;

            if (isExecuting)
            {
                currentScenario.Stop();
                WindowState = WindowState.Normal;
            }
            else
            {
                currentScenario.Execute();
                WindowState = WindowState.Minimized;
            }
        }

        private void ToggleScenarioRecording()
        {
            recording = !recording;

            if (recording)
            {
                currentScenario = new Scenario(ScenarioNameTextBox.Text);
                WindowState = WindowState.Minimized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        private void RecordMouseClick()
        {
            var cursorPosition = InputSimulatorWrapper.GetMousePosition();

            MouseClickAction clickAction = new MouseClickAction();
            clickAction.X = (int)cursorPosition.X;
            clickAction.Y = (int)cursorPosition.Y;

            SleepAction sleepAction = new SleepAction();

            currentScenario.AddAction(clickAction);
            currentScenario.AddAction(sleepAction);
        }

        private void ScenarioNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            currentScenario.Name = ScenarioNameTextBox.Text;
        }

        private void OnButtonKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.F7)
            {

            }
            else if (e.Key == Key.F8)
            {
            }
        }

        private void ToggleScenarioButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleScenarioExecution();
        }

        private void OpenScenarioButton_Click(object sender, RoutedEventArgs e)
        {
            currentScenario = JSONSerializer.DeserializeScenario(ScenarioNameTextBox.Text);
        }

        private void Open_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Scenario Files (*.scen)|*.scen";

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                String filename = openFileDialog.FileName;
                currentScenario = JSONSerializer.DeserializeScenario(filename);

                ScenarioNameTextBox.Text = currentScenario.Name;
            }
        }

        private void Save_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Scenario Files (*.scen)|*.scen";
            saveFileDialog.FileName = currentScenario.Name;
            saveFileDialog.DefaultExt = "scen";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.OverwritePrompt = true;

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ||
                saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                string fullPath = saveFileDialog.FileName;

                JSONSerializer.SerializeScenario(currentScenario, fullPath);
            }
        }

        private void Close_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Add wndproc hook
            var helper = new WindowInteropHelper(this);
            source = HwndSource.FromHwnd(helper.Handle);
            source.AddHook(WndProc);

            InputSimulatorWrapper.RegisterHotKey(toggleRunHotkeyId, InputSimulatorWrapper.VK_Codes.F6, InputSimulatorWrapper.Key_Modifiers.None);
            InputSimulatorWrapper.RegisterHotKey(toggleScenarioRecordingHotkeyId, InputSimulatorWrapper.VK_Codes.F7, InputSimulatorWrapper.Key_Modifiers.None);
            InputSimulatorWrapper.RegisterHotKey(recordMouseClickHotkeyId, InputSimulatorWrapper.VK_Codes.F8, InputSimulatorWrapper.Key_Modifiers.None);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            source.RemoveHook(WndProc);
            InputSimulatorWrapper.UnregisterHotKey(toggleRunHotkeyId);
            InputSimulatorWrapper.UnregisterHotKey(toggleScenarioRecordingHotkeyId);
            InputSimulatorWrapper.UnregisterHotKey(recordMouseClickHotkeyId);
        }

        private void About_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("F7 - Start/stop recording scenario\nF8 - Record click at mouse position while recording scenario\nF6 - Run/stop current scenario");
        }
    }
}
