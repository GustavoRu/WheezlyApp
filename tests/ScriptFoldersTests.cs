using Xunit;
// using WheezlyApp.ScriptFolders;
namespace WheezlyAppTests;

public class ScriptFoldersTests
{
    private readonly string _testFilesPath;

    public ScriptFoldersTests()
    {
        // creamos directorio temporal para tests
        _testFilesPath = Path.Combine(Path.GetTempPath(), "ScriptFoldersTests");
        Directory.CreateDirectory(_testFilesPath);
    }

    [Fact]
    public void ProcessFiles_ShouldAddAsyncSuffix()
    {
        // Arrange
        var testFile = Path.Combine(_testFilesPath, "TestAsync.cs");
        File.WriteAllText(testFile, @"
            public class TestClass {
                public async Task GetData() {
                    return Task.CompletedTask;
                }
            }");

        // Act
        var script = new ScriptFolders();
        script.ProcessFiles(_testFilesPath);

        // Assert
        var content = File.ReadAllText(testFile);
        Assert.Contains("GetDataAsync", content);
    }

    [Fact]
    public void ProcessFiles_ShouldFixNamingConventions()
    {
        // Arrange
        var testFile = Path.Combine(_testFilesPath, "TestNaming.cs");
        File.WriteAllText(testFile, @"
            public class TestClass {
                public UserDto GetUserDto() {
                    return new UserDto();
                }
                public CarVm GetCarVm() {
                    return new CarVm();
                }
            }");

        // Act
        var script = new ScriptFolders();
        script.ProcessFiles(_testFilesPath);

        // Assert
        var content = File.ReadAllText(testFile);
        Assert.Contains("GetUserDTO", content);
        Assert.Contains("GetCarVM", content);
    }

    [Fact]
    public void ProcessFiles_ShouldAddBlankLines()
    {
        // Arrange
        var testFile = Path.Combine(_testFilesPath, "TestSpacing.cs");
        File.WriteAllText(testFile, @"
            public class TestClass {
                public void Method1() {
                }
                public void Method2() {
                }
            }");

        // Act
        var script = new ScriptFolders();
        script.ProcessFiles(_testFilesPath);

        // Assert
        var content = File.ReadAllText(testFile);
        Assert.Contains("}\n\npublic", content);
    }

    public void Dispose()
    {
        // eliminamos archivos temporales
        if (Directory.Exists(_testFilesPath))
        {
            Directory.Delete(_testFilesPath, true);
        }
    }
}
