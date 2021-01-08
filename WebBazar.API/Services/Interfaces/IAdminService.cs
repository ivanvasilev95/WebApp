using System.Collections.Generic;
using System.Threading.Tasks;
using WebBazar.API.DTOs.Ad;
using WebBazar.API.DTOs.User;
using WebBazar.API.Infrastructure.Services;

namespace WebBazar.API.Services.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<AdForListDTO>> AdsForApprovalAsync();
        Task<IEnumerable<UserWithRolesServiceModel>> UsersWithRolesAsync();
        Task<string[]> RolesAsync();
        Task<Result> ApproveAdAsync(int id);
        Task<Result> EditUserRolesAsync(string userName, string[] selectedRoles);
    }
}