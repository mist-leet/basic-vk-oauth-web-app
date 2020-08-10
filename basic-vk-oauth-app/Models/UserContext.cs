using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace basic_vk_oauth_app.Models
{
    public class UserContext : DbContext
    {
        public UserContext() : base("DefaultConnection")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<VkFriend> VkFriends{ get; set; }
    }

    public class UserDbInitializer : DropCreateDatabaseAlways<UserContext>
    {
        protected override void Seed(UserContext context)
        {
            context.Users.Add(new User()
            {
                Id = 1,
                Name = "Vasya"
            });

            context.VkFriends.Add(new VkFriend()
            {
                UserId = 1,
                Name = "Vanya"
            });
            base.Seed(context);
        }
    }

}