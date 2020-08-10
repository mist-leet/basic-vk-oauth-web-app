using basic_vk_oauth_app.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace basic_vk_oauth_app.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            using (UserContext db = new UserContext())
            {
                User user = db.Users.FirstOrDefault(u => u.VkId == User.Identity.Name);

                ViewBag.UserName = user.Name;
                
                var model = db.VkFriends.Include(a => a.User).Where(b => b.UserId == user.Id).ToList();
                
                return View(model);
            }
        }
    }
}