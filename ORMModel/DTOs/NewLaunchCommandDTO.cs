using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com.ORMModel.DTOs
{
    public class NewLaunchCommandDTO
    {
        public NewLaunchCommandDTO()
        {
            DontCreateDuplicated = true;
        }

        public Guid TestGuid { get; set; } 

        public string Parameters { get; set; }

        public string LaunchedByUserName { get; set; }

        public Guid LaunchedByUserGuid { get; set; }

        public DateTime? ScheduledDate { get; set; }

        public bool DontCreateDuplicated { get; set; }

    }
}