using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheLineSportsOnline.Models
{
    public class LockOfTheWeek
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        public string Header { get; set; }
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        public string MSG { get; set; }
        public string Footer { get; set; }
        public bool Active { get; set; }
    }
}