using System;
using MatchDetailsApp.Models.Domain;

namespace MatchDetailsApp.Models.DTOs
{
	public class ValueDto
	{
        public string MatchId { get; set; }
        public int MatchDay { get; set; }
        public string HomeTeamName { get; set; }
        public string GuestTeamName { get; set; }
        public string PlannedKickoffTime { get; set; }
        public string StadiumName { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}

