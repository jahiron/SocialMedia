using Microsoft.Extensions.Options;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _options;

        public PostService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options)
        {
            _unitOfWork = unitOfWork;
            _options = options.Value;
        }
        public PagedList<Post> GetPosts(PostQueryFilter filter)
        {
            filter.PageNumber = filter.PageNumber == 0 ? _options.DefaultPageNumber : filter.PageNumber;
            filter.PageSize = filter.PageSize == 0 ? _options.DefaultPageSize : filter.PageSize;

            var posts = _unitOfWork.PostRepository.GetAll();

            if(filter.UserId != 0)
            {
                posts = posts.Where(x => x.UserId == filter.UserId);
            }

            if (!string.IsNullOrWhiteSpace(filter.Description))
            {
                posts = posts.Where(x => x.Description.ToLower().Contains(filter.Description.ToLower()));
            }


            var pagedList = PagedList<Post>.Create(posts.ToList(), filter.PageNumber, filter.PageSize);

            return pagedList;
        }
        public Task<Post> GetPost(int postId)
        {
            return _unitOfWork.PostRepository.GetById(postId);
        }
        
        public async Task<Post> InsertPost(Post post)
        {
            var user = await _unitOfWork.UserRepository.GetById(post.UserId);
            
            //verify user exists
            if(user == null)
            {
                throw new BusinessException("User doesn't exist");
            }

            //posts can't contain the word sex
            if (post.Description.ToLower().Contains("sex"))
            {
                throw new BusinessException("post can't contain the word sex");
            }

            //verify users with more than 10 posts can't post in the last 7 days
            int minPostLimit = 10;
            var userPosts = await _unitOfWork.PostRepository.GetPostsByUser(post.UserId); 
            if(userPosts.Count() < minPostLimit)
            {
                var lastPost = userPosts.OrderByDescending(x => x.Date).FirstOrDefault();
                if(lastPost != null && (DateTime.Now - lastPost.Date).TotalDays < 7)
                {
                    throw new BusinessException("The user has reached the maximum number of posts per user");
                }   
            }

            await _unitOfWork.PostRepository.Add(post);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0 ? post : null;
        }
        public async Task<bool> UpdatePost(Post post)
        {
            _unitOfWork.PostRepository.Update(post);
            var updated = await _unitOfWork.SaveChangesAsync();
            return updated > 0;
        }
        public async Task<bool> DeletePost(int postId)
        {
            await _unitOfWork.PostRepository.Delete(postId);
            var deleted = await _unitOfWork.SaveChangesAsync();
            return deleted > 0;
        }       
    }
}
