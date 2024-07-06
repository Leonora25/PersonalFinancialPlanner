using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

namespace proba_proekt
{
    public class UserDataManager
    {
        private static string GetUserFilePath(string username)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users", $"{username}.json");
        }

        public static void SaveUserData(User user)
        {
            string filePath = GetUserFilePath(user.Username);
            string json = JsonConvert.SerializeObject(user, Newtonsoft.Json.Formatting.Indented);

            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            File.WriteAllText(filePath, json);
        }

        public static User LoadUserData(string username)
        {
            string filePath = GetUserFilePath(username);
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<User>(json);
            }
            return null;
        }
    }
}
