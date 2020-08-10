using basic_vk_oauth_app.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace basic_vk_oauth_app.Controllers
{
    public class AccountController : Controller
    {

        public ActionResult Login()
        {
            ViewBag.url = VkOAuthData.GetRequestToAuth();
            return View();
        }

        public ActionResult VkOAuth(string code)
        {

            string code_from_vk = Request.Url.Query.Substring(6);

            VkOAuthAccessJSON accessData = VkOAuthData.GetAccessToken(code_from_vk);

            RootVkUserInfo userInfo = VkOAuthData.GetUserInfo(accessData.user_id, accessData.access_token);

            RootVkFriends friends = VkOAuthData.GetUserFriends(accessData.user_id, accessData.access_token);

            User newUser = new User();

            newUser.VkId = accessData.user_id;
            newUser.AccessToken = accessData.access_token;
            newUser.ExpiresIn = accessData.expires_in;

            newUser.Name = userInfo.response[0].first_name + " " + userInfo.response[0].last_name;

            List<VkFriend> newUserFriends = new List<VkFriend>();

            foreach (ItemVkFriends friend in friends.response.items)
            {
                newUserFriends.Add(
                    new VkFriend()
                    {
                        Id = friend.id,
                        Name = friend.first_name + " " + friend.last_name,
                        Img = friend.photo_100
                    }
                    );
            }

            newUser.Friends = newUserFriends;


            using (UserContext db = new UserContext())
            {
                db.Users.Add(newUser);
                db.SaveChanges();
            }
                FormsAuthentication.SetAuthCookie(newUser.VkId, true);
                return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); 
            return RedirectToAction("index", "Home");
        }
    }
}