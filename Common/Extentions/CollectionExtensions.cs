﻿using Domain.Models.Pagination;

namespace Common.Extensions
{
    public static class CollectionExtensions
    {
        // Paged result
        public static Paged<T> ToPaged<T>(this ICollection<T> source, PaginationRequestModel pagination, int totalRow)
        {
            return new Paged<T>
            {
                Data = source,
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow,
                },
            };
        }
    }
}
