using DanfolioBackend.Models;

namespace DanfolioBackend.Repositories;

public interface IPortfolioRepository
{
    List<WorkHistory> GetWorkHistory();
    
    // These are only used if we create a database
    // Task<IEnumerable<WorkHistory>> GetAllAsync();
    // Task<WorkHistory?> GetByIdAsync(int id);
    // Task<int> UpdateAsync(WorkHistory workHistory);
    // Task<bool> DeleteAsync(int id);
}