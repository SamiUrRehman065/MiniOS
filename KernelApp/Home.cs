using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KernelApp
{
    public partial class MiniOs : Form
    {
        public MiniOs()
        {
            InitializeComponent();
            lbldate.Text = "Date: " + DateTime.Now.ToLongDateString();
        }
        private async Task LoadControl(UserControl control)
        {
            MainPanel.Controls.Clear(); // Clear previous content
            control.Dock = DockStyle.Fill; // Make it fit the panel

            control.Margin = new Padding(2);

            MainPanel.Padding = new Padding(10); // Adjust as needed

            await Task.Run(() =>
            {
                Invoke(new Action(() =>
                {
                    MainPanel.Controls.Add(control); // Add new control inside Invoke
                }));
            });
        }

        private async void MiniOs_Load(object sender, EventArgs e)
        {
            await LoadControl(new UserControls.ProcessMgrControl());

        }

        private async void btnProcessManager_Click(object sender, EventArgs e)
        {
            await LoadControl(new UserControls.ProcessMgrControl());
        }

        private async void btnSystemConsole_Click(object sender, EventArgs e)
        {
            await LoadControl(new UserControls.ConsoleControl());
        }

        private async void btnMemoryUsage_Click(object sender, EventArgs e)
        {
            await LoadControl(new UserControls.MemoryVisControl());
        }

        private async void btnSyscallLogs_Click(object sender, EventArgs e)
        {
            await LoadControl(new UserControls.SysLogControl());
        }

        private void btnShutdown_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
