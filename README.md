# 🏋️ PumpMate - Fitness Tracking Application

PumpMate is a comprehensive web application that allows users to track their workouts and calorie intake.
## ✨ Features

- 🔐 **Secure User Authentication**

- 💪 **Workout Tracking (Gym, Home, Cardio, Yoga)**

- 🔥 **Calorie Calculation (Intake and Burn)**

- 📊 **Statistics and Reporting**

- 🌙 **Dark/Light Mode Support**

- 📱 **Responsive Design**

- 🎯 **Personalized Fitness Guidance**

## 🚀 Quick Start
### Method 1: Using the Batch File (Recommended)

1. Double-click the run-project.bat file
2. The application will start automatically
3. Open https://localhost:7001 in your browser

### Method 2: Manual Setup

    # Install dependencies
    dotnet restore

    # Update the database
    dotnet ef database update

    # Run the application
    dotnet run

## 📋 Requirements

- .NET 6.0 or higher
- SQL Server (including LocalDB)
- Modern web browser

## 🛠️ Technologies

- **Backend**: ASP.NET Core MVC
- **Database**: Entity Framework Core
- **Frontend**: HTML5, CSS3, JavaScript
- **UI Framework**: Bootstrap 5
- **Theme**: Dark/Light Mode CSS Variables

## 📁 Project Structure

    PumpMate/
    ├── Controllers/          # Business logic
    ├── Models/               # Data models
    ├── Views/                # User interface
    ├── Data/                 # Database connection
    ├── Migrations/           # Database schemas
    ├── wwwroot/              # Static files
    ├── run-project.bat       # Quick launch script
    └── README.md             # This file

## 🎮 Usage

1. **Sign Up**: Create a new account
2. **Log In**: Access your account
3. **Add Workout**: Record your exercises
4. **Track Calories**: Log your daily calorie intake
5. **Check Statistics**: Monitor your progress

## 🔧 Development
### Adding a New Feature

1. Create the model class
2. Add the controller
3. Create the view files
4. Add a migration: dotnet ef migrations add FeatureName

### Updating the Database

    dotnet ef migrations add MigrationName
    dotnet ef database update

## 📞 Support

If you encounter any issues:

- Use GitHub Issues
- Review the project documentation
---
Track your fitness journey with PumpMate! 💪
