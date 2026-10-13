using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogoJogos.Api.Models;

// As DataAnnotations ([Required], [Range]) fazem o [ApiController] devolver 400 automaticamente
public class Jogo
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("titulo")]
    [Required(ErrorMessage = "O título é obrigatório.")]
    public string Titulo { get; set; } = null!;

    [BsonElement("plataforma")]
    [Required(ErrorMessage = "A plataforma é obrigatória.")]
    public string Plataforma { get; set; } = null!;

    [BsonElement("genero")]
    [Required(ErrorMessage = "O gênero é obrigatório.")]
    public string Genero { get; set; } = null!;

    [BsonElement("preco")]
    [BsonRepresentation(BsonType.Decimal128)]
    [Range(0, double.MaxValue, ErrorMessage = "O preço não pode ser negativo.")]
    public decimal Preco { get; set; }

    [BsonElement("anoLancamento")]
    public int AnoLancamento { get; set; }

    [BsonElement("estoque")]
    [Range(0, int.MaxValue, ErrorMessage = "O estoque não pode ser negativo.")]
    public int Estoque { get; set; }
}
