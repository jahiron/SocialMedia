using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IPostService
    {
        PagedList<Post> GetPosts(PostQueryFilter query);
        Task<Post> GetPost(int PostId);
        Task<Post> InsertPost(Post post);
        Task<bool> UpdatePost(Post post);
        Task<bool> DeletePost(int PostId);
    }
}
