using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using Power66Radio.Modles;
using Power66Radio.TheBoys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Power66Radio
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("tune")]
        public Task TuneIn(string ping = null)
        {
            BigDaddies bd = BigDaddies.GetInstance(Context.Guild.Users.ToList());
            if(ping == "ping")
            {
                var b = DefaultNotify(true);
                if(b == null)
                {
                    return Task.CompletedTask;
                }
                return ReplyAsync(b);
            }
            var a = DefaultNotify(false);
            if(a == null)
            {
                return Task.CompletedTask;
            }
            return ReplyAsync(a);
        }

        [Command("tuneout")]
        public Task TuneOut()
        {
            SocketGuildChannel channel = Context.Guild.GetChannel(Context.Channel.Id);
            SocketGuildUser amen = channel.GetUser(369983942088196106);
            amen.ModifyAsync(Mute);
            return  ReplyAsync("**"+amen.Username+"** has been Tuned Out.");
        }

        [Command("tunein")]
        public Task Retune()
        {
            SocketGuildChannel channel = Context.Guild.GetChannel(Context.Channel.Id);
            SocketGuildUser amen = channel.GetUser(369983942088196106);
            if (Context.User != amen)
            {
                amen.ModifyAsync(UnMute);
            } else
            {
                return ReplyAsync("Silly goose, you can't unmute yourself >:)");
            }
            return ReplyAsync("**" + amen.Username + "** has been Tuned In.");
        }
        private void Mute(GuildUserProperties gp)
        {
            gp.Mute = true;
        }

        private void UnMute(GuildUserProperties gp)
        {
            gp.Mute = false;
        }

        [Command("add")]
        public Task AddPhrase([Remainder]string phrase)
        {
            List<UserPhrases> phrases = new List<UserPhrases>();
            using (StreamReader r = new StreamReader("C:\\Users\\kungl\\source\\repos\\Power66Radio\\phrases.json"))
            {
                string json = r.ReadToEnd();
                var allPhrases = JsonConvert.DeserializeObject<List<UserPhrases>>(json);
                var userPhrases = allPhrases.Where(x => x.Id == Context.User.Id).FirstOrDefault();
                if(userPhrases == null)
                {
                    UserPhrases newUser = new UserPhrases
                    {
                        Id = Context.User.Id,
                        Phrases = new List<string>() {phrase}
                    };
                    allPhrases.Add(newUser);
                } else
                {
                    userPhrases.Phrases.Add(phrase);
                }
                phrases = allPhrases;
            }
            using (StreamWriter file = File.CreateText("C:\\Users\\kungl\\source\\repos\\Power66Radio\\phrases.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, phrases);
            }
            return ReplyAsync($"{Context.User.Username} added **{phrase}** as their new phrase!");
        }

        [Command("remove")]
        public Task RemovePhrase([Remainder] string phrase)
        {
            List<UserPhrases> phrases = new List<UserPhrases>();
            using (StreamReader r = new StreamReader("C:\\Users\\kungl\\source\\repos\\Power66Radio\\phrases.json"))
            {
                string json = r.ReadToEnd();
                var allPhrases = JsonConvert.DeserializeObject<List<UserPhrases>>(json);
                var userPhrases = allPhrases.Where(x => x.Id == Context.User.Id).FirstOrDefault();
                if (userPhrases == null)
                {
                    return ReplyAsync("You don't even have any phrases idiot!");
                }
                else
                {
                    userPhrases.Phrases.Remove(phrase);
                }
                phrases = allPhrases;
            }
            using (StreamWriter file = File.CreateText("C:\\Users\\kungl\\source\\repos\\Power66Radio\\phrases.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, phrases);
            }
            return ReplyAsync($"{Context.User.Username} removed **{phrase}** as their phrases!");
        }

        [Command("phrases")]
        public Task GetPhrases()
        {
            BigDaddies bd = BigDaddies.GetInstance(Context.Guild.Users.ToList());
            var user = bd.bigDaddies.Where(x => x.Id == Context.User.Id).FirstOrDefault();
            string message = $"**{user.Nickname}**: \n ";
            foreach(string phrase in user.Phrases)
            {
                message += phrase + "\n";
            }
            return ReplyAsync(message);
        }

        [Command("gameflip")]
        public Task CoinFlipGame()
        {
            string[] games = { "League Of Legends", "Valorant", "Genshin Impact", "Rocket League", "Project One Piece","Processing..."
                    ,"Attack On Titan","Scibble.io"};
            Random rand = new Random();

            return ReplyAsync($"Tuned for: **{games[rand.Next(0,games.Length)]}**");
        }

        public string DefaultNotify(Boolean ping)
        {
            Random rand = new Random();
            SocketGuildChannel channel = Context.Guild.GetChannel(Context.Channel.Id);
            SocketRole theRole = Context.Guild.Roles.Where(x => x.Name == "Big Daddies" || x.Id == DiscordIds.Amen).FirstOrDefault();
            var emotes = Context.Guild.Emotes;

            string user = "**" + (channel.GetUser(Context.User.Id).Nickname == null ? Context.User.Username : channel.GetUser(Context.User.Id).Nickname) + "**: ";
            string message = user;

            BigDaddies bd = BigDaddies.GetInstance(Context.Guild.Users.ToList());

            BigDaddy dad = bd.bigDaddies.Where(x => x.Id == Context.User.Id).FirstOrDefault();
            
            message += dad.Phrases == null ? " *no phrases* " : dad.Phrases[rand.Next(0,dad.Phrases.Count)];
            return ping ? theRole.Mention + "\n" + message : message;
        }
    }
}
