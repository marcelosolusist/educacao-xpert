﻿namespace EducacaoXpert.Api.ViewModels;

public class UserTokenViewModel
{
    public string Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public IEnumerable<ClaimViewModel> Claims { get; set; }
}
