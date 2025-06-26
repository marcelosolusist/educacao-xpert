using EducacaoXpert.Core.Data;
using EducacaoXpert.Core.DomainObjects;

namespace EducacaoXpert.GestaoAlunos.Domain.Interfaces;

public interface IUsuarioRepository : IRepository<Usuario>
{
    void Adicionar(Usuario usuario);
}
