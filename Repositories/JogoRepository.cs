using CatalogoJogos.Api.Dtos;
using CatalogoJogos.Api.Models;
using CatalogoJogos.Api.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CatalogoJogos.Api.Repositories;

public class JogoRepository : IJogoRepository
{
    private readonly IMongoCollection<Jogo> _jogos;

    public JogoRepository(IMongoDatabase database, IOptions<MongoDbSettings> options)
    {
        _jogos = database.GetCollection<Jogo>(options.Value.JogosCollectionName);
    }

    // ===== Parte do Gustavo =====

    public async Task<List<Jogo>> ObterTodosAsync()
    {
        return await _jogos.Find(_ => true).ToListAsync();
    }

    public async Task<Jogo?> ObterPorIdAsync(string id)
    {
        return await _jogos.Find(j => j.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Jogo> CriarAsync(Jogo jogo)
    {
        jogo.Id = null; // garante que o Mongo gere o Id
        await _jogos.InsertOneAsync(jogo);
        return jogo;
    }

    // ===== Parte do Henrique =====

    public async Task<bool> AtualizarAsync(string id, Jogo jogo)
    {
        jogo.Id = id; // o Id da rota prevalece sobre o que vier no corpo
        var resultado = await _jogos.ReplaceOneAsync(j => j.Id == id, jogo);

        // MatchedCount (e não ModifiedCount): um PUT com dados idênticos
        // encontra o documento mas não altera nada, e não deve virar 404
        return resultado.MatchedCount > 0;
    }

    public async Task<bool> RemoverAsync(string id)
    {
        var resultado = await _jogos.DeleteOneAsync(j => j.Id == id);
        return resultado.DeletedCount > 0;
    }

    public async Task<List<Jogo>> BuscarPorPlataformaEPrecoAsync(string plataforma, decimal precoMaximo)
    {
        var builder = Builders<Jogo>.Filter;

        // { plataforma: <plataforma>, preco: { $lte: <precoMaximo> } }
        var filtro = builder.And(
            builder.Eq(j => j.Plataforma, plataforma),
            builder.Lte(j => j.Preco, precoMaximo));

        return await _jogos.Find(filtro).ToListAsync();
    }

    public async Task<List<RelatorioEstoquePlataformaDto>> ObterRelatorioEstoqueAsync()
    {
        // $group por plataforma -> $sum: 1 e $sum: { $multiply: [preco, estoque] }
        return await _jogos.Aggregate()
            .Group(
                j => j.Plataforma,
                g => new RelatorioEstoquePlataformaDto
                {
                    Plataforma = g.Key,
                    QuantidadeTitulos = g.Count(),
                    ValorTotalInventario = g.Sum(j => j.Preco * j.Estoque)
                })
            .SortBy(r => r.Plataforma)
            .ToListAsync();
    }
}