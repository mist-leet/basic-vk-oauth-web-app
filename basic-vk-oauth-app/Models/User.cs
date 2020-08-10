using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace basic_vk_oauth_app.Models
{
    public class User
    {
        public int Id { get; set; }

        public string VkId { get; set; }
        
        public string Name { get; set; }
        
        public string AccessToken { get; set; }

        public string ExpiresIn { get; set; }

        public ICollection<VkFriend> Friends{ get; set; }

        public User()
        {
            Friends = new List<VkFriend>();
        }
    }

    public class VkFriend
    {
        public User User { get; set; }
        public int UserId { get; set; }
        public int Id { get; set; }

        public string Name { get; set; }

        public string Img { get; set; }
    }
}