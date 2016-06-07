using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Test.IEmosoft.com.ORMModel;
using Test.IEmosoft.com.ORMModel.DTOs;
using Test.IEmosoft.com.BusinessLogic;

namespace Test.IEmosoft.com.Controllers
{
    public class SecurityController : ApiController
    {
        SecurityManager security = new SecurityManager();

        [HttpPost]
        public AuthenticationResult Authenticate(AuthenticationDTO user)
        {
            return security.Authenticate(user);
        }

        [HttpPost]
        public void RegisterAdminUser(AuthenticationDTO authDTO)
        {
            security.RegisterAdminUser(authDTO);
        }

        [HttpPost]
        public void ChangeUserPassword(ChangePasswordDTO chg)
        {
            security.ChangePassword(chg.UserId, chg.NewPassword);
        }

        [HttpPost]
        public void RegisterClientUser(UserDTO userDTO)
        {
            security.RegisterClientUser(userDTO);
        }
    }
}