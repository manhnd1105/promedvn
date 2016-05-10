using System;
using System.Linq.Expressions;

namespace Common.Repositories
{
    public class SortingRule<TSortedType>
    {
        public Expression<Func<TSortedType, object>> FieldSelector { get; private set; }
        public SortType SortDirection { get; private set; }

        public SortingRule(Expression<Func<TSortedType, object>> selector, SortType direction = SortType.Ascending)
        {
            FieldSelector = selector;
            SortDirection = direction;
        }
    }
    public enum SortType
    {
        None = 0,
        Ascending = 1,
        Descending = 2
    }
}
