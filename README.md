# DaprAspire - Sistema de Gerenciamento Financeiro com Arquitetura Orientada a Eventos

Este repositório contém uma aplicação financeira distribuída baseada em microserviços, utilizando o .NET Aspire com Dapr para comunicação assíncrona, projeções incrementais, autenticação via Identity Server com JWT e um front-end Blazor WebAssembly integrado via API Gateway (YARP).

---

## 🛠 Como rodar a aplicação

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Dapr CLI](https://docs.dapr.io/getting-started/install-dapr/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (para MongoDB)
- [Aspire Dashboard](https://learn.microsoft.com/en-us/dotnet/aspire/overview) (opcional, mas recomendado)

### Passos

1. **Clone o repositório:**

   ```bash
   git clone https://github.com/seu-usuario/DaprAspire.git
   cd DaprAspire
   ```

2. **Instale e inicialize o Dapr:**

   ```bash
   dapr init
   ```

3. **Suba o MongoDB com Docker:**

   ```bash
   docker run -d -p 27017:27017 --name dapr-mongo mongo
   ```

4. **Execute o Aspire AppHost:**

   ```bash
   dotnet run --project DaprAspire.AppHost
   ```

5. **Acesse via navegador:**

   - **Front-end:** http://localhost:7016  
   - **Swagger Gateway:** http://localhost:{porta-do-gateway}/swagger  
   - **Aspire Dashboard:** http://localhost:18888

---

## 🏗 Arquitetura

A arquitetura segue o estilo **microsserviços orientado a eventos**, com **Dapr Pub/Sub**, **JWT para autenticação**, **YARP como API Gateway**, **MongoDB como armazenamento principal** e **EventFlow para Event Sourcing**.

![Diagrama Arquitetural](./docs/arquitetura.png)

### Componentes principais:

| Componente               | Papel |
|--------------------------|-------|
| **Blazor UI**            | Interface do usuário (SPA) integrada via Gateway |
| **Gateway (YARP)**       | Roteia requisições para os microserviços e aplica autenticação/autorização |
| **Serviço de Identidade**| Autenticação JWT via ASP.NET Identity + MongoDB |
| **Serviço de Lançamentos**| Event Sourcing com comandos de crédito/débito |
| **Serviço de Consolidação** | Projeção incremental baseada em eventos via Dapr Pub/Sub |
| **MongoDB**              | Armazenamento dos eventos e projeções |

### Tecnologias

- **ASP.NET Core**
- **Blazor WebAssembly**
- **MudBlazor** (UI)
- **MongoDB**
- **Dapr** (Sidecar, Pub/Sub, State Store)
- **YARP** (API Gateway com suporte a Swagger)
- **Serilog** (Log estruturado)
- **EventFlow** (Event Sourcing/CQRS)
- **JWT** (Token de autenticação)
- **Aspire Dashboard** (Observabilidade e orquestração)

---

## ⚙️ Funcionamento

### Autenticação

- Login via `/identity/api/Account/login` pelo gateway.
- Retorno de **JWT** para requisições autenticadas subsequentes.
- O gateway valida e aplica escopo baseado na role (`admin`, `user`).

### Lançamentos

- Criação de um lançamento gera um **evento** persistido em MongoDB.
- Evento é publicado via **Dapr Pub/Sub** para consolidação.

### Consolidação

- Projeção **incremental** baseada nos eventos recebidos.
- Persistência em MongoDB e consulta via `/consolidation/projections/daily/{ledgerId}`.

### Frontend

- Tela de login (Blazor)
- Listagem de Ledgers com ações:
  - Criar Ledger
  - Adicionar Crédito/Débito
  - Consultar Saldo Diário

### Observabilidade

- **Serilog** para logs estruturados.
- **Aspire Dashboard** para status e tracing.

---

## 🔐 Segurança

- **JWT** para autenticação
- **Rate Limiting** via middleware no gateway
- **Global Error Handling** em todos os serviços
- Suporte a Swagger com token JWT

---

## 📦 Padrões

- Clean Architecture + DDD + CQRS
- Separação por camadas: `Domain`, `Application`, `Infrastructure`, `Api`

---

## 📌 Futuras melhorias

- Implementar **Snapshot Engine**
- Persistência incremental do estado
- Retry, circuit breaker, métricas
- Modelo granular de permissões por controller

---

## 🧠 Créditos

Projeto de arquitetura de referência para sistemas distribuídos financeiros com alta coesão, desacoplamento e observabilidade.

---