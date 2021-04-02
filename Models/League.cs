using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class League
    {
        [Key]
        public Guid LeagueId { get; set; }
        public string LeagueName { get; set; }

        public ICollection<UserLeagueVouch> UserVouches { get; set; }
    }
}
