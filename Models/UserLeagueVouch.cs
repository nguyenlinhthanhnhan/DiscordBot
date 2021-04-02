using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserLeagueVouch
    {
        [Column(TypeName = "bigint unsigned")]
        public ulong UserId { get; set; }
        public User User { get; set; }

        public Guid LeagueId { get; set; }
        public League League { get; set; }

        public uint Vouch { get; set; }

        public ICollection<VouchUser> VouchedUsers { get; set; }
    }
}
