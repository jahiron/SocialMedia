using SocialMedia.Core.QueryFilters;
using System;

namespace SocialMedia.Core.Interfaces
{
    public interface IUriService
    {
        Uri GetPostPaginationUri(PostQueryFilter filters, string actionUrl);
    }
}