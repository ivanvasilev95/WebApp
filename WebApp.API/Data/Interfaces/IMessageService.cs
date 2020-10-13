using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebApp.API.DTOs.Message;
using WebApp.API.Helpers;
using WebApp.API.Models;

namespace WebApp.API.Data.Interfaces
{
    public interface IMessageService
    {
         Task<Result<MessageToReturnDTO>> ByIdAsync(int id);
         Task<IEnumerable<MessageToReturnDTO>> MessageThreadAsync(int adId, int currentUserId, int recipientId);
         Task<IEnumerable<MessageToReturnDTO>> UserMessagesAsync(MessageParams messageParams, HttpResponse response);
         Task<Result<Message>> CreateAsync(MessageForCreationDTO model);
         Task<Result> DeleteAsync(int messageId, int currentUserId);
         Task<Result> MarkAsReadAsync(int messageId, int currentUserId);
         Task<int> UnreadMessagesCountAsync(int userId);
    }
}