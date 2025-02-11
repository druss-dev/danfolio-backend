using DanfolioBackend.Models;

namespace DanfolioBackend.Services;

public interface IPortfolioService
{
    Task<IEnumerable<WorkHistory>> GetAllAsync();
    Task<WorkHistory?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(WorkHistory workHistory);
    Task<bool> DeleteAsync(int id);
}