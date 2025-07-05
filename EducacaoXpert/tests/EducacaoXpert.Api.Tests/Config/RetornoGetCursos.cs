using EducacaoXpert.Api.ViewModels;
using System.Text.Json;

namespace EducacaoXpert.Api.Tests.Config;

public class RetornoGetCursos
{
    public bool sucesso { get; set; }
    public List<JsonElement> data { get; set; }
}
