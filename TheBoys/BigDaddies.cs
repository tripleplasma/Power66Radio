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
        public static ulong AdminRoleID = 356481452788940802;
        private static List<BigDaddy> bigDaddies = new List<BigDaddy>();
        public static BigDaddy GetBigDaddyByUser(SocketGuildUser user)
        {
            BigDaddy bd = bigDaddies.Where(x => x.Id == user.Id).FirstOrDefault();
            if (user.Roles.Where(x => x.Id == AdminRoleID).FirstOrDefault() != null && bd == null)
            {
                bd = new BigDaddy
                {
                    Id = user.Id,
                    Nickname = user.Nickname == null ? user.Username : user.Nickname,
                    Phrases = findPhrasesById(user.Id)
                };
                bigDaddies.Add(bd);
            }
            return bd;
        }

        public static List<BigDaddy> GetBigDaddies() => bigDaddies;
        private static List<string> findPhrasesById(ulong userId)
        {
            using (StreamReader r = new StreamReader("C:\\Users\\kungl\\source\\repos\\Power66Radio\\phrases.json"))
            {
                string json = r.ReadToEnd();
                UserPhrases dad = JsonConvert.DeserializeObject<List<UserPhrases>>(json).Where(x => x.Id == userId).FirstOrDefault();
                return dad != null ? dad.Phrases : new List<string>();
            }
        }
    }
}
