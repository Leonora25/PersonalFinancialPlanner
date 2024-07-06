using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proba_proekt
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public UserData UserData { get; set; }


        public User(string username,string password) 
        {
            Username = username;
            Password = password;
            UserData=new UserData();
            
        }
       

    }
}
