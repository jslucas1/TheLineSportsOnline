

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using TheLineSportsOnline.Models;
using TheLineSportsOnline.ViewModels;

namespace TheLineSportsOnline.Controllers
{
    
    public class GameController : Controller
    {
        private ApplicationDbContext _context;

        public GameController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Index(string display = "default")
        {
            var games = new List<Game>();
            ViewData["Display"] = display;
            if (display == "active")
            {
                games = _context.Games.Include(h => h.HomeTeam).Include(v => v.VisitTeam).Where(d => d.Active == true).OrderBy(g => g.Id).ToList();
            }
            else
            {
                games = _context.Games.Include(h => h.HomeTeam).Include(v => v.VisitTeam).OrderBy(g => g.Id).ToList();
            }

            return View(games);
        }
        [Authorize]
        public ActionResult List()
        {

            var games = _context.Games.Include(h => h.HomeTeam).Include(v => v.VisitTeam).Where(a => a.Active == true).ToList();

            return View(games);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {

            var viewModel = new GameFormViewModel
            {
                Teams = _context.Teams.OrderBy(x => x.Name).ToList(),
                Game = new Game()
            };

            return View("GameForm", viewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Game game)
        {
            game.Name = BuildGameName(game);
            if (!ModelState.IsValid)
            {
                var viewModel = new GameFormViewModel
                {
                    Teams = _context.Teams.OrderBy(x => x.Name).ToList(),
                    Game = game
                };
                return View("GameForm", viewModel);
            }

            if (game.Id == 0)
            {
                _context.Games.Add(game);
            }
            else
            {
                var gameInDb = _context.Games.Single(g => g.Id == game.Id);

                gameInDb.Name = game.Name;
                gameInDb.HomeTeamId = game.HomeTeamId;
                gameInDb.VisitTeamId = game.VisitTeamId;
                gameInDb.HomeTeamScore = game.HomeTeamScore;
                gameInDb.VisitTeamScore = game.VisitTeamScore;
                gameInDb.Spread = game.Spread;
                gameInDb.Active = game.Active;
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Game");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var game = _context.Games.SingleOrDefault(c => c.Id == id);

            if (game == null)
            {
                return HttpNotFound();
            }

            var viewModel = new GameFormViewModel
            {
                Teams = _context.Teams.ToList(),
                Game = game
            };

            return View("GameForm", viewModel);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            var game = _context.Games.SingleOrDefault(c => c.Id == id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View("Details", game);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var gameInDb = _context.Games.SingleOrDefault(c => c.Id == id);

            if (gameInDb == null)
            {
                return HttpNotFound();
            }

            _context.Games.Remove(gameInDb);

            _context.SaveChanges();
            return RedirectToAction("Index", "Game");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Deactivate()
        {
            var games = _context.Games.Where(x => x.Active == true).ToList();

            foreach (var game in games)
            {
                game.Active = false;
            }

            _context.SaveChanges();
            return RedirectToAction("index", "Game");
        }

        private string BuildGameName(Game game)
        {
            game.HomeTeam = _context.Teams.SingleOrDefault(ht => ht.Id == game.HomeTeamId);
            game.VisitTeam = _context.Teams.SingleOrDefault(ht => ht.Id == game.VisitTeamId);
            double sprd = (game.Spread > 0) ? (double)game.Spread : (double)(game.Spread * -1);

            string fav = (game.Spread > 0) ? game.VisitTeam.Name : "at " + game.HomeTeam.Name;
            string und = (game.Spread > 0) ? "at " + game.HomeTeam.Name : game.VisitTeam.Name;




            string gameName = fav + " " + sprd + " " + und;
            return gameName;
        }
    }
}