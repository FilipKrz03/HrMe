using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class PagedList<T> : List<T> , IPagedList<T>
    {

        private const int maxPageSize = 20;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > maxPageSize ? maxPageSize : value;
        }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; } 

        public bool HasPrevios => PageNumber > 1;
        public bool HasNext => (PageNumber < TotalPages);

     
       public PagedList(List<T> items , int pageNumber , int pageSize , int totalCount)
        {
            PageNumber = pageNumber;    
            PageSize = pageSize;   
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            AddRange(items);
        }

        public static async Task<PagedList<T>> 
            CreateAsync(IQueryable<T> source , int page , int pageSize)
        {
            var totalCount = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize)
                .ToListAsync();
            return new(items, page, pageSize, totalCount);
        }

    }
}
