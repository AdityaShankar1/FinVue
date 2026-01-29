using FinanceAPI.Core.Entities;

namespace FinanceAPI.Core.Interfaces;

public interface IFundRepository
{
    Task<IEnumerable<Fund>> GetAllAsync();
    Task<Fund?> GetByIdAsync(int id);
    Task AddAsync(Fund fund);
    // Add this line:
    Task DeleteAsync(int id); 
}