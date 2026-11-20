# AuthAspNetcore — Product Catalog API

Projeto de estudo legado em **ASP.NET Core 2.2**, com uma API REST de catálogo de **produtos e categorias**, autenticação via **JWT Bearer** e autorização por **permissões armazenadas em claims**. Apesar do nome do repositório destacar autenticação, a solução chama-se `ProductCatalog` e também demonstra persistência com Entity Framework Core e SQL Server.

> **Status:** projeto histórico/de demonstração. O código foi analisado para documentar o comportamento existente; não foi atualizado, executado ou validado com testes recentes. **Não é adequado para produção no estado atual.**

## O que o projeto faz

1. **Login:** recebe usuário e senha em `POST /v1/auth/login`, consulta usuários de exemplo em `Mock/User/Users.json` e, quando as credenciais correspondem, retorna um JWT.
2. **Autorização por claims:** o JWT inclui permissões com o nome do controller como tipo de claim e o verbo HTTP como valor. As políticas `Find` (`Get`) e `Add` (`Post`, `Put`, `Delete`) são avaliadas por um `AuthorizationHandler<Method>` customizado.
3. **Categorias:** consulta listagem e item por ID, e permite cadastrar categorias.
4. **Produtos:** consulta listagem e item por ID, permite cadastrar e excluir produtos.
5. **Banco de dados:** usa SQL Server com EF Core e um repositório genérico `IRepository<T>`. A migração inicial é SQL e executada pelo Evolve durante a inicialização em ambiente `Development`.

### Endpoints encontrados no código

| Método | Rota | Função | Autorização |
| --- | --- | --- | --- |
| GET | `/` | Verificação simples (`Api is running...`) | Anônima |
| POST | `/v1/auth/login` | Login e emissão do JWT | Anônima |
| GET | `/v1/categories` | Listar categorias | `Find` |
| GET | `/v1/categories/{id}` | Buscar categoria | `Find` |
| POST | `/v1/categories` | Criar categoria | `Add` |
| GET | `/v1/products` | Listar produtos | `Find` |
| GET | `/v1/products/{id}` | Buscar produto | `Find` |
| POST | `/v1/products` | Criar produto | `Add` |
| DELETE | `/v1/products/{id}` | Excluir produto | `Add` |

Não há rotas de atualização (`PUT`) expostas pelos controllers analisados, apesar da política `Add` incluir esse verbo. Também não há endpoints de criação de usuários, refresh token ou logout.

## Stack original

- **C# / ASP.NET Core 2.2** (`netcoreapp2.2`)
- **Entity Framework Core 2.2.6** e **SQL Server**
- **JWT Bearer** com assinatura HMAC-SHA256
- **FluentValidation 8.4** para validar login, produtos e categorias
- **Evolve 2.2** para executar migrações SQL
- **Repository Pattern** genérico e injeção de dependências nativa do ASP.NET Core

## Estrutura relevante

```text
ProductCatalog.sln
ProductCatalog.Api/
├── Controllers/
│   ├── AuthController.cs
│   ├── CategoryController.cs
│   ├── ProductController.cs
│   └── HomeController.cs
├── Data/
│   ├── StoreDataContext.cs
│   ├── Repository/
│   └── Migrations/V1_0_0__Create_Table.sql
├── Mock/User/
│   ├── UserRepositoryMock.cs
│   └── Users.json
├── Models/
│   ├── Product.cs
│   ├── Category.cs
│   └── Validate/
├── Security/
│   ├── Jwt/
│   └── Permission/PermissionByClaim.cs
├── ViewModels/Auth/LoginViewModel.cs
├── Startup.cs
├── Program.cs
└── appsettings.json
```

## Fluxo de autenticação e autorização

```text
POST /v1/auth/login
  → LoginViewModel + FluentValidation
  → UserRepositoryMock (arquivo JSON local)
  → JwtTokenBuilder (subject, issuer, audience, NameId e permissões)
  → JWT assinado com HS256
  → Authorization: Bearer <token>
  → JwtBearer valida assinatura, issuer, audience e expiração
  → PermissionByClaim compara controller + verbo da operação
```

As permissões são estabelecidas pelos usuários de demonstração em `Mock/User/Users.json`. Este **não é um sistema de identidade persistente**: o login compara diretamente credenciais armazenadas no arquivo local.

### Exemplo de requisição (sem credenciais reais)

```http
POST http://localhost:3001/v1/auth/login
Content-Type: application/json

{
  "username": "USUARIO_DE_TESTE",
  "password": "SENHA_DE_TESTE"
}
```

Em caso de sucesso, retorna um objeto com `token`. Para acessar endpoints protegidos, envie o JWT no cabeçalho `Authorization: Bearer <token>`.

## Dados e persistência

A migração SQL inicial cria `TB_CATEGORY` e `TB_PRODUCT`, com chave estrangeira de produto para categoria e `ON DELETE CASCADE`. O mapeamento de modelos usa `[Table]` e `[Column]`; a classe `StoreDataContext` expõe `DbSet<Category>` e `DbSet<Product>`.

O repositório genérico implementa `Create`, `FindAll`, `FindById` e `Delete`. As operações usam `SaveChanges()` de forma síncrona.

## Execução local (referência histórica)

> Requer um ambiente capaz de executar o SDK/runtime **.NET Core 2.2**, que já saiu de suporte, e uma instância SQL Server. Prefira executar apenas em ambiente de estudo isolado.

1. Configure uma conexão SQL Server própria em `ProductCatalog.Api/appsettings.json` e **substitua todas as credenciais e chaves de demonstração**.
2. Verifique o banco e a migração `Data/Migrations/V1_0_0__Create_Table.sql`.
3. Na raiz do repositório, execute:

```bash
dotnet restore ProductCatalog.sln
dotnet run --project ProductCatalog.Api/ProductCatalog.Api.csproj
```

O `Program.cs` lê `Host:Port` de `appsettings.json` (historicamente configurado como **3001**) e define a URL `http://localhost:<porta>`. A migração Evolve é executada no startup somente em `Development`. Os comandos acima são ilustrativos; **não foram executados nesta revisão**.

## Limitações e pontos de atenção

- **Framework sem suporte:** ASP.NET Core 2.2 e dependências antigas exigem modernização antes de qualquer uso sério.
- **Segredos no histórico:** o arquivo `appsettings.json` do repositório contém uma string de conexão com usuário/senha e uma chave JWT fixos. Trate-os como comprometidos; substitua/rotacione, retire da configuração versionada e avalie a limpeza do histórico Git caso tenham sido usados fora de um laboratório.
- **Usuários mock e senhas em texto puro:** `UserRepositoryMock` lê `Users.json` e compara senhas sem hashing. Apenas demonstração, nunca produção.
- **Tokens de duração longa:** `JwtTokenBuilder` usa o número de dias de `JwtConfiguration`, não o campo de minutos; não há refresh/revogação.
- **Configuração de desenvolvimento:** a inicialização das migrações usa `IsEraseDisabled = false` no Evolve. Revise com cuidado antes de apontar a aplicação a qualquer banco com dados importantes.
- **Erros e contratos:** controllers retornam mensagens de exceções em respostas `400`; alguns casos de ausência retornam `404` e outros `204`; não há padronização geral.
- **Validação e modelo:** conferir diferenças entre limites configurados no FluentValidation e no schema SQL, além do comportamento de datas e relacionamento de entidades.
- **Testes e documentação:** não foram encontrados projetos de teste automatizado nem configuração de Swagger nesta árvore de arquivos.

## Evolução sugerida

Para reaproveitar o conceito em um projeto moderno: migrar para uma versão **.NET LTS suportada**, usar configurações seguras por ambiente, Identity ou outro provedor confiável, hashing de senha, JWT com expiração curta e política de revogação/renovação apropriada, testes unitários e de integração, documentação OpenAPI e tratamento padronizado de erros.

---

**Sobre este README:** documentação reconstruída pela inspeção do código existente em `master`. Nenhuma funcionalidade nova foi implementada e nenhuma promessa de execução ou segurança em produção é feita.
