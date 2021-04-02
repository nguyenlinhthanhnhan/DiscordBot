using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class VouchUser
    {
        [Key]
        public Guid Id { get; set; }
        [Column(TypeName = "bigint unsigned")]
        public ulong UserVouchId { get; set; }
        public string Reason { get; set; }

        [Column(TypeName = "bigint unsigned")]
        public ulong UserLeagueVouchUserId { get; set; }
        public Guid UserLeagueVouchLeagueId { get; set; }
        public UserLeagueVouch UserLeagueVouch { get; set; }
    }
}
