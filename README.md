# 🐾 ClyvoVet — ERP para Clínicas Veterinárias

> Projeto acadêmico desenvolvido para a disciplina **Advanced Business Development with .NET** — FIAP, 2026.

O ClyvoVet é uma API RESTful para gerenciamento de clínicas veterinárias, desenvolvida com ASP.NET Core e integração com banco de dados Oracle via Entity Framework Core. O sistema permite o controle longitudinal da saúde de cada animal cadastrado na plataforma.

---

## 👥 Integrantes

| Nome | RM |
|---|---|
| Arthur Graciani | RM561728 |
| João Pedro Scarpin | RM565421 |
| Wesley Andrade | RM563593 |
| Lucas Hideki | RM565355 |
| Gustavo Oliveira | RM566358 |

---

## 🛠️ Tecnologias

| Tecnologia | Versão | Finalidade |
|---|---|---|
| .NET | 10 | Plataforma principal |
| ASP.NET Core | 10 | Framework da API Web |
| Entity Framework Core | 10.0.8 | ORM para acesso ao banco de dados |
| Oracle.EntityFrameworkCore | 10.23.26200 | Driver EF Core para Oracle |
| Swashbuckle (Swagger) | 10.1.7 | Documentação interativa da API |
| Oracle Database | — | Banco de dados relacional |
| LINQ | Nativo .NET | Consultas ao banco via EF Core |

---

## 📐 Arquitetura do Projeto

```
ClyvoVet/
├── Controllers/
│   └── PetsController.cs          # Endpoints REST do recurso Pet
├── Data/
│   └── AppDbContext.cs            # Contexto do Entity Framework Core
├── Models/
│   ├── Pet.cs                     # Entidade mapeada para a tabela T_CLV_PET
│   ├── PetRequest.cs              # DTO de entrada
│   ├── PetResponse.cs             # DTO de saída
│   └── Validations/
│       └── StatusCastradoAttribute.cs  # Validação customizada
├── Migrations/                    # Geradas automaticamente pelo EF Core
├── appsettings.json               # Configurações da aplicação
└── Program.cs                     # Bootstrap e injeção de dependências
```

---

## ⚙️ Instalação e Execução

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Acesso a uma instância Oracle Database
- Git

### 1. Clonar o repositório

```bash
git clone https://github.com/Challenge-Clyvo-Vet-2026/Challenge-Clyvo-Vet-.NET.git
cd Challenge-Clyvo-Vet-.NET
```

### 2. Instalar os pacotes NuGet

```bash
dotnet add package Oracle.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Swashbuckle.AspNetCore
```

### 3. Configurar as credenciais do banco

Crie um arquivo `.env` na raiz do projeto com a connection string do Oracle:

```env
ConnectionStrings__OracleDb=Data Source=HOST:1521/SERVICE;User Id=USUARIO;Password=SENHA
```

> ⚠️ Consulte o arquivo `.env.example` para um modelo de configuração.

> ⚠️ Substitua `HOST`, `SERVICE`, `USUARIO` e `SENHA` pelos dados reais da sua instância Oracle.

> ⚠️ O arquivo `.env` já está no `.gitignore` (nunca o envie para o repositório).

### 4. Instalar a ferramenta do EF Core

```bash
dotnet tool install --global dotnet-ef
```

Caso já esteja instalada, verifique se a versão está alinhada com os pacotes do projeto:

```bash
dotnet ef --version              # deve ser 10.0.8
dotnet tool update --global dotnet-ef --version 10.0.8
```

### 5. Rodar as Migrations

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

> Isso cria automaticamente a tabela `T_CLV_PET` no Oracle.

### 6. Iniciar a aplicação

```bash
dotnet run
```

O terminal exibirá a porta em uso, por exemplo:

```
Now listening on: https://localhost:7042
```

### 7. Acessar o Swagger

```
https://localhost:{porta}/swagger
```

---

## 🔗 Rotas da API — Pets

Base URL: `/api/pets`

| Método | Rota | Descrição | Status de Sucesso |
|---|---|---|---|
| `GET` | `/api/pets` | Lista todos os pets ordenados por nome | `200 OK` |
| `GET` | `/api/pets/{id}` | Busca um pet pelo ID | `200 OK` |
| `GET` | `/api/pets/responsavel/{idResponsavel}` | Lista todos os pets de um responsável | `200 OK` |
| `GET` | `/api/pets/especie/{especie}` | Lista pets por espécie | `200 OK` |
| `POST` | `/api/pets` | Cadastra um novo pet | `201 Created` |
| `PUT` | `/api/pets/{id}` | Atualiza os dados de um pet | `200 OK` |
| `DELETE` | `/api/pets/{id}` | Remove um pet | `204 No Content` |

### Códigos de retorno

| Status | Descrição |
|---|---|
| `200 OK` | Operação realizada com sucesso |
| `201 Created` | Recurso criado com sucesso |
| `204 No Content` | Recurso deletado com sucesso |
| `400 Bad Request` | Dados inválidos ou campos obrigatórios ausentes |
| `404 Not Found` | Recurso não encontrado |

### Exemplo de body — POST e PUT

```json
{
  "idResponsavel": 1,
  "nomePet": "Rex",
  "especiePet": "Cão",
  "racaPet": "Labrador",
  "dataNascimentoPet": "2020-03-15T00:00:00",
  "statusCastrado": "N"
}
```

> **Regras de negócio:**
> - Todos os campos são obrigatórios, exceto `statusCastrado`
> - `statusCastrado` aceita apenas `"S"`, `"N"` ou `null`
> - Um pet deve obrigatoriamente estar vinculado a um responsável (`idResponsavel`)

---

## 🗄️ Banco de Dados

### Tabela: `T_CLV_PET`

| Coluna | Tipo | Obrigatório | Descrição |
|---|---|---|---|
| `ID_PET` | `NUMBER(10)` | ✅ | Chave primária, gerada automaticamente |
| `ID_RESPONSAVEL` | `NUMBER(10)` | ✅ | FK para o responsável pelo pet |
| `NOME_PET` | `NVARCHAR2(100)` | ✅ | Nome do pet |
| `ESPECIE_PET` | `NVARCHAR2(100)` | ✅ | Espécie (ex: Cão, Gato) |
| `RACA_PET` | `NVARCHAR2(100)` | ✅ | Raça do pet |
| `DATA_NASCIMENTO_PET` | `TIMESTAMP(7)` | ✅ | Data de nascimento |
| `STATUS_CASTRADO` | `NCHAR(1)` | ❌ | `S` para castrado, `N` para não castrado |

### Migrations

O schema do banco é gerenciado pelo EF Core via Migrations. Para criar e aplicar novas migrations após alterações no modelo:

```bash
dotnet ef migrations add NomeDaMigration
dotnet ef database update
```

---

Todos os direitos reservados © 2026 ClyvoVet. Desenvolvido por Arthur Graciani, João Pedro Scarpin, Wesley Andrade, Lucas Hideki e Gustavo Oliveira para a disciplina Advanced Business Development with .NET — FIAP.