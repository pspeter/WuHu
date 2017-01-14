using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using WuHu.Domain;

namespace WuHu.BL
{
    public interface IUserManager
    {
        Player FindUser(string username, string password);
    }
}
