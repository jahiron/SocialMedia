using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.DTOs;
using AutoMapper;
using SocialMedia.Api.Responses;
using SocialMedia.Core.QueryFilters;
using SocialMedia.Core.CustomEntities;
using Newtonsoft.Json;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SocialMedia.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        public PostController(IPostService postService, IMapper mapper, IUriService uriService)
        {
            _postService = postService;
            _mapper = mapper;
            _uriService = uriService;
        }

        /// <summary>
        /// Get all posts
        /// </summary>
        /// <param name="filters">Filters to apply</param>
        /// <returns>List of posts</returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<PagedList<PostDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest) ]
        [HttpGet(Name = nameof(GetAsync))]
        public ApiResponse<PagedList<PostDto>> GetAsync([FromQuery] PostQueryFilter filters)
        {
            var posts = _postService.GetPosts(filters);
            
            var pagination = new Metadata
            {
                CurrentPage = posts.CurrentPage,
                PageSize = posts.PageSize,
                TotalCount = posts.TotalCount,
                TotalPages = posts.TotalPages,
                HasNextPage = posts.HasNextPage,
                HasPreviuosPage = posts.HasPreviuosPage,
                NextPageUrl = _uriService.GetPostPaginationUri(filters, Url.RouteUrl(nameof(GetAsync))).ToString(),
            };

            var postDto = _mapper.Map<PagedList<PostDto>>(posts);

            var response = new ApiResponse<PagedList<PostDto>>(postDto) 
            {
                Meta = pagination
            };

            Response.Headers.Add("X-pagination", JsonConvert.SerializeObject(pagination));

            return response;
        }

        // GET api/<controller>/5
        [HttpGet("{postId}")]
        public async Task<ApiResponse<PostDto>> GetAsync(int postId)
        {
            var post = await _postService.GetPost(postId);

            var postsDto = _mapper.Map<PostDto>(post);

            var response = new ApiResponse<PostDto>(postsDto);

            return response;
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<ApiResponse<PostDto>> InsertPost(PostDto postDto)
        {
           var post = _mapper.Map<Post>(postDto);
           await _postService.InsertPost(post);
           postDto = _mapper.Map<PostDto>(post);
           
           var response = new ApiResponse<PostDto>(postDto);

           return response;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<ApiResponse<PostDto>> Put(int id, PostDto postDto)
        {
            postDto.Id = id;
            Post post = _mapper.Map<Post>(postDto);
            await _postService.UpdatePost(post);
            postDto = _mapper.Map<PostDto>(post);
            var response = new ApiResponse<PostDto>(postDto);
            return response;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<ApiResponse<bool>> Delete(int id)
        {
            var isDeleted = await _postService.DeletePost(id);
            var response = new ApiResponse<bool>(isDeleted);
            return response;
        }
    }
}
