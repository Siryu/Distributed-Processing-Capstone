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
using ActorModel.Actors;
using ActorModel.Node;

namespace VisualWorkerNode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Node node { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            stopButton.IsEnabled = false;

            this.Closed += new EventHandler(MainWindow_Closed);
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            closeButton_Click(null, null);
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {       
            try
            {
                ServerIP.IsEnabled = false;
                ServerPort.IsEnabled = false;
                ListenPort.IsEnabled = false;
                stopButton.IsEnabled = true;
                startButton.IsEnabled = false;
                node = new Node(ServerIP.Text, int.Parse(ServerPort.Text), int.Parse(ListenPort.Text));
                node.Start();
            }
            catch
            {
                closeButton_Click(null, null);
            }   
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            if (node != null)
            {
                node.Close();
            }
            stopButton.IsEnabled = false;
            startButton.IsEnabled = true;
            ServerIP.IsEnabled = true;
            ServerPort.IsEnabled = true;
            ListenPort.IsEnabled = true;
        }
    }
}
