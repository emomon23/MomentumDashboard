using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Test.IEmosoft.com.ORMModel;
using Test.IEmosoft.com.ORMModel.DTOs;
using Test.IEmosoft.com.BusinessLogic;

namespace Test.IEmosoft.com.Controllers
{
    public class TestsController : BaseTestController
    {
                
        [HttpGet]
        public ApplicationHistory GetApplicationTestHistory([FromUri] AuthenticatedGetRequest request)
        {
            base.CheckAuthorization(request);
            return testManager.RetrieveTestHistory(request.Payload.ToGuid());
        }

        [HttpGet]
        public List<ClientDTO> GetClientList([FromUri] AuthenticatedGetRequest request)
        {
            base.CheckAuthorization(request);
            return testManager.RetrieveClients();
        }

        [HttpGet]
        public List<ApplicationDTO> GetApplicationList([FromUri] AuthenticatedGetRequest request)
        {
            base.CheckAuthorization(request);
            return testManager.RetrieveApplications(request.Payload.ToGuid());
        }

        [HttpPost]
        public void UpdateTestRunHistory(TestRunHistory historyItem)
        {
            testManager.UpdateTestRunHistory(historyItem);
        }

        [HttpPost]
        public PostResultDTO LaunchATest(NewLaunchCommandDTO newLaunch)
        {
            PostResultDTO result = null;

            try {
                bool created = this.launchManager.CreateLaunchCommand(newLaunch);
                if (created)
                {
                    result = new PostResultDTO() { Successful = true, Payload = true, Message = "A Launch Command has been created, test in in the queue" };
                }
                else
                {
                    result = new PostResultDTO() { Successful = true, Payload = false, Message = "A duplicate launch command already exists, and has yet to be executed" };
                }
            }
            catch (Exception exp)
            {
                return new PostResultDTO() { Successful = false, ErrorMessage = exp.Message, Payload = exp.ToString() };
            }

            return result;
        }

        [HttpGet]
        public List<LaunchCommandDTO> RetrieveLaunchCommandsToExecute(string applicationId)
        {
            return launchManager.PickupLaunchCommandsToBeExecuted_CreatePickupKey(applicationId);
        }

        [HttpGet]
        public void ConfirmPickup(string pickupKey)
        {
            launchManager.ConfirmLaunchCommandsPickedUp(pickupKey);
        }

        [HttpPost]
        public void ConfirmExecution(int launchCommandId)
        {
            launchManager.ConfirmTestExecution(launchCommandId);
        }
        
        [HttpPost]
        public PostResultDTO RecordTestRun(TestStatusDTO dto)
        {
            FTPFileNameCleanup.CleanupAnyFiles();
            return testManager.RecordTestRun(dto);
        }

        [HttpPost]
        public PostResultDTO DeleteTest(WebAPIString testId)
        {
            return testManager.DeleteTest(testId.Value);
        }

        [HttpPost]
        public RegisterClientResponse RegisterClient(ClientRegistration clientRegistration)
        {
            return testManager.RegisterClient(clientRegistration);
        }

        [HttpPost]
        public Guid RegisterApplication(ApplicationRegistration applicationRegistration)
        {
            return testManager.RegisterApplication(applicationRegistration);
        }

        [HttpPost]
        public string RegisterTestUnderDevelopment(TestRegistration registration)
        {
            return testManager.RegisterTest(registration);
        }
    }
}