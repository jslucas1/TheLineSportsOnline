using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheLineSportsOnline.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using TheLineSportsOnline.ViewModels;

namespace TheLineSportsOnline.Controllers
{
    [Authorize]
    public class WagerController : Controller
    {
        private ApplicationDbContext _context;
        protected UserManager<ApplicationUser> UserManager;


        public WagerController()
        {
            _context = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));

        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext()
                .GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var wagers = _context.Wagers
                .Include(h => h.Game)
                .Include(h => h.Game.HomeTeam)
                .Include(h => h.Game.VisitTeam)
                .Where(w => w.ApplicationUserId == user.Id)
                .Where(w => w.Game.Active == true)
                .ToList();

            return View(wagers);
        }

        public ActionResult New()
        {

            var games = _context.Games.Where(x => x.Active == true).ToList();

            var viewModel = new WagerFormViewModel()
            {
                Games = games,
                Wager = new Wager()
            };

            return View("WagerForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Wager wager)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new WagerFormViewModel()
                {
                    Games = _context.Games.Where(x => x.Active == true).ToList(),
                    Wager = wager
                };
                return View("WagerForm", viewModel);
            }

            if (wager.Id == 0)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>()
                    .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                wager.ApplicationUserId = user.Id;

                _context.Wagers.Add(wager);
            }
            else
            {
                var wagerInDb = _context.Wagers.Single(w => w.Id == wager.Id);

                wagerInDb.GameId = wager.GameId;
                wagerInDb.HomeOrVisit = wager.HomeOrVisit;
                wagerInDb.Amount = wager.Amount;

            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Wager");
        }


        public ActionResult Delete(int id)
        {
            var wagerInDb = _context.Wagers.SingleOrDefault(c => c.Id == id);

            if (wagerInDb == null)
            {
                return HttpNotFound();
            }

            _context.Wagers.Remove(wagerInDb);

            _context.SaveChanges();
            return RedirectToAction("index", "Wager");
        }


        //public ActionResult Edit(int id)
        //{
        //    var game = _context.Games.SingleOrDefault(c => c.Id == id);

        //    if (game == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    var viewModel = new GameFormViewModel
        //    {
        //        Teams = _context.Teams.ToList(),
        //        Game = game
        //    };

        //    return View("GameForm", viewModel);
        //}

        //public ActionResult Details(int id)
        //{
        //    var game = _context.Games.SingleOrDefault(c => c.Id == id);
        //    if (game == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View("Details", game);
        //}

    }
}