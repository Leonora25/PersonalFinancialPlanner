using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proba_proekt
{
    public class UserRepository
    {
        public List<User> users;

        public UserRepository()
        {
            users = LoadAllUsers();
        }

        private List<User> LoadAllUsers()
        {
            var usersDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users");
            var userFiles = Directory.GetFiles(usersDirectory, "*.json");

            var users = new List<User>();
            foreach (var file in userFiles)
            {
                var json = File.ReadAllText(file);
                var user = JsonConvert.DeserializeObject<User>(json);
                users.Add(user);
            }

            return users;
        }

        public User FindUserByUsername(string username)
        {
            return users.Find(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public void AddUser(User user)
        {
            users.Add(user);
            SaveUserData(user);
        }

        public void SaveUserData(User user)
        {
            var json = JsonConvert.SerializeObject(user, Formatting.Indented);
            File.WriteAllText(GetUserFilePath(user.Username), json);
        }

        private string GetUserFilePath(string username)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users", $"{username}.json");
        }
    }
}
