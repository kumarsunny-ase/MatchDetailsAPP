using System;
using MatchDetailsApp.Models.Domain;

namespace MatchDetailsApp.Models.DTOs
{
	public class ValueDto
	{
        public int MatchDay { get; set; }
        public string HomeTeamName { get; set; }
        public string GuestTeamName { get; set; }
        public string PlannedKickoffTime { get; set; }
        public string StadiumName { get; set; }
    }
}

