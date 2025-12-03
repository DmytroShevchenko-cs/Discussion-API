namespace Discussion.Core.Infrastructure.CQRS;

using MediatR;

public interface IQuery<out T> : IRequest<T>
{
}