using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLConnectionEF
{
    public class Team
    {
        public int TeamId { get; set; }
        public required string Name { get; set; }

        public List<LeagueTeam> LeagueTeams { get; set; } = new List<LeagueTeam>();
    }

}
/* 
    in ef6+ we can do following to omit out association table:
    public class Team
    {
        public int TeamId { get; set; }
        public required string Name { get; set; }
        public List<League> Leagues { get; set; } = new List<League>();
    }

 */