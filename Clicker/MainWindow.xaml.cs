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

namespace Clicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int toggleRunHotkeyId = 1;

        private Scenario currentScenario;
        private bool recording = false;

        private HwndSource source;

        public MainWindow()
        {
            InitializeComponent();

            ScenarioNameTextBox.TextChanged += ScenarioNameTextBox_TextChanged;

            this.KeyDown += new System.Windows.Input.KeyEventHandler(OnButtonKeyDown);

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
            }
            else
            {
                currentScenario.Execute();
            }
        }

        private void ScenarioNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            currentScenario = new Scenario(ScenarioNameTextBox.Text);
        }

        private void OnButtonKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.F7)
            {
                recording = !recording;

                if (recording)
                {
                    currentScenario = new Scenario(ScenarioNameTextBox.Text);
                    Opacity = 0;
                }
                else
                {
                    Opacity = 1;
                }

            }
            else if (e.Key == Key.F8)
            {
                var cursorPosition = InputSimulatorWrapper.GetMousePosition();

                MouseClickAction clickAction = new MouseClickAction();
                clickAction.X = (int)cursorPosition.X;
                clickAction.Y = (int)cursorPosition.Y;

                SleepAction sleepAction = new SleepAction();

                currentScenario.AddAction(clickAction);
                currentScenario.AddAction(sleepAction);
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

                int lastSlash = filename.LastIndexOf(@"\");
                int lastDot = filename.LastIndexOf(@".");
                string strippedFileName = filename.Substring(lastSlash + 1, lastDot - lastSlash - 1);

                ScenarioNameTextBox.Text = strippedFileName;
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
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            source.RemoveHook(WndProc);
            InputSimulatorWrapper.UnregisterHotKey(toggleRunHotkeyId);
        }
    }
}
