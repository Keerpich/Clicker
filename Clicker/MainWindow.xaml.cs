using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
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
        public MainWindow()
        {
            InitializeComponent();

            //MouseClickAction mca1 = new MouseClickAction();
            //mca1.X = 500;
            //mca1.Y = 800;

            //SleepAction sa = new SleepAction();

            //MouseClickAction mca2 = new MouseClickAction();
            //mca2.X = 1000;
            //mca2.Y = 300;

            //Scenario scenario = new Scenario("Test");

            //scenario.AddAction(mca1);
            //scenario.AddAction(sa);
            //scenario.AddAction(mca2);

            //JSONSerializer.SerializeScenario(scenario);

            //scenario.Execute();

            Scenario scenario = JSONSerializer.DeserializeScenario("Test");
            scenario.Execute();
        }
    }
}
