using System.Text.RegularExpressions;

public class ScriptFolders
{

    public void ProcessFiles(string folderPath)
    {
        try
        {
            var files = Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                try
                {
                    string content = File.ReadAllText(file);

                    // a) Métodos async sin sufijo Async
                    content = Regex.Replace(content,
                        @"async\s+Task\s+([A-Za-z0-9_]+)\s*\(",
                        m => m.Groups[1].Value.EndsWith("Async")
                            ? m.Value
                            : m.Value.Replace(m.Groups[1].Value, m.Groups[1].Value + "Async"));

                    // b) Renombrar Vm/Vms/Dto/Dtos
                    content = Regex.Replace(content, @"\bVm\b", "VM");
                    content = Regex.Replace(content, @"\bVms\b", "VMs");
                    content = Regex.Replace(content, @"\bDto\b", "DTO");
                    content = Regex.Replace(content, @"\bDtos\b", "DTOs");

                    // c) Insertar línea en blanco entre métodos
                    content = Regex.Replace(content,
                        @"\}\s*public",
                        "}\n\npublic");

                    File.WriteAllText(file, content);
                    Console.WriteLine($"processed: {file}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"error processing file {file}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error accessing folder {folderPath}: {ex.Message}");
            throw;
        }
    }
}
