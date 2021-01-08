using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebBazar.API.DTOs.Message;
using WebBazar.API.Infrastructure.Services;

namespace WebBazar.API.Services.Interfaces
{
    public interface IMessageService
    {
         Task<PaginatedMessagesServiceModel> MineAsync(MessageParams messageParams, int userId);
         Task<IEnumerable<MessageToReturnDTO>> ThreadAsync(int adId, int senderId, int recipientId);
         Task<int> UnreadCountAsync(int userId);
         Task<Result<DateTime>> CreateAsync(MessageForCreationDTO model);
         Task<Result> MarkAsReadAsync(int id);
         Task<Result> DeleteAsync(int id);
    }
}