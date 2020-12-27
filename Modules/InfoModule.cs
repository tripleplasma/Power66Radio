using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Power66Radio
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {

        [Command("tune")]
        public Task TuneIn()
        {
            Random rand = new Random();
            SocketGuildChannel channel = Context.Guild.GetChannel(Context.Channel.Id);
            SocketRole theRole = Context.Guild.Roles.Where(x => x.Name == "Big Daddies" || x.Id == 356481452788940802).FirstOrDefault();

            string user = "**" + (channel.GetUser(Context.User.Id).Nickname == null ? Context.User.Username : channel.GetUser(Context.User.Id).Nickname) + "**";
            string message = user + " wants to play games!";
            var coolPhraseChance = rand.Next(0, 2);
            if (coolPhraseChance == 0)
            {
                switch (Context.User.Id)
                {
                    case 356476962673393675:
                        message = user + " wants to play Valorant!";
                        break;
                    case 260953174150479875:
                        message = user + ": time to bully Trip :))";
                        break;
                    case 369983942088196106:
                        int a = rand.Next(0, 2);
                        if (a == 0)
                        {
                            message = user + " just wants to be in the call :(";
                        }
                        else
                        {
                            message = user + " in the blood clot build'n";
                        }

                        break;
                    case 194805034234544128:
                        message = user + ": its bucky time";
                        break;
                    case 195631909789892608:
                        message = user + ": let me finish my dailys";
                        break;
                    case 347255438779219969:
                        message = user + ": Im not doing anything rn";
                        break;
                    case 290247305829941249:
                        message = user + ": WE PLAY FORTNITE";
                        break;
                }
            }
            
            return ReplyAsync(theRole.Mention + "\n" + message);
        }

        [Command("tuneout")]
        public Task TuneOut()
        {
            SocketGuildChannel channel = Context.Guild.GetChannel(Context.Channel.Id);
            SocketGuildUser amen = channel.GetUser(369983942088196106);
            amen.ModifyAsync(Mute);
            return  ReplyAsync("**"+amen.Username+"** has been Tuned Out.");
        }

        private void Mute(GuildUserProperties gp)
        {
            gp.Mute = true;
        }

        private void UnMute(GuildUserProperties gp)
        {
            gp.Mute = false;
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
    }
}
