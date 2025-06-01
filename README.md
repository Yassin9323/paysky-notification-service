# 📣 Notification Service

A lightweight, scalable microservice designed for handling asynchronous notifications via **Email** and **SMS**, built with **.NET 8**, **Hangfire**, and **Clean Architecture** principles.

---

## ✨ Features

* ✅ Clean architecture and modular design
* ✉️ Send email notifications via **Mailtrap**
* 📱 Send SMS notifications via a **free SMS API**
* ⚙️ Asynchronous job processing using **Hangfire**
* 📦 Persistent job queue via **PostgreSQL**
* 📊 Logs delivery results and job lifecycle to **Elasticsearch**
* 🛠️ Built-in **retry policy** for failed jobs

---

## 🧱 Architecture Overview

```
External System
     |
[ POST /api/notifications ]
     ↓
API Layer (Validation + Mapping)
     ↓
Hangfire (Enqueue Job in PostgreSQL)
     ↓
Job Server (Retry + Routing Logic)
     ↓
Delivery Service (Email / SMS)
     ↓
Log to Elasticsearch
```

* **Architecture Style:** Clean Architecture
* **Background Processing:** Hangfire
* **Database Queue:** PostgreSQL
* **Logging:** Elasticsearch

---

## 🚀 Tech Stack

| Component       | Technology                  |
| --------------- | --------------------------- |
| Backend         | .NET 8                      |
| Background Jobs | Hangfire                    |
| Job Queue       | PostgreSQL                  |
| Email Service   | Mailtrap                    |
| SMS Service     | Free SMS API (e.g., Vonage) |
| Logging         | Elasticsearch               |
|                 |                             |

---

## 📦 Installation

```bash
# 1. Clone the repo
git clone https://github.com/your-org/notification-service.git

# 2. Navigate into the directory
cd notification-service

```

> Make sure your environment variables are set (see `.env.sample` or below).

---

## ⚙️ Configuration

Create a `.env` file or configure these variables in your deployment pipeline:

```env
MAILTRAP_API_KEY=????
MAILTRAP_INBOX_ID=????
SMS_API_KEY=????
POSTGRES_CONNECTION=Host=localhost;Database=jobs;Username=user;Password=pass
ELASTICSEARCH_URI=http://localhost:9200
```

---

## 📮 API Endpoint

**POST** `/api/notifications`

**Payload:**

```json
{
  "type": "email",
  "to": "user@example.com",
  "subject": "Welcome",
  "message": "Thanks for signing up!"
}
```

**Types Supported:**

* `email`
* `sms`

**Response:**

* `202 Accepted` — Notification enqueued
* `400 Bad Request` — Validation error

---

## 📓 Logging & Monitoring

* All notification attempts and job statuses are logged to **Elasticsearch**
* Retry policies are built-in via **Hangfire** to ensure robustness
* Future support for **Prometheus + Grafana** can be added

---

## 📘 Future Enhancements

* Add support for Push Notifications
* Role-based access for dashboard UI
* Multi-language message templates
* Rate limiting and throttling

---

---

## 🛡 License

Distributed under the MIT License. See `LICENSE` for more information.

---

## 👨‍💻 Authors

* **Yassin** Waleed – [LinkedIn](https://www.linkedin.com/in/your-profile)
* Ahmed Ehab – [LinkedIn](https://www.linkedin.com/in/your-profile)

---

