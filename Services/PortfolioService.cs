using DanfolioBackend.Models;
using DanfolioBackend.Repositories;
using DanfolioBackend.Services;

namespace DanfolioBackend.Services;

public class PortfolioService : IPortfolioService
{
    private readonly IPortfolioRepository _repository;

    public PortfolioService(IPortfolioRepository repository)
    {
        _repository = repository;
    }

    public List<WorkHistory> GetWorkHistory()
    {
        return _repository.GetWorkHistory();
    }
}

/*
// The repository has been altered to no longer connect to an active db, so these are inactive for now

using DanfolioBackend.Models;
using DanfolioBackend.Repositories;

namespace DanfolioBackend.Services;

public class PortfolioService : IPortfolioService
{
    private readonly IPortfolioRepository _repository;

    public PortfolioService(IPortfolioRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<WorkHistory>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<WorkHistory?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(WorkHistory workHistory)
    {
        return await _repository.UpdateAsync(workHistory) > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}
*/