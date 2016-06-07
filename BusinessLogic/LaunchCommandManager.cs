using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Test.IEmosoft.com.ORMModel;
using Test.IEmosoft.com.ORMModel.DTOs;

namespace Test.IEmosoft.com.BusinessLogic
{
    public class LaunchCommandManager
    {
        public TestDBContext dbContext = new TestDBContext();

        public bool CreateLaunchCommand(NewLaunchCommandDTO newLaunch)
        {
            if (newLaunch.DontCreateDuplicated && this.DoesAnUnexecutedLaunchCommandAlreadyExist(newLaunch.TestGuid))
            {
                return false;
            }

            LaunchCommand newCommand = new LaunchCommand()
            {
                DateCreated = DateTime.Now,
                TestId = this.GetTestId(newLaunch.TestGuid),
                CommandText = "LAUNCH",
                DateScheduled = newLaunch.ScheduledDate,
                LaunchedByUserGuuid = newLaunch.LaunchedByUserGuid,
                LaunchedByUserName = newLaunch.LaunchedByUserName,
                Parameters = newLaunch.Parameters,
            };

            dbContext.LaunchCommands.Add(newCommand);
            dbContext.SaveChanges();

            return true;
        }

        public List<LaunchCommandDTO> PickupLaunchCommandsToBeExecuted_CreatePickupKey(string appIdentifier)
        {
            int applicationId = this.GetApplicationId(appIdentifier);

            List<LaunchCommandDTO> result = new List<LaunchCommandDTO>();

            var apptests = (from tt in dbContext.Tests
                            join a in dbContext.Applications on tt.ApplicationId equals a.ApplicationId
                            join lc in dbContext.LaunchCommands on tt.TestId equals lc.TestId
                            where a.ApplicationId == applicationId
                            && (lc.PickupKey ?? "") == ""
                            select new { tt.TestNumber, tt.TestName, lc.CommandText, lc.LaunchedByUserGuuid, lc.LaunchedByUserName, lc.LuanchCommandId, lc.Parameters, lc.TestId }).ToList();

            string pickupKey = Guid.NewGuid().ToString();
            List<int> commandIdsList = new List<int>();

            foreach (var appTest in apptests)
            {
                commandIdsList.Add(appTest.LuanchCommandId);

                LaunchCommandDTO dto = new LaunchCommandDTO()
                {
                    CommandText = appTest.CommandText,
                    LaunchCommandId = appTest.LuanchCommandId,
                    LaunchedByUserGuuid = appTest.LaunchedByUserGuuid,
                    LaunchedByUserName = appTest.LaunchedByUserName,
                    Parameters = appTest.Parameters,
                    PickupKey = pickupKey,
                    TestName = appTest.TestName,
                    TestNumber = appTest.TestNumber
                };

                result.Add(dto);
            }

            SetPickupKeyForLaunchCommands(commandIdsList, pickupKey);

            return result;
        }

        public void ConfirmLaunchCommandsPickedUp(string pickupKey)
        {
            var launceCommands = dbContext.LaunchCommands.Where(l => l.PickupKey == pickupKey);
            DateTime now = DateTime.Now;

            foreach (var lc in launceCommands)
            {
                lc.DatePickedup = now;
            }

            dbContext.SaveChanges();
        }

        public void ConfirmTestExecution(int launchCommandId)
        {
            var launchCommand = dbContext.LaunchCommands.FirstOrDefault(lc => lc.LuanchCommandId == launchCommandId);

            if (launchCommand != null)
            {
                launchCommand.DateExecuted = DateTime.Now;
                dbContext.SaveChanges();
            }

        }

        private void SetPickupKeyForLaunchCommands(List<int> commandIds, string pickUpKey)
        {
            var lanuchCommands = from lc in dbContext.LaunchCommands
                                 where commandIds.Contains(lc.LuanchCommandId)
                                 select lc;

            foreach (var lc in lanuchCommands)
            {
                lc.PickupKey = pickUpKey;
            }

            dbContext.SaveChanges();
        }

        private bool DoesAnUnexecutedLaunchCommandAlreadyExist(Guid testGuid)
        {
            return false;
        }

        private int GetApplicationId(string appIdentifier)
        {
            int applicationId = appIdentifier.IsNumeric() ? appIdentifier.ToInt() : 0;

            if (applicationId == 0)
            {
                if (appIdentifier.IsGuid())
                {
                    Guid appGuid = appIdentifier.ToGuid();
                    var app = dbContext.Applications.FirstOrDefault(a => a.ApplicationGuid == appGuid);
                    applicationId = app == null ? 0 : app.ApplicationId;
                }
                else
                {
                    var app = dbContext.Applications.FirstOrDefault(a => a.ApplicationName == appIdentifier);
                    applicationId = app == null ? 0 : app.ApplicationId;
                }
            }

            return applicationId;
        }

        private int GetTestId(Guid testGuid)
        {
            var test = dbContext.Tests.FirstOrDefault(t => t.TestGuid == testGuid);

            if (test != null)
            {
                return test.TestId;
            }

            return 0;
        }
    }
}