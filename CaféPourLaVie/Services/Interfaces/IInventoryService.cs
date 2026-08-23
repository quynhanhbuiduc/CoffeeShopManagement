using CaféPourLaVie.ViewModels;

namespace CaféPourLaVie.Services.Interfaces
{
    public interface IInventoryService
    {
        // TODO: Add methods for inventory management, such as adding, updating, and deleting inventory items.
        Task<List<InventoryViewModel>> GetAllAsync();

        // TODO: Add a method for searching inventory items based on a search string.
        Task<List<InventoryViewModel>> SearchAsync(string? searchString);

        // TODO: Add a method for getting inventory transactions.
        Task<List<InventoryTransactionViewModel>> GetTransactionsAsync();
    }
}