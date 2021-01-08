namespace WebBazar.API.Infrastructure
{
    public class PaginationHeader
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
		
        public PaginationHeader(int currentPage, int itemsPerPage, int totalItems)
        {
            this.CurrentPage = currentPage;
            this.ItemsPerPage = itemsPerPage;
            this.TotalItems = totalItems;
        }
    }
}