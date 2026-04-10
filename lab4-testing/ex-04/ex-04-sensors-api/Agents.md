# Agents.md — Sensor Monitoring System (MongoDB, .NET 10 Minimal API)

## 🎯 Purpose

This document describes the **agents (logical components)** and how they interact inside the Sensor Monitoring System. It explains the **flow of data**, **responsibilities**, and **decision points**, especially around validation, persistence, and SMS notification logic.

---

# 🧠 System Overview

The system follows **Clean Architecture** principles and is divided into logical agents:

* **API Agent (Endpoints)**
* **Validation Agent (FluentValidation)**
* **Application Services Agent**
* **Mapping Agent (AutoMapper)**
* **Repository Agent (MongoDB)**
* **Notification Agent (SMS Service)**

Each agent has a **single responsibility** and communicates only through defined interfaces.

---

# 🧩 Agents and Responsibilities

## 1. API Agent (Minimal API Endpoints)

### Responsibility:

* Accept HTTP requests
* Delegate logic to services
* Return HTTP responses

### Rules:

* No business logic
* No database access
* Only orchestration

### Example Flow:

```
HTTP Request → Endpoint → Service Call → HTTP Response
```

---

## 2. Validation Agent (FluentValidation)

### Responsibility:

* Validate incoming DTOs before processing

### Behavior:

* Runs automatically via pipeline/middleware
* Returns **400 Bad Request** with ProblemDetails if invalid

### Example:

```
POST /sensors
 → Validate SensorCreateDto
   → ❌ Invalid → 400
   → ✅ Valid → Continue
```

---

## 3. Application Services Agent

### Responsibility:

* Core business logic
* Coordinates repositories and external services

### Services:

#### SensorService

* Create sensor
* Get sensor(s)
* Delete sensor

#### ReadingService

* Create reading
* Retrieve readings
* Trigger SMS logic

---

## 4. Mapping Agent (AutoMapper)

### Responsibility:

* Convert between:

  * DTOs ↔ Domain Entities

### Example:

```
SensorCreateDto → Sensor
Sensor → SensorResponseDto
```

---

## 5. Repository Agent (MongoDB)

### Responsibility:

* Data persistence
* Abstract MongoDB operations

### Repositories:

#### SensorRepository

* Get all sensors
* Get by ID
* Insert
* Delete

#### ReadingRepository

* Insert reading
* Get readings by sensorId

---

## 6. Notification Agent (SMS Service)

### Responsibility:

* Send SMS via external HTTP API

### Behavior:

* Uses HttpClient
* Base URL is configurable

---

# 🔄 Core Workflows

---

## 🟢 1. Register Sensor

```
Client
  ↓
API Endpoint
  ↓
Validation Agent
  ↓
SensorService
  ↓
Mapping Agent
  ↓
SensorRepository (MongoDB)
  ↓
Return 201 Created
```

---

## 🔵 2. Get Sensors

```
Client → Endpoint → Service → Repository → DB
                           ↓
                      Mapping Agent
                           ↓
                    Return 200 OK
```

---

## 🟡 3. Submit Reading (IMPORTANT FLOW)

```
Client
  ↓
Endpoint
  ↓
Validation Agent
  ↓
ReadingService
  ↓
Check Sensor Exists
  ↓
Create Reading
  ↓
Save to MongoDB
  ↓
Check Business Rule:
   IF 20 ≤ value ≤ 50
      AND phone exists
        ↓
     SMS Agent → External API
  ↓
Return 201 Created
```

---

## 🔴 4. Delete Sensor

```
Client → Endpoint → Service
                      ↓
                Check Exists
                      ↓
                Repository Delete
                      ↓
                Return 204 No Content
```

---

## 🟣 5. Get Sensor Readings

```
Client → Endpoint → Service → Repository
                           ↓
                      Mapping Agent
                           ↓
                    Return 200 OK
```

---

# ⚙️ Business Rules

### Rule 1: Sensor Must Exist

* All readings require a valid sensor
* If not → **404 Not Found**

---

### Rule 2: SMS Trigger Condition

SMS is sent ONLY if:

```
20 ≤ reading.Value ≤ 50
AND sensor.PhoneNumber != null
```

---

### Rule 3: Timestamp Handling

* Timestamp is generated **server-side**
* Always `DateTime.UtcNow`

---

# ❗ Error Handling Strategy

| Scenario           | Response                         |
| ------------------ | -------------------------------- |
| Validation failure | 400 Bad Request (ProblemDetails) |
| Not found          | 404 Not Found                    |
| Success (GET)      | 200 OK                           |
| Created            | 201 Created                      |
| Deleted            | 204 No Content                   |

---

# 🔌 External Integration

## SMS Service

### Input:

* Phone number
* Message

### Behavior:

* Sends HTTP POST request
* Uses configurable base URL

---

# 🧪 Testing Strategy

### Unit Tests:

* Services (mock repositories + SMS)

### Integration Tests:

* API endpoints with in-memory MongoDB or test container

---

# 🧭 Data Flow Summary

```
[Client]
   ↓
[API Endpoint]
   ↓
[Validation]
   ↓
[Service Layer]
   ↓
[Repositories]
   ↓
[MongoDB]

(+ optional)
   ↓
[SMS External API]
```

---

# 🚀 Key Design Principles

* Thin endpoints
* Strong separation of concerns
* Dependency inversion (interfaces everywhere)
* Business logic isolated in services
* Side effects (SMS) handled explicitly

---

# ✅ Final Notes

This architecture ensures:

* Scalability
* Testability
* Maintainability
* Clear responsibility boundaries

The **ReadingService** acts as the central decision-maker for triggering notifications, making it the most critical component in the system.

---
