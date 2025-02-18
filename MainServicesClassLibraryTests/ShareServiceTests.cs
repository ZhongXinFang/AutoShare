using MainServicesClassLibrary;

namespace MainServicesClassLibraryTests;

[TestClass()]
public class ShareServiceTests
{
    [TestMethod()]
    public void IsShareExists1Test()
    {
        var shareService = new ShareService();
        Console.WriteLine(shareService.IsShareExists1("Scan",@"C:\Scan")) ;
    }
}