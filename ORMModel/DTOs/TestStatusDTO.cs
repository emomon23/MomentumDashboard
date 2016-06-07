using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com.ORMModel.DTOs
{
    public class TestStatusDTO
    {
        public string ApplicationId { get; set; }
        public string TestNumber { get; set; }
        public int Status { get; set; }
        public string FTPPath { get; set; }
        public DateTime RunDate { get; set; }
        public TimeSpan TestTime { get; set; }
        public string TestResultString { get; set; }
        public string RunId { get; set; }

        public string StripOfImagePathString { get; set; }

    }
}
