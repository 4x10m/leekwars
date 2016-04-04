using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using LeekWars.LeekWarsAPI.Model;
using System.Collections.Specialized;
using LeekWars.LeekWarsAPI;
using LeekWars.Utils;
using LeekWars.LeekWarsAPI.Request;

namespace LeekWars.LeekWarsAPI.Client
{
    class LeekWarsWebClient : WebClient, LeekWarsClient
    {
        private CookieContainer _cookieContainer = new CookieContainer();

        public event EventHandler onLoginEndEvent;
        public event EventHandler onLoginErrorEvent;

        private string token;

        private static LeekWarsWebClient instance;

        public static LeekWarsWebClient getInstance()
        {
            if (instance == null)
            {
                instance = new LeekWarsWebClient();
            }

            return instance;
        }

        private LeekWarsWebClient()
        {

        }

        public Garden getGarden()
        {
            string url = Urls.gardenUrl.Value + Parser.parseLeekWarsGetParameters(token);
            var gardenAnswer = DownloadString(url);

            Stream stream = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(gardenAnswer));
            var serializer = new DataContractJsonSerializer(typeof(GardenRequest));
            GardenRequest gardenRequest = (GardenRequest)serializer.ReadObject(stream);

            Parser.parseFightsNumberFromJSON(ref gardenRequest.garden.leeks, gardenAnswer);
            Parser.parseSoloEnemies(ref gardenRequest.garden.leeks, gardenAnswer);

            return gardenRequest.garden;
        }

        public int getPrivateTeam(Team team)
        {
            string url = Urls.privateTeamUrl.Value + Parser.parseLeekWarsGetParameters(new String[] {
                team.id.ToString(),
                token
            });
            var privateTeamAnswer = DownloadString(url);

            Stream stream = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(privateTeamAnswer));
            var serializer = new DataContractJsonSerializer(typeof(StartSoloFightRequest));
            StartSoloFightRequest request = (StartSoloFightRequest)serializer.ReadObject(stream);

            return request.fight;
        }

        public void login(string username, string password)
        {
            var data = new NameValueCollection
            {
                { "login", username },
                { "password", password },
            };
            var response2 = UploadValues(Urls.loginUrl.Value, data);
            string loginAnswer = Encoding.Default.GetString(response2);

            Stream stream = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(loginAnswer));
            var serializer = new DataContractJsonSerializer(typeof(LoginRequest));
            LoginRequest request = (LoginRequest)serializer.ReadObject(stream);

            if (request.success)
            {
                request.farmer.leeks = Parser.parseLeeksFromJSON(loginAnswer);

                token = request.token;

                onLoginEndEventEventArgs args = new onLoginEndEventEventArgs();
                args.farmer = request.farmer;

                onLoginEndEvent(this, args);
            }
            else
            {
                onLoginErrorEvent(this, null);
            }
        }

        public int startSoloFight(Leek leek, Leek enemy)
        {
            var data = new NameValueCollection
            {
                { "leek_id", leek.id.ToString() },
                { "target_id", enemy.id.ToString() },
                { "token", token },
            };
            var response2 = UploadValues(Urls.startSoloFightUrl.Value, data);
            string soloFightAnswer = Encoding.Default.GetString(response2);

            Stream stream = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(soloFightAnswer));
            var serializer = new DataContractJsonSerializer(typeof(StartSoloFightRequest));
            StartSoloFightRequest request = (StartSoloFightRequest)serializer.ReadObject(stream);

            return request.fight;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).CookieContainer = _cookieContainer;
            }
            return request;
        }
    }
}