using Xunit;
using Moq;
using FluentAssertions;
using MinhasFinancas.Application.Services;
using MinhasFinancas.Domain.Interfaces;
using MinhasFinancas.Application.DTOs;
using MinhasFinancas.Domain.Entities;

public class PessoaServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly PessoaService _service;

    public PessoaServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _service = new PessoaService(_uowMock.Object);
    }

    [Fact]
    public async Task Deve_Criar_Pessoa()
    {
        var dto = new CreatePessoaDto
        {
            Nome = "João",
            DataNascimento = new DateTime(2000, 1, 1)
        };

        var result = await _service.CreateAsync(dto);

        result.Nome.Should().Be("João");
    }

    [Fact]
    public async Task Deve_Lancar_Excecao_Quando_Pessoa_Nao_Existe_Ao_Atualizar()
    {
        _uowMock.Setup(x => x.Pessoas.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Pessoa?)null);

        var act = async () => await _service.UpdateAsync(Guid.NewGuid(), new UpdatePessoaDto());

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}