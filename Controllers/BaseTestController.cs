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
    public class BaseTestController : ApiController
    {
        protected TestManager testManager = new TestManager();
        protected LaunchCommandManager launchManager = new LaunchCommandManager();

        protected void CheckAuthorization(AuthenticatedGetRequest getRequest)
        {
            SecurityManager secManager = new SecurityManager();
            if (!secManager.UserExists(getRequest.UserId))
            {
                throw new Exception("Not authorized to call method");
            }
        }
    }
}