var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddGraphQLServer().AddQueryType<Query>();

var app = builder.Build();

app.MapGraphQL();
app.UseHttpsRedirection();
app.Run();

public record Livro(int Id, string Titulo, string Autor);

public class Query
{
    private readonly List<Livro> _livros = new()
    {
        new Livro(1, "O Senhor dos Anéis", "J.R.R. Tolkien"),
        new Livro(2, "O Hobbit", "J.R.R. Tolkien"),
        new Livro(3, "O Silmarillion", "J.R.R. Tolkien"),
        new Livro(4, "Contos Inacabados", "J.R.R. Tolkien")
    };

    public IEnumerable<Livro> GetLivros() => _livros;

    public Livro? GetLivroPorId(int id) => _livros.FirstOrDefault(l => l.Id == id);
}