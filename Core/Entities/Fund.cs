namespace FinanceAPI.Core.Entities;

public class Fund
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Ticker { get; set; } = string.Empty;
    public decimal Nav { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}