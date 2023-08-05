using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.Consumers;
using Post.Query.Infrastructure.DataAccess;
using Post.Query.Infrastructure.Dispatchers;
using Post.Query.Infrastructure.Handlers;
using Post.Query.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

Action<DbContextOptionsBuilder> configureDbContext = 
    o => o.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
builder.Services.AddDbContext<DatabaseContext>(configureDbContext);
builder.Services.AddSingleton(new DatabaseContextFactory(configureDbContext));

// create database and tables from code
builder.Services.AddOptions<DatabaseContext>().Configure((options) => options.Database.EnsureCreated());

// var dataContext = builder.Services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
// dataContext.Database.EnsureCreated();

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IQueryHandler, QueryHandler>();
builder.Services.AddScoped<IEventHandler, Post.Query.Infrastructure.Handlers.EventHandler>();
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
builder.Services.AddScoped<IEventConsumer, EventConsumer>();

builder.Services.AddHostedService<ConsumerHostedService>();

var dispatcher = new QueryDispatcher();
builder.Services.AddOptions<IQueryHandler>().Configure((queryHandler) => 
{
    dispatcher.RegisterHandler<FindAllPostsQuery>(queryHandler.HandlerAsync);
    dispatcher.RegisterHandler<FindPostByIdQuery>(queryHandler.HandlerAsync);
    dispatcher.RegisterHandler<FindPostsByAuthorQuery>(queryHandler.HandlerAsync);
    dispatcher.RegisterHandler<FindPostsWithCommentsQuery>(queryHandler.HandlerAsync);
    dispatcher.RegisterHandler<FindPostsWithLikesQuery>(queryHandler.HandlerAsync);
});
builder.Services.AddSingleton<IQueryDispatcher<PostEntity>>(_ => dispatcher);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
