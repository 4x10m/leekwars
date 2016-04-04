using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeekWars.LeekWarsAPI;
using LeekWars.LeekWarsAPI.Model;
using System.Threading;
using System.Windows.Forms;
using LeekWars.LeekWarsAPI.Client;
using LeekWars.LeekWarsAPI.Model;

namespace LeekWars.View
{
    public partial class LoginForm : Form
    {
        private LeekWarsClient client = LeekWarsWebClient.getInstance();

        LoadingUserForm loadingUserForm = new LoadingUserForm();

        private delegate void ShowEventHanlder();
        private delegate void ShowMainFormEventHanlder(Farmer farmer);

        public Farmer farmer { get; set; }

        public LoginForm()
        {
            InitializeComponent();

            loadingUserForm.Hide();
            Controls.Add(loadingUserForm);
            
            loadingUserForm.Anchor = (AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);

            loginUserForm1.onButonClicked += new LoginUserForm.LoginUserFormConnectionButtonClicked(loadingUserFormConnectionButtonClicked);
            client.onLoginEndEvent += new EventHandler(onLoginUserFormConnectionEnd);
            client.onLoginErrorEvent += new EventHandler(onLoginUserFormConnectionError);
        }

        public void loadingUserFormConnectionButtonClicked(string username, string password)
        {
            Thread loginThread = new Thread(() => client.login(username, password));
            loginThread.Start();

            loginUserForm1.Hide();
            loadingUserForm.Show();
        }

        private void onLoginUserFormConnectionError(object sender, EventArgs args)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler(onLoginUserFormConnectionError));
            }
            else
            {
                loadingUserForm.Hide();
                loginUserForm1.Show();
                loginUserForm1.enable();
            }
        }

        private void onLoginUserFormConnectionEnd(object sender, EventArgs e)
        {
            onLoginEndEventEventArgs eventArgs = (onLoginEndEventEventArgs)e;
            Farmer farmer = eventArgs.farmer;

            if (InvokeRequired)
            {
                Invoke(new EventHandler(onLoginUserFormConnectionEnd), sender, e);
            }
            else
            {
                this.farmer = farmer;
                DialogResult = DialogResult.OK;

                Close();
            }
        }
    }
}