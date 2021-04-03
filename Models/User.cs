using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class User
    {
        [Key]
        [Column(TypeName = "bigint unsigned")]
        public ulong Id { get; set; }
        public DateTime JoinedAt { get; set; }
        public uint TotalVouch { get; set; }
        public uint TotalUniqueVouch { get; set; }

        public ICollection<UserLeagueVouch> UserVouches { get; set; }
    }
}
