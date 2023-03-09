using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Azure_Task_2.Models;
using System.Web.Mvc;

namespace Azure_Task_2.Controllers
{
    public class LoginController : Controller
    {
        
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
          
            return View();
        }

      

        [HttpPost]
        public ActionResult Authorise(User Userdata)
        {
            DataEntities _db = new DataEntities();
            string Username = Userdata.Username.Trim();
            string Password = Userdata.Password.Trim();

            if(Username.Length == 0 || Password.Length == 0)
            {
                return View("Error");
            }

            var users = _db.Users.ToList();
            foreach (var i in users)
            {
                string PersonUsername = i.Username.Trim();
                string PersonPassword = i.Password.Trim();

                if (Username == PersonUsername)
                {
                    if (Password != PersonPassword)
                    {

                        return View("PasswordIncorrect");
                       
                    }         
                    _db.Users.Where(p => p.Id == i.Id)
                    .ToList()
                    .ForEach(x => x.LastLoginDateTime = DateTime.Now);
                    _db.SaveChanges();
                    return View("Success");
                }
            }
                return View("UserNotFound");
        }

    }
}