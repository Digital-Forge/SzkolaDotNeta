using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public partial interface IAuthorizationService
    {
        interface ILoginModel
        {
            string Username { get; set; }
            string Password { get; set; }
            bool RememberMe { get; set; }
        }
    }
}
