using Xunit;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;

public class TransacaoIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TransacaoIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Deve_Criar_Transacao_Valida()
    {
        var pessoaResponse = await _client.PostAsJsonAsync("/pessoas", new
        {
            nome = "Teste",
            dataNascimento = "2000-01-01"
        });

        var pessoaId = await pessoaResponse.Content.ReadFromJsonAsync<Guid>();

        var categoriaResponse = await _client.PostAsJsonAsync("/categorias", new
        {
            descricao = "Salário",
            finalidade = 1 
        });

        var categoriaId = await categoriaResponse.Content.ReadFromJsonAsync<Guid>();

        var response = await _client.PostAsJsonAsync("/transacoes", new
        {
            descricao = "Receita teste",
            valor = 100,
            tipo = 1,
            pessoaId = pessoaId,
            categoriaId = categoriaId,
            data = DateTime.Now
        });

        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task BUG_Deve_Nao_Permitir_Receita_Para_Menor()
    {
        var pessoaResponse = await _client.PostAsJsonAsync("/pessoas", new
        {
            nome = "Menor",
            dataNascimento = "2012-01-01"
        });

        var pessoaId = await pessoaResponse.Content.ReadFromJsonAsync<Guid>();

        var categoriaResponse = await _client.PostAsJsonAsync("/categorias", new
        {
            descricao = "Salário",
            finalidade = 1
        });

        var categoriaId = await categoriaResponse.Content.ReadFromJsonAsync<Guid>();

        var response = await _client.PostAsJsonAsync("/transacoes", new
        {
            descricao = "Receita inválida",
            valor = 100,
            tipo = 1,
            pessoaId = pessoaId,
            categoriaId = categoriaId,
            data = DateTime.Now
        });

        response.IsSuccessStatusCode.Should().BeFalse();
    }
}