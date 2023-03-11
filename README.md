# restaurant-api
A RESTful web API that returns information about restaurants

# Projects

- `Businesses.DataAccess` - Interfaces and data classes + a service implementation that uses the Yelp API.
- `Businesses.DataAccess.Tests` - Tests for the `Businesses.DataAccess` project.
- `Businesses.WebApi` - A simple web API that uses the service provided by the `Businesses.DataAccess` project to service requests.
- `Businesses.WebApi.Tests` - Tests for the `Businesses.WebApi` project. (Currently WIP)

# Setup

The WebApi project uses the dotnet [secrets manager](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows) to store the Yelp API key and it's own authorization key, which is required when calling its end points.

From the terminal:
- Navigate to the `Businesses.WebApi` project directory.
- Run `dotnet user-secrets set "YelpSettings:ApiKey" "[YelpApiKey]"`
- Run `dotnet user-secrets set "WebApiSettings:ApiKey" "[InternalApiKey]"`

# Tests

Run all tests by executing `dotnet test` in the root directory.

# Documentation

Swagger docs are available at:

```
https://localhost:7055/swagger/index.html
```

# Examples

Retrieve specific restaurant details: 

```
https://localhost:7055/api/restaurants/G0AB4-VN3v_Qd-icr8BfEg
```

Search for matching restaurants:

```
https://localhost:7055/api/restaurants?location=New%20York%20City&term=sushi
```