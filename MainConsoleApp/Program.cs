using MainServicesClassLibrary;
using System.Runtime.Versioning;

internal class Program
{
    [SupportedOSPlatform("windows")]
    private static void Main()
    {
        var userName = "PrinterScanUserT";
        var userPassword = "Scan2024";
        var shareName = "Scan";
        var shareDirectoryPath = @"C:\Scan";

        // 创建指定的用户
        Console.WriteLine("UserAndUserGroupService");
        var userService = new UserAndUserGroupService();
        userService.CreateUser(userName,userPassword);
        userService.RemovingUserFromGroup(userName,"Users");
        Console.WriteLine("UserAndUserGroupService Finish.");

        // 创建指定的文件夹
        Console.WriteLine("Create directory");
        var directoryInfo = new DirectoryInfo(shareDirectoryPath);
        if (!directoryInfo.Exists)
            directoryInfo.Create();
        Console.WriteLine("Create directory Finish.");

        // 授予文件夹指定用户访问权限
        Console.WriteLine("FileAndDirectoryAccessService");
        var accessService = new FileAndDirectoryAccessService();
        accessService.CreateDirectoryAccess(shareDirectoryPath,userName);
        Console.WriteLine("FileAndDirectoryAccessService Finish.");

        // 共享指定文件夹
        Console.WriteLine("ShareService");
        var shareService = new ShareService();
        if (!shareService.IsShareExists(shareName,shareDirectoryPath))
            shareService.CreateNewShare(shareName,shareDirectoryPath);
        Console.WriteLine("ShareService Finish.");

        // 创建快捷方式
        Console.WriteLine("CreateShortcut");
        var shortcutService = new ShortcutService();
        shortcutService.CreateShortcut(shareDirectoryPath,"扫描.");
        Console.WriteLine("CreateShortcut Finish.");

        Console.WriteLine(@$"共享授权用户: {userName}");
        Console.WriteLine(@$"用户密码: {userPassword}");
        Console.WriteLine(@$"共享名称: {shareName}");
        Console.WriteLine(@$"共享路径: \\{Environment.MachineName}\{shareName}");
        Console.WriteLine(@$"磁盘地址: {shareDirectoryPath}");

        Console.WriteLine("Done.");
        Console.ReadKey();
    }
}