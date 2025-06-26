using MediatR;

namespace EducacaoXpert.Core.Messages;

public abstract class Event : Message, INotification
{
    public DateTime Timestamp { get; protected set; }
    protected Event()
    {
        Timestamp = DateTime.Now;
    }
}
