using Power66Radio.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Power66Radio.Models
{
    public class ChampionsModel
    {
        public Object data { get; set; }
    }
    
    public class Champion
    {
        public string name { get; set; }
        public int key { get; set; }
    }

    public class ChampionDisplay
    {
        public string name;
        public int masteryLevel;
        public int masteryPoints;

        public ChampionDisplay(MasteryModel mm)
        {
            name = LeagueModule.AllChamps.Where(x => x.key == mm.championId).FirstOrDefault().name;
            masteryLevel = mm.championLevel;
            masteryPoints = mm.championPoints;
        }
    }
}
