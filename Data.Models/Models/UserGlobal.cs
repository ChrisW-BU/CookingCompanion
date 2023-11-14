using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Models
{
    public class UserGlobal
    {
        public int Id { get; set; }
        public bool LoggedIn { get; set; }
        public string UserName { get; set; } = string.Empty;

        public bool IsAdmin { get; set; }

        public void SetUserInfo(User user)
        {
            Id = user.Id;
            UserName = user.Name;
            IsAdmin = user.IsAdmin;
            LoggedIn = true;
        }

        public void ResetUserInfo()
        {
            Id = 0;
            UserName = "";
            IsAdmin = false;
            LoggedIn = false;
        }
    }
}
