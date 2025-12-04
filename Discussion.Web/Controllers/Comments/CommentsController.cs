namespace Discussion.Web.Controllers.Comments;

using Base;
using Core.DTO.Comment.AddComment;
using Core.DTO.Comment.GetComments;
using Core.Infrastructure.Common.Result;
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
    [ProducesResponseType(typeof(Result), 200)]
    public async Task<IActionResult> AddNewCommentAsync([FromForm] AddCommentRequestDTO dto)
    {
        var result = await commentService.AddCommentAsync(dto);
        
        return FromResult(result);
    }
    
    [HttpDelete]
    [SwaggerOperation("Deltee comment")]
    [Produces("application/json")]
    [Route("api/comments")]
    [ProducesResponseType(typeof(Result), 200)]
    public async Task<IActionResult> DeleteCommentAsync()
    {

        return FromResult(null);
    }
    
    [HttpGet]
    [SwaggerOperation("Get comments")]
    [Produces("application/json")]
    [Route("api/comments")]
    [ProducesResponseType(typeof(Result<GetCommentsResponseDTO>), 200)]
    public async Task<IActionResult> GetCommentsAsync()
    {

        return FromResult(null);
    }
}