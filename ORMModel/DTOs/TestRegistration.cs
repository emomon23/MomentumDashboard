using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com.ORMModel.DTOs
{
    public class TestRegistration
    {
        public string ApplicationId { get; set; }

        public string TestNumber { get; set; }

        public string TestFamily { get; set; }

        public string TestName { get; set; }

        public string TestDescription { get; set; }

        public DateTime? ETA { get; set; }
    }
}