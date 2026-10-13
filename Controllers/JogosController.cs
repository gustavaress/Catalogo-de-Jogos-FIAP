using CatalogoJogos.Api.Dtos;
using CatalogoJogos.Api.Models;
using CatalogoJogos.Api.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace CatalogoJogos.Api.Controllers;

[ApiController]
[Route("api/jogos")]
public class JogosController : ControllerBase
{
	private readonly JogoService _service;

	public JogosController(JogoService service)
	{
		_service = service;
	}

	// ===== Parte do Gustavo =====

	[HttpGet]
	public async Task<ActionResult<List<Jogo>>> ObterTodos()
	{
		var jogos = await _service.ObterTodosAsync();
		return Ok(jogos);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<Jogo>> ObterPorId(string id)
	{
		if (!IdValido(id))
			return BadRequest("Id inválido: informe um ObjectId com 24 caracteres hexadecimais.");

		var jogo = await _service.ObterPorIdAsync(id);

		if (jogo is null)
			return NotFound();

		return Ok(jogo);
	}

	[HttpPost]
	public async Task<ActionResult<Jogo>> Criar(Jogo jogo)
	{
		var criado = await _service.CriarAsync(jogo);
		return CreatedAtAction(nameof(ObterPorId), new { id = criado.Id }, criado);
	}

	// ===== Parte do Henrique =====

	[HttpPut("{id}")]
	public async Task<ActionResult<Jogo>> Atualizar(string id, Jogo jogo)
	{
		if (!IdValido(id))
			return BadRequest("Id inválido: informe um ObjectId com 24 caracteres hexadecimais.");

		var atualizado = await _service.AtualizarAsync(id, jogo);

		if (!atualizado)
			return NotFound();

		return Ok(jogo);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Remover(string id)
	{
		if (!IdValido(id))
			return BadRequest("Id inválido: informe um ObjectId com 24 caracteres hexadecimais.");

		var removido = await _service.RemoverAsync(id);

		if (!removido)
			return NotFound();

		return NoContent();
	}

	[HttpGet("busca")]
	public async Task<ActionResult<List<Jogo>>> Buscar([FromQuery] string? plataforma, [FromQuery] decimal? precoMaximo)
	{
		if (string.IsNullOrWhiteSpace(plataforma))
			return BadRequest("Informe a plataforma.");

		if (precoMaximo is null || precoMaximo < 0)
			return BadRequest("Informe um precoMaximo maior ou igual a zero.");

		var jogos = await _service.BuscarPorPlataformaEPrecoAsync(plataforma, precoMaximo.Value);
		return Ok(jogos);
	}

	[HttpGet("relatorio-estoque")]
	public async Task<ActionResult<List<RelatorioEstoquePlataformaDto>>> ObterRelatorioEstoque()
	{
		var relatorio = await _service.ObterRelatorioEstoqueAsync();
		return Ok(relatorio);
	}

	// Um id que não é ObjectId faria o driver lançar FormatException (500); aqui vira 400
	private static bool IdValido(string id) => ObjectId.TryParse(id, out _);
}