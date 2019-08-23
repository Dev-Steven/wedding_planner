using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;    // For session
using wedding_planner.Models;

namespace wedding_planner.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User NewUser)
        {
            if(ModelState.IsValid)
            {
                    // If a User exists with provided email
                if(dbContext.Users.Any(u => u.Email == NewUser.Email))
                {
                    // Manually add a ModelState error to the Email field, with provided
                    // error message
                    ModelState.AddModelError("Email", "Email already in use!");
                    
                    // You may consider returning to the View at this point
                    return View("Index");
                }
                else
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);
                    dbContext.Add(NewUser);
                    dbContext.SaveChanges();
                    NewUser.CreatedAt = DateTime.Now;
                    NewUser.UpdatedAt = DateTime.Now;
                    dbContext.SaveChanges();
                    
                    HttpContext.Session.SetInt32("id",dbContext.Users.Last().UserId);

                    return RedirectToAction("Dashboard");
                }   
            }
            else
            {
                return View("Index");
            }
        }



        [HttpPost("logging")]
        public IActionResult Logging(LogInUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.LoginEmail);
                // If no user exists with provided email
                if(userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                    return View("Index");
                }
                
                // Initialize hasher object
                var hasher = new PasswordHasher<LogInUser>();
                
                // verify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);
                
                // result can be compared to 0 for failure
                if(result == 0)
                {
                    // handle failure (this should be similar to how "existing email" is handled)
                    return View("Index");
                }

                User currentUser = dbContext.Users
                    .FirstOrDefault(user => user.Email == userSubmission.LoginEmail); 

                HttpContext.Session.SetInt32("id", currentUser.UserId);

                int? id = HttpContext.Session.GetInt32("id");

                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            int? UserId = HttpContext.Session.GetInt32("id");
            if(UserId == null)
            {
                return View("Index");
            }
            else
            {
                ViewBag.UserId = UserId;
                User user = dbContext.Users
                    .FirstOrDefault(u => u.UserId == UserId);
                ViewBag.UserName = user.FirstName;

                IEnumerable<dynamic> allWeddings = dbContext.Weddings
                    .Include(w => w.Creator)
                    .Include(w => w.AllGuests)
                       .ThenInclude(g => g.Rsvped)
                    .ToList();

                return View("Dashboard", allWeddings);
            }
        }

        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet("createwedding")]
        public IActionResult CreateWedding()
        {
            return View();
        }

        [HttpPost("addwedding")]
        public IActionResult AddWedding(Wedding newWed)
        {
            int? UserId = HttpContext.Session.GetInt32("id");
            if(ModelState.IsValid)
            {
                dbContext.Add(newWed);
                newWed.CreatorId = (int)UserId;
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View("CreateWedding");
        }

        [HttpGet("weddingdets")]
        public IActionResult WeddingDets(int wedId)
        {

            var theWedding = dbContext.Weddings
                .FirstOrDefault(w => w.WeddingId == wedId);
            return View(theWedding);
        }

        [HttpGet("{wedId}/rsvp")]
        public IActionResult Rsvp(int wedId)
        {
            int? UserId = HttpContext.Session.GetInt32("id");
            if(ModelState.IsValid)
            {
                Rsvp newRsvp = new Rsvp()
                {
                    UserId = (int)UserId,
                    WeddingId = wedId
                };
                dbContext.Rsvps.Add(newRsvp);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("Dashboard");
            }
        }

        [HttpGet("delete/{wedId}")]
        public IActionResult Delete(int wedId)
        {
            Wedding wedToDelete = dbContext.Weddings.FirstOrDefault(w => w.WeddingId == wedId);
            dbContext.Remove(wedToDelete);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("unrsvp/{wedId}")]
        public IActionResult UnRsvp(int wedId)
        {
            int? UserId = HttpContext.Session.GetInt32("id");

            var wedToUnRsvp = dbContext.Rsvps.FirstOrDefault(g => g.WeddingId == wedId && g.UserId == UserId);

            if(wedToUnRsvp != null)
            {
                dbContext.Remove(wedToUnRsvp);
                dbContext.SaveChanges();
            }

            // Delete the guest from that wedding
            
            return RedirectToAction("Dashboard");
        }
    }
}
