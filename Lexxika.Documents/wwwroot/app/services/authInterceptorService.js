'use strict';
app.factory('authInterceptorService', [
    '$q', 'authService', '$injector', 'sessionService',  function($q, authService, $injector, sessionService) {

        var authInterceptorServiceFactory = {};

        var request = function(config) {

            config.headers = config.headers || {};

            var token = authService.userData.token;
            if (token !== null) {
                config.headers.Authorization = 'Bearer ' + token;
            }

            return config;
        };

        // redirect to home if authorisation fails with 401 or 403
        var responseError = function(response) {
            if ([401, 403].indexOf(response.status) >= 0) {

                authService.logOut();

                // retrieve $state manually to avoid circular reference error on $injector
                var $state = $injector.get('$state');

                sessionService.state.accessTokenExpired = true;
                $state.go('home.login');
                return response;
            } else {
                return $q.reject(response);
            }
        };

        authInterceptorServiceFactory.request = request;
        authInterceptorServiceFactory.responseError = responseError;

        return authInterceptorServiceFactory;
    }
]);