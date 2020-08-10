using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace basic_vk_oauth_app.Models
{
    public class VkOAuthData
    {
        private const string client_id = "7561772";

        private const string client_secret = "6KgkDvpEuGNENPYBGmcf";

        private const string redirect_uri = "https://localhost:44369/VkOAuth/";

        private const string url_auth = "http://oauth.vk.com/authorize";

        private const string url_token = "https://oauth.vk.com/access_token";

        private const string url_user_info = "https://api.vk.com/method/users.get";

        private const string url_user_friends = "https://api.vk.com/method/friends.get";


        public static string GetRequestToAuth()
        {
            return GetQueryUrl(url_auth,
                new Dictionary<string, string>() {
                    {"client_id", client_id },
                    {"redirect_uri", redirect_uri },
                    {"responser_type", "code"}
                });
        }

        private static string GetRequestForAccessToken(string code)
        {
            return GetQueryUrl(
                url_token,
                new Dictionary<string, string>()
                {
                    { "client_id", client_id},
                    {"client_secret", client_secret },
                    {"code", code },
                    {"redirect_uri", redirect_uri }
                }
                );
        }

        private static string GetRequestForUserInfo(string userId, string accessToken)
        {
            return GetQueryUrl(url_user_info,
                new Dictionary<string, string>()
                {
                    {"uids", userId },
                    {"fields", "first_name,last_name" },
                    {"access_token",accessToken },
                    {"v", "5.122"}
                }
                );
        }

        private static string GetRequestForUserFrineds(string userId, string accessToken)
        {
            return GetQueryUrl(
                url_user_friends,
                new Dictionary<string, string>()
                {
                    {"user_id", userId },
                    {"order", "random" },
                    { "count", "5"},
                    {"fields", "first_name,last_name,photo_100" },
                    {"access_token",accessToken },
                    {"v","5.122" }
                }
                );
        }

        private static string GetQueryUrl(string main, Dictionary<string, string> parameters)
        {
            string result = main + "?";
            foreach (KeyValuePair<string, string> keyValue in parameters)
            {
                result += keyValue.Key + "=" + keyValue.Value + "&";
            }
            return result.Substring(0, result.Length - 1);
        }

        public static VkOAuthAccessJSON GetAccessToken(string code)
        {
            WebRequest request = WebRequest.Create(VkOAuthData.GetRequestForAccessToken(code));

            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();

                VkOAuthAccessJSON responseData = JsonConvert.DeserializeObject<VkOAuthAccessJSON>(responseFromServer);
                return responseData;
            }
        }

        public static RootVkUserInfo GetUserInfo(string userId, string accessToken)
        {
            WebRequest request = WebRequest.Create(VkOAuthData.GetRequestForUserInfo(userId, accessToken));

            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();

                RootVkUserInfo responseData = JsonConvert.DeserializeObject<RootVkUserInfo>(responseFromServer);
                return responseData;
            }
        }

        public static RootVkFriends GetUserFriends(string userId, string accessToken)
        {
            WebRequest request = WebRequest.Create(VkOAuthData.GetRequestForUserFrineds(userId, accessToken));

            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();

                RootVkFriends responseData = JsonConvert.DeserializeObject<RootVkFriends>(responseFromServer);
                return responseData;
            }
        }
    }

    public class VkOAuthAccessJSON
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string user_id { get; set; }
    }

    public class REsponseVkUserInfo
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public bool is_closed { get; set; }
        public bool can_access_closed { get; set; }
    }

    public class RootVkUserInfo
    {
        public List<REsponseVkUserInfo> response { get; set; }
    }


    public class ItemVkFriends
    {
        public int id;
        public string first_name;
        public string last_name;
        public bool is_closed;
        public bool can_access_closed;
        public string photo_100;
        public int online;
        public string track_code;
    }

    public class ResponseVkFrineds
    {
        public int count;
        public List<ItemVkFriends> items;
    }

    public class RootVkFriends
    {
        public ResponseVkFrineds response;
    }

}