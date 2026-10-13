namespace CatalogoJogos.Api.Settings;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string JogosCollectionName { get; set; } = null!;
}