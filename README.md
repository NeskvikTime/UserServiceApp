# DiffServiceApp Setup Guide

UserServiceApp is designed to create and manage users. 

Follow these steps to get started:

# Prerequisites
Docker: Ensure Docker is installed on your PC. If not, download and install it from Docker's official website.

Setup Instructions

# 1. Start Docker
Ensure Docker is running on your machine to proceed with the setup and for running integration tests.

# 2. Prepare the Environment
Open PowerShell and navigate to the API layer directory of the project.

cd C:\yourPathToAssignment\UserServiceApp\src\UserServiceApp.API\UserServiceApp.API.csproj

Run the following command to start the database container using Docker Compose:

powershell
Copy code
`docker compose -f .\docker-compose-database.yml up -d`
This command runs the database in detached mode.

# 3. Initialize the Database
Run the following command to start the database container using Docker Compose:

# 4. Run the Application
With the database up and running, start the application. 
Database migrations will be applied automatically upon startup.

# 5. Testing
For API testing, you can use:

Swagger: Accessible at http://localhost:<port>/swagger once the app is running. 
To enter bearer token in swagger, click the button "Authorize" (upper right corner of the UI) and enter the token in the format "Bearer <token>"

Postman: Send requests to the application's endpoints.
Use the JSON request payloads as described in the assignment instructions.

During all types of testing (API and integration tests), docker should be running on your machine!

# 6. Functionalities
