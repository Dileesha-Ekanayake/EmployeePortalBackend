# Employee Collaboration Portal - Server
A .NET Core Web API backend for the Employee Collaboration Portal application that enables employees to create posts, comment, and interact with each other's content.

## 🚀 Features
- **User Management**: Admin-only user creation and management with role-based access
- **Post Management**: Create, read, update, delete posts with author tracking
- **Comments System**: Add comments to posts with timestamp and author information
- **Interactions**: Like/dislike functionality for posts with real-time counters
- **Filtering & Sorting**: Filter posts by author, sort by recent or most liked
- **Dashboard Metrics**: Total users, posts, comments statistics
- **Authentication**: JWT-based authentication system
- **Form Validation**: Comprehensive input validation and error handling

## 🛠️ Technology Stack
- **.NET Core 8** - Web API Framework
- **Entity Framework Core** - ORM
- **SQL Server** - Database
- **JWT Authentication** - Security
- **Swagger/OpenAPI** - API documentation

## 📋 Prerequisites
- [.NET SDK 8.0 or higher](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or SQL Server Express
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

## ⚙️ Installation & Setup

### 1. Clone the Repository
```bash
git clone https://github.com/Dileesha-Ekanayake/EmployeePortalBackend.git
```

### 2. Database Setup

#### Option 1: Using SQL Script (Recommended)
1. Navigate to `project_resources/database/`
2. Open the provided `db_script.sql` file in **SQL Server Management Studio (SSMS)**
3. Execute the script — this will:
   * Create the database `EmployeePortal`
   * Create all required tables, relationships, and constraints
   * Insert sample data (including the default Admin account)
4. Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=EmployeePortal;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

#### Option 2: Using Entity Framework Migrations
1. Update the connection string in `appsettings.json` (same as above)
2. Run database migrations from the project root:

```bash
dotnet ef database update
```

### 3. Install Dependencies
```bash
dotnet restore
```

### 4. Build the Project
```bash
dotnet build
```

### 5. Run the Application
```bash
dotnet run --launch-profile "https"
```
The API will be available at `https://localhost:5050 and http://localhost:5055`

## 📊 Database Schema
### Tables Structure:
- **Users**: User information with roles (Admin/Employee)
- **Role**: User role information
- **Posts**: Post content with author references and timestamps
- **Comments**: Comments linked to posts and users
- **PostLikes**: Like/dislike tracking for posts

### Default Admin Account:
- **Username**: Dileesha
- **Password**: Admin@1234

## For every other User Account:
- **Username**: The username of the User
- **Password**: {"Username"}@1234

## 📁 Project Structure
```
EmployeePortalBackend/
├── Controllers/        # API Controllers
├── Models/             # Data Models & Entity Classes
├── Dto/                # Data Transfer Objects
├── Data/               # Entity Framework Context & Configurations
├── Migrations/         # Database Migrations
├── Auth/               # Authentication & Authorization
├── appsettings.json    # Configuration
└── Program.cs          # Application Entry Point
```

## 📚 Additional Resources
- [.NET Core Documentation](https://docs.microsoft.com/en-us/dotnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [JWT Authentication](https://jwt.io/)

## 🤝 Contributing
1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request
