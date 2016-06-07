using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com.ORMModel
{
    public class Client
    {
        public Client()
        {
            this.ClientGuid = Guid.NewGuid();
        }

        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public Guid ClientGuid { get; set; }
        public List<Application> Applications { get; set; }
        public List<User> Users { get; set; }
       
    }
}