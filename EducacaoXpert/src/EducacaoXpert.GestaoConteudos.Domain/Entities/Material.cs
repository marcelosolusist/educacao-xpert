﻿using EducacaoXpert.Core.DomainObjects;

namespace EducacaoXpert.GestaoConteudos.Domain.Entities;

public class Material : Entity
{
    public string Nome { get; private set; }
    public string Tipo { get; private set; }
    public Guid AulaId { get; private set; }

    // EF relationship
    public Aula Aula { get; private set; }

    // Ef Constructor
    protected Material() { }
    public Material(string nome, string tipo)
    {
        Nome = nome;
        Tipo = tipo;
        Validar();
    }

    public void AssociarAula(Guid aulaId)
    {
        AulaId = aulaId;
    }

    public void EditarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new DomainException("Nome inválido.");
        Nome = nome;
    }

    public void EditarTipo(string tipo)
    {
        if (string.IsNullOrWhiteSpace(tipo)) throw new DomainException("Tipo inválido.");
        Tipo = tipo;
    }

    public void Validar()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            throw new DomainException("O nome do material é obrigatório.");
        if (string.IsNullOrWhiteSpace(Tipo))
            throw new DomainException("O tipo do material é obrigatório.");
    }
}