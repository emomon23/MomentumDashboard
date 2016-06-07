using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com.ORMModel.DTOs
{
    public class TestRunHistory
    {
        public Guid TestRunId { get; set; }
        public string TestReportLink { get; set; }
        public string BugReportLink { get; set; }

        public DateTime ? RecordedBugDate { get; set; }

        public DateTime DateRan { get; set; }

        public TimeSpan ? TimeToRun { get; set; }
                
        public string DisplayTest
        {
            get
            {
                return DateRan.ToString("MM/dd/yy HH:mm");
            }
            set { }
        }
        public string Comments { get; set; }
        public TestRun.TestRunStatusEnumeration RunStatus { get; set; }
    }
}