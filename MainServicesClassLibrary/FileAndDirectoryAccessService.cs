using System.Runtime.Versioning;
using System.Security.AccessControl;

namespace MainServicesClassLibrary;
/// <summary>
/// 文件夹用户权限控制服务
/// </summary>
[SupportedOSPlatform("windows")]
public class FileAndDirectoryAccessService
{
    /// <summary>
    /// 给指定文件夹添加指定用户的访问权限
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <param name="accessUser"></param>
    [SupportedOSPlatform("windows")]
    public void CreateDirectoryAccess(string directoryPath,string accessUser)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(directoryPath);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(accessUser);

        // 创建 DirectoryInfo 对象
        DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
        if (!directoryInfo.Exists)
            directoryInfo.Create();

        // 获取文件夹的现有访问控制
        DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();

        // 设置新的访问规则
        FileSystemAccessRule accessRule = new FileSystemAccessRule(
            accessUser,
            FileSystemRights.FullControl,
            InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
            PropagationFlags.None,
            AccessControlType.Allow);

        // 添加访问规则
        directorySecurity.AddAccessRule(accessRule);

        // 应用新的访问控制
        directoryInfo.SetAccessControl(directorySecurity);
    }
}
