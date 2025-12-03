namespace Discussion.Core.Queries.Comments.GetComments;

using Infrastructure.CQRS;using Infrastructure.Models.Common;
using Infrastructure.Result;

public sealed class GetCommentsQuery : PaginationModel, IQuery<Result<GetCommentsResult>>
{
}