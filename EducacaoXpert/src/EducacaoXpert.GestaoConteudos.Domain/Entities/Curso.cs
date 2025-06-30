using EducacaoXpert.Core.DomainObjects;
using EducacaoXpert.Core.DomainObjects.Interfaces;

namespace EducacaoXpert.GestaoConteudos.Domain.Entities;

public class Curso : Entity, IAggregateRoot
{
    public string Nome { get; private set; }
    public string ConteudoProgramatico { get; private set; }
    public Guid UsuarioCriacaoId { get; private set; }
    public int Preco { get; private set; } //O Preço é em centavos

    private readonly List<Aula> _aulas;
    public IReadOnlyCollection<Aula> Aulas => _aulas;

    private readonly List<ProgressoCurso> _progressoCursos;
    public IReadOnlyCollection<ProgressoCurso> ProgressoCursos => _progressoCursos;

    // Ef Constructor
    protected Curso() { }
    public Curso(string nome, string conteudoProgramatico, Guid usuarioCriacaoId, int preco)
    {
        Nome = nome;
        ConteudoProgramatico = conteudoProgramatico;
        UsuarioCriacaoId = usuarioCriacaoId;
        Preco = preco;
        _aulas = [];

        Validar();
    }

    public void AdicionarAula(Aula aula)
    {
        if (AulaExistente(aula))
            throw new DomainException("Aula já associada a este curso.");

        aula.AssociarCurso(Id);
        _aulas.Add(aula);
    }

    public void AtualizarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("O nome do curso é obrigatório.");
        Nome = nome;
    }
    public void AtualizarConteudo(string conteudoProgramatico)
    {
        if (string.IsNullOrWhiteSpace(conteudoProgramatico))
            throw new DomainException("O conteúdo programático é obrigatório.");
        ConteudoProgramatico = conteudoProgramatico;
    }
    public void AtualizarPreco(int preco)
    {
        if (preco <= 0)
            throw new DomainException("O preço do curso deve ser maior que zero.");
        Preco = preco;
    }

    private void Validar()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            throw new DomainException("O nome do curso é obrigatório.");
        if (string.IsNullOrWhiteSpace(ConteudoProgramatico))
            throw new DomainException("O conteúdo programático é obrigatório.");
        if (UsuarioCriacaoId == Guid.Empty)
            throw new DomainException("O ID do usuário criador é obrigatório.");
        if (Preco <= 0)
            throw new DomainException("O preço do curso deve ser maior que zero.");
    }

    private bool AulaExistente(Aula aula)
    {
        return _aulas.Any(a => a.Id == aula.Id);
    }
}