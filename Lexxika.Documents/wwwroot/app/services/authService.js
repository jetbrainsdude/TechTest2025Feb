'use strict';
app.service('authService', [
    'sessionService', '$injector', function(sessionService, $injector) {

        // initialise factory
        var authServiceFactory = {};

        var userData = {
            isAdmin: false,
            userName: '',
            token: null
        };

        // ================================================================
        // Log out
        var logOut = function() {

            userData.isAdmin = false;
            userData.userName = '';
            userData.token = null;

            sessionService.clear();
        };

        // ======================
        // refresh authority data
        var refresh = function(failMessage, refreshReturn, route) {

            // retrieve apiService manually to avoid circular reference error on $injector
            var apiService = $injector.get('apiService');

            apiService.GetData().then(function(response) {

                    userData.isAdmin = response.data.IsAdmin;
                    userData.userName = response.data.UserName;

                    // ======================================================
                    // Navigation refreshed automatically on new route 
                    // otherwise divert to refresh page and back to refresh
                    // display of new session scope variables.
                    // ======================================================

                    var $state = $injector.get('$state');
                    if (route) {
                        $state.go(route);
                    } else {
                        sessionService.state.refreshReturn = refreshReturn;
                        $state.go('refresh');
                    }
                },
                function(response) {

                    logOut();
                    sessionService.state.isFailure = true;
                    sessionService.state.failMessage = failMessage;

                    var $state = $injector.get('$state');
                    $state.go('home.login');
                });
        };

        // ================================================================
        // finalise factory
        authServiceFactory.logOut = logOut;
        authServiceFactory.userData = userData;
        authServiceFactory.refresh = refresh;

        return authServiceFactory;
    }
]);