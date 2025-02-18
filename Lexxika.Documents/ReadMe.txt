## Document Editor

This is a simple document editor app built with AngularJS (version 1.5) as a client to an ASP.NET Core (.NET 8) Web API. 
Admin users can add, edit and delete all documents, while normal users can only add and edit their own documents.
Authentication and authorisation are implemented using JWT tokens. 
The web API uses an in-memory collection for storing documents. 
This in-memory collection is seeded with three initial documents. Note the GUIDs will be different at each web API startup.
Web API tested using test.http file in the root of the web API project.
Users are hardcoded (credentials are below). 
Main NUGet packages include Microsoft.AspNetCore.Authentication.JwtBearer and Nlog.


## Document Structure

{ 
    "Id": <GUID>,
    "title": "Document 1",
    "translationText": "This is the content of document 1.",
    "user": "admin"
}


## Hard Coded User Credentials

`admin` / `admin`
`user1` / `password1`
`user2` / `password2`


## API Endpoints

GET 'api/documents': Get all documents
GET 'api/documents/{id}': Get a document by Id
POST 'api/documents': Add a new document.
PUT 'api/documents': Update a document
DELETE 'api/documents/{id}': Delete a document by Id
POST 'api/auth/login': Login expecting a JSON object with username and password
GET 'api/auth/Data': Get the user information


## Time out of JWT token

This is set to 30 minutes in Controllers/AuthorityController.cs file in the GenerateJwtToken method.


## Configure Base URL in app.js

app.constant('ngSettings', {
    apiServiceBaseUri: 'https://localhost:7089/'
    //apiServiceBaseUri: 'https://jrh.azurewebsites.net/'


## Configure URL in Web API

The solution launch file and URLs for web.api are set in Properties/launchSettings.json
