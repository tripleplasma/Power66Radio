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
using System.Threading;
using System.Threading.Tasks;

namespace Power66Radio
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("tune")]
        public Task TuneIn(string ping = null)
        {
            Random rand = new Random();
            SocketGuild guild = Context.Guild;
            SocketRole theRole = Context.Guild.Roles.Where(x => x.Name == "Big Daddies").FirstOrDefault();
            var emotes = Context.Guild.Emotes;

            BigDaddy dad = BigDaddies.GetBigDaddyByUser(guild.GetUser(Context.User.Id));
            string message = "**" + dad.Nickname + "**: ";

            if (dad == null) return ReplyAsync($"**{Context.User.Username}** is not a Big Daddy");
            if (dad.Phrases.Count == 0) return ReplyAsync($"**{Context.User.Username}** does not have any phrases");

            message += dad.Phrases[rand.Next(0, dad.Phrases.Count)];
            return ReplyAsync(ping == "ping" ? theRole.Mention + "\n" + message : message);
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
            if (Context.User == amen)
            {
                return ReplyAsync("Silly goose, you can't unmute yourself >:)");
            }
            amen.ModifyAsync(UnMute);
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
            BigDaddy bd = BigDaddies.GetBigDaddyByUser(Context.Guild.GetUser(Context.User.Id));
            if (bd == null) return ReplyAsync($"**{Context.User.Username}** is not a Big Daddy");
            List<UserPhrases> serializePhrases = new List<UserPhrases>();
            using (StreamReader r = new StreamReader("C:\\Users\\kungl\\source\\repos\\Power66Radio\\phrases.json"))
            {
                string json = r.ReadToEnd();
                var allUserPhrases = JsonConvert.DeserializeObject<List<UserPhrases>>(json);
                var currentUserPhrases = allUserPhrases.Where(x => x.Id == bd.Id).FirstOrDefault();
                if(currentUserPhrases == null)
                {
                    UserPhrases newUser = new UserPhrases
                    {
                        Id = bd.Id,
                        Phrases = new List<string>() {phrase}
                    };
                    allUserPhrases.Add(newUser);
                } else
                {
                    currentUserPhrases.Phrases.Add(phrase);
                    bd.Phrases.Add(phrase);
                }
                serializePhrases = allUserPhrases;
            }
            using (StreamWriter file = File.CreateText("C:\\Users\\kungl\\source\\repos\\Power66Radio\\phrases.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, serializePhrases);
            }
            return ReplyAsync($"{Context.User.Username} added **{phrase}** as their new phrase!");
        }

        [Command("remove")]
        public Task RemovePhrase(int index = -1)
        {
            List<UserPhrases> phrases = new List<UserPhrases>();
            string phrase = String.Empty;
            BigDaddy bd = BigDaddies.GetBigDaddyByUser(Context.Guild.GetUser(Context.User.Id));
            using (StreamReader r = new StreamReader("C:\\Users\\kungl\\source\\repos\\Power66Radio\\phrases.json"))
            {
                string json = r.ReadToEnd();
                List<UserPhrases> allPhrases = JsonConvert.DeserializeObject<List<UserPhrases>>(json);
                UserPhrases userPhrases = allPhrases.Where(x => x.Id == bd.Id).FirstOrDefault();
                if (userPhrases.Phrases.Count==0)
                {
                    return ReplyAsync("You don't even have any phrases idiot!");
                }
                else
                {
                    if (index >= 0 && index < userPhrases.Phrases.Count)
                    {
                        phrase = userPhrases.Phrases[index];
                        userPhrases.Phrases.RemoveAt(index);
                        bd.Phrases.RemoveAt(index);
                    } else
                    {
                        return ReplyAsync("**Oops!** You typed the wrong position");
                    }
                    
                }
                phrases = allPhrases;
            }
            using (StreamWriter file = File.CreateText("C:\\Users\\kungl\\source\\repos\\Power66Radio\\phrases.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, phrases);
            }
            return ReplyAsync($"{Context.User.Username} removed **{phrase}** from their phrases!");
        }

        [Command("phrases")]
        public Task GetPhrases()
        {
            BigDaddy user = BigDaddies.GetBigDaddyByUser(Context.Guild.GetUser(Context.User.Id));
            string message = $"**{user.Nickname}**: \n";
            if (user.Phrases == null) return ReplyAsync($"{Context.User.Username} does not have any phrases");
            for(int i = 0; i < user.Phrases.Count; i++)
            {
                string phrase = user.Phrases[i];
                message += $"[{i}] {phrase} \n";
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
    }
}
