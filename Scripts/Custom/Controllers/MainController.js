//Dummy data removed


angular.module("app")
    .controller("mainCTRL", function ($scope, $interval, localServer) {
        $scope.data = {};
        $scope.data.user = { isAuthenticated: false };
        $scope.data.newTest = {};
        $scope.modalFlags = {};

        //Set up the callback functions for the localServer
        localServer.clientListUpdatedCallBack = clientListUpdatedEvent;
        localServer.applicationListUpdatedCallback = applicationListUpdatedEvent;
        localServer.testHistoryUpdatedCallBack = applicationTestHistoryUpdatedEvent;
        localServer.errorCallback = errorMessageChangeEvent;
        localServer.requirePasswordChange = requirePasswordChange;

        $scope.authenticate = function () {
            var user = $scope.data.user;
            localServer.authenticate(user);
        }

        $scope.getApplications = function (clientGuid) {
            localServer.currentClient = clientGuid;
            localServer.retrieveApplications(clientGuid);
        }

        $scope.promptForNewClient = function () {
            $scope.modalFlags.showNewClientDialog = true;
        }

        $scope.promptForNewApplication = function () {
            $scope.modalFlags.showNewApplicationDialog = true;
        }

        $scope.saveNewClient = function () {
            localServer.createNewClient($scope.data.newClient);
            $scope.modalFlags.showNewClientDialog = false;
        }

        $scope.saveNewApplication = function () {
            localServer.createNewApplication($scope.data.newApplicationName);
            $scope.modalFlags.showNewApplicationDialog = false;
        }
                
        $scope.showTestRegistrationDialog = function () {
            $scope.modalFlags.showTestRegistrationDialog = true;
        }

        $scope.saveNewUnregisteredTest = function () {
            var input = $scope.data.newTest;

            input.applicationId = localServer.currentApplication;
            localServer.createNewUnregisteredTest(input);
            $scope.modalFlags.showTestRegistrationDialog = false;
        }

        $scope.setCurrentTestRunHistory = function (runHistoryId) {
            var runItem = localServer.findTestRunHistoryItem(runHistoryId);
            $scope.data.currentTestRunHistoryItem = runItem
            $scope.data.bugPopupMenuText = runItem.bugReportLink == '' || runItem.bugReportLink == null ? 'Create Bug' : 'Edit Bug';
            $scope.data.commentPopupMenuText = runItem.comments == '' || runItem.comments == null ? 'Create Comment' : 'Edit Comment';
            $scope.data.viewBugMenuText = $scope.data.bugPopupMenuText == 'Edit Bug' ? 'View Bug' : '';
        }

        $scope.runItemBugClick = function () {
            $scope.modalFlags.editRunHistoryItem = true;
        }
              

        $scope.runItemCommentClick = function () {
            $scope.modalFlags.editRunHistoryItem = true;
        }

        $scope.updateRunHistoryItem = function () {
            localServer.updateRunHistoryItem($scope.data.currentTestRunHistoryItem);
            $scope.modalFlags.editRunHistoryItem = false;
        }
        
        $scope.runItemViewBugClick = function () {
            window.open($scope.data.currentTestRunHistoryItem.bugReportLink);
        }

        $scope.getTestHistory = function (applicationGuid) {
            localServer.currentApplication = applicationGuid;
            localServer.retrieveTestHistory();
        }

        $scope.deleteTest = function (testNumber) {
            localServer.deleteTest(testNumber);
        }


        $scope.executeTest = function (testNumber) {
            localServer.launchTest(testNumber, function (result) {
                $scopoe.data.errorMessage = result.message;
            });
        }

        $scope.changePassword = function () {
            var confirm = $scope.data.confirmNewPassword;
            var newPwd = $scope.data.newPassword;

            $scope.newPasswordValidationMessage = '';
            if (confirm == newPwd) {
                localServer.changePassword(newPwd);
                $scope.showChangePwd = false;
            }
            else {
                $scope.newPasswordValidationMessage = "Password do not match";
            }
        }

        $scope.showNewClientUserDialog = function () {
            $scope.modalFlags.showNewClientUserDialog = true;
        }

        $scope.saveNewClientUser = function () {
            localServer.createNewClientUser($scope.data.newClientUser);
            $scope.modalFlags.showNewClientUserDialog = false;
        }

        self.capital_D_Is_Pressed = false;
        self.capital_R_Is_Pressed = false;

        $scope.keyPress = function (event) {
            if (event.keyCode == 68) {
                 self.capital_D_Is_Pressed = true;
            }
            else if (event.keyCode == 82) {
                self.capital_R_Is_Pressed = true;
            }

        }


        $scope.keyUp = function (event) {
            if (event.keyCode == 68 || event.keyCode == 100) {
               self.capital_D_Is_Pressed = false;
            }
            else if (event.keyCode == 82 || event.keyCode == 114) {
                self.capital_R_Is_Pressed = false;
            }
        }

        $scope.testHistoryKeyClick = function (testNumber) {
            if (self.capital_D_Is_Pressed) {
                if (confirm("Delete Test " + testNumber + ".  Are you sure?")) {
                    $scope.deleteTest(testNumber);
                }
            }

            if (self.capital_R_Is_Pressed) {
                $scope.executeTest(testNumber);
            }
        }

        $scope.getRunHistoryClass = function (run) {
            /*
            Passed = 0,
            Failed = 1,
            FailedPrereqs = 2*/
            
            if (run.runStatus == 0) {
                return 'passed';
            }
            else if (run.runStatus == 1) {
                return 'failed';
            }
            else if (run.runStatus == 3){
                return 'failedPrereqs';
            }
            else {
                return 'underDevelopment';
            }
       }

       

        //*** EVENT HANDLERS *****
        //If the client list or application list is updated (eg. SignalR, the local server will notify us)
        function clientListUpdatedEvent(newClientList) {
            $scope.data.clients = newClientList;
        }

        function applicationListUpdatedEvent(newApplicationsList) {
            $scope.data.applications = newApplicationsList;
        }

        function applicationTestHistoryUpdatedEvent(applicationHistory) {
            var maxSize = 7;

            applicationHistory.testFamilies.forEach(function (testFamily) {
                testFamily.tests.forEach(function (testHistory) {
                    if (testHistory.runHistory.length > maxSize) {
                        testHistory.runHistory.splice(maxSize);
                    }

                    var loopCount = maxSize - testHistory.runHistory.length;
                    for (var i = 0; i < loopCount; i++) {
                        testHistory.runHistory.push({ displayTest: '' });
                    }
                });
            });

            $scope.data.appHistory = applicationHistory;
        }

        function errorMessageChangeEvent(errMessage) {
            $scope.data.errorMessage = errMessage;
            $scope.errorOpacity = 1;

            $interval(function () {
                $interval(function () {
                    var newOpacity = $scope.errorOpacity - .05;
                    $scope.errorOpacity = newOpacity;
                }, 100, 30);
            }, 2000, 1);
        }

        function requirePasswordChange() {
            $scope.modalFlags.showChangePwd = true;
        }
    });