# Catálogo de Jogos - CP5 .NET & MongoDB

Web API em .NET 8 para gerenciar um catálogo de jogos de videogame, com persistência no MongoDB, CRUD completo, filtro por plataforma/preço e relatório de estoque por agregação.

## Participantes

| Nome | RM |
| --- | --- |
| Gustavo Tavares da Silva | RM562827 |
| Henrique Rodrigues Vespasiano | RM562917 |

## Tecnologias

- .NET 8 (ASP.NET Core Web API com Controllers)
- MongoDB.Driver 3.12.0
- Swagger (Swashbuckle)
- MongoDB em container Docker

## Estrutura

```
Controllers/   JogosController       -> endpoints HTTP e status codes
Services/      JogoService           -> camada de serviço
Repositories/  IJogoRepository       -> contrato de acesso a dados
               JogoRepository        -> implementação com MongoDB.Driver
Models/        Jogo                  -> documento com atributos BSON e validações
Dtos/          RelatorioEstoquePlataformaDto
Settings/      MongoDbSettings       -> Options Pattern (IOptions<MongoDbSettings>)
```

Fluxo: `Controller > JogoService > IJogoRepository`. As dependências são registradas no `Program.cs` (`IMongoClient` e `IMongoDatabase` como Singleton, repositório e serviço como Scoped).

## Como executar

1. Subir o MongoDB:

```bash
docker run -d -p 27017:27017 --name mongo-local mongo
```

2. Rodar a API:

```bash
dotnet run --launch-profile http
```

3. Abrir o Swagger em http://localhost:5264/swagger

A conexão fica em `appsettings.json`, na seção `MongoDbSettings` (`ConnectionString`, `DatabaseName` e `JogosCollectionName`).

## Endpoints

| Método | Rota | Retornos |
| --- | --- | --- |
| POST | `/api/jogos` | 201 Created, 400 |
| GET | `/api/jogos` | 200 |
| GET | `/api/jogos/{id}` | 200, 400 (id inválido), 404 |
| PUT | `/api/jogos/{id}` | 200, 400, 404 |
| DELETE | `/api/jogos/{id}` | 204 No Content, 400, 404 |
| GET | `/api/jogos/busca?plataforma={plataforma}&precoMaximo={preco}` | 200, 400 |
| GET | `/api/jogos/relatorio-estoque` | 200 |

Exemplo de corpo para POST/PUT:

```json
{
  "titulo": "Gran Turismo 5",
  "plataforma": "PlayStation 3",
  "genero": "Corrida",
  "preco": 79.90,
  "anoLancamento": 2010,
  "estoque": 10
}
```

## Desafios

**Filtro por plataforma e preço** (`/api/jogos/busca`): usa `Builders<Jogo>.Filter.And` combinando `Eq` na plataforma e `Lte` no preço, o que gera no MongoDB `{ plataforma: X, preco: { $lte: Y } }`.

**Relatório de estoque** (`/api/jogos/relatorio-estoque`): pipeline de agregação (`Aggregate().Group(...)`) que agrupa por plataforma e retorna a quantidade de títulos (`$sum: 1`) e o valor total do inventário (`$sum` de `$multiply: [preco, estoque]`), ordenado por plataforma.

O campo `preco` é gravado como `Decimal128` (`[BsonRepresentation(BsonType.Decimal128)]`) para que a comparação `$lte` e o cálculo do relatório funcionem como número.
