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

namespace LeekWars.View
{
    public partial class LoginForm : Form
    {
        private LeekWarsClient client = LeekWarsClient.getInstance();

        LoadingUserForm loadingUserForm = new LoadingUserForm();

        private delegate void ShowEventHanlder();
        private delegate void ShowMainFormEventHanlder(Farmer farmer);

        public LoginForm()
        {
            InitializeComponent();

            loadingUserForm.Hide();
            Controls.Add(loadingUserForm);
            
            loadingUserForm.Anchor = (AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);

            loginUserForm1.onButonClicked += new LoginUserForm.LoginUserFormConnectionButtonClicked(loadingUserFormConnectionButtonClicked);
            client.onLoginEndEvent += new LeekWarsClient.LoginEndEventHandler(onLoginUserFormConnectionEnd);
            client.onLoginErrorEvent += new LeekWarsClient.LoginErrorEventHandler(onLoginUserFormConnectionError);
        }

        public void loadingUserFormConnectionButtonClicked(string username, string password)
        {
            Thread loginThread = new Thread(() => client.login(username, password));
            loginThread.Start();

            loginUserForm1.Hide();
            loadingUserForm.Show();
        }

        private void onLoginUserFormConnectionError()
        {
            if (InvokeRequired)
            {
                Invoke(new ShowEventHanlder(onLoginUserFormConnectionError));
            }
            else
            {
                loadingUserForm.Hide();
                loginUserForm1.Show();
                loginUserForm1.enable();
            }
        }



        private void onLoginUserFormConnectionEnd(Farmer farmer)
        {
            if (InvokeRequired)
            {
                Invoke(new ShowMainFormEventHanlder(onLoginUserFormConnectionEnd), farmer);
            }
            else
            {
                Main mainForm = new Main(farmer);
                mainForm.Show();

                Hide();
            }
        }
    }
}