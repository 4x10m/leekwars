using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeekWars.LeekWarsAPI.Model;

namespace LeekWars.LeekWarsAPI.Client
{
    interface LeekWarsClient
    { 
        void login(string username, string password);
        Garden getGarden();
        int getPrivateTeam(Team team);
        int startSoloFight(Leek leek, Leek enemy);
        
        event EventHandler onLoginEndEvent;
        event EventHandler onLoginErrorEvent;
    }

    public class onLoginEndEventEventArgs : EventArgs
    {
        public Farmer farmer;
    }
}
