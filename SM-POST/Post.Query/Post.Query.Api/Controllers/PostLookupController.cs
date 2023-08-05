using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Common.DTOs;
using Post.Query.Api.Controllers.BaseController;
using Post.Query.Api.DTOs;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Controllers;

public class PostLookupController : MyControllerBase<PostLookupController>
{
    public PostLookupController(ILogger<PostLookupController> logger, IQueryDispatcher<PostEntity> queryDispatcher) 
        : base(logger, queryDispatcher)
    {
    }

    [HttpGet]
    public async Task<ActionResult> GetAllPostAsync()
    {
        try 
        {
            var posts = await _queryDispatcher.SendAsync(new FindAllPostsQuery());

            if (posts is null || !posts.Any()) return NoContent();

            var count = posts.Count;

            return Ok(new PostLookupResponse
            {
                Posts = posts,
                Message = $"Successfully return {count} post{(count > 1 ? "s" : string.Empty)}!"
            });
        }
        catch(Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve all posts!";
            return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
        }
    }

    [HttpGet("byId/{postId}")]
    public async Task<ActionResult> GetByPostIdAsync(Guid postId)
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostByIdQuery { Id = postId });
            return NormalResponse(posts);

        }
        catch (Exception ex)
        {            
            const string SAFE_ERROR_MESSAGE = "Error while processing request to find post by ID!";
            return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
        }
    }

    [HttpGet("byAuthor/{author}")]
    public async Task<ActionResult> GetPostsByAuthorAsync(string author)
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostsByAuthorQuery { Author = author });
            return NormalResponse(posts);

        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to find posts by author!";
            return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
        }
    }


    [HttpGet("withComments")]
    public async Task<ActionResult> GetPostsWithCommentsAsync()
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostsWithCommentsQuery());
            return NormalResponse(posts);
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to find posts with comments!";
            return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
        }
    }
    
    [HttpGet("withLikes/{numberOfLikes}")]
    public async Task<ActionResult> GetPostsWithLikesAsync(int numberOfLikes)
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostsWithLikesQuery { NumberOfLikes = numberOfLikes });
            return NormalResponse(posts);
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to find posts with likes!";
            return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
        }
    }

    private ActionResult NormalResponse(List<PostEntity>? posts)
    {
        if (posts is null || !posts.Any()) return NoContent();

        var count = posts.Count;
        return Ok(new PostLookupResponse
        {
            Posts = posts,
            Message = $"Successfully returned {count} post{(count > 1 ? "s" : string.Empty)}!"
        });
    }

    private ActionResult ErrorResponse(Exception ex, string safeErrorMessage)
    {
        _logger.LogError(ex, safeErrorMessage);

        return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
        {
            Message = safeErrorMessage
        });
    }
}
