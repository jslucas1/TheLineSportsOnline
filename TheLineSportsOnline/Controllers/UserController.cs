﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheLineSportsOnline.Models;
using TheLineSportsOnline.ViewModels;

namespace TheLineSportsOnline.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private ApplicationDbContext _context;

        public UserController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var users = _context.Users.Include(x => x.Roles).OrderBy(w => w.Email).ToList();
            return View(users);
        }
        public ActionResult Delete(string id)
        {
            var userInDb = _context.Users.SingleOrDefault(c => c.Id == id);

            if (userInDb == null)
            {
                return HttpNotFound();
            }

            _context.Users.Remove(userInDb);

            _context.SaveChanges();
            return RedirectToAction("Index", "User");
        }
        public ActionResult Default(string id)
        {
            var user = _context.Users.SingleOrDefault(c => c.Id == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            var games = _context.Games
                .Where(a => a.Active == true)
                .OrderByDescending(b => b.Spread).ToList();
            var g1 = games.First();
            var g2 = games.Last();
            var game = (Math.Abs((double)g1.Spread) > Math.Abs((double)g2.Spread)) ? g1 : g2;

            Wager defaultWager = new Wager();
            defaultWager.Amount = (int)user.getMinWager();
            defaultWager.ApplicationUserId = user.Id;
            defaultWager.GameId = game.Id;
            defaultWager.HomeOrVisit = (game.Spread < 0) ? "Home" : "Away";

            var wagers = _context.Wagers
                    .Where(w => w.Game.Active)
                    .Where(w => w.ApplicationUserId == user.Id)
                    .ToList();
            foreach (var w in wagers)
            {
                _context.Wagers.Remove(w);
            }
            _context.SaveChanges();
            _context.Wagers.Add(defaultWager);

            _context.SaveChanges();
            return RedirectToAction("Index", "User");
        }
        public ActionResult Lock(string id, bool setLock)
        {
            var user = _context.Users.SingleOrDefault(c => c.Id == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            user.Locked = setLock;

            _context.SaveChanges();
            return RedirectToAction("Index", "User");
        }
        public ActionResult LockAll(bool setLock)
        {
            var users = _context.Users;

            if (users == null)
            {
                return HttpNotFound();
            }

            foreach (var user in users)
            {
                user.Locked = setLock;
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "User");
        }
        public ActionResult CalcResults()
        {
            var wagers = _context.Wagers
                .Include(w => w.Game)
                .Where(w => w.Game.Active)
                .OrderBy(w => w.ApplicationUserId)
                .ToArray();
            var currentUser = _context.Users.OrderBy(u => u.Id).ToList().First();
            long applyToWallet = 0L;
            foreach (var item in wagers)
            {
                if (item.ApplicationUserId != currentUser.Id)
                {
                    // Update the wallet
                    currentUser.Wallet += applyToWallet;
                    // Reset the variables 
                    applyToWallet = 0;
                    currentUser = _context.Users.Where(u => u.Id == item.ApplicationUserId).SingleOrDefault();
                }
                // Calculate how much to apply based on how much the Wager awards
                applyToWallet += Wager.Award(item);

            }
            currentUser.Wallet += applyToWallet;
            _context.SaveChanges();
            return RedirectToAction("Index", "User");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Info(string id)
        {
            var user = _context.Users.SingleOrDefault(c => c.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var wagers = _context.Wagers
                .Include(g => g.Game)
        .Where(w => w.Game.Active)
        .Where(w => w.ApplicationUserId == user.Id)
        .ToList();
            var viewModel = new UserInfoViewModel()
            {
                User = user,
                Wagers = wagers
            };

            return View("UserInfo", viewModel);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DefaultPassword(string id)
        {
            var user = _context.Users.SingleOrDefault(c => c.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.PasswordHash = "AJ0zTa3gIPU85AjWiGqH/Q2/luyvRRPYV2aUQzXylTaHPtj7HBHikUsdzGnZW/hyvA==";
            var wagers = _context.Wagers
                .Include(g => g.Game)
                .Where(w => w.Game.Active)
                .Where(w => w.ApplicationUserId == user.Id)
                .ToList();
            var viewModel = new UserInfoViewModel()
            {
                User = user,
                Wagers = wagers
            };
            _context.SaveChanges();
            return View("UserInfo", viewModel);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Save(Game game)
        //{
        //    game.Name = BuildGameName(game);
        //    if (!ModelState.IsValid)
        //    {
        //        var viewModel = new GameFormViewModel
        //        {
        //            Teams = _context.Teams.ToList(),
        //            Game = game
        //        };
        //        return View("GameForm", viewModel);
        //    }

        //    if (game.Id == 0)
        //    {
        //        _context.Games.Add(game);
        //    }
        //    else
        //    {
        //        var gameInDb = _context.Games.Single(g => g.Id == game.Id);

        //        gameInDb.Name = game.Name;
        //        gameInDb.HomeTeamId = game.HomeTeamId;
        //        gameInDb.VisitTeamId = game.VisitTeamId;
        //        gameInDb.HomeTeamScore = game.HomeTeamScore;
        //        gameInDb.VisitTeamScore = game.VisitTeamScore;
        //        gameInDb.Spread = game.Spread;
        //        gameInDb.Active = game.Active;
        //    }

        //    _context.SaveChanges();
        //    return RedirectToAction("Index", "Game");
        //}

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