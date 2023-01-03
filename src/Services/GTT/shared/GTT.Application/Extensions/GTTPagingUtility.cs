namespace GTT.Application.Extensions
{
    public class GTTPagingUtility
    {
        public static GTTPageResults<T> CreatePagedResults<T>(
            IEnumerable<T> queryable,
            int page,
            int pageSize)
        {
            var skipAmount = pageSize * (page - 1);

            var enumerable = queryable as IList<T> ?? queryable?.ToList();
            var projection = enumerable?
                .Skip(skipAmount)
                .Take(pageSize);

            var totalNumberOfRecords = enumerable?.Count ?? 0;
            var results = projection?.ToList();

            var mod = totalNumberOfRecords % pageSize;
            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new GTTPageResults<T>
            {
                Results = results,
                PageNumber = page,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }

        public static GTTPageResults<T> CreatePagedResultsQuery<T>(
            IEnumerable<T> results,
            int page,
            int pageSize,
            int totalNumberOfRecords)
        {
            var mod = totalNumberOfRecords % pageSize;
            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new GTTPageResults<T>
            {
                Results = results,
                PageNumber = page,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
    }
}
