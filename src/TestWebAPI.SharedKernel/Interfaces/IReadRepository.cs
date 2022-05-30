using Ardalis.Specification;

namespace TestWebAPI.SharedKernel.Interfaces {
    public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot {
    }
}