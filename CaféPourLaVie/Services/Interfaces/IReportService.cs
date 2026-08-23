using CaféPourLaVie.ViewModels;

namespace CaféPourLaVie.Services.Interfaces
{
    public interface IReportService
    {
        Task<ReportViewModel> GetReportAsync(DateTime fromDate, DateTime toDate);
    }
}