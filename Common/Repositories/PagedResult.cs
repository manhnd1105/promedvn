﻿using System.Collections.Generic;

namespace Common.Repositories
{
    public class PagedResult<TEntity>
        where TEntity : class
    {
        IEnumerable<TEntity> _items;
        int _totalCount;

        public PagedResult(IEnumerable<TEntity> items, int totalCount)
        {
            _items = items;
            _totalCount = totalCount;
        }

        public IEnumerable<TEntity> Items { get { return _items; } }
        public int TotalCount { get { return _totalCount; } }
    }
}
