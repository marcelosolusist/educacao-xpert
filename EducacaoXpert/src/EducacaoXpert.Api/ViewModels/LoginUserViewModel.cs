﻿using System.ComponentModel.DataAnnotations;

namespace EducacaoXpert.Api.ViewModels;

public class LoginUserViewModel
{
    [Required(ErrorMessage = "Campo {0} é obrigatório.")]
    [EmailAddress(ErrorMessage = "Campo {0} está em formato inválido.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Campo {0} é obrigatório.")]
    public string? Senha { get; set; }
}
