using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheLineSportsOnline.Models;

namespace TheLineSportsOnline.Controllers
{
    [Authorize]
    public class StandingsController : Controller
    {
        private ApplicationDbContext _context;

        public StandingsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Standings
        public ActionResult Index()
        {
            var users = _context.Users.OrderByDescending(u => u.Wallet).ToList();
            return View(users);
        }
    }
}