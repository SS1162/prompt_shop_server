# Microservices Architecture Plan — WebApiShope

## Overview
The monolith is split into **6 microservices** along domain boundaries.

---

## Services

### 1. User Service
| | |
|---|---|
| **Scope** | Registration, login, Google sign-in, password management, user profile updates |
| **Current files** | `User` entity · `UsersReposetory` · `UsersService` · `PasswordsService` · `UsersController` · `PasswordController` |
| **DTOs** | `RegisterUserDTO`, `LoginUserDTO`, `UserDTO`, `UpdateUser`, `PasswordDTO` |
| **Language** | **C# / ASP.NET Core** — security-critical; benefits from mature .NET auth libraries (Identity, JWT) |
| **Database** | **PostgreSQL** — relational, strong consistency for user credentials and profiles |

---

### 2. Product Catalog Service
| | |
|---|---|
| **Scope** | Products, Categories, MainCategories, Platforms, BasicSites, SiteTypes — CRUD + search / filter / pagination |
| **Current files** | `Product`, `Category`, `MainCategory`, `Platform`, `BasicSite`, `SiteType` entities · matching repositories, services, and controllers |
| **DTOs** | `ProductDTO`, `AddProductDTO`, `UpdateProductDTO`, `CategoryDTO`, `AddCategoryDTO`, `MainCategoriesDTO`, `PlatformsDTO`, `BasicSiteDTO`, `SiteTypeDTO`, `ResponePage` |
| **Language** | **C# / ASP.NET Core** — complex LINQ queries and EF Core pagination translate cleanly |
| **Database** | **PostgreSQL** — relational (products → categories → main categories); supports full-text search |

> `BasicSite` and `SiteType` are tightly coupled to the product/platform domain and stay in this service rather than becoming a separate service.

---

### 3. Cart & Order Service
| | |
|---|---|
| **Scope** | Shopping cart management, order creation, order status tracking, order history |
| **Current files** | `CartItem`, `Order`, `OrdersItem`, `Status` entities · `CartsReposetory` · `OrdersReposetory` · `StatusesReposetory` · `CartItemServise` · `OrdersServise` · `CartsItemsController` · `OrdersController` |
| **DTOs** | `AddToCartDTO`, `CartItemDTO`, `OrdersDTO`, `FullOrderDTO`, `OrderDetielsDTO`, `OrderItemDTO`, `UserOrdersDTO` |
| **Language** | **Go** — high-throughput, low-latency; excellent concurrency model for frequent add-to-cart / checkout requests |
| **Database** | **PostgreSQL** for orders (ACID, financial records) + **Redis** as a cache for active carts (fast read/write, TTL for abandoned carts) |

---

### 4. Payment Service
| | |
|---|---|
| **Scope** | PayPal integration, payment processing, payment status callbacks |
| **Current files** | `PayPalService` · `PaymentsController` |
| **Language** | **C# / ASP.NET Core** — strong PayPal SDK support; typed `HttpClient` via existing `AddHttpClient<PayPalService>` pattern |
| **Database** | **PostgreSQL** — stores transaction logs, idempotency keys, and reconciliation records; ACID guarantees essential for financial data |

---

### 5. Review & Rating Service
| | |
|---|---|
| **Scope** | Reviews (text + images), star ratings, rating aggregation |
| **Current files** | `Review`, `Rating` entities · `ReviewsReposetory` · `RatingsReposetory` · `ReviewsServise` · `RatingsServise` · `ReviewController` · `RatingMiddleware` |
| **DTOs** | `ReviewDTO`, `AddReviewDTO` |
| **Language** | **Node.js (TypeScript)** — lightweight CRUD; excellent ecosystem for image upload/streaming |
| **Database** | **MongoDB** — reviews are document-shaped (variable-length text, nested metadata, image URLs); flexible schema; aggregation pipeline for rating calculations |

---

### 6. AI Chatbot Service
| | |
|---|---|
| **Scope** | Gemini API integration, prompt management, chat sessions, prompt construction |
| **Current files** | `GeminiPrompt` entity · `GeminiPromptsReposetory` · `gemini` · `GeminiServise` · `GeminiSdkChatService` · `ChatBotServise` · `CreatePrompt` · `ChatBot` · `ChatController` · `GeminiController` |
| **DTOs** | `ChatRequestDTO`, `GeminiInput`, `GeminiPromptDTO` |
| **Language** | **Python (FastAPI)** — richest AI/ML ecosystem; best Gemini SDK support; easy to extend with LangChain or future model swaps |
| **Database** | **MongoDB** — chat histories and prompts are document-shaped with variable structure |

---

## Cross-Cutting Concerns

| Concern | Recommendation |
|---|---|
| **API Gateway** | **YARP** (C#) or **Kong** — single entry point routing to each service |
| **Authentication** | User Service issues **JWT tokens**; all other services validate JWTs via shared middleware |
| **Sync communication** | REST / gRPC between services (e.g., Order Service validates product IDs against Product Catalog) |
| **Async communication** | **RabbitMQ** or **Kafka** for domain events (e.g., `OrderPlaced` → triggers Payment Service) |
| **Image storage** | Move from local `wwwroot/` to **Azure Blob Storage** or **AWS S3**; services reference URLs only |
| **Centralized logging** | Replace per-service NLog with **ELK stack** (Elasticsearch + Logstash + Kibana) or **Seq** |
| **Secrets management** | Move from `appsettings.json` to **Azure Key Vault**, **AWS Secrets Manager**, or **HashiCorp Vault** |
| **Containerization** | Each service gets its own `Dockerfile`; orchestrate with **Docker Compose** (dev) / **Kubernetes** (prod) |

---

## Summary Table

| Service | Language | Database(s) | Key Entities |
|---|---|---|---|
| User | C# / ASP.NET Core | PostgreSQL | User |
| Product Catalog | C# / ASP.NET Core | PostgreSQL | Product, Category, MainCategory, Platform, BasicSite, SiteType |
| Cart & Order | Go | PostgreSQL + Redis | CartItem, Order, OrdersItem, Status |
| Payment | C# / ASP.NET Core | PostgreSQL | PayPal transactions |
| Review & Rating | Node.js (TypeScript) | MongoDB | Review, Rating |
| AI Chatbot | Python (FastAPI) | MongoDB | GeminiPrompt, chat sessions |
