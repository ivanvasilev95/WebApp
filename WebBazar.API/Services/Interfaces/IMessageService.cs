using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebBazar.API.DTOs.Message;
using WebBazar.API.Helpers;

namespace WebBazar.API.Services.Interfaces
{
    public interface IMessageService
    {
         Task<IEnumerable<MessageToReturnDTO>> MessageThreadAsync(int adId, int currentUserId, int recipientId);
         Task<IEnumerable<MessageToReturnDTO>> UserMessagesAsync(MessageParams messageParams);
         Task<Result<MessageToReturnDTO>> CreateAsync(MessageForCreationDTO model);
         Task<Result> DeleteAsync(int messageId, int currentUserId);
         Task<Result> MarkAsReadAsync(int messageId, int currentUserId);
         Task<int> UnreadMessagesCountAsync(int userId);
    }
}