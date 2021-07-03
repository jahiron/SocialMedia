using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialMedia.Core.CustomEntities
{
    public class PagedList<T> : List<T>
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage => CurrentPage < TotalPages;
        public bool HasPreviuosPage => CurrentPage > 1;
        public int? NextPageNumber => HasNextPage ? CurrentPage + 1 : (int?)null;
        public int? PreviousPageNumber => HasPreviuosPage ? CurrentPage - 1 : (int?)null;
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            AddRange(items);
        }

        public static PagedList<T> Create(List<T> items, int pageNumber, int pageSize)
        {
            var list = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(list, items.Count(), pageNumber, pageSize);
        }
    }
}
