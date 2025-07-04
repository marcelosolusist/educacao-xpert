using EducacaoXpert.Core.DomainObjects;
using EducacaoXpert.Core.DomainObjects.Interfaces;

namespace EducacaoXpert.GestaoConteudos.Domain.Entities;

public class Curso : Entity, IAggregateRoot
{
    public string Nome { get; private set; }
    public string Conteudo { get; private set; }
    public Guid UsuarioCriacaoId { get; private set; }
    public int Preco { get; private set; } //O Preço é em centavos

    private readonly List<Aula> _aulas;
    public IReadOnlyCollection<Aula> Aulas => _aulas;

    private readonly List<ProgressoCurso> _progressoCursos;
    public IReadOnlyCollection<ProgressoCurso> ProgressoCursos => _progressoCursos;

    // Ef Constructor
    protected Curso() { }

    public Curso(string nome, string conteudo, Guid usuarioCriacaoId, int preco)
    {
        Nome = nome;
        Conteudo = conteudo;
        UsuarioCriacaoId = usuarioCriacaoId;
        Preco = preco;
        Validar();
        _aulas = new List<Aula>();
        _progressoCursos = new List<ProgressoCurso>();
    }

    public void IncluirAula(Aula aula)
    {
        if (AulaExistente(aula))
            throw new DomainException("Aula já associada a este curso.");

        aula.AssociarCurso(Id);
        _aulas.Add(aula);
    }

    public void EditarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("O nome do curso é obrigatório.");
        Nome = nome;
    }
    public void EditarConteudo(string conteudo)
    {
        if (string.IsNullOrWhiteSpace(conteudo))
            throw new DomainException("O conteúdo é obrigatório.");
        Conteudo = conteudo;
    }
    public void EditarPreco(int preco)
    {
        if (preco <= 0)
            throw new DomainException("O preço do curso deve ser maior que zero.");
        Preco = preco;
    }

    private void Validar()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            throw new DomainException("O nome do curso é obrigatório.");
        if (string.IsNullOrWhiteSpace(Conteudo))
            throw new DomainException("O conteúdo é obrigatório.");
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