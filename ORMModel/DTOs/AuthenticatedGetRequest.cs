using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com.ORMModel.DTOs
{
    public class AuthenticatedGetRequest
    {
        public Guid UserId { get; set; }
        public object Payload { get; set; }
    }
}