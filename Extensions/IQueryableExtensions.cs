using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using cars.Core.Models;

namespace cars.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<Car> ApplyOrdering(this IQueryable<Car> query, IQueryObject queryObj, Dictionary<string, Expression<Func<Car, object>>> columnsMap)
        {
            // check for the existence of the key in the columnsMap
            if (String.IsNullOrWhiteSpace(queryObj.SortBy) || !columnsMap.ContainsKey(queryObj.SortBy))
                // if we do have a column with that key
                return query;

            if (queryObj.IsSortAscending)
                // sort by ascending using the name of the key(column name)
                return query.OrderBy(columnsMap[queryObj.SortBy]);
            else
                // sort by descending using the name of the key(column name)
                return query.OrderByDescending(columnsMap[queryObj.SortBy]);
        }  
        
        // Pagination
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, IQueryObject queryObj) 
        {
            // not accepting Page number <= 0
            if (queryObj.Page <= 0)
                queryObj.Page = 1; 
            // not accepting PageSize <= 0
            if (queryObj.PageSize <= 0)
                queryObj.PageSize = 10;
                    
            return query.Skip((queryObj.Page - 1) * queryObj.PageSize).Take(queryObj.PageSize);
        }     
    }
}