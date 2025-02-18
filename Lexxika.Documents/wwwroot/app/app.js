'use strict';
var app = angular.module('LexxikaDashboardModule', ['ngMessages', 'ngAnimate', 'ui.bootstrap', 'LocalStorageModule', 'ui.router', 'ngSanitize']);

app.config(['$stateProvider', '$urlRouterProvider', function($stateProvider, $urlRouterProvider) {

    $urlRouterProvider.otherwise('/home/login');

    $stateProvider
        .state('home', {
            url: '/home',
            views: {
                'navigation': {
                    templateUrl: '/app/partials/navHome.html'
                },
                'content': {
                    templateUrl: '/app/partials/home.html',
                    controller: 'HomeController'
                }
            }
        })
        .state('home.login', {
            url: '/login',
            views: {
                'content': {
                    templateUrl: '/app/partials/nestedlogin.html',
                    controller: 'HomeController'
                }
            }
        })
        .state('dashBoard', {
            url: '/dashBoard',
            views: {
                'navigation': {
                    templateUrl: '/app/partials/navDashBoard.html',
                    controller: 'NavDashBoardController'

                },
                'content': {
                    templateUrl: '/app/partials/dashBoard.html',
                    controller: 'DashBoardController'
                }
            }
        })
        .state('dashBoardMaintain', {
            url: '/dashBoardMaintain',
            views: {
                'navigation': {
                    templateUrl: '/app/partials/navDashBoard.html',
                    controller: 'NavDashBoardController'

                },
                'content': {
                    templateUrl: '/app/partials/dashBoardMaintain.html',
                    controller: 'DashBoardMaintainController'
                }
            }
        });
}]);

app.constant('ngSettings', {
    apiServiceBaseUri: 'https://localhost:7089/'
    //apiServiceBaseUri: 'https://jrh.azurewebsites.net/'
});

app.config(['$httpProvider', function($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
}]);

$('#menu-toggle').click(function(e) {
    e.preventDefault();
    $('#wrapper').toggleClass('toggled');
});
