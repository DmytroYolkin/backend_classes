using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Text.Json;
using System.Linq;

namespace Ex01.Tests;

public class AuthorTests : IClassFixture<LibraryApiFactory>
{
    private readonly HttpClient _client;

    public AuthorTests(LibraryApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAuthors_ReturnsSeededAuthors()
    {
        var doc = await GraphQlHelper.ExecuteAsync(_client,
            "{ authors { name } }");
        var authors = doc.RootElement
            .GetProperty("data")
            .GetProperty("authors")
            .EnumerateArray()
            .ToList();

        Assert.Equal(2, authors.Count);
    }

    [Fact]
    public async Task AddAuthor_ValidInput_ReturnsNewAuthor()
    {
        var mutation = """
            mutation {
              addAuthor(input: { name: "Neil Gaiman", biography: "English author of novels and comics." }) {
                id
                name
                biography
              }
            }
            """;

        var doc = await GraphQlHelper.ExecuteAsync(_client, mutation);
        var root = doc.RootElement;
        Assert.False(root.TryGetProperty("errors", out var e1),
            $"Unexpected errors: {(root.TryGetProperty("errors", out var e2) ? e2.GetRawText() : "")}");

        var author = root.GetProperty("data").GetProperty("addAuthor");
        Assert.Equal("Neil Gaiman", author.GetProperty("name").GetString());
        Assert.True(author.GetProperty("id").GetInt32() > 0);
    }
}
