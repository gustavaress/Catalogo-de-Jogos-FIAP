using CatalogoJogos.Api.Dtos;
using CatalogoJogos.Api.Models;

namespace CatalogoJogos.Api.Repositories;

public interface IJogoRepository
{
    Task<List<Jogo>> ObterTodosAsync();
    Task<Jogo?> ObterPorIdAsync(string id);
    Task<Jogo> CriarAsync(Jogo jogo);
    Task<bool> AtualizarAsync(string id, Jogo jogo);
    Task<bool> RemoverAsync(string id);
    Task<List<Jogo>> BuscarPorPlataformaEPrecoAsync(string plataforma, decimal precoMaximo);
    Task<List<RelatorioEstoquePlataformaDto>> ObterRelatorioEstoqueAsync();
}