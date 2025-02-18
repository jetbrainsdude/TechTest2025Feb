'use strict';
app.service('sessionService', function() {

        // initialise factory
        var sessionServiceFactory = {};

        var state = {
            accessTokenExpired: false,
            isFailure: false,
            failMessage: '',
            refreshReturn: '',
            isNewDocument: false,
        };

        // ================================================================
        // clear session
        var clear = function() {
            state.accessTokenExpired = false;
            state.isFailure = false;
            state.failMessage = '';
            state.refreshReturn = '';
            state.isNewDocument = false;
        };

        // ================================================================
        // finalise factory
        sessionServiceFactory.clear = clear;
        sessionServiceFactory.state = state;

        return sessionServiceFactory;
    }
);