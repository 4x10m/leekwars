using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LeekWars.LeekWarsAPI.Client;
using LeekWars.LeekWarsAPI.Model;

namespace LeekWars.View
{
    public partial class MainForm : Form
    {
        LeekWarsClient client = LeekWarsWebClient.getInstance();
        private Farmer farmer = null;
        Garden garden;

        public MainForm()
        {
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();

            this.farmer = loginForm.farmer;

            InitializeComponent();

            foreach (Leek leek in farmer.leeks)
            {
                LeekUserForm leekUserForm = new LeekUserForm();
                leekUserForm.leek = leek;

                TabPage leekPage = new TabPage();
                leekPage.Text = leek.name;
                leekPage.Controls.Add(leekUserForm);

                tabControl1.TabPages.Add(leekPage);
            }
        }
    }
}
