using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com.ORMModel
{
    public class Run
    {
        
        public int RunId { get; set; }

        public Guid RunGuid { get; set; }

        public List<TestRun> TestRan { get; set; }

        public DateTime DateStarted { get; set; }

        public DateTime DateEnded { get; set; }
    }
}