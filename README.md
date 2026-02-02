# Device Management API

A .NET 10 Web API for managing devices, including CRUD operations, filtering by brand and state, with Swagger documentation and Docker support.

---

## Technology Stack

- .NET 10 Web API
- C#
- SQL Server
- Docker
- Swagger / OpenAPI

---

## Requirements

- .NET 10 SDK
- SQL Server (local or remote)
- Docker (optional, for containerized run)
- Visual Studio / VS Code or any IDE that supports .NET 10

---

## Installation & Setup

### Clone the repository

```bash
git clone <repo-url>
cd device-management-api
```

### Configure database connection

Edit `appsettings.Development.json`:

```json
"ConnectionStrings": {
  "DeviceManagementDatabase": "Server=localhost;Database=DeviceManagement;User Id=yourUser;Password=yourPassword;"
}
```

> **Note:** If running inside Docker, use:

```json
"ConnectionStrings": {
  "DeviceManagementDatabase": "Server=host.docker.internal;Database=DeviceManagement;User Id=yourUser;Password=yourPassword;"
}
```

### Apply database migrations

```bash
dotnet ef database update
```

### Run the project locally

```bash
dotnet run
```

API will be available at `http://localhost:5087`.

### Run the project via Docker

```bash
docker build -t device-management-api .
docker run -d -p 8080:80 -e DOTNET_ENVIRONMENT=Development devicemanagementapi
```

Swagger will be available at: `http://localhost:8080/swagger/index.html`

---

## API Endpoints

| Method | Endpoint               | Description                   |
| ------ | ---------------------- | ----------------------------- |
| POST   | /devices               | Create a new device           |
| GET    | /devices               | Get all devices               |
| GET    | /devices/{id}          | Get device by ID              |
| GET    | /devices/brand/{brand} | Get devices filtered by brand |
| GET    | /devices/state/{state} | Get devices filtered by state |
| PATCH  | /devices/{id}          | Update a device               |
| DELETE | /devices/{id}          | Delete a device               |

---

## DTOs and Enums

- CreateDeviceRequest: DTO for creating a device.
- UpdateDeviceRequest: DTO for updating a device.
- DeviceState: Enum representing the state of a device (Available, InUse, etc.).

---

## Business Rules

- Device `Name` and `Brand` are required
- Devices are created with state `Available`
- Creation time is automatically assigned
- Devices in use **cannot be deleted**
- Name and Brand **cannot be changed** if device is in use

---

## Tests

This project uses xUnit for unit testing.
Tests are located in the DeviceManagementApi.Tests project.

- Run tests locally with:

```bash
dotnet test
```

Tests cover service layer logic, including:

- Device creation with required fields.
- Updating devices with business rules.
- Deletion and conflict scenarios.
- Filtering logic by brand and state.

---

## Docker Notes

- To connect to a local SQL Server from inside a Docker container, use:

```
Server=host.docker.internal;Database=DeviceManagement;User Id=<user>;Password=<password>;
```

- Ensure SQL Server authentication allows SQL logins.

---

---

# Future Improvements

This section outlines potential improvements and missing features.

## Future Improvements / Possible Missing or Malfunctioning Parts

- _SQL Server Connection Handling_
  - Currently, if the API cannot connect to the database, it may throw a generic exception and return a 500 Internal Server Error.
  - Future improvement: handle SQL connection exceptions gracefully, returning a 503 Service Unavailable with a clear message and logging the issue.

- _Global Exception Handling Middleware_
  - Business rule conflicts (InvalidOperationException) are currently handled in controllers individually.
  - Future improvement: implement a global exception handling middleware to capture all unhandled exceptions, standardize the response with ProblemDetails, and log errors consistently.

- _Request Validation_
  - Input validation currently relies on basic checks in DTOs.
  - Future improvement: use FluentValidation or DataAnnotations for robust request validation and standardized ValidationProblemDetails responses.

- _Swagger examples_
  - Add example for all requests for all endpoints incluinding minimal and complete examples
  - Add example for all responses for all endpoints

- _Logging and Monitoring_
  - No centralized logging or metrics collection is currently implemented.
  - Future improvement: integrate Serilog, Application Insights, or another logging/monitoring solution.

- _Missing Features_
  - Endpoint for bulk updates of devices.
  - Advanced filtering with search and pagination.
  - Soft delete instead of hard delete to prevent accidental data loss.

## Authors

- Jhonny Marcelo

---

## License

MIT License
