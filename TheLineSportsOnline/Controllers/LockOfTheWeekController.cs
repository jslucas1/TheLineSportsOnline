using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using TheLineSportsOnline.Models;

namespace TheLineSportsOnline.Controllers
{
    public class LockOfTheWeekController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LockOfTheWeek
        public ActionResult Index()
        {
            return View(db.Locks.ToList());
        }
        public ActionResult Display()
        {
            var lotw = db.Locks.Where(l => l.Active == true).OrderByDescending(x => x.Id).ToList();
            return View(lotw);
        }
        // GET: LockOfTheWeek/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LockOfTheWeek lockOfTheWeek = db.Locks.Find(id);
            if (lockOfTheWeek == null)
            {
                return HttpNotFound();
            }
            return View(lockOfTheWeek);
        }

        // GET: LockOfTheWeek/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LockOfTheWeek/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Header,MSG,Footer,Active")] LockOfTheWeek lockOfTheWeek)
        {
            if (ModelState.IsValid)
            {
                db.Locks.Add(lockOfTheWeek);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lockOfTheWeek);
        }

        // GET: LockOfTheWeek/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LockOfTheWeek lockOfTheWeek = db.Locks.Find(id);
            if (lockOfTheWeek == null)
            {
                return HttpNotFound();
            }
            return View(lockOfTheWeek);
        }

        // POST: LockOfTheWeek/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Header,MSG,Footer,Active")] LockOfTheWeek lockOfTheWeek)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lockOfTheWeek).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lockOfTheWeek);
        }

        // GET: LockOfTheWeek/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LockOfTheWeek lockOfTheWeek = db.Locks.Find(id);
            if (lockOfTheWeek == null)
            {
                return HttpNotFound();
            }
            return View(lockOfTheWeek);
        }

        // POST: LockOfTheWeek/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            LockOfTheWeek lockOfTheWeek = db.Locks.Find(id);
            db.Locks.Remove(lockOfTheWeek);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SendEmail()
        {
            var lotw = db.Locks.Where(l => l.Active == true).OrderByDescending(x => x.Id).ToList().First();
            var users = db.Users.ToList();
            foreach (ApplicationUser user in users)
            {
                EmailLOTW(user.UserName, user.Email, lotw);
            }

            return RedirectToAction("Index");
        }

        public void EmailLOTW(string email, string name, LockOfTheWeek lotw)
        {

            var fromAddress = new MailAddress("thelinesportsonline@gmail.com", "The Line Sports");
            var toAddress = new MailAddress(email, name);
            string fromPassword = "Rolltide1!";
            string subject = lotw.Header;
            string body = lotw.MSG + Environment.NewLine + Environment.NewLine + "- " + lotw.Footer;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
