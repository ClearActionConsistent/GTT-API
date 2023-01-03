using Newtonsoft.Json;

namespace GTT.Application.Extensions
{
    public class GTTPageResults<T>
    {
        /// <summary>
        /// The page number this page represents.
        /// </summary>
        [JsonProperty("pageNumber")]
        public int PageNumber { get; set; }

        /// <summary>
        /// The size of this page.
        /// </summary>
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        /// <summary>
        /// The total number of pages available.
        /// </summary>
        [JsonProperty("totalNumberOfPages")]
        public int TotalNumberOfPages { get; set; }

        /// <summary>
        /// The total number of records available.
        /// </summary>
        [JsonProperty("totalNumberOfRecords")]
        public int TotalNumberOfRecords { get; set; }

        /// <summary>
        /// The records this page represents.
        /// </summary>
        [JsonProperty("results")]
        public IEnumerable<T> Results { get; set; } = new List<T>();
    }
}
