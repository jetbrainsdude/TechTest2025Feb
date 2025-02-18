'use strict';

// ========================================================================
// allows confirm password to be compared with password
var compareTo = function () {
    return {
        require: 'ngModel',
        scope: {
            otherModelValue: '=compareTo'
        },
        link: function (scope, element, attributes, ngModel) {

            ngModel.$validators.compareTo = function (modelValue) {
                return modelValue === scope.otherModelValue;
            };

            scope.$watch('otherModelValue', function () {
                ngModel.$validate();
            });
        }
    };
};

app.directive('compareTo', compareTo);

// ========================================================================
// sets $scope.httpInProgress to true during $http action - great for hourglass display
app.directive('httpactivity', ['$http', function ($http) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {

            try {

                scope.isHttpActivity = function() {
                    return $http.pendingRequests.length > 0;
                };

                scope.$watch(scope.isHttpActivity, function(value) {
                    if (value) {

                        // -----------------------
                        // http action in progress
                        // -----------------------
                        scope.httpInProgress = true;

                    } else {

                        // -----------------------
                        // http action completed
                        // -----------------------
                        scope.httpInProgress = false;

                    }
                });

            } finally {
                scope.httpInProgress = false;
            }
        }
    };
}]);

