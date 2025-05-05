using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLConnectionEF
{
    public class League
    {
        public int LeagueId { get; set; }
        public required string Name { get; set; }

        public List<LeagueTeam> LeagueTeams { get; set; } = new List<LeagueTeam>();
    }

}

/* 
    in ef6+ we can do following to omit out association table:
    public class League
    {
        public int LeagueId { get; set; }
        public required string Name { get; set; }
        public List<Team> Teams { get; set; } = new List<Team>();
    }

 */