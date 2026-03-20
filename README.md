# 🎮 Game Backend API

A RESTful API built with **.NET** and **Entity Framework Core**, designed to serve as a foundational backend server for video games. This project handles essential game mechanics, including player authentication, virtual economy, inventory management, and combat reward systems.

## ✨ Features

* **🔐 Authentication & Security:** Secure player registration and login using **BCrypt** for password hashing and **JWT (JSON Web Tokens)** for stateless session management.
* **💰 Economy & Store System:** Transactional endpoints for players to buy items with in-game currency, ensuring ACID compliance to prevent exploits.
* **🎒 Inventory Management:** A Many-to-Many relational database structure allowing players to own, accumulate, and consume items.
* **⚔️ Gameplay Mechanics:** Server-side logic for killing enemies, calculating XP, leveling up, and a 20% chance for randomized loot drops.
* **🎁 Retention Mechanics:** A secure 24-hour Daily Reward system to keep players engaged.
* **🏆 Leaderboard:** Global ranking system fetching top players based on XP and Level.

## 🛠️ Tech Stack

* **Framework:** .NET 9.0+ (ASP.NET Core Web API)
* **Language:** C# 13+
* **ORM:** Entity Framework Core
* **Database:** SQLite (Easily swappable to PostgreSQL/SQL Server via Migrations)
* **Security:** BCrypt.Net-Next, JWT Bearer Authentication
* **Documentation:** Swagger / OpenAPI

## 🚀 Getting Started

Follow these steps to set up and run the project locally.

### Prerequisites
* [.NET 9.0 SDK or later](https://dotnet.microsoft.com/download/dotnet)
* A code editor (VS Code, Visual Studio, or Rider)

### Installation

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/metinayingit/game-backend-api.git](https://github.com/metinayingit/game-backend-api.git)
    cd game-backend-api/GameBackend.API
    ```

2.  **Restore dependencies:**
    ```bash
    dotnet restore
    ```

3.  **Apply Database Migrations:**
    ```bash
    dotnet ef database update
    ```

4.  **Run the application:**
    ```bash
    dotnet run
    ```

5.  **Open Swagger UI:**
    Check your terminal output to find the active port (e.g., `Now listening on: http://localhost:5019`). Navigate to `http://localhost:<port>/swagger` in your browser to test the API endpoints interactively.

## 📡 API Endpoints Documentation

All protected endpoints require a valid JWT token. In Swagger UI, click the **Authorize** button at the top and enter your token as: `Bearer <your_token>`.

### 🛡️ Auth (`/api/Auth`)
* `POST /register` - Register a new player.
* `POST /login` - Authenticate player and generate a JWT.

### 👤 Player (`/api/Player`)
* `GET /me` - 🔒 Retrieve the current logged-in player's profile, stats, and full inventory.
* `GET /leaderboard` - Get the top 10 players ranked by XP and Level.

### 🏪 Store (`/api/Store`)
* `POST /items` - Create a new item in the database (Admin/Cheat).
* `POST /add-coins` - 🔒 Add coins to the player's balance (Admin/Cheat).
* `POST /buy` - 🔒 Purchase an item from the store (deducts coins, adds to inventory).

### ⚔️ Game (`/api/Game`)
* `POST /kill-enemy` - 🔒 Simulate killing an enemy to gain XP, Coins, and potentially trigger a Level Up or a randomized Loot Drop.
* `POST /daily-reward` - 🔒 Claim 500 bonus coins (has a strict 24-hour cooldown).

### 🎒 Inventory (`/api/Inventory`)
* `POST /consume/{itemId}` - 🔒 Consume a specific item from the inventory, reducing its quantity or removing it entirely.

---

## 👨‍💻 Author
**Metin AYIN**