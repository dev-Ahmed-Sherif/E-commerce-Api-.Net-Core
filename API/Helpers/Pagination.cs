namespace E_commerce_Api.Helpers
{
    public class Pagination<T>(
        int pageIndex, 
        int pageSize, 
        int count, 
        IReadOnlyList<T> data
        ) where T : class
    {
        //public Pagination(int pageIndex,int pageSize,int count,IReadOnlyList<T> data) 
        //{
            
        //}

        public int PageIndex { get; set; } = pageIndex;
        public int PageSize { get; set; } = pageSize;
        public int Count { get; set; } = count;
        public IReadOnlyList<T> Data { get; set; } = data;
    }
}
