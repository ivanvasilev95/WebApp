namespace WebApp.API.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 15;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
        public string SearchText { get; set; }// = "";
        public int CategoryId { get; set; }// = 0;
        public string SortCriteria { get; set; }// = "newest";
    }
}