using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com.ORMModel.DTOs
{
    public class ApplicationDTO
    {
        public int ApplicationId { get; set; }

        public string ApplicationName { get; set; }

        public Guid ApplicationGuid { get; set; }

        public Guid ClientId { get; set; }

        public DateTime DateCreated { get; set; }
    }
}