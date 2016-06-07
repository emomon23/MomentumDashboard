angular.module('app')
  .constant('loginURL', 'api/Security/Authenticate')
  .constant('createClientUserURL', 'api/Security/RegisterClientUser')
  .constant('updateRunHistoryItemURL', 'api/Tests/UpdateTestRunHistory')
  .constant('getTestHistoryURL', 'api/Tests/GetApplicationTestHistory/')
  .constant('getClientListURL', 'api/Tests/GetClientList')
  .constant('getAppListURL', 'api/Tests/GetApplicationList/')
  .constant('registerClientURL', 'api/Tests/RegisterClient')
  .constant('registerApplicationURL', 'api/Tests/RegisterApplication')
  .constant('changePasswordURL', 'api/Security/ChangeUserPassword')
  .constant('deleteTestUrl', 'api/Tests/DeleteTest')
  .constant('registerTestUnderDevelopmentURL', 'api/Tests/RegisterTestUnderDevelopment')
  .constant('launchATestURL', 'api/Tests/LaunchATest')
  .factory('localServer', function ($http, $location, $interval, loginURL, launchATestURL,  deleteTestUrl, getTestHistoryURL, getClientListURL, getAppListURL, registerClientURL, registerApplicationURL, registerTestUnderDevelopmentURL, changePasswordURL, updateRunHistoryItemURL, createClientUserURL) {
      var accountLockedEnum = 0;
      var isAdminUserEnum = 1;
      var isClientUserEnum = 2;
      var invalidCredentialsEnum = 3;

      function localServerConstructor() {
          var self = this;
          
          //*** PROPERTIES ***
          self.clients = [];
          self.applications = [];
          self.errorMessage = '';
          
          //*** METHODS (PUBLIC) ***
          self.authenticate = function (user) {

              user.isAuthenticated = false;
              user.isAdmin = false;
              user.invalidCredentials = false;
              user.accountIsLocked = false;

              httpPost(loginURL, user, null, function (authenticationResult) {
                      var status = authenticationResult.authentiationStatus;
                      user.userId = authenticationResult.userId;
                      self.user = user;

                      if (status == isAdminUserEnum) {
                          user.isAdmin = true;
                          user.isAuthenticated = true;
                         
                          getClients();
                       
                      }
                      else if (status == isClientUserEnum) {
                          user.isAuthenticated = true;
                          self.currentClient = authenticationResult.clientId;
                      
                          if (authenticationResult.mustChangePassword) {
                              self.requirePasswordChange();
                          }

                          getApplications();
                     }
                      else {
                          user.invalidCredentials = status == invalidCredentialsEnum;
                          user.accountIsLocked = status == accountLockedEnum;
                          self.errorMessage = user.accountIsLocked ? "Your accuont is locked." : "Invalid username or password";
                          self.errorCallback(self.errorMessage);
                      }
                      
                  });
          }

          self.launchTest = function (testGuuid, callback) {
              var data = { testGuid: testGuuid, launchedByUserGuid: self.user.userId, launchedByUserName: self.user.userName, dontCreateDuplicate: true };
              httpPost(launchATestURL, data, null, callback);
          }

          self.deleteTest = function (testNumber) {
              var data = { value: testNumber };

              httpPost(deleteTestUrl, data, null, null);
          }

          self.changePassword = function (newPwd) {
              var data = { userId: self.user.userId, newPassword: newPwd };
              httpPost(changePasswordURL, data, null, null);
          }

          self.retrieveApplications = function (clientGuid) {
              self.currentClient = clientGuid;
              getApplications();
          }

          self.retrieveTestHistory = function (doNotRedirect) {
              var redirect = doNotRedirect ? '' : '/testHistory';

              httpGet(getTestHistoryURL, localServer.currentApplication.toString(), redirect, function (getResult) {
                  self.testHistory = getResult;
                  if (self.testHistoryUpdatedCallBack) {
                      self.testHistoryUpdatedCallBack(getResult);
                  }
              });
          }

          self.updateRunHistoryItem = function (runHistoryItem) {
              httpPost(updateRunHistoryItemURL, runHistoryItem, null, function () {
                 
              });
          }
        
          self.findTestRunHistoryItem = function (testRunId) {
              var result = undefined;

              for (var t = 0; t < self.testHistory.testFamilies.length; t++) {
                  var family = self.testHistory.testFamilies[t];

                  for (var i = 0; i < family.tests.length; i++) {
                      var test = family.tests[i];

                      for (var r = 0; r < test.runHistory.length; r++) {
                          if (test.runHistory[r].testRunId == testRunId) {
                              result = test.runHistory[r];
                              break;
                          }
                      }

                      if (result != null) {
                          break;
                      }
                  }

                  if (result != null) {
                      break;
                  }
              }

              return result;
          }
          
          self.createNewClientUser = function (newUser) {
              newUser.clientId = self.currentClient;
              httpPost(createClientUserURL, newUser, null, function () {
                  newUser = {};
              });
          }

          self.createNewClient = function (newClient) {
              httpPost(registerClientURL, newClient, null, function () {
                  getClients();
              });
          }

          self.createNewApplication = function (newAppName) {
              var data = { clientId: self.currentClient, applicationName: newAppName };
              httpPost(registerApplicationURL, data, null, function () {
                  getApplications();
              });
          }

          self.createNewUnregisteredTest = function (newTest) {
              httpPost(registerTestUnderDevelopmentURL, newTest, null, function () {
                  self.retrieveTestHistory(self.currentApplication);
              });
          }

          //** PRIVATE FUNCTIONS ****
          function getClients(doNotRedirect) {
              var redirect = doNotRedirect ? null : '/clients';

              httpGet(getClientListURL, true, redirect, function (getClientsResult) {
                  self.clients = getClientsResult;
                  if (self.clientListUpdatedCallBack) {
                      self.clientListUpdatedCallBack(getClientsResult);
                  }
              });
          }

          function getApplications(doNotRedirect) {
              var redirect = doNotRedirect ? null : '/applications';

              httpGet(getAppListURL, self.currentClient, redirect, function (getAppResultList) {
                  if (self.applications.length != getAppResultList.length) {
                      self.applications = getAppResultList;
                      if (self.applicationListUpdatedCallback) {
                          self.applicationListUpdatedCallback(getAppResultList);
                      }
                  }
              });
          }

          function httpGet(getUrl, data, redirectOnSuccess, callBack) {
              $http({
                  url: getUrl,
                  method: 'GET',
                  params: {
                      'request.userId': self.user.userId,
                      'request.payLoad': data
                  }
              })
                .success(function (getResults) {
                    if (callBack) {
                        callBack(getResults);
                    }

                    if (redirectOnSuccess) {
                        $location.path(redirectOnSuccess);
                    }
                })
                .error(function (error) {
                    self.errorMessage = error.message;
                    self.errorCallback(error.message);
                })
                .finally(function () {
                    //locationService.path('/complete');
                });
          }

          function httpPost(url, data, redirectOnSuccess, callBack) {
              $http.post(url, data)
                 .success(function (postResult) {
                     if (callBack) {
                         callBack(postResult);
                     }

                     if (redirectOnSuccess) {
                         $location.path(redirectOnSuccess);
                     }

                 })
                 .error(function (error) {
                     self.errorMessage = error.message;
                     self.errorCallback(error.message);
                 })
                 .finally(function () {

                 });
          }

          //Polling
          $(function () {
              $interval(function () {
                  if (self.user) {
                      if (self.user.isAdmin) {
                          getClients(true);
                      }

                      if (self.currentClient) {
                          getApplications(true);
                      }

                      if (localServer.currentApplication) {
                          self.retrieveTestHistory(true);
                      }
                  }

              }, 10000);
          });
      }

      var localServer = new localServerConstructor();
      return localServer;
  });