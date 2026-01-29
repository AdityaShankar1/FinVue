using Microsoft.AspNetCore.Mvc;
using FinanceAPI.Core.Entities;
using FinanceAPI.Core.Interfaces;

namespace FinanceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FundsController : ControllerBase
{
    private readonly IFundRepository _repository;

    public FundsController(IFundRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var funds = await _repository.GetAllAsync();
        return Ok(funds);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Fund fund)
    {
        await _repository.AddAsync(fund);
        return CreatedAtAction(nameof(GetAll), new { id = fund.Id }, fund);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(id);
        return NoContent(); // 204 No Content is the standard response for a successful delete
    }
}