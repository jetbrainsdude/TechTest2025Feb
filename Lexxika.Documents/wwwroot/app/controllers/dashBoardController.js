'use strict';

// =============================================================================================
// =============================================================================================
app.controller('DashBoardController', [
    '$scope', '$state', 'apiService', 'authService', 'sessionService',
    function($scope, $state, apiService, authService, sessionService) {

        $scope.isAdmin = authService.userData.isAdmin;
        $scope.user = authService.userData.userName;

        $scope.listDocuments = function() {
            apiService.listDocuments()
                .success(function(data) {

                    $scope.apiMessage = '';

                    $scope.dataPresent = false;
                    if (data.length > 0) {
                        $scope.dataPresent = true;
                        $scope.documents = data;
                    }
                    else {
                        $scope.dataPresent = false;
                        $scope.documents = null;
                    }
                })
                .error(function(error) {
                    $scope.apiMessage = error.Message;
                })
                .catch(function(error) {
                    if (error && error.Message) {
                        $scope.apiMessage = error.Message;
                    }
                });
        };

        // invoke listDocuments() to show all documents
        $scope.listDocuments();

        // new document
        $scope.newDocument = function() {
            sessionService.state.isNewDocument = true;
            $state.go('dashBoardMaintain');
        };

        // delete document
        $scope.deleteDocument = function(document) {

            apiService.deleteDocument(document.id)
                .success(function(data) {

                    $scope.apiMessage = '';
                    $scope.listDocuments();

                })
                .error(function(error) {
                    $scope.apiMessage = error.Message;
                })
                .catch(function(error) {
                    if (error && error.Message) {
                        $scope.apiMessage = error.Message;
                    }
                });
        };

        // update document
        $scope.updateDocument = function(document) {

            // set session state
            sessionService.state.isNewDocument = false;
            sessionService.state.document = document;
            $state.go('dashBoardMaintain');

        };
    }
]);

// =============================================================================================
// =============================================================================================
app.controller('DashBoardMaintainController', [
    '$scope', '$state', 'apiService', 'authService', 'sessionService',
    function ($scope, $state, apiService, authService, sessionService) {

        $scope.LoggedInUser = authService.userData.userName;
        $scope.isNewDocument = sessionService.state.isNewDocument;

        if ($scope.isNewDocument) {
            $scope.update = {};
            $scope.update.user = authService.userData.userName;
            $scope.update.title = '';
            $scope.update.translationText = '';
        } else {
            // initialise for edit
            $scope.update = {};
            $scope.update.user = sessionService.state.document.user;
            $scope.update.title = sessionService.state.document.title;
            $scope.update.translationText = sessionService.state.document.translationText;
        }

        // new document
        $scope.newDocument = function(update) {

            $scope.apiMessage = '';

            var documentInPlay = {};
            documentInPlay.id = "dummy_GUID"; // to pass server side validation
            documentInPlay.user = update.user;
            documentInPlay.title = update.title;
            documentInPlay.translationText = update.translationText;

            apiService.addDocument(documentInPlay)
                .success(function(data) {

                    $state.go('dashBoard');

                })
                .error(function(error) {
                    $scope.apiMessage = error.Message;
                })
                .catch(function(error) {
                    if (error && error.Message) {
                        $scope.apiMessage = error.Message;
                    }
                });
        };

        // edit document
        $scope.updateDocument = function(update) {

            $scope.apiMessage = '';

            var updatedDocument = {};
            updatedDocument.id = sessionService.state.document.id;
            updatedDocument.title = update.title;
            updatedDocument.user = update.user;
            updatedDocument.translationText = update.translationText;

            apiService.updateDocument(updatedDocument)
                .success(function(data) {

                    $state.go('dashBoard');

                })
                .error(function(error) {
                    $scope.apiMessage = error.Message;
                });
        };

        // cancel update
        $scope.cancel = function() {
            $state.go('dashBoard');
        };
    }
]);