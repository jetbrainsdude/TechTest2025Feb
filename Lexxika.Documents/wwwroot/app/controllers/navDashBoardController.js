'use strict';
app.controller('NavDashBoardController', [
    '$scope', 'authService',
    function ($scope, authService) {

        $scope.isAdmin = authService.userData.isAdmin;
        $scope.userName = authService.userData.userName;

        $scope.logOut = function() {
            authService.logOut();
        };
    }
]);