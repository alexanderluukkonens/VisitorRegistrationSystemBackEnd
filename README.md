# Visitor Registration System - Backend

This is a cloud-based visitor system registration for backend using azure functions and PostgeSQL database. 

## Description

This is the backend component of a cloud-based visitor registration system built on Microsoft Azure. The backend consists of Azure Functions that handle POST requests from the frontend, process visitor data and store it in a PostgreSQL database. All operations are automatically logged using Application Insights for monitoring and auditing purposes.

## Getting Started

### Dependencies

* .NET 8.0 SDK
* Azure Functions Core Tools v4
* Azure Functions extension for VS Code
* PostgreSQL extension for VS Code
* An active Azure subscription (student subscriptions were used)

### Architecture

Frontend (Github pages) 
    --> POST /api/register
Backend (Azure Functions)
    --> Entity Framework
PostgreSQL Database
    --> Logging
Application Insights

### Installing

* Clone the repository to your local machine
* Navigate to the backend project directory
* Restore NuGet packages:
```
dotnet restore
```
* Set up your local settings file (local.settings.json) with connection strings:
```
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "ConnectionStrings:DefaultConnection": "your_postgresql_connection_string",
    "APPLICATIONINSIGHTS_CONNECTION_STRING": "your_app_insights_connection_string"
  }
}
```

### Executing program

Local Development

* Start the Azure Functions runtime locally:
```
func start
```
* The API will be available at http://localhost:7071

Deploy to Azure

* Build the project:
```
dotnet build --configuration Release
```

* Deploy using Azure Functions Core Tools:
```
func azure functionapp publish <your-function-app-name>
```
Database Setup

* Apply existing migrations to update database schema:
```
dotnet ef database update
```

## Help

Common issues and solutions:

Connection String Issues:
```
# Test PostgreSQL connection
dotnet ef database update --verbose
```
Missing Dependencies:
```
# Install Azure Functions Core Tools
npm install -g azure-functions-core-tools@4 --unsafe-perm true
```

## Authors

* Alexander Luukkonen - alexanderluukkonens@hotmail.com

## License

This project is licensed under the MIT-license License - see the LICENSE.md file for details

## Acknowledgments

Inspiration, code snippets, etc.
* Microsoft Azure documentation
* Azure Functions tutorials
* Adam Marczak - Azure for Everyone: https://www.youtube.com/@AdamMarczakYT
* Cloud Knowledge https://www.youtube.com/@CloudKnowledgeChannel
