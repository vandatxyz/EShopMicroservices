using MediatR;

namespace Ordering.Domain.Abstractions
{
    public interface IDomainEvent :  INotification
    {
        Guid EvenId => Guid.NewGuid();
        DateTime OccurredOn => DateTime.Now;
        public string? EventType => GetType().AssemblyQualifiedName;
    }
}
