using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LeekWars.LeekWarsAPI;

namespace LeekWars.View
{
    public partial class Main : Form
    {
        LeekWarsClient client = LeekWarsClient.getInstance();
        private Farmer farmer = null;
        Garden garden;

        public Main(Farmer farmer)
        {
            this.farmer = farmer;

            InitializeComponent();
        }

        private void Main_Shown(object sender, EventArgs e)
        {
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
