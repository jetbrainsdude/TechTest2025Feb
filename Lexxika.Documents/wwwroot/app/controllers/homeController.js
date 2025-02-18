'use strict';
app.controller('HomeController', [
    '$scope', '$state', '$uibModal', 'apiService', 'authService', 'sessionService',
    function ($scope, $state, $uibModal, apiService, authService, sessionService) {

        // check for failure message
        if (sessionService.state.accessTokenExpired) {
            $scope.isFailure = true;
            $scope.failMessage = 'Sorry...Your Session Timed Out. Please Login Again.';
        } else {
            $scope.isFailure = sessionService.state.isFailure;
            $scope.failMessage = sessionService.state.failMessage;
        }

        // nested navigation
        $scope.navigateLogin = function() {
            $state.go('home.login');
        };

        // login
        $scope.doLogin = function(login) {

            authService.logOut();

            apiService.doLogin(login).then(function(response) {

                        // clear the error message
                        $scope.apiMessage = '';

                        // store the access token (for use in interceptor service)
                        authService.userData.token = response.data.token;

                        // retrieve user data from webAPI
                        apiService.GetData().then(function(response) {

                                authService.userData.isAdmin = response.data.isAdmin;
                                authService.userData.userName = response.data.userName;

                            $state.go('dashBoard');
                            },
                            function(response) {

                                authService.logOut();

                                if (!response.data.Message) {
                                    $scope.apiMessage = 'Login failed.';
                                } else {
                                    $scope.apiMessage = 'Login failed: ' + response.data.Message;
                                }
                            });
                    },
                    function(response) {

                        authService.logOut();

                        if (!response.data.Message) {
                            $scope.apiMessage = 'Login failed.';
                        } else {
                            $scope.apiMessage = 'Login failed: ' + response.data.Message;
                        }
                    })
                .catch(function (error) {
                    if (error && error.Message) {
                        $scope.apiMessage = error.Message;
                    }
                });
        };
    }
]);