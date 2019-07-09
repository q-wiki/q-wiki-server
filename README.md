# Qwiki - Wikidata Game (Back-End)
Qwiki is a Wikidata based game (Unity front-end with ASP.NET Core back-end) developed by HTW Berlin students in cooperation with Wikimedia Germany.
## Project Details/Usage
Once you clone the repository you might want to use Visual Studio or VS Code depending on your OS.
### WikidataGame.Backend
The WikidataGame.Backend project is the main REST API back-end project written in C# with ASP.NET Core 2.2.
By default Sqlite will be used as the database system, to switch to MSSQL just add an environment variable "SQLAZURECONNSTR_SQL" with the connection string. To make use of push notifications add an environment variable "CUSTOMCONNSTR_NotificationHub" and supply the connection string of the Azure Notification Hub.
#### REST
Check out the swagger documentation that is automatically generated via [swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) from code: https://wikidatagame.azurewebsites.net

### WikidataGame.Backend.Webjob
A simple Microsoft Azure Webjob which closes a game if a player is inactive for a certain amount of time and sends out push notifications to both players to notify them.

### WikidataGame.Backend.Tests
The WikidataGame.Backend.Tests contains unit tests (XUnit) for the asp.net core project. Tests are mainly used to assure that map generation and sparql queries are working properly.

### WikidataGame.ApiClient
The WikidataGame.ApiClient project is an API client, completely auto generated with [autorest](https://github.com/Azure/autorest) from swagger.json.

### WikidataGame.ApiClient.Sample
A single file sample console application to showcase the WikidataGame.ApiClient.

### WikidataGame.ApiClient.Tests
An integration test project (XUnit) to assure that both, the endpoints in combination with the Api client are working properly.
