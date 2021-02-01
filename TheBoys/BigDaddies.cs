using Discord.WebSocket;
using Newtonsoft.Json;
using Power66Radio.Modles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Power66Radio.TheBoys
{
    public class BigDaddies
    {
        public List<BigDaddy> bigDaddies;
        private static List<SocketGuildUser> context;
        private static BigDaddies currInstance = null;
        private BigDaddies(List<SocketGuildUser> c)
        {
            context = c;
            bigDaddies = createBigDaddies(context);
        }
        public static BigDaddies GetInstance(List<SocketGuildUser> users)
        {
            currInstance = currInstance == null ? new BigDaddies(users) : currInstance;
            return currInstance;
        }
        private static List<BigDaddy> createBigDaddies(List<SocketGuildUser> users)
        {
            List<BigDaddy> bois = new List<BigDaddy>();
            foreach(SocketGuildUser user in users)
            {
                if (user.Roles.Where(x => x.Name == "Big Daddies").FirstOrDefault() != null)
                {
                    bois.Add(new BigDaddy
                    {
                        Id = user.Id,
                        Nickname = user.Nickname == null ? user.Username : user.Nickname,
                        Phrases = findPhrasesById(user.Id)
                    });
                }
            }
            return bois;
        }
        private static List<string> findPhrasesById(ulong userId)
        {
            using (StreamReader r = new StreamReader("C:\\Users\\kungl\\source\\repos\\Power66Radio\\phrases.json"))
            {
                string json = r.ReadToEnd();
                UserPhrases dad = JsonConvert.DeserializeObject<List<UserPhrases>>(json).Where(x => x.Id == userId)?.FirstOrDefault();
                return dad != null ? dad.Phrases : null;
            }
        }
    }
}
