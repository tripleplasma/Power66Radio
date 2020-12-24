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
            SocketGuildChannel channel = Context.Guild.GetChannel(Context.Channel.Id);
            var everyone = channel.Users;
            var theRole = Context.Guild.Roles.Where(x => x.Position == Context.Guild.Roles.Count-2).FirstOrDefault();
            return ReplyAsync(theRole.Mention +"\n"
                +(channel.GetUser(Context.User.Id).Nickname == null ? Context.User.Username : channel.GetUser(Context.User.Id).Nickname) 
                + " in the buildin!");
        }
    }
}
