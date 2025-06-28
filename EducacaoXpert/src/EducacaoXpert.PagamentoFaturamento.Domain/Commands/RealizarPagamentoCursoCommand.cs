﻿using EducacaoXpert.Core.Messages;
using FluentValidation;

namespace EducacaoXpert.PagamentoFaturamento.Domain.Commands;

public class RealizarPagamentoCursoCommand(Guid alunoId, Guid cursoId, string cvvCartao, string expiracaoCartao, string nomeCartao, string numeroCartao, int valor) : Command
{
    public Guid CursoId { get; set; } = cursoId;
    public Guid AlunoId { get; set; } = alunoId;
    public int Valor { get; set; } = valor;
    public string NomeCartao { get; set; } = nomeCartao;
    public string NumeroCartao { get; set; } = numeroCartao;
    public string ExpiracaoCartao { get; set; } = expiracaoCartao;
    public string CvvCartao { get; set; } = cvvCartao;

    public override bool EhValido()
    {
        ValidationResult = new RealizarPagamentoCursoCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}
public class RealizarPagamentoCursoCommandValidation : AbstractValidator<RealizarPagamentoCursoCommand>
{
    public static string AlunoIdErro = "O campo AlunoId é obrigatório.";
    public static string CursoIdErro = "O campo CursoId é obrigatório.";
    public static string CvvCartaoErro = "O campo CVV do Cartão é obrigatório.";
    public static string ExpiracaoCartaoErro = "O campo Expiração do Cartão é obrigatório.";
    public static string NumeroCartaoErro = "O campo Número do Cartão é obrigatório.";
    public static string NomeCartaoErro = "O campo Nome do Cartão é obrigatório.";
    public static string ValorErro = "O campo Valor deve ser maior que zero.";
    public static string NumeroCartaoInvalidoErro = "O número do cartão de crédito é inválido.";

    public RealizarPagamentoCursoCommandValidation()
    {
        RuleFor(x => x.CursoId)
            .NotEqual(Guid.Empty)
            .WithMessage(CursoIdErro);
        RuleFor(x => x.AlunoId)
            .NotEqual(Guid.Empty)
            .WithMessage(AlunoIdErro);
        RuleFor(x => x.NomeCartao)
            .NotEmpty()
            .WithMessage(NomeCartaoErro);
        RuleFor(x => x.NumeroCartao)
            .NotEmpty()
            .WithMessage(NumeroCartaoErro)
            .CreditCard()
            .WithMessage(NumeroCartaoInvalidoErro);
        RuleFor(x => x.ExpiracaoCartao)
            .NotEmpty()
            .WithMessage(ExpiracaoCartaoErro);
        RuleFor(x => x.CvvCartao)
            .NotEmpty()
            .WithMessage(CvvCartaoErro);
        RuleFor(x => x.Valor)
            .GreaterThan(0)
            .WithMessage(ValorErro);
    }
}
