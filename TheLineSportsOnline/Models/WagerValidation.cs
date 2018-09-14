using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace TheLineSportsOnline.Models
{
    public class WagerValidation : ValidationAttribute
    {
        private const string ERR_POS_BET = "Wager amount must be a positve number.";
        private const string ERR_MOD_TEN = "Wager amount must be in $10 increments.";
        private const string ERR_BET_LOCK = "Wagering is currently locked. Defualt may be applied";
        private const string ERR_MAX_BET = "This Wager will put you over your maximum wager amount.";
        private const string ERR_ELIMINATED = "You have been eliminated. Wagers locked.";
        private const string ERR_LOCKED = "Wagers are currently locked.";

        private ApplicationDbContext _context = new ApplicationDbContext();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ApplicationUser user = HttpContext.Current.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>()
                    .FindById(HttpContext.Current.User.Identity.GetUserId());

            var wager = (Wager)validationContext.ObjectInstance;

            // Still in game
            if (user.Wallet <= -1000)
            {
                return new ValidationResult(ERR_ELIMINATED);
            }

            // Locked
            if (user.Locked)
            {
                return new ValidationResult(ERR_LOCKED);
            }


            // Positive Number
            if (wager.Amount < 1)
            {
                return new ValidationResult(ERR_POS_BET);
            }

            // Divisible by 10
            if (wager.Amount % 10 != 0)
            {
                return new ValidationResult(ERR_MOD_TEN);
            }
            // max bet
            long newWagerAmount = TotalWagersForUser(user.Id) + wager.Amount;

            if (newWagerAmount > user.getMaxWager())
            {
                return new ValidationResult(ERR_MAX_BET);

            }
         
            // Wager Locked based on DateTime
            var day = DateTime.Now.DayOfWeek.ToString().ToLower();
            TimeSpan cutOfTime = new TimeSpan(17, 0, 0); //5 o'clock
            TimeSpan now = DateTime.Now.TimeOfDay;

            if (day == "saturday" || day == "sunday")
            {
                return new ValidationResult(ERR_BET_LOCK);
            }

            return ValidationResult.Success;
        }

        private long TotalWagersForUser(string userID)
        {
            long totalAmountWageredThisWeek = 0L;
            var wagers = _context
                .Wagers
                .Include(w => w.Game)
                .Where(w => w.ApplicationUserId == userID)
                .Where(w => w.Game.Active == true)
                .ToList();
            foreach (var wager in wagers)
            {
                totalAmountWageredThisWeek += wager.Amount;
            }
            return totalAmountWageredThisWeek;
        }

    }
}

//- Everyone starts with $1000, so your min bet this week is $500.  Your max bet is $2000.
//*- You must bet in $10 increments
//*- If you want to bet on a Thursday night game, all of your picks must be in by 5:00 on Thursday
//- If you are not picking a Thursday game, all of your picks must be in by 12:00 on Friday
//- I will send out final lines on Thursday.  These are opening lines and may change
//- Just send me an email with your picks (i.e. $500 on Bama, $100 on Clemson, etc..)
//- Trash talking to the group is not only allowed, but highly encouraged :)

//Roll Tide,
//Jeff


