# 🏋️ GymAppC — Modern Fullstack Application

> A scalable fullstack application built with **.NET Web API**, **Next.js**, and **Docker**, following **Clean Architecture principles** and secure **JWT-based authentication**.

---

## 📌 Overview

GymAppC is a fullstack application developed as part of a backend-focused education.  
The project demonstrates how to design and build a **production-ready system** with:

- 🔐 Secure authentication
- 🧱 Clean and maintainable architecture
- ⚡ Modern frontend integration
- 🐳 Containerized environment for consistent deployment

The goal has been to create a **solid technical foundation** that is easy to extend, test, and deploy.

---

## 🧠 Key Concepts Demonstrated

- Clean Architecture (Separation of Concerns)
- RESTful API design
- JWT Authentication & Authorization
- Dependency Injection
- Repository Pattern
- Containerization with Docker
- Fullstack integration (API ↔ Frontend)

---

## 🏗️ Architecture

The backend is structured using **Clean Architecture principles**, ensuring low coupling and high maintainability.

┌──────────────────────┐
│     Presentation     │  → Controllers (HTTP layer)
├──────────────────────┤
│      Application     │  → Services (Business logic)
├──────────────────────┤
│      Infrastructure  │  → Repositories (Database access)
├──────────────────────┤
│        Domain        │  → Models & Entities
└──────────────────────┘

### 🔍 Why this matters

- Easier to test and maintain
- Independent of frameworks
- Scalable for future features
- Clear separation of responsibilities

---

## 🔐 Authentication & Security

Authentication is handled using **JSON Web Tokens (JWT)**.

### Flow

1. User logs in with credentials
2. Server validates input
3. JWT is generated with claims (e.g. userId)
4. Token is returned to client
5. Client includes token in requests
6. Protected endpoints require valid token

```http
Authorization: Bearer <token>

Security considerations

* Passwords are hashed and salted
* Tokens are signed with a secret key
* Token validation includes issuer, audience, and expiration

⚛️ Frontend (Next.js)

The frontend is built with Next.js and communicates with the API.

Features

* Login & authentication flow
* Persistent session via token
* API integration using environment variables

NEXT_PUBLIC_API_URL=http://localhost:5000

🐳 Docker & Environment

The entire application runs in containers using Docker.
docker compose up --build

Service

Description

Backend

ASP.NET Web API

Frontend

Next.js application

Database

SQL Server (container)

Benefits

* Identical environments across machines
* Simplified setup
* Ready for cloud deployment (Azure)

⚙️ Local Development (without Docker)

Backend:
cd backend
dotnet restore
dotnet run

Frontend:
cd frontend
npm install
npm run dev

🗄️ Database

The project uses SQL Server with Entity Framework Core.

Features

* Code-first approach
* Migrations support
* Structured data access via repositories

🔄 API Endpoints

Authentication
POST /api/auth/register
POST /api/auth/login

Protected
GET /api/user/me

🚀 Future Improvements

This project is designed to be extended with:

* 📊 Workout tracking (CRUD)
* 🧩 CQRS + MediatR
* ☁️ Deployment to Azure
* 📈 Logging & monitoring
* 🧪 Unit & integration tests

💡 Design Philosophy

The focus of this project has been:

“Build it like it could go to production”

Meaning:

* Structured code over quick hacks
* Security from the start
* Scalable architecture
* Clear separation between layers

👨‍💻 Author

Martin Andersson
Anderssons Webblösningar

📎 Repository

👉https://github.com/Andersson76/GymAppC-
