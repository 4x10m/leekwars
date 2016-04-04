using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LeekWars.LeekWarsAPI;
using System.Threading;
using LeekWars.LeekWarsAPI.Model;
using LeekWars.LeekWarsAPI.Client;

namespace LeekWars.View
{
    public partial class FarmerUserForm : UserControl
    {
        LeekWarsClient client = LeekWarsWebClient.getInstance();

        private Farmer farmer;
        public Leek leek;
        Garden garden;
        Leek gardenLeek;

        private delegate void LogDelegate(string message);
        private delegate void UpdateProgressDelegate(int progress);

        public FarmerUserForm(Farmer farmer)
        {
            InitializeComponent();

            Dock = DockStyle.Fill;

            this.farmer = farmer;
            leek = farmer.leeks[0];

            setup();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(fight));

            thread.Start();
        }

        private void setup()
        {
            gardenLeek = null;

            garden = client.getGarden();

            foreach (Leek leek in garden.leeks)
            {
                if (this.leek.id == leek.id)
                {
                    gardenLeek = leek;
                }
            }

            if (gardenLeek != null)
            {
                label2.Text = gardenLeek.fights.ToString();
                progressBar1.Maximum = gardenLeek.fights;
            }
        }

        private void log(string message)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                LogDelegate logDelegate = new LogDelegate(log);
                this.Invoke(logDelegate, new object[] { message });
            }
            else
            {
                this.richTextBox1.Text += message + "\n";
            }
        }

        private void updateProgress(int progress)
        {
            if (this.progressBar1.InvokeRequired)
            {
                UpdateProgressDelegate updateProgressDelegate = new UpdateProgressDelegate(updateProgress);
                this.Invoke(updateProgressDelegate, new object[] { progress });
            }
            else
            {
                this.progressBar1.Value = progress;
            }
        }

        private void fight()
        {
            log("fights started");
            updateProgress(0);

            for(int nbFight = 0; nbFight < gardenLeek.fights; nbFight++)
            {
                Leek enemyLeek = gardenLeek.ennemies[0];

                log("fight against" + enemyLeek.name + "(" + enemyLeek.id.ToString() + ")");
                updateProgress(nbFight);

                client.startSoloFight(gardenLeek, enemyLeek);

                garden = client.getGarden();

                foreach (Leek leek in garden.leeks)
                {
                    if (this.leek.id == leek.id)
                    {
                        gardenLeek = leek;
                    }
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
