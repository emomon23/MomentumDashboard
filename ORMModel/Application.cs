using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com.ORMModel
{
    public class Application
    {
        public Application()
        {
            this.ApplicationGuid = Guid.NewGuid();
            this.DateCreated = DateTime.Now;
        }
        public int ApplicationId { get; set; }

        public string ApplicationName { get; set; }

        public Guid ApplicationGuid { get; set; }

        public int ClientId { get; set; }

        public DateTime DateCreated { get; set; }

        public List<Test> Tests { get; set; }

    }
}