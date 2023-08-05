using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers;

public class EditCommentController : MyControllerBase<EditCommentController>
{
    public EditCommentController(ILogger<EditCommentController> logger, ICommandDispatcher commandDispatcher) 
        : base(logger, commandDispatcher)
    {
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditCommentAsync(Guid id, EditCommentCommand command)
    {
        try {
            command.Id = id;
            await _commandDispatcher.SendAsync(command);

            return Ok(new BaseResponse
            {
                Message = "Edit comment request completed successfully!"
            });
        }
        catch(InvalidOperationException ex) 
        {
            _logger.Log(LogLevel.Warning, ex, "Client made a bad request!");
            return BadRequest(new BaseResponse
            {
                Message = ex.Message
            });
        }
        catch(AggregateNotFoundException ex) 
        {
            _logger.Log(LogLevel.Warning, ex, "Could not retrieve aggregate, client passed an incorrect post ID targeting the aggregate!");
            return BadRequest(new BaseResponse
            {
                Message = ex.Message
            });
        }
        catch(Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to edit a comment on a post!";
            _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = SAFE_ERROR_MESSAGE
            });
        }
    }
}
