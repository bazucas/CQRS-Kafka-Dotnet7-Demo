using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Controllers.BaseController;

public abstract class MyControllerBase<T> : ControllerBase where T : ControllerBase
{
    protected readonly ILogger<T> _logger;
    protected readonly IQueryDispatcher<PostEntity> _queryDispatcher;

    public MyControllerBase(ILogger<T> logger, IQueryDispatcher<PostEntity> queryDispatcher)
    {
        _logger = logger;
        _queryDispatcher = queryDispatcher;
    }  
}