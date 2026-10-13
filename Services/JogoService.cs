using CatalogoJogos.Api.Dtos;
using CatalogoJogos.Api.Models;
using CatalogoJogos.Api.Repositories;

namespace CatalogoJogos.Api.Services;

public class JogoService
{
    private readonly IJogoRepository _repository;

    public JogoService(IJogoRepository repository)
    {
        _repository = repository;
    }

    // ===== Parte do Gustavo =====

    public async Task<List<Jogo>> ObterTodosAsync()
    {
        return await _repository.ObterTodosAsync();
    }

    public async Task<Jogo?> ObterPorIdAsync(string id)
    {
        return await _repository.ObterPorIdAsync(id);
    }

    public async Task<Jogo> CriarAsync(Jogo jogo)
    {
        return await _repository.CriarAsync(jogo);
    }

    // ===== Parte do Henrique =====

    public async Task<bool> AtualizarAsync(string id, Jogo jogo)
    {
        return await _repository.AtualizarAsync(id, jogo);
    }

    public async Task<bool> RemoverAsync(string id)
    {
        return await _repository.RemoverAsync(id);
    }

    public async Task<List<Jogo>> BuscarPorPlataformaEPrecoAsync(string plataforma, decimal precoMaximo)
    {
        return await _repository.BuscarPorPlataformaEPrecoAsync(plataforma, precoMaximo);
    }

    public async Task<List<RelatorioEstoquePlataformaDto>> ObterRelatorioEstoqueAsync()
    {
        return await _repository.ObterRelatorioEstoqueAsync();
    }
}