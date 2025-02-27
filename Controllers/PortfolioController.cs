using DanfolioBackend.Models;
using DanfolioBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanfolioBackend.Controllers;

[Authorize]
[ApiController]
[Route("api/portfolio")]
public class PortfolioController : ControllerBase
{
    private readonly IPortfolioService _service;

    public PortfolioController(IPortfolioService service)
    {
        _service = service;
    }

    [HttpGet("workhistory")]
    public ActionResult<List<WorkHistory>> GetWorkHistory()
    {
        return Ok(_service.GetWorkHistory());
    }
}

/*
// The repository has been altered to no longer connect to an active db, so these are inactive for now

using DanfolioBackend.Models;
using DanfolioBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace DanfolioBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PortfolioController : ControllerBase
{
    private readonly IPortfolioService _portfolioService;

    public PortfolioController(IPortfolioService portfolioService)
    {
        _portfolioService = portfolioService;
    }

    [HttpGet("work-history")]
    public async Task<IActionResult> GetWorkHistories()
    {
        var workHistories = await _portfolioService.GetAllAsync();
        return Ok(workHistories);
    }

    [HttpGet("work-history/{id}")]
    public async Task<IActionResult> GetWorkHistory(int id)
    {
        var workHistory = await _portfolioService.GetByIdAsync(id);
        return workHistory is null ? NotFound() : Ok(workHistory);
    }

    [HttpPut("work-history/{id}")]
    public async Task<IActionResult> UpdateWorkHistory(int id, [FromBody] WorkHistory workHistory)
    {
        if (id != workHistory.Id) return BadRequest("ID mismatch");
        
        var success = await _portfolioService.UpdateAsync(workHistory);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("work-history/{id}")]
    public async Task<IActionResult> DeleteWorkHistory(int id)
    {
        var success = await _portfolioService.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}
*/
