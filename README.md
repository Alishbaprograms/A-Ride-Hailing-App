# 🚕 Ride-Hailing Application (Uber/Kareem Clone) — ASP.NET MVC + PostgreSQL

A simple ride-hailing system designed to demonstrate the **MVC (Model–View–Controller)** architecture using **ASP.NET MVC** and **PostgreSQL**.
Users can book rides, and drivers can accept and complete them — demonstrating real-world request/response workflow in an MVC system.

---

## 📌 **Features**

### 👤 User Module

* Register & Login
* Book a Ride (Pickup & Drop Location)
* View Ride History

### 🚗 Driver Module

* Mark availability
* View all **Pending Rides**
* Accept a ride
* Update ride status:

  * **Pending → Accepted → In Progress → Completed**

### 🧩 System Features

* Authentication using Cookies
* Proper MVC separation: Model, View, Controller
* PostgreSQL database integration via **Npgsql + Entity Framework Core**
* Entity relationships (User ↔ Driver ↔ Ride)

---

# 🗂️ **Technology Stack**

| Layer             | Technology                              |
| ----------------- | --------------------------------------- |
| Backend Framework | ASP.NET MVC / ASP.NET Core MVC          |
| Database          | PostgreSQL                              |
| ORM               | Entity Framework Core + Npgsql Provider |
| Authentication    | Cookie Authentication                   |
| UI                | Razor Views                             |
| Language          | C#                                      |

---

# 📁 **Project Structure**

```
RideHailingApp/
│
├── Controllers/
│     ├── UserController.cs
│     ├── RideController.cs
│     └── DriverController.cs
│
├── Models/
│     ├── User.cs
│     ├── Driver.cs
│     ├── Ride.cs
│     ├── LoginViewModel.cs
│     └── RegisterViewModel.cs
│
├── Data/
│     └── ApplicationDbContext.cs
│
├── Views/
│     ├── User/
│     │    ├── Register.cshtml
│     │    ├── Login.cshtml
│     │
│     ├── Ride/
│     │    ├── BookRide.cshtml
│     │    └── MyRides.cshtml
│     │
│     └── Driver/
│          ├── Dashboard.cshtml
│
└── appsettings.json / Web.config
```

---

# 🗃️ **Database Schema (PostgreSQL)**

Run the following SQL in pgAdmin or psql:

```sql
CREATE TABLE users (
    userid SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    passwordhash VARCHAR(255) NOT NULL,
    role VARCHAR(20) NOT NULL
);

CREATE TABLE drivers (
    driverid SERIAL PRIMARY KEY,
    userid INT NOT NULL,
    vehicle VARCHAR(100) NOT NULL,
    availability BOOLEAN NOT NULL DEFAULT TRUE,
    FOREIGN KEY (userid) REFERENCES users(userid)
);

CREATE TABLE rides (
    rideid SERIAL PRIMARY KEY,
    userid INT NOT NULL,
    driverid INT NULL,
    pickuplocation VARCHAR(200) NOT NULL,
    droplocation VARCHAR(200) NOT NULL,
    status VARCHAR(50) NOT NULL,
    requestedat TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (userid) REFERENCES users(userid),
    FOREIGN KEY (driverid) REFERENCES drivers(driverid)
);
```

---

# 🔧 **Setup Instructions**

## 1️⃣ Clone the Repository

```
git clone https://github.com/yourname/RideHailingApp.git
cd RideHailingApp
```

---

## 2️⃣ Install Dependencies

NuGet packages:

```
Install-Package Npgsql.EntityFrameworkCore.PostgreSQL
Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package Microsoft.AspNetCore.Authentication.Cookies
```

---

## 3️⃣ Configure PostgreSQL Connection String

📌 Add to **appsettings.json**:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=RideAppDB;Username=postgres;Password=YOURPASSWORD"
}
```

📌 Or for **Web.config**:

```xml
<connectionStrings>
    <add name="DefaultConnection"
         connectionString="Host=localhost;Port=5432;Database=RideAppDB;Username=postgres;Password=YOURPASSWORD"
         providerName="Npgsql" />
</connectionStrings>
```

---

## 4️⃣ Configure EF Core in Program.cs / Startup.cs

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
```

---

## 5️⃣ Run Migrations

```
Add-Migration InitialCreate
Update-Database
```

This will create all required tables inside PostgreSQL.

---

# 🚀 Usage Guide

### 🧍 User Workflow:

1. Register an account
2. Login
3. Enter Pickup & Drop locations
4. Submit Ride Request
5. View Ride History

### 🚕 Driver Workflow:

1. Login as Driver
2. View **Pending Rides**
3. Accept a ride
4. Set status to **In Progress**
5. Complete the ride

---

# 🧠 Learning Outcomes

This project demonstrates:

* MVC Design Pattern
* Routing and Controllers
* Model Binding & Forms
* CRUD operations using EF Core
* Authentication & Authorization
* PostgreSQL integration
* Working with relational entities

Perfect for understanding ASP.NET MVC fundamentals through a real-world scenario.

---

# 🤝 Contributing

Pull requests and improvements are welcome!
Feel free to fork this repository and enhance features (maps, driver tracking, payments, etc.).

---


