using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com.ORMModel
{
    public class TestRun
    {
        public TestRun()
        {
            this.TestRunGuid = Guid.NewGuid();
        }

        public enum TestRunStatusEnumeration {
            Passed = 0,
            Failed = 1,
            FailedPrereqs = 2,
        }
        
        public int TestRunId { get; set; }

        public Guid TestRunGuid { get; set; }
        public DateTime RunDate { get; set; }
        
        public TestRunStatusEnumeration RunStatus { get; set; }
        public string TestReportLink { get; set; }
        public string BugReportLink { get; set; }
        public DateTime ? RecordedBugDate { get; set; }
        public TimeSpan ? TimeToRun { get; set; }
        public int TestId { get; set; }
        public int RunId { get; set; }
        public Test ParentTest { get; set; }
        public Run ParentRun { get; set; }
        public string Comments { get; set; }
        public string RollingComments { get; set; }
    }
}