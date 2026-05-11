# 🏪 WebApiShope — .NET 9

A RESTful Web API for an **AI-Driven Website Builder Prompt Store**, built with C# and .NET 9, following clean architecture principles and modern backend development best practices.

Users compose their desired website by selecting from a structured feature catalog, pay for their selections, and receive a **professional-grade AI prompt** ready to paste into any AI website builder — such as Bolt, Base44, Lovable, or v0 — to instantly generate a complete website.

---

## 🎯 How It Works

1. **Configure a site** — choose a **Site Type** (E-commerce Store, CRM System, LMS, Social Network, Analytics Dashboard, and more) and a **Platform** — one of 120+ combinations of user roles (Admin, Secretary, Accountant, Employee, Logistics, Tech Support, Project Manager) ranging from a single role up to the full "Full System Bundle" with all 7 roles.

2. **Build a cart** — browse and filter a 3-tier catalog (**Main Categories → Categories → Products**). Each product is an individual website feature carrying its own pre-written prompt snippet. Can't find what you need? Describe it in natural language — Gemini AI converts it into a precise technical specification as a fallback.

3. **Pay via PayPal** and place the order.

4. **Receive the prompt** — `CreatePrompt` walks the entire order grouped by main category → category → product, stitching every snippet together with section headers, the platform's roles architecture and seeded credentials, and the site's identity into one complete, structured AI prompt delivered to the user.

---

## 🗂️ Prompt Composition Model

The data model that drives the final prompt assembly:

| Entity | Contribution to the Final Prompt |
|---|---|
| **Site Type** | Opens with the specialist role & core purpose of the site |
| **Main Category** | Top-level section header (e.g., `## 4. PAGE SPECIFICATIONS`) |
| **Category** | Sub-section header with context (e.g., `Product Catalog: Act as a Senior System Architect...`) |
| **Product** | Specific feature instruction snippet |
| **Platform** | Full roles architecture + seeded credentials for all selected roles |

---

## 🏗️ Architecture & Layers

The project is structured in clearly separated layers to ensure maintainability, testability, and scalability:

| Layer | Responsibility |
|---|---|
| **API** | Entry point, controllers, middleware, routing |
| **Services** | Business logic and domain operations |
| **Repositories** | Data access abstraction over the database |
| **Entities** | Domain models representing database tables |
| **DTO** | Data transfer objects to decouple layers |

Layers are connected via **Dependency Injection (DI)**, achieving loose coupling between components and making the system easy to test and extend.

---

## 🔑 Key Features

### 📝 AI Prompt Assembly
The core feature. After an order is placed, `CreatePrompt` iterates through all selected items grouped by main category → category → product, stitching every pre-written prompt snippet together with the platform's roles architecture and site type identity into one complete, developer-grade AI prompt.

### 🤖 Google Gemini AI — Two Uses
- **Catalog Fallback** — when a user can't find a feature in the catalog, they describe it in natural language. Gemini returns strict JSON (`{ "technical_value": "..." }`), saved to the `GeminiPrompts` table and linked to the cart item.
- **Chat Assistant** — stateless conversational endpoint. The client sends its own history with each request; the server trims context to the last 11 messages before forwarding to Gemini. No chat data is persisted.

### ✅ RESTful Design
Proper use of HTTP verbs, status codes, and resource-oriented routing across all domains: site configuration, catalog browsing, cart, orders, users, reviews, payments, and more.

### 💳 PayPal Payment Integration
Secure checkout via the PayPal REST API, with credentials managed externally in configuration.

### 🗄️ Entity Framework Core (ORM)
Strongly-typed, async database access via EF Core with SQL Server.

### 📦 DTO Layer with Records & AutoMapper
DTOs implemented as C# Records for immutability, mapped automatically with AutoMapper.

### 🛡️ Centralized Error Handling
Global **Error Middleware** catches all exceptions and returns consistent error responses.

### 📊 Traffic Auditing
**Rating Middleware** logs every request (path, method, host, user agent, timestamp) to a dedicated DB table.

### 📋 Logging with NLog
Structured logging across all layers via NLog.

### 🧪 Unit & Integration Testing with xUnit
xUnit test suite covering repositories for users, orders, categories, main categories, and cart — with dedicated test fixtures.

---

## 🛠️ Tech Stack

| Technology | Purpose |
|---|---|
| .NET 9 / C# | Core framework & language |
| ASP.NET Core Web API | REST API host |
| Entity Framework Core | ORM / Data access (SQL Server) |
| AutoMapper | Object-to-object mapping |
| Google Gemini API (`Google.GenAI` SDK) | Catalog fallback & chat assistant |
| PayPal REST API | Payment processing |
| NLog | Logging |
| xUnit | Unit & integration testing |
| Dependency Injection (built-in) | Decoupling layers |
| OpenAPI / Swagger | API documentation |

---

## 📁 Project Structure

```
├── WebApiShope/          # Entry point, controllers, middleware
│   ├── Controllers/      # UsersController, ProductsController, OrdersController,
│   │                     # GeminiController, ChatController, PaymentsController, ...
│   └── MiddleWare/       # ErrorMiddleware, RatingMiddleware
├── Services/             # Business logic (CreatePrompt, ChatBot, GeminiService, PayPalService, ...)
├── Repositories/         # Data access implementations + EF DbContext
├── Entities/             # Domain models (BasicSite, Product, Category, MainCategory,
│                         # Platform, SiteType, Order, GeminiPrompt, ...)
├── DTO/                  # Record-based data transfer objects
├── Tests/                # xUnit unit & integration test projects
└── appsettings.json      # External configuration
```

---

## 🚀 Getting Started

### Prerequisites
- .NET 9 SDK
- SQL Server
- PayPal developer account (sandbox or live credentials)
- Google Gemini API key

### Configure `appsettings.json`
```json
{
  "ConnectionStrings": { "DefaultConnection": "Your SQL Server connection string" },
  "PayPal": {
    "ClientId": "your-paypal-client-id",
    "ClientSecret": "your-paypal-client-secret",
    "BaseUrl": "https://api-m.sandbox.paypal.com"
  },
  "GEMINI_API_KEY": "your-gemini-api-key"
}
```

### Run the API
```bash
# Restore dependencies
dotnet restore

# Apply migrations
dotnet ef database update --project Repositories

# Run the project
dotnet run --project WebApiShope
```

### Run Tests
```bash
dotnet test
```

---

## 📄 License

This project is licensed under the MIT License.
