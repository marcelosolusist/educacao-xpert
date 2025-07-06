using System.Text.Json;

namespace EducacaoXpert.Api.Tests.Config;

public class RetornoGenericoGet
{
    public bool sucesso { get; set; }
    public List<JsonElement> data { get; set; }
}
