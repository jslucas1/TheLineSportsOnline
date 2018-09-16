using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheLineSportsOnline.Models;


namespace TheLineSportsOnline.ViewModels
{
    public class UserInfoViewModel
    {
        public ApplicationUser User { get; set; }
        public IEnumerable<Wager> Wagers { get; set; }
    }
}