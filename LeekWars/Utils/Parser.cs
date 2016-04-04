using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeekWars.LeekWarsAPI.Model;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace LeekWars.Utils
{
    class Parser
    {
        public static Leek[] parseLeeksFromJSON(string data)
        {
            Leek[] result = null;

            Regex leeksRegex = new Regex("\"leeks\":{(.*})},");

            Match leeksMatch = leeksRegex.Match(data);
            string leeksstring = leeksMatch.Groups[1].Value;
            
            Regex leekRegex = new Regex("({[\"0-9a-zA-Z:,#]+}{1}?)");

            MatchCollection leekMatches = leekRegex.Matches(leeksstring);

            result = new Leek[leekMatches.Count];

            for (int nbLeeks = 0; nbLeeks < leekMatches.Count; nbLeeks++)
            {
                Match match = leekMatches[nbLeeks];
                string leeks = match.Groups[1].Captures[0].Value;

                Stream stream = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(leeks));
                var serializer = new DataContractJsonSerializer(typeof(Leek));
                Leek leek = (Leek)serializer.ReadObject(stream);

                result[nbLeeks] = leek;
            }

            return result;
        }

        public static void parseFightsNumberFromJSON(ref Leek[] leeks, string data) {
            //parse fights
            Regex qariRegex = new Regex("\"leek_fights\":{(?<nbFights>.*?)}");
            Match match = qariRegex.Match(data);
            string leekfights = match.Groups["nbFights"].Value;

            qariRegex = new Regex("([0-9]+)\":([0-9]*),?");
            MatchCollection mc = qariRegex.Matches(leekfights);

            foreach(Match idFightsMatch in mc)
            {
                int id = Int32.Parse(idFightsMatch.Groups[1].Value);
                int value = Int32.Parse(idFightsMatch.Groups[2].Value);

                foreach(Leek leek in leeks)
                {
                    if(leek.id == id)
                    {
                        leek.fights = value;
                    }
                }
            }
        }

        public static void parseSoloEnemies(ref Leek[] leeks, string data)
        {
            //parse ennemies
            Regex soloEnemiesRegex = new Regex("\"solo_enemies\":{(.*])}+?");
            Match soloEnemiesMatch = soloEnemiesRegex.Match(data);
            string soloEnemiesString = soloEnemiesMatch.Groups[1].Captures[0].Value;

            //Regex soloEnemyRegex = new Regex("\"([0-9]*)\":([.*]+?)");
            Regex soloEnemyRegex = new Regex("\"([0-9]*)\":(" + @"\[" + "[\"0-9a-zA-Z:,#{}@]*]+?)");
            MatchCollection soloEnemyCollectionMatch = soloEnemyRegex.Matches(soloEnemiesString);

            foreach (Match soloEnemyMatch in soloEnemyCollectionMatch)
            {
                int id = Int32.Parse(soloEnemyMatch.Groups[1].Captures[0].Value);
                string enemiesString = soloEnemyMatch.Groups[2].Captures[0].Value;

                foreach (Leek leek in leeks)
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
        }

        public static string parseLeekWarsGetParameters(params string[] parameters)
        {
            string result = String.Empty;

            if (parameters != null)
            {
                foreach (string parameter in parameters)
                {
                    result += "/" + parameter;
                }
            }

            return result;
        }
    }
}
