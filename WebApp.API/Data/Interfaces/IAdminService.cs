using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.API.DTOs.Ad;
using WebApp.API.Helpers;

namespace WebApp.API.Data.Interfaces
{
    public interface IAdminService
    {
         Task<dynamic> GetUsersWithRoles();
         Task<string[]> GetRoles();
         Task<List<AdForListDTO>> GetAdsForModeration();
         Task ApproveAd(int id);
         Task RejectAd(int id);
         Task<Result> EditRoles(string userName, string[] selectedRoles);
    }
}