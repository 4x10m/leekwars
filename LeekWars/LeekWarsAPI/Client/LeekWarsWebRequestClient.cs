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
using LeekWars.Utils;
using LeekWars.LeekWarsAPI.Model;
using LeekWars.LeekWarsAPI.Request;

namespace LeekWars.LeekWarsAPI.Client
{
    class LeekWarsWebRequestClient : LeekWarsClient
    {
        enum HTTPMethod
        {
            POST,
            GET
        }

        public event EventHandler onLoginEndEvent;
        public event EventHandler onLoginErrorEvent;

        private static LeekWarsWebRequestClient instance;

        private static string token;
        private static string phpSessID;

        public static LeekWarsWebRequestClient getInstance()
        {
            if (instance == null)
            {
                instance = new LeekWarsWebRequestClient();
            }

            return instance;
        }

        private LeekWarsWebRequestClient()
        {

        }

        public void login(string username, string password)
        {
            Stream responseStream = doRequest(Urls.loginUrl.Value, HTTPMethod.POST, new String[] {
                String.Format("login={0}", username),
                String.Format("password={0}", password)
            });

            string responseString = new StreamReader(responseStream).ReadToEnd();

            Stream stream = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(responseString));
            var serializer = new DataContractJsonSerializer(typeof(LoginRequest));
            LoginRequest request = (LoginRequest)serializer.ReadObject(stream);

            if(request.success)
            {
                request.farmer.leeks = Parser.parseLeeksFromJSON(responseString);

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

        public int getPrivateTeam(Team team)
        {
            Stream responseStream = doRequest(Urls.privateTeamUrl.Value, HTTPMethod.GET, new String[] {
                team.id.ToString(),
                token
            });

            string responseString = new StreamReader(responseStream).ReadToEnd();

            Stream stream = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(responseString));
            var serializer = new DataContractJsonSerializer(typeof(StartSoloFightRequest));
            StartSoloFightRequest request = (StartSoloFightRequest)serializer.ReadObject(stream);

            return request.fight;
        }

        public int startSoloFight(Leek leek, Leek enemy)
        {
            Stream responseStream = doRequest(Urls.startSoloFightUrl.Value, HTTPMethod.POST, new String[] {
                String.Format("leek_id={0}", leek.id.ToString()),
                String.Format("target_id={0}", enemy.id.ToString()),
                String.Format("token={0}", token)
            });

            string responseString = new StreamReader(responseStream).ReadToEnd();

            Stream stream = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(responseString));
            var serializer = new DataContractJsonSerializer(typeof(StartSoloFightRequest));
            StartSoloFightRequest request = (StartSoloFightRequest)serializer.ReadObject(stream);

            return request.fight;
        }

        public Garden getGarden()
        {
            string parameters = String.Format("token={0}", token);

            Stream responseStream = doRequest(Urls.gardenUrl.Value, HTTPMethod.GET, token);
            string responseString = new StreamReader(responseStream).ReadToEnd();

            Stream stream = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(responseString));
            var serializer = new DataContractJsonSerializer(typeof(GardenRequest));
            GardenRequest gardenRequest = (GardenRequest)serializer.ReadObject(stream);

            Parser.parseFightsNumberFromJSON(ref gardenRequest.garden.leeks, responseString);
            Parser.parseSoloEnemies(ref gardenRequest.garden.leeks, responseString);

            return gardenRequest.garden;
        }

        private Stream doRequest(string url, HTTPMethod method)
        {
            return doRequest(url, method, null);
        }

        private Stream doRequest(string url, HTTPMethod method, params string[] parameters)
        {
            HttpWebRequest request = null;
            
            if(method == HTTPMethod.GET)
            {
                string realUrl = url + Parser.parseLeekWarsGetParameters(parameters);

                request = (HttpWebRequest)WebRequest.Create(realUrl);
                request.Method = "GET";
            }

            if(method == HTTPMethod.POST)
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                string concatedParameters = parameters[0];
                parameters = parameters.Skip<string>(1).ToArray<string>();

                foreach (string parameter in parameters)
                {
                    concatedParameters += "&" + parameter;
                }

                byte[] data = Encoding.ASCII.GetBytes(concatedParameters);

                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            if (phpSessID != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(new Uri(Urls.baseUrl.Value), new Cookie("PHPSESSID", phpSessID));
            }

            var response = (HttpWebResponse)request.GetResponse();

            if(response.Headers["Set-cookie"] != null)
            {
                string cookie = response.Headers["Set-cookie"];
                Match match = new Regex("PHPSESSID=([a-z0-9]+);").Match(cookie);
                cookie = match.Groups[1].Captures[0].Value;
                phpSessID = cookie;
            }

            return response.GetResponseStream();
        }
    }
}