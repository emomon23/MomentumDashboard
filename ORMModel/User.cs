using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com.ORMModel
{
    public class User
    {
        public User()
        {
            this.UserGuid = Guid.NewGuid();
            this.DateCreated = DateTime.Now;
        }

        public int UserId { get; set; }
        public Guid UserGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public int ClientId {get;set;}
        public DateTime DateCreated { get; set; }
        public Client Parent { get; set; }
        public bool MustChangePwd { get; set; }
    }
}