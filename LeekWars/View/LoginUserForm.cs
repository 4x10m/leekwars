using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

namespace LeekWars.View
{
    public partial class LoginUserForm : UserControl
    {
        public delegate void LoginUserFormConnectionButtonClicked(string username, string password);
        public event LoginUserFormConnectionButtonClicked onButonClicked;

        public LoginUserForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;

            button1.Enabled = false;

            if(onButonClicked != null) onButonClicked(textBox1.Text, textBox2.Text);
        }

        public void enable()
        {
            textBox1.Enabled = true;
            textBox2.Enabled = true;

            button1.Enabled = true;
        }
    }
}
