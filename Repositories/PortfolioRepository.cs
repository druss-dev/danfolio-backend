using DanfolioBackend.Models;
using DanfolioBackend.Repositories;

namespace DanfolioBackend.Repositories;

public class PortfolioRepository : IPortfolioRepository
{
    private readonly List<WorkHistory> _workHistories;

    public PortfolioRepository(List<WorkHistory> workHistories)
    {
        _workHistories = workHistories;
    }

    public List<WorkHistory> GetWorkHistory()
    {
        return _workHistories;
    }
}

/*
//Deprecated because Azure SQL cost more than free and this was more of an exercise than a necessity.

using System.Data;
using System.Data.SqlClient;
using DanfolioBackend.Models;
using DanfolioBackend.Utilities;
using Dapper;

namespace DanfolioBackend.Repositories;

public class PortfolioRepository : IPortfolioRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public PortfolioRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    public async Task<IEnumerable<WorkHistory>> GetAllAsync()
    {
        var sql = @"
            SELECT * FROM Portfolio.WorkHistory;
            SELECT wht.WorkHistoryId, ts.Id, ts.Name
            FROM Portfolio.WorkHistory_TechStack wht
            INNER JOIN Portfolio.TechStack ts ON wht.TechStackId = ts.Id;";

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var result = await connection.QueryMultipleAsync(sql);

        var workHistories = result.Read<WorkHistory>().ToList();
        var techStackMap = result.Read<(int WorkHistoryId, int Id, string Name)>();

        foreach (var work in workHistories)
        {
            work.TechStacks = techStackMap
                .Where(t => t.WorkHistoryId == work.Id)
                .Select(t => new TechStack { Id = t.Id, Name = t.Name })
                .ToList();
        }

        return workHistories;
    }

    public async Task<WorkHistory?> GetByIdAsync(int id)
    {
        var sql = @"
            SELECT * FROM Portfolio.WorkHistory WHERE Id = @Id;
            SELECT wht.WorkHistoryId, ts.Id, ts.Name
            FROM Portfolio.WorkHistory_TechStack wht
            INNER JOIN Portfolio.TechStack ts ON wht.TechStackId = ts.Id
            WHERE wht.WorkHistoryId = @Id;";

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var result = await connection.QueryMultipleAsync(sql, new { Id = id });

        var workHistory = result.ReadSingleOrDefault<WorkHistory>();
        if (workHistory == null) return null;

        workHistory.TechStacks = result.Read<(int WorkHistoryId, int Id, string Name)>()
            .Select(t => new TechStack { Id = t.Id, Name = t.Name })
            .ToList();

        return workHistory;
    }

    public async Task<int> UpdateAsync(WorkHistory workHistory)
    {
        var sql = @"
            UPDATE Portfolio.WorkHistory 
            SET Company = @Company, JobTitle = @JobTitle, Location = @Location, 
                Description = @Description, StartDate = @StartDate, EndDate = @EndDate
            WHERE Id = @Id;";

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        return await connection.ExecuteAsync(sql, workHistory);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sql = "DELETE FROM Portfolio.WorkHistory WHERE Id = @Id;";
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
        return affectedRows > 0;
    }
}
*/