using MainServicesClassLibrary;
using System.Runtime.Versioning;

namespace MainServicesClassLibraryTests;

[TestClass()]
[SupportedOSPlatform("windows")]
public class ShortcutServiceTests
{
    [TestMethod()]
    public void CreateShortcutTest()
    {
        var service = new ShortcutService();
        service.CreateShortcut(@"C:\Users\User\Desktop\Data\NowSolution\new共享文件夹解决方案\ConsoleApp1\bin\Debug\net8.0\win-x64",
            "好东西");
    }
}