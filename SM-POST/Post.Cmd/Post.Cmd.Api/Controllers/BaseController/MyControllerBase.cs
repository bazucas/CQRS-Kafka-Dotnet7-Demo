using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Post.Cmd.Api.Controllers;

public abstract class MyControllerBase<T> : ControllerBase where T : ControllerBase
{
    protected readonly ILogger<T> _logger;
    protected readonly ICommandDispatcher _commandDispatcher;

    public MyControllerBase(ILogger<T> logger, ICommandDispatcher commandDispatcher)
    {
        _logger = logger;
        _commandDispatcher = commandDispatcher;
    }  
}