using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Test.IEmosoft.com.ORMModel;

namespace Test.IEmosoft.com.ORMModel.DTOs
{
    public class TestHistory
    {
        public TestHistory()
        {
            this.RunHistory = new List<TestRunHistory>();
            this.Categories = new List<string>();
        }
        public Guid TestId { get; set; }
        public string TestName { get; set; }
        public string TestNumber { get; set; }
        public string TestDescription { get; set; }
        public string Comments { get; set; }
        public bool IsUnderDevelopment { get; set; }

        public string ETA { get; set; }
        public List<string> Categories { get; set; }
        public string TestFamily { get; set; }

        public List<TestRunHistory> RunHistory { get; set; }
    }
}