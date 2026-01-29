using Npgsql;
using FinanceAPI.Core.Entities;
using FinanceAPI.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace FinanceAPI.Infrastructure.Data;

public class PostgresFundRepository : IFundRepository
{
    private readonly string _connectionString;

    public PostgresFundRepository(IConfiguration configuration)
    {
        // This pulls from the "Default" connection string in appsettings.json
        _connectionString = configuration.GetConnectionString("Default")!;
    }

    public async Task<IEnumerable<Fund>> GetAllAsync()
    {
        var funds = new List<Fund>();
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new NpgsqlCommand("SELECT id, name, ticker, nav FROM mutual_funds ORDER BY id DESC", connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            funds.Add(new Fund
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Ticker = reader.GetString(2),
                Nav = reader.GetDecimal(3)
            });
        }
        return funds;
    }

    public async Task<Fund?> GetByIdAsync(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new NpgsqlCommand("SELECT id, name, ticker, nav FROM mutual_funds WHERE id = @id", connection);
        command.Parameters.AddWithValue("id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Fund
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Ticker = reader.GetString(2),
                Nav = reader.GetDecimal(3)
            };
        }
        return null;
    }

    public async Task AddAsync(Fund fund)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        // BFSI Best Practice: Explicitly naming columns and using parameterized queries to prevent SQL Injection
        const string sql = "INSERT INTO mutual_funds (name, ticker, nav) VALUES (@name, @ticker, @nav)";
        using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("name", fund.Name);
        command.Parameters.AddWithValue("ticker", fund.Ticker);
        command.Parameters.AddWithValue("nav", fund.Nav);

        await command.ExecuteNonQueryAsync();
    }
    public async Task DeleteAsync(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new NpgsqlCommand("DELETE FROM mutual_funds WHERE id = @id", connection);
        command.Parameters.AddWithValue("id", id);

        await command.ExecuteNonQueryAsync();
    }
}