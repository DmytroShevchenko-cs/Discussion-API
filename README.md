# Discussion

## About project
Discussion API demonstrates .NET developer skills using modern architecture patterns.
Tech Stack: ASP.NET Core Web API, PostgreSQL, Cloudflare R2 (file storage), RabbitMQ (message queue), SignalR (real-time updates)

### Endpoints:
| Method | Endpoint           | Description                     |
|--------|--------------------|---------------------------------|
| POST   | /api/comments      | Send message to queue to processe the photos, save to db and send via WebSocker.|
| DELETE | /api/comments/{id} | Delete a comment by its ID. Returns result status of the delete operation. |
|  GET   |  /api/comments     | Retrieve comments with optional filters. Returns list of comments in JSON, ordered by most recent first.|

### Features
- Asynchronous comment processing via RabbitMQ queue

- Image/file upload to Cloudflare R2 storage

- Real-time comment notifications via SignalR WebSocket

- LIFO comment ordering with pagination/filtering

- CAPTCHA validation and XSS/SQL protection

- Docker containerization for easy deployment

### Tech Stack

| Layer       | Technology              |
|-------------|-------------------------|
| API         | ASP.NET Core 8 Web API  |
| ORM         | Entity Framework Core   |
| Database    | PostgreSQL 15           |
| Queue       | RabbitMQ                |
| Storage     | Cloudflare R2           |
| Real-time   | SignalR WebSocket       |
| Container   | Docker Compose          |
| Docs        | Swagger/OpenAPI         |


## Deployment

**Link** 

http://194.37.81.107:8080/swagger/index.html

**Video link** 

https://drive.google.com/drive/folders/108nDvDD3OcY7pAB_xAZm9P8AX-jzHfsb?usp=sharing

## How to Build Locally

### Prerequisites (Install These First)

| OS | Docker | Git |
|----|--------|-----|
| **Windows** | [Docker Desktop](https://www.docker.com/products/docker-desktop/) | [Download Git](https://git-scm.com/downloads) |
| **Mac** | [Docker Desktop](https://www.docker.com/products/docker-desktop/) | [Download Git](https://git-scm.com/downloads) |
| **Linux** | `sudo apt update && sudo apt install docker.io docker-compose` | `sudo apt install git` |

### Verify Installations

**Windows/Mac/Linux** (same commands):
```
docker --version
docker compose version
git --version
```

All show versions = ✅ Ready!

### Step-by-Step Setup

#### 1. Open Terminal
| OS | How to Open |
|----|-------------|
| **Windows** | `Win+R` → `cmd` → Enter **OR** PowerShell |
| **Mac** | `Cmd+Space` → `Terminal` |
| **Linux** | `Ctrl+Alt+T` |

#### 2. Clone Repository

```
git clone https://github.com/DmytroShevchenko-cs/Discussion-API

```


#### 3. Enter Project Folder

```
cd Discussion-API

```

**Verify**: `ls` (Mac/Linux) or `dir` (Windows) → see `docker-compose.yml` ✅

#### 4. Edit .env
```
nano .env
```
Add environment ...

```
CONNECTION_STRING=Host=postgres;Port=5432;Database=discussion-dev;Username=postgres;Password=verysecurepass123!

R2_ACCESS_KEY="Your_R2_ACCESS_KEY"
R2_SECRET_KEY="Your_R2_SECRET_KEY"
R2_ACCOUNT_ID="Your_R2_ACCOUNT_ID"
R2_BUCKET=discussion-storage

RABBITMQ_USER=guest
RABBITMQ_PASSWORD=password123!
RABBITMQ_PORT=5672
```


#### 5. Start Services
**Docker Compose v2+** (modern, all OS):

```
docker compose up --build

```

**Old Docker Compose** (if error above):

```
docker-compose up --build

```

**First run**: 5-10 minutes (downloads images). **Stop**: `Ctrl+C`

### Verify It's Working ✅

| Service | URL |  |
|---------|-----|--------|
| **Swagger Docs** | http://localhost:8080/swagger | Make requests |
| **RabbitMQ** | http://localhost:15672 | Creds `guest` / `Verysecurepass123!` |

### Stop & Clean Up

```
docker compose down

```

**🎉 Your API is live at http://localhost:8080/swagger**


