using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers;

public class RestoreReadDbController : MyControllerBase<RestoreReadDbController>
{
    public RestoreReadDbController(ILogger<RestoreReadDbController> logger, ICommandDispatcher commandDispatcher) 
        : base(logger, commandDispatcher)
    {
    }

    [HttpPost]
    public async Task<ActionResult> RestoreReadDbAsync()
    {
        try{
            await _commandDispatcher.SendAsync(new RestoreReadDbCommand());

            return StatusCode(StatusCodes.Status201Created, new BaseResponse
            {
                Message = "Read database restore request completed successfully!"
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
        catch(Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to restore read database!";
            _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = SAFE_ERROR_MESSAGE
            });
        }
    }
}
