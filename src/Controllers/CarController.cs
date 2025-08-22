using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WheezlyApp.Data;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Dynamic;


namespace WheezlyApp.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CarController : ControllerBase
{

    private readonly ApplicationDbContext _context;

    public CarController(ApplicationDbContext context)
    {
        _context = context;
    }


    [HttpGet("GetCarInfoWithEF")]
    public async Task<IActionResult> GetCarInfoWithEF(int id)
    {
        var cars = await _context.Cars
    .Include(c => c.Make)
    .Include(c => c.Model)
    .Include(c => c.SubModel)
    .Include(c => c.ZipCode)
    .Include(c => c.Quotes.Where(q => q.IsCurrentQuote))
        .ThenInclude(q => q.Buyer)
    .Include(c => c.StatusHistories
        .OrderByDescending(sh => sh.CreatedDate)
        .Take(1))
        .ThenInclude(sh => sh.Status)
    .Where(c => c.IsActive && c.Id == id)
    .Select(c => new
    {
        Year = c.Year,
        Make = c.Make.Name,
        Model = c.Model.Name,
        SubModel = c.SubModel.Name,
        ZipCode = c.ZipCode.ZipCode1,
        BuyerName = c.Quotes.FirstOrDefault(q => q.IsCurrentQuote).Buyer.Name,
        CurrentQuote = c.Quotes.FirstOrDefault(q => q.IsCurrentQuote).CurrentAmount,
        CurrentStatus = c.StatusHistories.OrderByDescending(sh => sh.CreatedDate).First().Status.Name,
        StatusDate = c.StatusHistories.OrderByDescending(sh => sh.CreatedDate).First().StatusDate
    })
    .ToListAsync();
        return Ok(cars);
    }

    [HttpGet("GetCarInfoWithSql")]
    public async Task<IActionResult> GetCarInfoWithSql(int id = 3)
    {
        var sql = @"SELECT 
            C.Year,
            M.Name Make,
            MD.Name Model,
            SM.Name SubModel,
            Z.ZipCode,
            B.Name BuyerName,
            Q.CurrentAmount CurrentQuote,
            S.Name CurrentStatus,
            SH.StatusDate
        FROM Cars C
        INNER JOIN Makes M ON C.MakeId = M.Id
        INNER JOIN Models MD ON C.ModelId = MD.Id
        INNER JOIN SubModels SM ON C.SubModelId = SM.Id
        INNER JOIN ZipCodes Z ON C.ZipCodeId = Z.Id
        LEFT JOIN Quotes Q ON C.Id = Q.CarId AND Q.IsCurrentQuote = 1
        LEFT JOIN Buyers B ON Q.BuyerId = B.Id
        LEFT JOIN StatusHistory SH ON C.Id = SH.CarId
        LEFT JOIN Status S ON SH.StatusId = S.Id
        WHERE C.IsActive = 1 AND C.Id = @Id
        AND SH.Id = (
            SELECT TOP 1 Id 
            FROM StatusHistory 
            WHERE CarId = C.Id 
            ORDER BY CreatedDate DESC
        )";

        using var connection = new SqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", id);
        
        using var reader = await command.ExecuteReaderAsync();
        var result = new List<Dictionary<string, object>>();
        
        // get de names de columnas
        var columns = Enumerable.Range(0, reader.FieldCount)
            .Select(reader.GetName)
            .ToList();
            
        // leer data
        while (await reader.ReadAsync())
        {
            var row = new Dictionary<string, object>();
            foreach (var column in columns)
            {
                row[column] = reader[column] == DBNull.Value ? null : reader[column];
            }
            result.Add(row);
        }
        
        return Ok(result);
    }
}