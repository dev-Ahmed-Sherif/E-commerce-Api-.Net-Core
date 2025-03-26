namespace E_commerce_Api.Helpers
{
    public class Pagination<T>(
        int pageIndex, 
        int pageSize, 
        int count, 
        IReadOnlyList<T> data
        ) where T : class
    public class Pagination<T> where T : class
    {
        //public Pagination(int pageIndex,int pageSize,int count,IReadOnlyList<T> data) 
        //{
            
        //}
        public Pagination(int pageIndex, int pageSize, int count, IReadOnlyList<T> data) 
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }

        public int PageIndex { get; set; } = pageIndex;
        public int PageSize { get; set; } = pageSize;
        public int Count { get; set; } = count;
        public IReadOnlyList<T> Data { get; set; } = data;
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
    }
}
