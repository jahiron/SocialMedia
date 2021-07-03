using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using System;

namespace SocialMedia.Core.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUrl;

        public UriService(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public Uri GetPostPaginationUri(PostQueryFilter filters, string actionUrl)
        {
            var baseUrl = $"{_baseUrl}{actionUrl}";
            return new Uri(baseUrl);
        }
    }
}
