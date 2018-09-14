using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheLineSportsOnline.Models
{
    public class Wager
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Game")]
        public byte GameId { get; set; }
        public Game Game { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        [Display(Name = "Team")]
        public string HomeOrVisit { get; set; }

        [Required]
        [WagerValidation]
        public int Amount { get; set; }

        internal static long Award(Wager wager)
        {
            long winnings = 0L;
            bool homeTeamCovered = ((wager.Game.HomeTeamScore + wager.Game.Spread) > wager.Game.VisitTeamScore);

            if ((wager.HomeOrVisit.ToLower() == "home" && homeTeamCovered) || (wager.HomeOrVisit.ToLower() == "away" && !homeTeamCovered))
            {
                winnings = wager.Amount;
            }
            else 
            {
                winnings = -wager.Amount;
            }


            return winnings;
        }
    }
}