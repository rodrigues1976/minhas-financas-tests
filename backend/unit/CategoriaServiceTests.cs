using Xunit;
using Moq;
using FluentAssertions;
using MinhasFinancas.Application.Services;
using MinhasFinancas.Domain.Interfaces;
using MinhasFinancas.Application.DTOs;

public class CategoriaServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly CategoriaService _service;

    public CategoriaServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _service = new CategoriaService(_uowMock.Object);
    }

    [Fact]
    public async Task Deve_Criar_Categoria()
    {
        var dto = new CreateCategoriaDto
        {
            Descricao = "Alimentação",
            Finalidade = TipoCategoria.Despesa
        };

        var result = await _service.CreateAsync(dto);

        result.Descricao.Should().Be("Alimentação");
    }
}