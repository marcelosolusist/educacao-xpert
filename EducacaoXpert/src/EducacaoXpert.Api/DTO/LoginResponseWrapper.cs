﻿namespace EducacaoXpert.Api.DTO;

public class LoginResponseWrapper
{
    public bool Sucesso { get; set; }
    public LoginResponseDto Data { get; set; } = new();
}
