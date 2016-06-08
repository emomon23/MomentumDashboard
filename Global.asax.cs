using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Data;
using System.Data.Entity;
using Newtonsoft.Json.Serialization;
using System.Web.Routing;
using Test.IEmosoft.com.BusinessLogic;

namespace Test.IEmosoft.com
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
             WebApiConfig.Register(GlobalConfiguration.Configuration);

	     GlobalConfiguration.Configuration
	          .Formatters
        	  .JsonFormatter
	          .SerializerSettings
        	  .ContractResolver = new CamelCasePropertyNamesContractResolver();

         Database.SetInitializer<TestDBContext>(null);

         //Create a new SaltVector file.
         SaltVector saltVector = new SaltVector();
       
        }

        protected void Session_Start()
        {
            SaltVector saltVector = new SaltVector();
        }
        
    }
}