using Microsoft.AspNetCore.Mvc;

namespace WheezlyApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestFilesController : ControllerBase
{
    [HttpPost("ProcessFiles")]
    public IActionResult ProcessFiles([FromQuery] string path = "tests/TestFiles")
    {
        try
        {
            var script = new ScriptFolders();
            
            if (!Directory.Exists(path))
            {
                return BadRequest($"not found: {path}");
            }
            
            script.ProcessFiles(path);
            return Ok($"processed successfully in: {path}");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }
}