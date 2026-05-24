# Atividade Prática — Desenvolvimento de API GraphQL com Hot Chocolate
 
**Disciplina:** Programação Back-end
**Semana:** 9 | **Aulas:** 1 / 2 / 3
**Tecnologia:** ASP.NET Core + HotChocolate.AspNetCore
 
---
 
## Objetivo da Atividade
 
Nesta semana você irá construir uma **API GraphQL** utilizando **.NET 8** com a biblioteca **HotChocolate.AspNetCore**. A atividade foi inspirada no projeto de exemplo fornecido pelo professor — uma API de livros — e o seu desafio será expandi-la ou criar um domínio similar com **Queries** e **Mutations**.
 
Ao final, você compreenderá como o GraphQL difere do padrão REST, saberá quando escolher cada abordagem e terá uma API funcional publicada no GitHub.
 
---
 
## Conceitos de Revisão
 
### 1. O que é REST / RESTful?
 
**REST** (Representational State Transfer) é um **estilo arquitetural** para construção de APIs web, descrito por Roy Fielding em 2000. Ele define um conjunto de restrições que, quando seguidas, resultam em serviços escaláveis, simples e interoperáveis.
 
Uma API é considerada **RESTful** quando obedece a esses princípios:
 
- **Interface uniforme** — cada recurso possui uma URL única (`/products`, `/users/42`).
- **Stateless** — cada requisição carrega tudo que o servidor precisa; o servidor não guarda estado do cliente.
- **Client-Server** — o cliente e o servidor são desacoplados.
- **Cacheable** — as respostas podem ser marcadas como cacheáveis para melhorar a performance.
- **Camadas** — o cliente não precisa saber se está falando diretamente com o servidor ou com um intermediário.
Como funciona na prática: cada recurso é acessado por um endpoint HTTP e cada verbo tem um significado fixo:
 
| Verbo HTTP | Ação            | Exemplo de endpoint  |
|------------|-----------------|----------------------|
| `GET`      | Listar / buscar | `GET /products`      |
| `POST`     | Criar           | `POST /products`     |
| `PUT`      | Atualizar       | `PUT /products/1`    |
| `DELETE`   | Remover         | `DELETE /products/1` |
 
> **Analogia:** Pense no REST como um cardápio de restaurante com pratos fixos. Você pede "o prato 3" e recebe exatamente o que está descrito no cardápio — não importa se você queria só a guarnição.
 
---
 
### 2. O que é GraphQL?
 
**GraphQL** é uma **linguagem de consulta para APIs**, criada pelo Facebook em 2012 e aberta ao público em 2015. Diferente do REST, o GraphQL expõe um **único endpoint** (geralmente `/graphql`) e o cliente especifica exatamente quais campos quer receber.
 
Existem três operações principais:
 
- **Query** — leitura de dados (equivalente ao `GET` do REST).
- **Mutation** — escrita de dados (equivalente a `POST`, `PUT` e `DELETE`).
- **Subscription** — recebimento de dados em tempo real via WebSocket.
Exemplo de Query GraphQL:
 
```graphql
# O cliente pede SOMENTE os campos que precisa
query {
  livros {
    titulo
    autor
  }
}
 
# Resposta — apenas os campos solicitados:
# { "data": { "livros": [
#   { "titulo": "O Hobbit", "autor": "J.R.R. Tolkien" }
# ]}}
```
 
> **Analogia:** No GraphQL, você monta o seu próprio prato no buffet. Você diz exatamente o que quer — "só arroz e feijão, sem a salada" — e paga apenas pelo que pediu.
 
---
 
### 3. REST vs GraphQL — Diferenças e Quando Usar
 
As duas abordagens têm forças diferentes. Conheça as principais distinções:
 
| Aspecto              | REST                                        | GraphQL                                  |
|----------------------|---------------------------------------------|------------------------------------------|
| Endpoints            | Múltiplos (`/users`, `/posts`…)             | Um único (`/graphql`)                    |
| Dados retornados     | Formato fixo definido pelo servidor         | O cliente escolhe os campos              |
| Over-fetching        | Comum — recebe campos desnecessários        | Eliminado — só o que foi pedido          |
| Under-fetching       | Frequente — pode precisar de várias chamadas| Resolvido em uma única query             |
| Curva de aprendizado | Baixa — verbos HTTP intuitivos              | Média — sintaxe própria de query         |
| Documentação         | OpenAPI / Swagger (externo)                 | Schema autodocumentado (intrínseco)      |
| Versionamento        | Versões de API (`/v1`, `/v2`)               | Schema evolutivo sem versões             |
| Cache HTTP           | Nativo (`GET` é cacheável)                  | Requer implementação adicional           |
 
#### Quando usar REST?
 
- APIs públicas consumidas por terceiros que não controlamos.
- Sistemas simples com recursos bem definidos e poucos campos.
- Quando o cache HTTP nativo é essencial (ex: CDNs, APIs de conteúdo).
- Equipes com pouca familiaridade com GraphQL.
#### Quando usar GraphQL?
 
- Apps mobile que precisam economizar dados — controlam exatamente o que baixam.
- Front-end com múltiplas views que precisam de dados diferentes do mesmo recurso.
- BFF (Backend for Frontend) — uma camada única que agrega múltiplas fontes de dados.
- Times separados de front-end e back-end que evoluem em velocidades diferentes.
- Dashboards e relatórios com consultas complexas e flexíveis.
> **Resumo:** REST é ótimo para simplicidade e padronização. GraphQL brilha quando o cliente precisa de flexibilidade e eficiência no tráfego de dados. Não existe uma escolha universalmente melhor — depende do contexto.
 
---
 
## Projeto de Exemplo do Professor
 
O professor disponibilizou um projeto de referência (**GraphQLSimple**) com uma API mínima de livros. Analise-o antes de começar o seu.
 
```
GraphQLSimple/
├── GraphQLSimple.csproj   ← Dependência: HotChocolate.AspNetCore 16.x
└── Program.cs             ← Todo o código em um único arquivo
```
 
**Program.cs do projeto de exemplo:**
 
```csharp
var builder = WebApplication.CreateBuilder(args);
 
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>();
 
var app = builder.Build();
app.MapGraphQL();
app.UseHttpsRedirection();
app.Run();
 
// ── Modelo ──────────────────────────────────────────────────────
public record Livro(int Id, string Titulo, string Autor);
 
// ── Query (leitura) ─────────────────────────────────────────────
public class Query
{
    private readonly List<Livro> _livros = new()
    {
        new Livro(1, "O Senhor dos Anéis",  "J.R.R. Tolkien"),
        new Livro(2, "O Hobbit",             "J.R.R. Tolkien"),
        new Livro(3, "O Silmarillion",       "J.R.R. Tolkien"),
        new Livro(4, "Contos Inacabados",    "J.R.R. Tolkien")
    };
 
    public IEnumerable<Livro> GetLivros() => _livros;
    public Livro? GetLivroPorId(int id) => _livros.FirstOrDefault(l => l.Id == id);
}
```
 
> **Observe:** com apenas algumas linhas o Hot Chocolate já expõe um endpoint GraphQL completo, com schema gerado automaticamente a partir das suas classes C#. Sem controllers, sem atributos de rota, sem Swagger manual.
 
---
 
## O que você vai precisar
 
> **Atenção:** o tutorial a seguir é desenvolvido exclusivamente com o **Visual Studio Community**. Se você preferir utilizar **Node.js** (por exemplo, com `graphql-yoga` ou `apollo-server`), isso é permitido, mas o roteiro e os exemplos de código abaixo foram escritos somente para o .NET. Você estará por conta própria na configuração do ambiente Node.js.
 
| Ferramenta                | Versão / Observação                          |
|---------------------------|----------------------------------------------|
| Visual Studio Community   | 2022 ou superior                             |
| .NET SDK                  | 8.0 (instalado junto com o VS)               |
| HotChocolate.AspNetCore   | 16.x (via NuGet)                             |
| Navegador moderno         | Para acessar o Banana Cake Pop               |
 
Durante a instalação do Visual Studio (ou via **Ferramentas → Obter Ferramentas e Recursos**), selecione a carga de trabalho **"Desenvolvimento Web e ASP.NET"**.
 
---
 
## Passo a Passo — Visual Studio Community
 
### Passo 1 — Criar o Projeto
 
1. Abra o Visual Studio Community e clique em **"Criar um novo projeto"**.
2. Na caixa de pesquisa, procure por **"ASP.NET Core Web API"** e selecione o template. Clique em "Avançar".
3. Dê o nome **`GraphQLProductsApi`** ao projeto e escolha uma pasta. Clique em "Avançar".
4. Na tela de configurações adicionais, selecione **.NET 8.0**, desmarque **"Usar controladores"** e desmarque "Habilitar suporte ao OpenAPI". Clique em **"Criar"**.
> **Dica:** se preferir, crie via terminal com `dotnet new web -n GraphQLProductsApi`, mas o tutorial usará o Visual Studio.
 
---
 
### Passo 2 — Instalar o HotChocolate via NuGet
 
1. No menu superior, clique em **Projeto → Gerenciar Pacotes NuGet…**
2. Na aba "Procurar", pesquise por **`HotChocolate.AspNetCore`**.
3. Selecione o pacote publicado pela **ChilliCream** e instale a versão mais recente estável (16.x). Aceite as licenças.
4. Verifique que o `.csproj` contém:
```xml
<PackageReference Include="HotChocolate.AspNetCore" Version="16.*" />
```
 
---
 
### Passo 3 — Criar o Modelo de Dados
 
1. Clique com o botão direito no projeto → **Adicionar → Nova Pasta** → nomeie como `Models`.
2. Dentro de `Models`, clique com o botão direito → **Adicionar → Novo Item → Classe** → nomeie como `Produto.cs`.
3. Substitua o conteúdo pelo código abaixo:
```csharp
namespace GraphQLProductsApi.Models;
 
public record Produto(
    int     Id,
    string  Nome,
    string  Categoria,
    decimal Preco
);
```
 
---
 
### Passo 4 — Criar a Query (Leitura)
 
A `Query` é a classe que expõe as operações de leitura do GraphQL (equivalente aos GETs do REST).
 
1. Crie uma pasta chamada `GraphQL` na raiz do projeto.
2. Dentro de `GraphQL`, crie o arquivo `Query.cs`:
```csharp
using GraphQLProductsApi.Models;
 
namespace GraphQLProductsApi.GraphQL;
 
public class Query
{
    // Dados em memória — na prática viriam de um repositório / banco de dados
    private static readonly List<Produto> _produtos = new()
    {
        new Produto(1, "Notebook Dell",    "Eletrônicos", 4500.00m),
        new Produto(2, "Mouse Logitech",   "Periféricos",   89.90m),
        new Produto(3, "Teclado Mecânico", "Periféricos",  249.00m),
        new Produto(4, "Monitor 27\"",      "Eletrônicos", 1299.00m),
    };
 
    // Expõe a lista para ser compartilhada com Mutation
    public static List<Produto> Produtos => _produtos;
 
    // Query: buscar todos os produtos
    public IEnumerable<Produto> GetProdutos() => _produtos;
 
    // Query: buscar produto por ID
    public Produto? GetProduto(int id) =>
        _produtos.FirstOrDefault(p => p.Id == id);
 
    // Query: buscar por categoria
    public IEnumerable<Produto> GetProdutosPorCategoria(string categoria) =>
        _produtos.Where(p => p.Categoria.Equals(categoria,
            StringComparison.OrdinalIgnoreCase));
}
```
 
---
 
### Passo 5 — Criar a Mutation (Escrita)
 
A `Mutation` é a classe que expõe operações de escrita (equivalente a POST, PUT e DELETE do REST).
 
1. Dentro da pasta `GraphQL`, crie o arquivo `Mutation.cs`:
```csharp
using GraphQLProductsApi.Models;
 
namespace GraphQLProductsApi.GraphQL;
 
public class Mutation
{
    private static readonly List<Produto> _produtos = Query.Produtos;
 
    // Mutation: adicionar produto
    public Produto AdicionarProduto(string nome, string categoria, decimal preco)
    {
        var novoProduto = new Produto(
            Id:        _produtos.Count + 1,
            Nome:      nome,
            Categoria: categoria,
            Preco:     preco
        );
        _produtos.Add(novoProduto);
        return novoProduto;
    }
 
    // Mutation: remover produto
    public bool RemoverProduto(int id)
    {
        var produto = _produtos.FirstOrDefault(p => p.Id == id);
        if (produto is null) return false;
        _produtos.Remove(produto);
        return true;
    }
}
```
 
> **Dica:** em uma aplicação real, extraia a lista para um repositório singleton registrado no contêiner de DI (`builder.Services.AddSingleton<ProdutoRepository>()`) e injete-o via construtor em ambas as classes.
 
---
 
### Passo 6 — Configurar o Program.cs
 
Abra o arquivo `Program.cs` e substitua o conteúdo pelo código abaixo:
 
```csharp
using GraphQLProductsApi.GraphQL;
 
var builder = WebApplication.CreateBuilder(args);
 
// Registra o servidor GraphQL com Query e Mutation
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();
 
var app = builder.Build();
 
// Mapeia o endpoint /graphql
app.MapGraphQL();
app.UseHttpsRedirection();
app.Run();
```
 
---
 
### Passo 7 — Executar e Testar com o Banana Cake Pop
 
1. Pressione **F5** (ou clique no botão ▶) para executar a aplicação.
2. Abra o navegador e acesse **`https://localhost:{porta}/graphql`** — o Hot Chocolate abrirá o **Banana Cake Pop**, uma interface gráfica de testes integrada, semelhante ao Postman, mas especializada para GraphQL.
3. Teste as operações abaixo:
**Listar todos os produtos (Query):**
```graphql
query {
  produtos {
    id
    nome
    categoria
    preco
  }
}
```
 
**Buscar produto por ID (Query):**
```graphql
query {
  produto(id: 2) {
    nome
    preco
  }
}
```
 
**Adicionar produto (Mutation):**
```graphql
mutation {
  adicionarProduto(nome: "Webcam HD", categoria: "Periféricos", preco: 199.90) {
    id
    nome
    preco
  }
}
```
 
**Remover produto (Mutation):**
```graphql
mutation {
  removerProduto(id: 1)
}
```
 
> **Lembre-se:** no GraphQL você só recebe os campos que pediu. Tente remover ou adicionar campos na query e observe que a resposta muda — sem nenhuma alteração no servidor!
 
---
 
## Estrutura Esperada do Projeto
 
```
GraphQLProductsApi/
├── GraphQL/
│   ├── Query.cs          ← Operações de leitura (buscar produtos)
│   └── Mutation.cs       ← Operações de escrita (criar, remover)
├── Models/
│   └── Produto.cs        ← Modelo de dados
├── Program.cs            ← Ponto de entrada e registro do GraphQL
└── GraphQLProductsApi.csproj
```
 
| Arquivo        | Responsabilidade                                          |
|----------------|-----------------------------------------------------------|
| `Produto.cs`   | Define a estrutura (schema) do objeto Produto             |
| `Query.cs`     | Expõe operações de leitura — getAll, getById, getByCategory |
| `Mutation.cs`  | Expõe operações de escrita — add, remove                  |
| `Program.cs`   | Registra o servidor GraphQL e mapeia o endpoint `/graphql`|
 
---
 
## Entrega
 
### Subindo o projeto no GitHub
 
1. Acesse [github.com](https://github.com) e crie uma conta caso ainda não tenha.
2. Clique em **"New repository"**, nomeie como `graphql-products-api` e deixe como repositório **público**.
3. No terminal, dentro da pasta do projeto, execute:
```bash
git init
git add .
git commit -m "feat: API GraphQL com Hot Chocolate - Semana 9"
git branch -M main
git remote add origin https://github.com/seu-usuario/graphql-products-api.git
git push -u origin main
```
 
4. Crie um arquivo `README.md` na raiz do projeto descrevendo o que a API faz e como executá-la.
5. Copie a URL do repositório (ex: `https://github.com/seu-usuario/graphql-products-api`).
### Envio pelo Relatório de Aula
 
No relatório de aula disponível no ambiente virtual, preencha **apenas o link do repositório público no GitHub**. Não é necessário enviar arquivos compactados ou prints. Certifique-se de que o repositório está **público** para que o professor consiga acessá-lo.
 
**Checklist antes de enviar:**
- [ ] Repositório está público no GitHub
- [ ] O projeto compila e executa sem erros
- [ ] A Query com ao menos dois resolvers funciona no Banana Cake Pop
- [ ] A Mutation com ao menos um resolver funciona no Banana Cake Pop
- [ ] O `README.md` explica como rodar o projeto
---

## Dicas Finais
 
- Analise o projeto de exemplo do professor antes de começar — entender o código mínimo ajuda muito.
- Use o **Banana Cake Pop** para explorar o schema da sua API — ele mostra todos os campos disponíveis automaticamente.
- Se travar em algum passo, consulte a documentação oficial: [chillicream.com/docs/hotchocolate](https://chillicream.com/docs/hotchocolate).
- No GraphQL, o schema é gerado a partir das suas classes C#. Se um campo não aparece no schema, verifique se a propriedade é pública.
- Compartilhe dados entre `Query` e `Mutation` usando injeção de dependência com `AddSingleton` — evite listas estáticas em produção.
---
 
*Programação Back-end — Semana 9*
