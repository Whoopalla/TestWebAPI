using MediatR;

namespace TestWebAPI.SharedKernel {
    public abstract class BaseDomainEvent : INotification {
        public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
    }
}