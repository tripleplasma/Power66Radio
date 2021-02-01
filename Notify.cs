using Discord.Commands;
using Discord.WebSocket;
using Power66Radio.TheBoys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Power66Radio
{
    public class Notify
    {
        SocketCommandContext Context;
        static int bettyWarning = 0;
        public Notify(SocketCommandContext context)
        {
            Context = context;
        }
        public string DefaultNotify(Boolean ping)
        {
            Random rand = new Random();
            SocketGuildChannel channel = Context.Guild.GetChannel(Context.Channel.Id);
            SocketRole theRole = Context.Guild.Roles.Where(x => x.Name == "Big Daddies" || x.Id == DiscordIds.Amen).FirstOrDefault();
            var emotes = Context.Guild.Emotes;

            string user = "**" + (channel.GetUser(Context.User.Id).Nickname == null ? Context.User.Username : channel.GetUser(Context.User.Id).Nickname) + "**";
            string message = user + " wants to play games!";


            List<string> AllenPhrases = new List<string>() { 
                ": let me finish din din real quick",
                ": peepee first",
                ": anything but normals..."
            };
            List<string> BettyPhrases = new List<string>() {
                ": *unfortunate*",
                ": time to bully Trip :))",
                $": stop doing homework {emotes.Where(x => x.Name == "wop").FirstOrDefault()?.ToString()}",
                emotes.Where(x => x.Name == "Wtf").FirstOrDefault()?.ToString()
            };
            List<string> AmenPhrases = new List<string>() { 
                $" just wants to be in the call { emotes.Where(x => x.Name == "kms").FirstOrDefault()?.ToString()}",
                " in the blood clot build'n",
                ": pull up gang"
            };
            List<string> JevPhrases = new List<string>() { 
                ": let me finish my dailys",
                ": I need to finish my weeklys",
                ": let me hit up that aim lab first",
                ": got dinner pretty soon"
            };
            List<string> VicPhrases = new List<string>() { 
                ": are u guys full?",
                ": Im not playing",
                ": you suck Allen"};
            List<string> MandePhrases = new List<string>() { 
                ": WE PLAY FORTNITE", 
                ": *Enables bannana power spike*", 
                ": Rocket League?" 
            };
            List<string> MikePhrases = new List<string>() {
                ": can you guys pick up some shoes for me?",
                ": just wanted to say luv yall"
            };
            List<string> TripPhrases = new List<string>() {
                ": Betty won't stop bothering me",
                $": *tripware destroyed* {emotes.Where(x => x.Name == "wop").FirstOrDefault()?.ToString()}"
            };
            switch (Context.User.Id)
                {
                    case DiscordIds.Allen:
                        message = user + AllenPhrases[rand.Next(0,AllenPhrases.Count)];
                        break;
                    case DiscordIds.Betty:
                        message = user + BettyPhrases[rand.Next(0,BettyPhrases.Count)];
                       /* if (bettyWarning > 0) return null;
                        message = "Sorry, due to spam, the radio host has took away ur rights " + emotes.Where(x => x.Name == "wop").FirstOrDefault()?.ToString();
                        bettyWarning++;*/
                        break;
                    case DiscordIds.Amen:
                        message = user + AmenPhrases[rand.Next(0, AmenPhrases.Count)];
                        break;
                    case DiscordIds.Trip:
                        //message = user + BigDaddies.GetTrip().phrases[rand.Next(0, BigDaddies.GetTrip().phrases.Count)];
                        message = user + TripPhrases[rand.Next(0, TripPhrases.Count)];
                        break;
                    case DiscordIds.Jev:
                        message = user + JevPhrases[rand.Next(0, JevPhrases.Count)];
                        break;
                    case DiscordIds.Vic:
                        message = user + VicPhrases[rand.Next(0, VicPhrases.Count)]; ;
                        break;
                    case DiscordIds.Mande:
                        message = user + MandePhrases[rand.Next(0, MandePhrases.Count)]; ;
                        break;
                    case DiscordIds.Mike:
                        message = user + MikePhrases[rand.Next(0, MikePhrases.Count)]; ;
                        break;
                }
            return ping ? theRole.Mention+"\n"+message : message;
        } 
    }
}
