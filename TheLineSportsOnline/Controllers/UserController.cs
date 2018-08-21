﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheLineSportsOnline.Models;

namespace TheLineSportsOnline.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext _context;

        public UserController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var users = _context.Users.Include(x => x.Roles).ToList();
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