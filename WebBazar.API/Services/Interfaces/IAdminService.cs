using System.Collections.Generic;
using System.Threading.Tasks;
using WebBazar.API.DTOs.Ad;
using WebBazar.API.Helpers;

namespace WebBazar.API.Services.Interfaces
{
    public interface IAdminService
    {
         Task<dynamic> GetUsersWithRoles();
         Task<string[]> GetRoles();
         Task<List<AdForListDTO>> GetAdsForApproval();
         Task ApproveAd(int id);
         Task RejectAd(int id);
         Task<Result> EditUserRoles(string userName, string[] selectedRoles);
    }
}