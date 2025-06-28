namespace EducacaoXpert.Api.ViewModels;

public class LoginResponseWrapper
{
    public bool Sucesso { get; set; }
    public LoginResponseViewModel Data { get; set; } = new();
}
