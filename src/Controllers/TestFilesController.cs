using Microsoft.AspNetCore.Mvc;

namespace WheezlyApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestFilesController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;

    public TestFilesController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    [HttpPost("ProcessFiles")]
    public IActionResult ProcessFiles([FromQuery] string path = "tests/TestFiles")
    {
        try
        {
            // ruta absoluta
            var basePath = Directory.GetCurrentDirectory();
            var fullPath = Path.GetFullPath(Path.Combine(basePath, "..", path));
            
            Console.WriteLine($"Trying path: {fullPath}");
            
            if (!Directory.Exists(fullPath))
            {
                return BadRequest($"directory not found: {fullPath}");
            }
            
            var script = new ScriptFolders();
            script.ProcessFiles(fullPath);
            return Ok($"fFiles processed succesfully in: {fullPath}");
        }
        catch (Exception ex)
        {
            return BadRequest($"error: {ex.Message}");
        }
    }
}