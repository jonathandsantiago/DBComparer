using DevExpress.XtraNavBar;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace DBComparer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            ConfigurarNavBar();
            WindowState = FormWindowState.Maximized;
        }

        private void ConfigurarNavBar()
        {
            foreach (NavBarItem item in navBarControl.Items)
            {
                item.LinkClicked += Item_LinkClicked;
            }
        }

        private void Item_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            var controlName = (sender as NavBarItem).Tag?.ToString();
            AbrirControl(controlName);
        }

        public void AbrirControl(string nome)
        {
            var type= Assembly.GetExecutingAssembly().GetTypes().Where(c => c.Name.EndsWith(nome)).FirstOrDefault();
            var control = Activator.CreateInstance(type) as Control;
            control.Dock = DockStyle.Fill;
            splitContainerControl.Panel2.Controls.Clear();
            splitContainerControl.Panel2.Controls.Add(control);
        }
    }
}