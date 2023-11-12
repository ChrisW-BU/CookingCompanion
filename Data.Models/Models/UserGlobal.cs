using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Models
{
    public class UserGlobal
    {
        public bool LoggedIn { get; set; }
        public string UserName { get; set; } = string.Empty;

        public void SetUserInfo(User user)
        {
            UserName = user.Name;
        }
    }
}
