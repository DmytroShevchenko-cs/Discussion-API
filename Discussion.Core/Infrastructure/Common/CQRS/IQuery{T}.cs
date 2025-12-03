namespace Discussion.Core.Infrastructure.Common.CQRS;

using MediatR;

public interface IQuery<out T> : IRequest<T>
{
}