# Tentacles Web Store API

### Introduction
Tentacles Web Store API serves as the backend of the Tentacles Web Store project and is designed for utilization with that project. It handles both authorization requests to register and use the system as well as requests to view product details and update their quantity.

### Supported Features
* Guests can register a new account to utilize the service
* Existing users can log into their account with an email and password
* Existing users can reset their account's password
* Existing users can logout
* Can view all products stored in the system
* Can view a single product via its ID
* Products can be purchased, updating all products in the request with the requested quantity removed from the database

### API Endpoints Overview
| HTTP Methods | Endpoints | Action | Status Code Error Reponse |
| ---- | ---- | ---- | ---- |
| POST | /auth/register | Creates a new user account | N/A (500) |
| POST | /auth/login | Logs into an existing user account | 400 |
| PATCH | /auth/reset-password | Changes the password of an existing user account | 204 |
| POST | /auth/logout | Logs a user out | N/A |
| GET | /api/product/{id} | Retrieves the desired product by its id | 404 |
| GET | /api/product | Retrieves all products | N/A (500) |
| PATCH | /api/product | Purchases products by reducing all requested products by the requested quantity | 500 |

### Technologies Utilized
* .NET 6.0 - This application targets .NET Framework 6.0
* SQL Server - A SQL Server database stores the data
* EntityFramework Core 6.0.12 - Used to update and handle the database requests
