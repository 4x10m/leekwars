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

namespace LeekWars.LeekWarsAPI
{
    class LeekWarsClient
    {
        private enum Method
        {
            POST,
            GET
        }

        public delegate void LoginEndEventHandler(Farmer farmer);
        public event LoginEndEventHandler onLoginEndEvent;

        public delegate void LoginErrorEventHandler();
        public event LoginErrorEventHandler onLoginErrorEvent;

        string baseUrl = "http://leekwars.com";
        string loginUrl = "http://leekwars.com/api/farmer/login-token";
        string gardenUrl = "http://leekwars.com/api/garden/get";
        string startSoloFightUrl = "http://leekwars.com/api/garden/start-solo-fight";
        string privateTeamUrl = "http://leekwars.com/team/get-private";

        private static LeekWarsClient instance;

        string token;
        string phpSessID;

        public static LeekWarsClient getInstance()
        {
            if (instance == null)
            {
                instance = new LeekWarsClient();
            }

            return instance;
        }

        private LeekWarsClient()
        {

        }

        public void login(string username, string password)
        {
            Stream responseStream = doRequest(loginUrl, Method.POST, new String[] {
                String.Format("login={0}", username),
                String.Format("password={0}", password)
            });

            string responseString = new StreamReader(responseStream).ReadToEnd();

            Stream stream = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(responseString));
            var serializer = new DataContractJsonSerializer(typeof(LoginRequest));
            LoginRequest request = (LoginRequest)serializer.ReadObject(stream);

            if(request.success)
            {
                Regex leeksRegex = new Regex("\"leeks\":{(.*})},");
                Match leeksMatch = leeksRegex.Match(responseString);
                string leeksstring = leeksMatch.Groups[1].Value;

                Regex leekRegex = new Regex("({[\"0-9a-zA-Z:,#]+}{1}?)");

                MatchCollection leekMatches = leekRegex.Matches(leeksstring);

                request.farmer.leeks = new Leek[leekMatches.Count];


                for (int nbLeeks = 0; nbLeeks < leekMatches.Count; nbLeeks++)
                {
                    Match match = leekMatches[nbLeeks];
                    string leeks = match.Groups[1].Captures[0].Value;

                    Stream stream1 = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(leeks));
                    var serializer1 = new DataContractJsonSerializer(typeof(Leek));
                    Leek leek = (Leek)serializer1.ReadObject(stream1);

                    request.farmer.leeks[nbLeeks] = leek;
                }

                token = request.token;

                onLoginEndEvent(request.farmer);
            }
            else
            {
                onLoginErrorEvent();
            }
        }

        Leek[] parseLeeks(string leeksString)
        {
            Leek[] leeks;

            Regex leeksRegex = new Regex("\"leeks\":{(.*})},");
            Match leeksMatch = leeksRegex.Match(leeksString);
            string leeksstring = leeksMatch.Groups[1].Value;

            //Regex leekRegex = new Regex("[0-9\"]+:({[.^}]*})");
            Regex leekRegex = new Regex("({[\"0-9a-zA-Z:,#]+}{1}?)");

            MatchCollection leekMatches = leekRegex.Matches(leeksstring);

            leeks = new Leek[leekMatches.Count];

            for (int nbLeeks = 0; nbLeeks < leekMatches.Count; nbLeeks++)
            {
                Match match = leekMatches[nbLeeks];
                string leekString = match.Groups[1].Captures[0].Value;

                Stream stream1 = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(leekString));
                var serializer1 = new DataContractJsonSerializer(typeof(Leek));
                Leek leek = (Leek)serializer1.ReadObject(stream1);

                leeks[nbLeeks] = leek;
            }

            return leeks;
        }

        public int getPrivateTeam(Team team)
        {
            Stream responseStream = doRequest(privateTeamUrl, Method.GET, new String[] {
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
            Stream responseStream = doRequest(startSoloFightUrl, Method.POST, new String[] {
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

            Stream responseStream = doRequest(gardenUrl, Method.GET, token);
            string responseString = new StreamReader(responseStream).ReadToEnd();

            Stream stream = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(responseString));
            var serializer = new DataContractJsonSerializer(typeof(GardenRequest));
            GardenRequest gardenRequest = (GardenRequest)serializer.ReadObject(stream);

            //parse fights
            Regex qariRegex = new Regex("\"leek_fights\":{(?<nbFights>.*?)}");
            Match match = qariRegex.Match(responseString);
            string leekfights = match.Groups["nbFights"].Value;
            qariRegex = new Regex("[0-9\"]+:([0-9]*),?");
            MatchCollection mc = qariRegex.Matches(leekfights);

            for (int nbLeeks = 0; nbLeeks < gardenRequest.garden.leeks.Length; nbLeeks++)
            {
                Match match1 = mc[nbLeeks];
                string value = match1.Groups[1].Value;
                gardenRequest.garden.leeks[nbLeeks].fights = Int32.Parse(value);
            }

            //parse ennemies
            Regex soloEnemiesRegex = new Regex("\"solo_enemies\":{(.*])}+?");
            Match soloEnemiesMatch = soloEnemiesRegex.Match(responseString);
            string soloEnemiesString = soloEnemiesMatch.Groups[1].Captures[0].Value;

            //Regex soloEnemyRegex = new Regex("\"([0-9]*)\":([.*]+?)");
            Regex soloEnemyRegex = new Regex("\"([0-9]*)\":(" + @"\[" + "[\"0-9a-zA-Z:,#{}@]*]+?)");
            MatchCollection soloEnemyCollectionMatch = soloEnemyRegex.Matches(soloEnemiesString);

            foreach (Match soloEnemyMatch in soloEnemyCollectionMatch)
            {
                int id = Int32.Parse(soloEnemyMatch.Groups[1].Captures[0].Value);
                string enemiesString = soloEnemyMatch.Groups[2].Captures[0].Value;

                foreach (Leek leek in gardenRequest.garden.leeks)
                {
                    if (leek.id == id)
                    {
                        Stream enemyStream = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(enemiesString));
                        var enemySerializer = new DataContractJsonSerializer(typeof(Leek[]));
                        Leek[] enemies = (Leek[])enemySerializer.ReadObject(enemyStream);

                        leek.ennemies = enemies;
                    }
                }
            }

            return gardenRequest.garden;
        }

        private Stream doRequest(string url, Method method)
        {
            return doRequest(url, method, null);
        }

        private Stream doRequest(string url, Method method, params string[] parameters)
        {
            HttpWebRequest request = null;
            
            if(method == Method.GET)
            {
                string realUrl = url;

                if(parameters != null)
                {
                    foreach (string parameter in parameters)
                    {
                        realUrl += "/" + parameter;
                    }
                }

                request = (HttpWebRequest)WebRequest.Create(realUrl);
                request.Method = "GET";
            }

            if(method == Method.POST)
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
                request.CookieContainer.Add(new Uri(baseUrl), new Cookie("PHPSESSID", phpSessID));
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