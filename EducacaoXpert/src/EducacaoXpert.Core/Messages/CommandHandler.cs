namespace EducacaoXpert.Core.Messages;

public abstract class CommandHandler
{
    protected virtual Task IncluirNotificacao(string type, string descricao, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    protected virtual void IncluirNotificacao(string type, string descricao)
    {
        throw new NotImplementedException();
    }
}
