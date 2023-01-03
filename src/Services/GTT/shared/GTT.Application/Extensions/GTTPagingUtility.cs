namespace GTT.Application.Extensions
{
    public class GTTPagingUtility
    {
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
