using System.Collections.Generic;
using System.Threading.Tasks;
using WebBazar.API.DTOs.Ad;
using WebBazar.API.Helpers;

namespace WebBazar.API.Services.Interfaces
{
    public interface IAdminService
    {
         Task<dynamic> GetUsersWithRolesAsync();
         Task<string[]> GetRolesAsync();
         Task<List<AdForListDTO>> GetAdsForApprovalAsync();
         Task<Result> ApproveAdAsync(int id);
         // Task RejectAd(int id);
         Task<Result> EditUserRolesAsync(string userName, string[] selectedRoles);
    }
}