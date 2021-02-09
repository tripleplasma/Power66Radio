using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Power66Radio.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Power66Radio.Modules
{
    public class LeagueModule : ModuleBase<SocketCommandContext>
    {
        private string base_url = "https://na1.api.riotgames.com";
        private string key = File.ReadAllText("C:\\Key\\leaguekey.txt");
        public static List<Champion> AllChamps = SetChamps();
        private HttpClient client = new HttpClient();

        private static List<Champion> SetChamps()
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync("http://ddragon.leagueoflegends.com/cdn/11.2.1/data/en_US/champion.json");
            var json = response.Result.Content.ReadAsStringAsync().Result;

            ChampionsModel model = JsonConvert.DeserializeObject<ChampionsModel>(json);
            List<Champion> champs = new List<Champion>();
            var champObject = model.data;

            List<PropertyInfo> props = new List<PropertyInfo>(champObject.GetType().GetProperties());
            PropertyInfo p = props.Where(x => x.Name == "First").FirstOrDefault();
            JToken allChamps = ((JToken)p.GetValue(champObject));
            while (allChamps != null)
            {
                Champion currentChamp = JsonConvert.DeserializeObject<Champion>(allChamps.First.ToString());
                champs.Add(currentChamp);
                allChamps = allChamps.Next;
            }

            return champs;
        }

        private SummonerModel GetSummoner(string summoner)
        {
            var response = client.GetAsync($"{base_url}/lol/summoner/v4/summoners/by-name/{summoner}?api_key={key}");
            var json = response.Result.Content.ReadAsStringAsync().Result;

            return response.Result.StatusCode == System.Net.HttpStatusCode.Forbidden ? null : JsonConvert.DeserializeObject<SummonerModel>(json);
        }
        private List<MatchInfo> GetSummonerMatchHistory(string accountId, int endIndex = 3)
        {
            var response = client.GetAsync($"{base_url}/lol/match/v4/matchlists/by-account/{accountId}?endIndex={endIndex}&api_key={key}");
            var json = response.Result.Content.ReadAsStringAsync().Result;
            List<MatchInfo> matches= JsonConvert.DeserializeObject<MatchHistoryModel>(json).matches;
            return matches;
        }

        [Command("lol")]
        public Task League([Remainder]string summoner)
        {
            var user = GetSummoner(summoner);

            if (user.id != null)
            {
                var champResponse = client.GetAsync($"{base_url}/lol/champion-mastery/v4/champion-masteries/by-summoner/{user.id}?api_key={key}");
                var champJson = champResponse.Result.Content.ReadAsStringAsync().Result;

                List<MasteryModel> masteries = JsonConvert.DeserializeObject<List<MasteryModel>>(champJson);

                List<ChampionDisplay> display = new List<ChampionDisplay>()
                {
                new ChampionDisplay(masteries[0]),
                new ChampionDisplay(masteries[1]),
                new ChampionDisplay(masteries[2])
                };
                string message = $"{user.name} is level {user.summonerLevel}\n";
                foreach (ChampionDisplay cd in display)
                {
                    message += $"Mastery Level {cd.masteryLevel} {cd.name} with {String.Format("{0:n0}", cd.masteryPoints)} points!\n";
                }
                return ReplyAsync(message);
            }
            return ReplyAsync("The summoner you asked for does not exist");
            
        }

        [Command("lolstats")]
        public Task LolStats([Remainder] string summoner)
        {
            var user = GetSummoner(summoner);
            if(user.id == null)
            {
                return ReplyAsync("The summoner you asked for does not exist");
            }
            List<MatchInfo> matchHistory = GetSummonerMatchHistory(user.accountId);
            List<Match> normals = new List<Match>();
            int totalKills = 0;
            int totalDeaths = 0;
            int totalAssist = 0;
            long totalDmg = 0;
            foreach (MatchInfo m in matchHistory)
            {
                var response = client.GetAsync($"{base_url}/lol/match/v4/matches/{m.gameId}?api_key={key}");
                var json = response.Result.Content.ReadAsStringAsync().Result;
                var match = JsonConvert.DeserializeObject<Match>(json);
                var playerId = match.participantIdentities.Where(x => x.player.accountId == user.accountId).FirstOrDefault().participantId;
                ParticipantStats playerStats = match.participants.Where(x => x.participantId == playerId).FirstOrDefault().stats;
                totalKills += playerStats.kills;
                totalDeaths += playerStats.deaths;
                totalAssist += playerStats.assists;
                totalDmg += playerStats.totalDamageDealtToChampions;
            }
            return ReplyAsync($"*Past 3 games*" +
                $"\n**{user.name}**\n**Kills:** {totalKills}\n**Deaths:** {totalDeaths}\n**Assist:** {totalAssist}\n**Dmg:** {String.Format("{0:n0}", totalDmg)}");
        }
    }
}
