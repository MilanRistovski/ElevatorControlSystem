<b>🚀 Elevator Control Simulation</b>

  A .NET 8 web application that simulates a multi-car elevator control system.

<b>📂 Project Structure</b>

  Domain – Core entities (Car, Direction) and enums.
  
  Application – Interfaces, services, helpers, and options (CarDispatcher, CarMover, etc.).
  
  Infrastructure – State management (ElevatorState, CarManager) and logging (InMemoryLog).
  
  Web – ASP.NET Core MVC UI with controller and views.
  
  Tests – Unit tests for dispatcher, mover, and helper logic.

<b>⚙️ Features</b>

  Simulates multiple elevators (configurable).
  
  Handles hall calls (Up/Down) and car-specific requests.
  
  Allgorithm that picks the most appropriate car.
  
  Cars move floor-by-floor with configurable delays.
  
  Door open/close events simulated.
  
  In-memory event log tracks all activity.
  
  Simple UI for interaction (no JS required, auto-refresh).

<b>🛠️ Tech Stack</b>

  .NET 8
  
  ASP.NET Core MVC
  
  Dependency Injection
  
  xUnit (for tests)

<b>📦 Getting Started</b>
  Prerequisites:
  .NET 8 SDK
  Visual Studio 2022 / VS Code

  Run the app
  git clone https://github.com/your-repo/elevator-control.git
  cd elevator-control
  dotnet run --project ElevatorControl.Web

  Navigate to localhost

  Alternative - run directly through VS (Make ElevatorControl.Web your startup project)

<b>⚙️ Configuration</b>
Adjust appsettings.json:

"ElevatorOptions": {
  "FloorCount": 10,
  "CarCount": 4,
  "MoveSecondsPerFloor": 10,
  "DoorSeconds": 10,
  "InitialFloor": 1
}

<b>🧪 Running Tests</b>

  dotnet test

<b>📸 UI Preview</b>
  
  Hall Calls: Request elevators from any floor.
  
  Car Controls: Direct a specific car to a floor.
  
  Log: View detailed movement and door activity.

<b>👤 Author</b>

  Built by Milan Ristovski as a demo project for interview and learning purposes.
