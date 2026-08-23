using CaféPourLaVie.Models;
using CaféPourLaVie.Services.Common;
using CaféPourLaVie.ViewModels;

namespace CaféPourLaVie.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllAsync();

        Task<Employee?> GetByIdAsync(int id);

        Task<ServiceResult> CreateAsync(EmployeeCreateViewModel model);

        Task<ServiceResult> UpdateAsync(EmployeeEditViewModel model);

        Task<EmployeeEditViewModel?> GetEditViewModelByIdAsync(int id);

        Task<ServiceResult> ToggleStatusAsync(int id);

        Task<ServiceResult> ResetPasswordAsync(int id);
    }
}
