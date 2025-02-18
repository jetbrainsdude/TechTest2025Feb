'use strict';
app.service('apiService', [
    'ngSettings', '$http', function (ngSettings, $http) {

        var serviceBase = ngSettings.apiServiceBaseUri;

        // ===============================================================
        // Documents
        // ===============================================================
        // add document
        this.addDocument = function (document) {
            return $http.post(serviceBase + 'api/documents',
                JSON.stringify(document),
                { headers: { 'Content-Type': 'application/json' } });
        };

        // ===============================================================
        // list all documents
        this.listDocuments = function () {
            return $http.get(serviceBase + 'api/documents');
        };

        // ===============================================================
        // get individual document
        this.getDocument = function (documentId) {
            return $http.get(serviceBase + 'api/documents/' + documentId);
        };

        // ===============================================================
        // delete individual document
        this.deleteDocument = function (documentId) {
            return $http.delete(serviceBase + 'api/documents/' + documentId);
        };

        // ===============================================================
        // update individual document
        this.updateDocument = function (document) {
            return $http.put(serviceBase + 'api/documents',
                JSON.stringify(document),
                { headers: { 'Content-Type': 'application/json' } });
        };

        // ===============================================================
        // login to middleware
        this.doLogin = function (login) {

            let credens = {};
            credens.username = login.playerName;
            credens.password = login.playerPassword;

            return $http.post(serviceBase + 'api/auth/login',
                JSON.stringify(credens),
                { headers: { 'Content-Type': 'application/json' } });
        };

        // ===============================================================
        // get the user data from access token
        this.GetData = function () {
            return $http.get(serviceBase + 'api/auth/Data');
        };
    }
]);