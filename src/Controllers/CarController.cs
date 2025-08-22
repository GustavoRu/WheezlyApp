using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WheezlyApp.Data;

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

    [HttpGet]
    public async Task<IActionResult> GetCars()
    {
        var cars = await _context.Cars.ToListAsync();
        return Ok(cars);
    }
}