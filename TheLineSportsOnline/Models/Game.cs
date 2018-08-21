using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheLineSportsOnline.Models
{
    public class Game
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Home Team")]
        public byte? HomeTeamId { get; set; }
        [Display(Name = "Home Team")]
        public Team HomeTeam { get; set; }

        [Display(Name = "Away Team")]
        public byte? VisitTeamId { get; set; }
        [Display(Name = "Home Team")]
        public Team VisitTeam { get; set; }

        public double? Spread { get; set; }

        [Display(Name = "Home Score")]
        public int? HomeTeamScore { get; set; }

        [Display(Name="Away Score")]
        public int? VisitTeamScore { get; set; }


        public bool Active { get; set; }

    }
}