using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Core.QueryFilters
{
    public class PostQueryFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } 
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }

    }
}
