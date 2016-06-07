using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Test.IEmosoft.com.ORMModel;
using Test.IEmosoft.com.ORMModel.DTOs;

namespace Test.IEmosoft.com.BusinessLogic
{
    public class TestManager
    {
        private TestDBContext dbContext = new TestDBContext();
        public string RegisterTest(TestRegistration testRegistration)
        {
            Test.IEmosoft.com.ORMModel.Test test = null;

            Application application = this.RetrieveApplication(testRegistration.ApplicationId);
            if (application == null)
            {
                application = new Application()
                {
                    ApplicationName = string.Format("App created for test '{1} {0}'", testRegistration.TestNumber, testRegistration.TestName)
                };

                dbContext.Applications.Add(application);
            }
            else
            {
                 test = dbContext.Tests.FirstOrDefault(t => t.TestNumber == testRegistration.TestNumber && t.ApplicationId == application.ApplicationId);
            }

            if (test == null)
            {
                test = new ORMModel.Test()
                {
                    CurrentStatus = ORMModel.Test.TestDefinitionStatusEnumeration.UnderDevelopment,
                    Description = testRegistration.TestDescription,
                    TestName = testRegistration.TestName,
                    TestNumber = testRegistration.TestNumber,
                    TestFamily = testRegistration.TestFamily,
                    ApplicationId = application.ApplicationId,
                    OriginalETA = testRegistration.ETA
                };

                dbContext.Tests.Add(test);
                dbContext.SaveChanges();
            }
            else
            {
                if (test.Description != testRegistration.TestDescription || test.TestFamily != testRegistration.TestFamily || test.TestName != test.TestName)
                {
                    test.Description = testRegistration.TestDescription;
                    test.TestFamily = testRegistration.TestFamily;
                    test.TestName = testRegistration.TestName;
                    test.DateLastModified = DateTime.Now;

                    if (testRegistration.ETA != test.OriginalETA)
                    {
                        test.CurrentETA = testRegistration.ETA;
                    }

                    dbContext.SaveChanges();
                }
            }

            return test.TestGuid.ToString();
        }
        public ApplicationHistory RetrieveTestHistory(Guid applicationId)
        {
            ApplicationHistory result = new ApplicationHistory();
            
            var application = dbContext.Applications.FirstOrDefault(a => a.ApplicationGuid == applicationId);
            if (application != null)
            {
                result = new ApplicationHistory()
                {
                    Applicationid = applicationId,
                    ApplicatioName = application.ApplicationName
                };

                var tests = dbContext.Tests.Where(t => t.ApplicationId == application.ApplicationId).ToList();
                var testRuns = (from tr in dbContext.TestRuns
                               join t in dbContext.Tests on tr.TestId equals t.TestId
                               where t.ApplicationId == application.ApplicationId
                               select tr
                               ).OrderByDescending(t => t.RunDate).ToList();

                foreach (var test in tests)
                {
                    TestHistory th = new TestHistory()
                    {
                        Comments = test.RollingComments,
                        IsUnderDevelopment = test.CurrentStatus == ORMModel.Test.TestDefinitionStatusEnumeration.UnderDevelopment,
                        TestDescription = test.Description,
                        TestName = test.TestName,
                        TestNumber = test.TestNumber,
                        TestId = test.TestGuid,
                        TestFamily = test.TestFamily,
                        ETA = test.CurrentETA.HasValue ? test.CurrentETA.Value.ToString("MM/dd/yyyy") : test.OriginalETA.HasValue ? test.OriginalETA.Value.ToString("MM/dd/yyyy") : ""
                    };

                    var runs = testRuns.Where(t => t.TestId == test.TestId);
                    foreach (var run in runs)
                    {
                        TestRunHistory runHistory = new TestRunHistory()
                        {
                            BugReportLink = run.BugReportLink,
                            RecordedBugDate = run.RecordedBugDate,
                            TestReportLink = run.TestReportLink,
                            Comments = run.Comments,
                            DateRan = run.RunDate,
                            RunStatus = run.RunStatus,
                            TestRunId = run.TestRunGuid
                        };

                        th.RunHistory.Add(runHistory);
                    }

                    result.Add(th);
                }
            }

            return result;
        }

        public PostResultDTO DeleteTest(string testNumber)
        {
            try
            {
                
                var test = dbContext.Tests.FirstOrDefault(t => t.TestNumber == testNumber);
                if (test != null)
                {
                    var testRuns = dbContext.TestRuns.Where(r => r.TestId == test.TestId);
                    if (testRuns != null && testRuns.Count() > 0)
                    {
                        dbContext.TestRuns.RemoveRange(testRuns);
                    }

                    dbContext.Tests.Remove(test);
                    dbContext.SaveChanges();
                  
                }

                return new PostResultDTO() { Successful = true };
            }
            catch (Exception exp)
            {
                return new PostResultDTO(){ Successful = false, ErrorMessage = exp.Message, ErrorType = "Exception"};
            }
        }

        public PostResultDTO RecordTestRun(TestStatusDTO dto)
        {
            string descriptor = "";

            try
            {
                var application = this.RetrieveApplication(dto.ApplicationId);
                if (application == null)
                {
                    throw new Exception("Unable to find application: " + dto.ApplicationId);
                }

                var test = dbContext.Tests.FirstOrDefault(t => t.TestNumber == dto.TestNumber && t.ApplicationId == application.ApplicationId);
                if (test == null)
                {
                    throw new Exception("Unable to find test number: " + dto.TestNumber);
                }

                descriptor = "Check if run already exists";
                Guid runId = Guid.Parse(dto.RunId);
                var run = dbContext.Runs.FirstOrDefault(r => r.RunGuid == runId);

                if (run == null)
                {
                    descriptor = "Create new Run record";
                    run = new Run()
                    {
                        DateStarted = DateTime.Now,
                        DateEnded = DateTime.Now,
                        RunGuid = runId
                    };
                    dbContext.Runs.Add(run);
                    dbContext.SaveChanges();

                    run = dbContext.Runs.FirstOrDefault(r => r.RunGuid == runId);
                }

                descriptor = "Create new TestRun";
                run.DateEnded = DateTime.Now;
                TestRun testRun = new TestRun()
                {
                    TestId = test.TestId,
                    RunId = run.RunId,
                    RunDate = dto.RunDate,
                    TestReportLink = "TestReports/" + dto.FTPPath,
                    TimeToRun = dto.TestTime,
                    RunStatus = (TestRun.TestRunStatusEnumeration)dto.Status,
                };
                dbContext.TestRuns.Add(testRun);

                if (test.CurrentStatus == ORMModel.Test.TestDefinitionStatusEnumeration.UnderDevelopment)
                {
                    test.CurrentStatus = ORMModel.Test.TestDefinitionStatusEnumeration.Active;
                    test.FirstActivated = DateTime.Now;
                }

                dbContext.SaveChanges();

                return new PostResultDTO() { Successful = true };
            }
            catch (Exception exp)
            {
                return new PostResultDTO() { Successful = false, ErrorMessage = exp.ToString(), ErrorType = exp.GetType().ToString(), Payload = descriptor };
            }
        }

        public void UpdateTestRunHistory(TestRunHistory historyItem)
        {
            var existingItem = dbContext.TestRuns.FirstOrDefault(r => r.TestRunGuid == historyItem.TestRunId);
            if (existingItem != null)
            {
                existingItem.Comments = historyItem.Comments;
                existingItem.BugReportLink = historyItem.BugReportLink;
                existingItem.TimeToRun = historyItem.TimeToRun;

                if (!string.IsNullOrEmpty(historyItem.BugReportLink) && existingItem.RecordedBugDate.HasValue == false)
                {
                    existingItem.RecordedBugDate = DateTime.Now;
                }

                dbContext.SaveChanges();
            }
        }
        public List<ApplicationDTO> RetrieveApplications(Guid clientId)
        {
            var result = from a in dbContext.Applications
                         join c in dbContext.Clients
                           on a.ClientId equals c.ClientId
                         where c.ClientGuid == clientId
                         select new ApplicationDTO
                         {
                             ClientId = clientId,
                             ApplicationGuid = a.ApplicationGuid,
                             ApplicationId = a.ApplicationId,
                             ApplicationName = a.ApplicationName,
                             DateCreated = a.DateCreated
                         };

            return result.ToList();
        }
        public List<ClientDTO> RetrieveClients()
        {
            var result = from c in dbContext.Clients
                         select new ClientDTO
                         {
                             ClientGuid = c.ClientGuid,
                             ClientId = c.ClientId,
                             ClientName = c.ClientName
                         };

            return result.ToList();
        }
        public RegisterClientResponse RegisterClient(ClientRegistration registration)
        {
            Client client = new Client()
            {
                ClientName = registration.ClientName
            };

            dbContext.Clients.Add(client);
            dbContext.SaveChanges();

            client = dbContext.Clients.FirstOrDefault(c => c.ClientGuid == client.ClientGuid);

            User user = new User()
            {
                ClientId = client.ClientId,
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                UserName = registration.UserName,
                Password = AES.Encrypt(registration.Password),
                MustChangePwd = true
            };

            dbContext.Users.Add(user);

            Application application = new Application()
            {
                ApplicationName = registration.ApplicationName,
                ClientId = client.ClientId
            };
            dbContext.Applications.Add(application);

            dbContext.SaveChanges();

            return new RegisterClientResponse()
            {
                ClientId = client.ClientGuid,
                ApplicationId = application.ApplicationGuid
            };
        }
        public Guid RegisterApplication(ApplicationRegistration applicationRegistration)
        {
            var client = dbContext.Clients.FirstOrDefault(c => c.ClientGuid == applicationRegistration.ClientId);
            if (client == null)
            {
                throw new Exception("Unable to find clientId " + applicationRegistration.ClientId);
            }

            var application = new Application()
            {
                ClientId = client.ClientId,
                ApplicationName = applicationRegistration.ApplicationName
            };

            dbContext.Applications.Add(application);
            dbContext.SaveChanges();

            return application.ApplicationGuid;
        }

        private Application RetrieveApplication(string applicationQueryParameter)
        {
            Application result;

            if (applicationQueryParameter.IsGuid())
            {
                Guid appGuid = applicationQueryParameter.ToGuid();
                result = dbContext.Applications.FirstOrDefault(a => a.ApplicationGuid == appGuid);
            }
            else if (applicationQueryParameter.IsNumeric())
            {
                int appId = applicationQueryParameter.ToInt();
                result = dbContext.Applications.FirstOrDefault(a => a.ApplicationId == appId);
            }
            else
            {
                result = dbContext.Applications.FirstOrDefault(a => a.ApplicationName == applicationQueryParameter);
            }

            return result;
        }
    }
}