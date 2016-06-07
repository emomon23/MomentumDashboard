using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com.ORMModel.DTOs
{
    public class ApplicationHistory
    {
        public ApplicationHistory()
        {
            this.TestFamilies = new List<TestFamily>();
        }
        public Guid Applicationid { get; set; }
        public string ApplicatioName { get; set; }
       
        public List<TestFamily> TestFamilies { get; set; }

        public void Add(TestHistory testHistory)
        {
            TestFamily testFamily = this.TestFamilies.FirstOrDefault(tf => tf.FamilyText == testHistory.TestFamily);
            if (testFamily == null)
            {
                testFamily = new TestFamily() { FamilyText = testHistory.TestFamily };
                this.TestFamilies.Add(testFamily);
            }

            testFamily.Tests.Add(testHistory);
        }
    }

    public class TestFamily
    {
        public TestFamily()
        {
            this.Tests = new List<TestHistory>();
        }
        public string FamilyText { get; set; }
        public List<TestHistory> Tests { get; set; }
    }
}