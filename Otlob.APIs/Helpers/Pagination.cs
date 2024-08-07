using Otlob.APIs.DTOs;

namespace Otlob.APIs.Helpers
{
    public class Pagination<T>
    {

        public Pagination(int pageIndex, int pageSize, IReadOnlyList<T> mappedObjects,int count)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Data = mappedObjects;
            Count = count;
        }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }


    }
}
