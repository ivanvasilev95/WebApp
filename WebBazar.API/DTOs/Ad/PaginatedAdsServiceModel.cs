using System.Collections.Generic;

namespace WebBazar.API.DTOs.Ad
{
    public class PaginatedAdsServiceModel : PaginationServiceModel
    {
        public IEnumerable<AdForListDTO> Ads { get; set; }
    }
}