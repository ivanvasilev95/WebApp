using System.Collections.Generic;

namespace WebBazar.API.DTOs.Message
{
    public class PaginatedMessagesServiceModel : PaginationServiceModel
    {
        public IEnumerable<MessageToReturnDTO> Messages { get; set; }
    }
}