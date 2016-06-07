using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com.ORMModel
{
    public class Test
    { 
        public enum TestDefinitionStatusEnumeration
        {
            UnderDevelopment,
            Active,
            Suspended,
            BuggyAndBackInDevelopment
        }

        public Test()
        {
            this.TestGuid = Guid.NewGuid();
            this.DateCreated = DateTime.Now;
            this.DateLastModified = DateTime.Now;
            
            this.Categories = new List<string>();
        }

        public int TestId { get; set; }

        public Guid TestGuid { get; set; }

        public string TestName { get; set; }

        public string TestNumber { get; set; }

        public string Description { get; set; }

        public TestDefinitionStatusEnumeration CurrentStatus { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? FirstActivated { get; set; }

        public DateTime? OriginalETA { get; set; }

        public DateTime? CurrentETA { get; set; }

        public DateTime DateLastModified { get; set; }

        public int ApplicationId { get; set; }

        public Application Parent { get; set; }

        public List<TestRun> TestRuns { get; set; }

        public string RollingComments { get; set; }

        public List<string> Categories { get; set; }

        public string TestFamily { get; set; }

        public virtual List<LaunchCommand> LaunchCommands { get; set; }
    }
}