using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Power66Radio.Models
{
    public class MatchHistoryModel
    {
        public List<MatchInfo> matches { get; set; }
    }

    public class MatchInfo
    {
        public long gameId { get; set; }
        public int champion { get; set; }
    }

    public class Match
    {
        public long gameId { get; set; }
        public List<Participant> participants { get; set; }
        public List<ParticipantIdentity> participantIdentities { get; set; }
        public string gameMode { get; set; }
    }

    public class Participant
    {
        public int participantId { get; set; }
        public ParticipantStats stats { get; set; }
    }
    public class ParticipantStats
    {
        public bool win { get; set; }
        public int kills { get; set; }
        public int deaths { get; set; }
        public int assists { get; set; }
        public long totalDamageDealtToChampions { get; set; }
    }
    public class ParticipantIdentity
    {
        public int participantId { get; set; }
        public SummonerModel player { get; set; }
    }
}
