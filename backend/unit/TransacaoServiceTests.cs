using Xunit;
using Moq;
using FluentAssertions;
using MinhasFinancas.Application.Services;
using MinhasFinancas.Domain.Entities;
using MinhasFinancas.Domain.Interfaces;
using MinhasFinancas.Application.DTOs;

public class TransacaoServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly TransacaoService _service;

    public TransacaoServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _service = new TransacaoService(_uowMock.Object);
    }

    [Fact]
    public async Task Deve_Criar_Transacao_Quando_Dados_Validos()
    {
        var categoria = new Categoria { Id = Guid.NewGuid(), Descricao = "Salário" };
        var pessoa = new Pessoa { Id = Guid.NewGuid(), Nome = "João", DataNascimento = DateTime.Now.AddYears(-30) };

        _uowMock.Setup(x => x.Categorias.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(categoria);

        _uowMock.Setup(x => x.Pessoas.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(pessoa);

        var dto = new CreateTransacaoDto
        {
            Descricao = "Teste",
            Valor = 100,
            Data = DateTime.Now,
            Tipo = TipoTransacao.Receita,
            CategoriaId = categoria.Id,
            PessoaId = pessoa.Id
        };

        var result = await _service.CreateAsync(dto);

        result.Should().NotBeNull();
        result.Descricao.Should().Be("Teste");
    }

    [Fact]
    public async Task Deve_Lancar_Excecao_Quando_Categoria_Nao_Existe()
    {
        _uowMock.Setup(x => x.Categorias.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Categoria?)null);

        var dto = new CreateTransacaoDto
        {
            CategoriaId = Guid.NewGuid(),
            PessoaId = Guid.NewGuid()
        };

        var act = async () => await _service.CreateAsync(dto);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Categoria não encontrada*");
    }

    [Fact]
    public async Task Deve_Lancar_Excecao_Quando_Pessoa_Nao_Existe()
    {
        var categoria = new Categoria { Id = Guid.NewGuid() };

        _uowMock.Setup(x => x.Categorias.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(categoria);

        _uowMock.Setup(x => x.Pessoas.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Pessoa?)null);

        var dto = new CreateTransacaoDto
        {
            CategoriaId = categoria.Id,
            PessoaId = Guid.NewGuid()
        };

        var act = async () => await _service.CreateAsync(dto);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Pessoa não encontrada*");
    }

    [Fact]
    public async Task BUG_Deve_Nao_Permitir_Receita_Para_Menor()
    {
        var categoria = new Categoria { Id = Guid.NewGuid() };

        var pessoa = new Pessoa
        {
            Id = Guid.NewGuid(),
            DataNascimento = DateTime.Now.AddYears(-15)
        };

        _uowMock.Setup(x => x.Categorias.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(categoria);

        _uowMock.Setup(x => x.Pessoas.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(pessoa);

        var dto = new CreateTransacaoDto
        {
            Tipo = TipoTransacao.Receita,
            CategoriaId = categoria.Id,
            PessoaId = pessoa.Id
        };

        var act = async () => await _service.CreateAsync(dto);

        await act.Should().ThrowAsync<Exception>();
    }
}