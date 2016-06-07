using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com.ORMModel.DTOs
{
    public class ClientDTO
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public Guid ClientGuid { get; set; }
    }
}