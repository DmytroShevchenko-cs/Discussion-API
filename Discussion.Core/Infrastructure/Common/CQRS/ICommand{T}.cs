namespace Discussion.Core.Infrastructure.Common.CQRS;

using MediatR;

public interface ICommand<out T> : IRequest<T>
{
}