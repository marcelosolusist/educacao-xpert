using Business.Entities;
using Business.Entities.Validations;
using Business.Interfaces;
using Business.Messages;
using Business.Services.Base;

namespace Business.Services
{
    public class AulaService(IAppIdentityUser appIdentityUser,
                                IAulaRepository aulaRepository,
                                INotificador notificador) : BaseService(appIdentityUser, notificador), IAulaService
    {
        public async Task<IEnumerable<Aula>> ListarAtivas()
        {
            var aulas = await aulaRepository.Buscar(predicate: x => x.Ativo, 
                                                                orderBy: x => x);
            return aulas;
        }
        public async Task<Aula?> ObterPorId(Guid id)
        {   
            var aula = await aulaRepository.ObterPorId(id);

            if (aula == null)
            {   
                Notificar(Mensagens.RegistroNaoEncontrado);
                return null;
            }

            return aula;
        }

        public async Task Adicionar(Aula aula)
        {
            if(!ExecutarValidacao(new AulaValidation(), aula)) return;

            aula.UsuarioCriacaoId = UsuarioId;

            await aulaRepository.Adicionar(aula);
        }

        public async Task Atualizar(Aula aula)
        {
            if (!ExecutarValidacao(new AulaValidation(), aula)) return;

            var aulaBanco = await aulaRepository.ObterPorId(aula.Id);

            if (aulaBanco == null)
            {
                Notificar(Mensagens.RegistroNaoEncontrado);
                return;
            }

            await aulaRepository.Atualizar(aula);
        }

        public async Task Excluir(Guid id)
        {
            var aula = await aulaRepository.ObterPorId(id);

            if (aula == null)
            {
                Notificar(Mensagens.RegistroNaoEncontrado);
                return;
            }

            await aulaRepository.Excluir(aula!);
        }
    }
}
