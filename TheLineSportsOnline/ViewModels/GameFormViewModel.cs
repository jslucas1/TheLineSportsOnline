using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheLineSportsOnline.Models;

namespace TheLineSportsOnline.ViewModels
{
    public class GameFormViewModel
    {
        public Game Game { get; set; }
        public IEnumerable<Team> Teams { get; set; }
    }
}