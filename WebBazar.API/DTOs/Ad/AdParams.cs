namespace WebBazar.API.DTOs.Ad
{
    public class AdParams
    {
        private const int MaxPageSize = 18;
        private int pageSize = 6;
        
        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
        
        public string SearchText { get; set; }

        public int CategoryId { get; set; }
        
        public string SortCriteria { get; set; }
    }
}