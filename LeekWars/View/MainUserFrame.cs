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
using LeekWars.LeekWarsAPI.Client;
using LeekWars.LeekWarsAPI.Model;

namespace LeekWars.View
{
    public partial class MainUserFrame : UserControl
    {
        LeekWarsClient client = LeekWarsWebClient.getInstance();

        public Farmer farmer;
        private Leek leek;
        Garden garden;
        Leek gardenLeek;

        private delegate void LogDelegate(string message);
        private delegate void UpdateProgressDelegate(int progress);

        public MainUserFrame()
        {
            InitializeComponent();

            Dock = DockStyle.Fill;

            int total = 0;

            garden = client.getGarden();

            foreach (Leek leek in garden.leeks)
            {
                total += leek.fights;
            }

            label2.Text = total.ToString();
            progressBar1.Maximum = total;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(fight));

            thread.Start();
        }

        private void LeekUserForm_VisibleChanged(object sender, EventArgs e)
        {
            if(Visible && !Disposing)
            {

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
            log("fights startedddd");
            updateProgress(0);

            int currentFight = 0;
            int total = 0;

            foreach(Leek leek in garden.leeks)
            {
                total += leek.fights;
            }

            log("a total of " + total.ToString() + " fights to do");

            Leek[] leeksCopy = garden.leeks;

            foreach (Leek leek in leeksCopy)
            {
                for (int nbFight = 0; nbFight < leek.fights; nbFight++)
                {
                    currentFight++;

                    garden = client.getGarden();

                    Leek leekFromGarden = null;

                    foreach(Leek tmpLeek in garden.leeks)
                    {
                        if(leek.id == tmpLeek.id)
                        {
                            leekFromGarden = tmpLeek;
                        }
                    }

                    Leek enemyLeek = leekFromGarden.ennemies[0];

                    log("fight " + "(" + currentFight.ToString() + ") against" + enemyLeek.name + "(" + enemyLeek.id.ToString() + ") with " + leekFromGarden.name + "(" + leekFromGarden.id.ToString() + ")");
                    updateProgress(currentFight);

                    client.startSoloFight(leekFromGarden, enemyLeek);
                    client.startSoloFight(leekFromGarden, enemyLeek);
                }
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
