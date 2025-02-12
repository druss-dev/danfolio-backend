using DanfolioBackend.Models;

namespace DanfolioBackend.Services;

public interface IPortfolioService
{
    List<WorkHistory> GetWorkHistory();
    
    // These are only used if we create a database
    // Task<IEnumerable<WorkHistory>> GetAllAsync();
    // Task<WorkHistory?> GetByIdAsync(int id);
    // Task<bool> UpdateAsync(WorkHistory workHistory);
    // Task<bool> DeleteAsync(int id);
}