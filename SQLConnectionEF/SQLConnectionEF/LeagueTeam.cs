using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLConnectionEF
{
    public class LeagueTeam
    {
        public int LeagueId { get; set; }
        public League League { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
    }

}
