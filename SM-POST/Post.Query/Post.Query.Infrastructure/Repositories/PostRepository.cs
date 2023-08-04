using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    public Task CreateAsync(PostEntity post)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid postId)
    {
        throw new NotImplementedException();
    }

    public Task<PostEntity> GetByIdAsync(Guid postId)
    {
        throw new NotImplementedException();
    }

    public Task<List<PostEntity>> ListAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<PostEntity>> ListByAuthorAsync(string author)
    {
        throw new NotImplementedException();
    }

    public Task<List<PostEntity>> ListWithCommentsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<PostEntity>> ListWithLikesAsync(int numberOfLikes)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(PostEntity post)
    {
        throw new NotImplementedException();
    }
}
