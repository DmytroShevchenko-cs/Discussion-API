namespace Discussion.Core.Infrastructure.CQRS;

using MediatR;

public interface ICommand<out T> : IRequest<T>
{
}