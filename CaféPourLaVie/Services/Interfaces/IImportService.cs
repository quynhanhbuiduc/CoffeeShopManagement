using CaféPourLaVie.Models;

namespace CaféPourLaVie.Services.Interfaces
{
    public interface IImportService
    {
        Task<List<ImportReceipt>> GetAllAsync();

        Task<ImportReceipt?> GetByIdAsync(int id);

        Task<int> CreateAsync(int accountId, List<ImportDetail> details);

        Task ApproveAsync(int id);

        Task RejectAsync(int id);
    }
}