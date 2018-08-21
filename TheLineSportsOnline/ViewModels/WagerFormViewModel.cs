using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheLineSportsOnline.Models;

namespace TheLineSportsOnline.ViewModels
{
    public class WagerFormViewModel
    {
        public WagerFormViewModel()
        {
            Teams = new List<string>()
            {
                "Home", "Away"
            };
        }
        public Wager Wager { get; set; }
        public IEnumerable<Game> Games { get; set; }
        public IEnumerable<string> Teams { get; }
    }
}