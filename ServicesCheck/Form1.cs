using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceProcess;

namespace ServicesCheck
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string servicename;
        private void Form1_Load(object sender, EventArgs e)
        {
            servicename = "PhoneSvc";
            CheckServices();
            checkServiceStatus(servicename);
            label3.Text = servicename;
        }

        private void CheckServices()
        {
            if (serviceExists(servicename))
            {
                lstResult.Items.Add("service::" + servicename + " exists");
            }
            else
            {
                lstResult.Items.Add("Service doesn't exists");
            }
        }
        //check Service exists
        public bool serviceExists(string ServiceName)
        {
            return ServiceController.GetServices().Any(serviceController => serviceController.ServiceName.Equals(ServiceName));
        }

        public void checkServiceStatus(string service)
        {
            ServiceController sc = new ServiceController();
            sc.ServiceName = service;

            if (sc.Status == ServiceControllerStatus.Stopped)
            {
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                lstResult.Items.Add(string.Format("The {0} service status is currently set to {1}",
                              service, sc.Status.ToString()));
            }else if
             (sc.Status == ServiceControllerStatus.Running)
            {
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                lstResult.Items.Add(string.Format("The {0} service status is currently set to {1}",
                              service, sc.Status.ToString()));
            }
            else if (sc.Status == ServiceControllerStatus.Paused)
            {
                lstResult.Items.Add(string.Format("The {0} service status is currently set to {1}",
                              service, sc.Status.ToString()));
            }
        }

        #region startservice
        public void startService(string ServiceName)
        {
            ServiceController sc = new ServiceController();
            sc.ServiceName = ServiceName;
            //lstResult.Items.Add(string.Format("The {0} service status is currently set to {1}",                ServiceName, sc.Status.ToString()));
            
            if (sc.Status == ServiceControllerStatus.Stopped)
            {
                // Start the service if the current status is stopped.
                lstResult.Items.Add(string.Format("Starting the {0} service ...", ServiceName));

                try
                {
                    // Start the service, and wait until its status is "Running".
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running);

                    // Display the current service status.
                    lstResult.Items.Add(string.Format("The {0} service status is now set to {1}.", ServiceName, sc.Status.ToString()));
                    MessageBox.Show("Service :"+ ServiceName + " started.");
                }
                catch (InvalidOperationException e)
                {
                    lstResult.Items.Add(string.Format("Could not start the {0} service.", ServiceName));
                    lstResult.Items.Add(e.Message);
                }
            }
            else
            {
                MessageBox.Show(string.Format("Service:: {0} already running.", ServiceName));
            }
            checkServiceStatus(ServiceName);
        }
        #endregion


        #region StopService
        public void stopService(string ServiceName)
        {
            ServiceController sc = new ServiceController();
            sc.ServiceName = ServiceName;

            //Console.WriteLine("The {0} service status is currently set to {1}", ServiceName, sc.Status.ToString());

            if (sc.Status == ServiceControllerStatus.Running)
            {
                // Start the service if the current status is stopped.
                lstResult.Items.Add(string.Format("Stopping the {0} service ...", ServiceName));
               
                try
                {
                    // Start the service, and wait until its status is "Running".
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);

                    // Display the current service status.
                    MessageBox.Show(string.Format("The {0} service status is now set to {1}.", ServiceName, sc.Status.ToString()));
                   // Console.WriteLine(string.Format("The {0} service status is now set to {1}.", ServiceName, sc.Status.ToString()));
                }
                catch (InvalidOperationException e)
                {
                    lstResult.Items.Add(string.Format("Could not stop the {0} service.", ServiceName));
                    MessageBox.Show(e.Message);
                    //Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("Cannot stop service {0} because it's already inactive.", ServiceName);
            }
            checkServiceStatus(ServiceName);
        }
        #endregion

        private void btnStart_Click(object sender, EventArgs e)
        {
            startService(servicename);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            stopService(servicename);
        }
    }
}
