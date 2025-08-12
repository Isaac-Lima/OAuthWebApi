# 🛡️ Web API com OAuth 2.0

Este projeto é uma Web API desenvolvida em .NET 9 com PostgreSQL, projetada para estudos e prática dos conceitos de OAuth 2.0 e autenticação com JWT (JSON Web Token) com suporte a Refresh Token.  
A aplicação utiliza ASP.NET Identity para gerenciamento de usuários e segue uma arquitetura em Camadas (Domain, Application e Infrastructure).  

Além disso, o projeto será integrado a uma aplicação React + Vite para consumo dos endpoints.

## 🚀 Tecnologias Utilizadas

- C# 13
- .NET 9
- ASP.NET Core
- ASP.NET Identity
- OAuth 2.0
- JWT (JSON Web Token)
- Refresh Token
- Entity Framework Core
- PostgreSQL
- Arquitetura em camadas
- React + vite

## 📦  Funcionalidades da Aplicação

- **Cadastro de usuários** com armazenamento seguro de senha (Identity + hashing).
- **Login** com emissão de **JWT** e **Refresh Token**.
- **Renovação de token** usando refresh token via cookie HttpOnly.
- **Proteção de endpoints** com autenticação e autorização baseada em JWT.
- **Integração com OAuth 2.0** para autenticação segura e moderna.


## 📁 Endpoints

###  Registro de usuário

| Método | Endpoint                                  | Descrição                         |
|--------|-------------------------------------------|-----------------------------------|
| POST    | `/api/account/register`                       | Efetua o cadastro e retorna um status HTTP           |

#### Exemplo de corpo da requisição:

```json
{
  "username": "usuario_teste",
  "email": "teste@email.com",
  "password": "SenhaForte123!"
}
```

#### Resposta:

```
200 OK
```

---

### Login de Usuário

| Método | Endpoint                                  | Descrição                         |
|--------|-------------------------------------------|-----------------------------------|
| POST    | `/api/account/login`                       | Efetua o login e retorna um status HTTP           

Exemplo de corpo da requisição
```json
{
  "email": "teste@email.com",
  "password": "SenhaForte123!"
}
```
**Resposta**:
```
200 OK
```

---


### Refresh Token

| Método | Endpoint                                  | Descrição                         |
|--------|-------------------------------------------|-----------------------------------|
| POST    | `/api/account/refresh`                       | Efetua o login e retorna um status HTTP     


**Cookie**: `REFRESH_TOKEN` armazenado no navegador.  

**Resposta**:
```
200 OK
```




## ▶️ Como Executar a Aplicação

Siga os passos abaixo para configurar e executar a API localmente.

### 1. Pré-requisitos

* [.NET 9 SDK](https://dotnet.microsoft.com/download)
* [PostgreSQL](https://www.postgresql.org/download/)
* [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [Visual Studio Code](https://code.visualstudio.com/)
* Ferramenta de chamadas HTTP (Postman, Insomnia)

### 2. Clonar o Repositório

```bash
git clone https://github.com/Isaac-Lima/OAuthWebApi.git
cd OAuthWebApi
```

### 3. Configurar a Connection String

No arquivo `appsettings.json`, configure a conexão com o banco de dados. Exemplo:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=5432;Database=DemoDbForOAuth;User Id=seuUsuario;Password=suaSenha;"
}
```



### 4. Instale os pacotes necessários

```bash
dotnet restore
```

### 5. Crie o banco de dados com migrations

```bash
dotnet ef database update --project OAuthWebApi.Infraestructure --startup-project .\OAuthWebApi\
```

### 6. Execute o projeto

```bash
dotnet run



