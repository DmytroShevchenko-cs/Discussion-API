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
    [Route("api/comments/{commentId:int:min(1)}")]
    [ProducesResponseType(typeof(Result), 200)]
    public async Task<IActionResult> DeleteCommentAsync([FromRoute] int commentId)
    {
        var result = await commentService.DeleteCommentAsync(commentId);
        return FromResult(result);
    }
    
    [HttpGet]
    [SwaggerOperation("Get comments")]
    [Produces("application/json")]
    [Route("api/comments")]
    [ProducesResponseType(typeof(Result<GetCommentsResponseDTO>), 200)]
    public async Task<IActionResult> GetCommentsAsync([FromQuery] GetCommentsRequestDTO request)
    {
        var result = await commentService.GetCommentsAsync(request);
        return FromResult(result);
    }
}