using EducacaoXpert.Core.DomainObjects;
using EducacaoXpert.Core.DomainObjects.Interfaces;

namespace EducacaoXpert.GestaoConteudos.Domain.Entities;

public class Aula : Entity, IAggregateRoot
{
    public string Nome { get; private set; }
    public string Conteudo { get; private set; }
    public Guid CursoId { get; private set; }

    private readonly List<Material> _materiais = [];
    public IReadOnlyCollection<Material> Materiais => _materiais;

    private readonly List<ProgressoAula> _progressoAulas = [];
    public IReadOnlyCollection<ProgressoAula> ProgressoAulas => _progressoAulas;

    // EF relationship
    public Curso? Curso { get; private set; }

    // Ef Constructor
    protected Aula() { }

    public Aula(string nome, string conteudo)
    {
        Nome = nome;
        Conteudo = conteudo;
        Validar();
        _materiais = new List<Material>();
        _progressoAulas = new List<ProgressoAula>();
    }

    public void AssociarCurso(Guid cursoId)
    {
        CursoId = cursoId;
    }

    public void IncluirMaterial(Material material)
    {
        if (MaterialExistente(material))
            throw new DomainException("Material já associado a esta aula.");

        material.AssociarAula(Id);
        _materiais.Add(material);
    }

    public void EditarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("O nome da aula é obrigatório.");
        Nome = nome;
    }
    public void EditarConteudo(string conteudo)
    {
        if (string.IsNullOrWhiteSpace(conteudo))
            throw new DomainException("O conteúdo da aula é obrigatório.");
        Conteudo = conteudo;
    }

    public void EditarMaterial(Material material)
    {
        if (!MaterialExistente(material))
            throw new DomainException("Material não está associado a esta aula.");

        var materialNaLista = _materiais.FirstOrDefault(m => m.Id == material.Id);
        material.AssociarAula(Id);
        _materiais.Remove(materialNaLista);
        _materiais.Add(material);

    }

    public void ExcluirMaterial(Guid idMaterial)
    {
        var materialNaLista = _materiais.FirstOrDefault(m => m.Id == idMaterial);
        if (materialNaLista == null) throw new DomainException("Material não está associado a esta aula.");
        _materiais.Remove(materialNaLista);
    }

    private void Validar()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            throw new DomainException("O nome da aula é obrigatório.");

        if (string.IsNullOrWhiteSpace(Conteudo))
            throw new DomainException("O conteúdo da aula é obrigatório.");
    }

    private bool MaterialExistente(Material material)
    {
        return _materiais.Any(m => m.Id == material.Id);
    }
}