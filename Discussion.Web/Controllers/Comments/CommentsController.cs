namespace Discussion.Web.Controllers.Comments;

using Base;
using Core.DTO.Comment.GetComments;
using Core.Infrastructure.Result;
using Core.Services.CommentService;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

public sealed class CommentsController(
    ICommentService commentService
) : BaseApiController
{
    [HttpPost]
    [SwaggerOperation("Add new comment")]
    [Produces("application/json")]
    [Route("api/comments")]
    [ProducesResponseType(typeof(Result<int>), 200)]
    public async Task<IActionResult> AddNewCommentAsync()
    {

        return FromResult(null);
    }
    
    [HttpDelete]
    [SwaggerOperation("Add new comment")]
    [Produces("application/json")]
    [Route("api/comments")]
    [ProducesResponseType(typeof(Result), 200)]
    public async Task<IActionResult> DeleteCommentAsync()
    {

        return FromResult(null);
    }
    
    [HttpGet]
    [SwaggerOperation("Add new comment")]
    [Produces("application/json")]
    [Route("api/comments")]
    [ProducesResponseType(typeof(Result<GetCommentsResponseDTO>), 200)]
    public async Task<IActionResult> GetCommentsAsync()
    {

        return FromResult(null);
    }
}