using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com.ORMModel
{
    public class Administrator
    {
        public Administrator()
        {
            this.AdministratorId = Guid.NewGuid();
        }

        public Guid AdministratorId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}