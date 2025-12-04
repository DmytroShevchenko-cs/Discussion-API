namespace Discussion.Core.Queries.Comments.GetComments;

using Infrastructure.Common.CQRS;
using Infrastructure.Common.Result;
using Infrastructure.Models.Common;

public sealed class GetCommentsQuery : PaginationModel, IQuery<Result<GetCommentsQueryResult>>
{
}