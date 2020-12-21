namespace WebBazar.API.DTOs
{
    public class PaginationServiceModel
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}