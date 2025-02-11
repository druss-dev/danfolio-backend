using DanfolioBackend.Models;

namespace DanfolioBackend.Repositories;

public interface IPortfolioRepository
{
    Task<IEnumerable<WorkHistory>> GetAllAsync();
    Task<WorkHistory?> GetByIdAsync(int id);
    Task<int> UpdateAsync(WorkHistory workHistory);
    Task<bool> DeleteAsync(int id);
}