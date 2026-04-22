using Xunit;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;

public class PessoaIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PessoaIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Deve_Excluir_Transacoes_Ao_Excluir_Pessoa()
    {
        var pessoaResponse = await _client.PostAsJsonAsync("/pessoas", new
        {
            nome = "Teste",
            dataNascimento = "2000-01-01"
        });

        var pessoaId = await pessoaResponse.Content.ReadFromJsonAsync<Guid>();

        var categoriaResponse = await _client.PostAsJsonAsync("/categorias", new
        {
            descricao = "Teste",
            finalidade = 1
        });

        var categoriaId = await categoriaResponse.Content.ReadFromJsonAsync<Guid>();

        await _client.PostAsJsonAsync("/transacoes", new
        {
            descricao = "Despesa",
            valor = 50,
            tipo = 2,
            pessoaId = pessoaId,
            categoriaId = categoriaId,
            data = DateTime.Now
        });

        await _client.DeleteAsync($"/pessoas/{pessoaId}");

        var transacoes = await _client.GetFromJsonAsync<List<dynamic>>("/transacoes");

        transacoes.Should().NotContain(t => t.pessoaId == pessoaId);
    }
}