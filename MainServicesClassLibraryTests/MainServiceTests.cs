using MainServicesClassLibrary;

namespace MainServicesClassLibraryTests;

[TestClass()]
public class MainServiceTests
{
    [TestMethod()]
    public void MainFunTest()
    {
        var service = new MainService();
        service.MainFun();
    }
}