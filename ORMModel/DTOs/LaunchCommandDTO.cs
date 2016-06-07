using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com.ORMModel.DTOs
{
    public class LaunchCommandDTO
    {
        public int LaunchCommandId { get; set; }

        public string TestNumber { get; set; }

        public string TestDescription { get; set; }

        public string TestName { get; set; }

        public string LaunchedByUserName { get; set; }

        public Guid LaunchedByUserGuuid { get; set; }

        public string CommandText { get; set; }

        public string Parameters { get; set; }
              
        public string PickupKey { get; set; }
  
    }
}