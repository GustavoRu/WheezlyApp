using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WheezlyApp.Data;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Dynamic;

namespace WheezlyApp.Controllers;


[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    // public async Task<List<OrderDTO>> GetOrders(
    // DateTime? dateFrom,
    // DateTime? dateTo,
    // List<int> customerIds,
    // List<int> statusIds,
    // bool? isActive)
    // {
    //     var query = _context.Orders.AsQueryable();

    //     if (dateFrom.HasValue)
    //         query = query.Where(o => o.Date >= dateFrom);

    //     if (dateTo.HasValue)
    //         query = query.Where(o => o.Date <= dateTo);

    //     if (customerIds != null && customerIds.Any())
    //         query = query.Where(o => customerIds.Contains(o.CustomerId));

    //     if (statusIds != null && statusIds.Any())
    //         query = query.Where(o => statusIds.Contains(o.StatusId));

    //     if (isActive.HasValue)
    //         query = query.Where(o => o.IsActive == isActive);

    //     return await query
    //         .Select(o => new OrderDTO
    //         {
    //             OrderId = o.Id,
    //             CustomerId = o.CustomerId,
    //             StatusId = o.StatusId,
    //             Date = o.Date,
    //             IsActive = o.IsActive
    //             // otros campos que necesites mapear
    //         })
    //         .ToListAsync();

    //     return Ok();
    // }
}