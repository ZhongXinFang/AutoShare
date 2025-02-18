using System.Runtime.Versioning;

namespace MainServicesClassLibrary;
[SupportedOSPlatform("windows")]
public class MainService
{
    [SupportedOSPlatform("windows")]
    public void MainFun()
    {
        var userName = "PrinterScanUserT";
        var userPassword = "Scan2024";
        var shareName = "Scan";
        var shareDirectoryPath = @"C:\Scan";

        // 创建指定的用户
        var userService = new UserAndUserGroupService();
        userService.CreateUser(userName,userPassword);
        userService.RemovingUserFromGroup(userName,"Users");

        // 创建指定的文件夹
        var directoryInfo = new DirectoryInfo(shareName);
        if (!directoryInfo.Exists)
            directoryInfo.Create();

        // 授予文件夹指定用户访问权限
        var accessService = new FileAndDirectoryAccessService();
        accessService.CreateDirectoryAccess(shareDirectoryPath,userName);

        // 共享指定文件夹
        var shareService = new ShareService();
        if (shareService.IsShareExists(shareName,shareDirectoryPath))
            return;
        shareService.CreateNewShare(shareName,shareDirectoryPath);
    }
}
