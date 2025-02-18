using MainServicesClassLibrary.CMDServices;
using System.Management;
using System.Runtime.Versioning;

namespace MainServicesClassLibrary;
/// <summary>
/// 共享相关服务
/// </summary>
[SupportedOSPlatform("windows")]
public class ShareService
{
    /// <summary>
    /// 创建一个共享文件夹
    /// </summary>
    /// <param name="shareName"></param>
    /// <param name="directoryPath"></param>
    [SupportedOSPlatform("windows")]
    public void CreateNewShare(string shareName,string directoryPath)
    {
        var dinfo = new DirectoryInfo(directoryPath);
        if (!dinfo.Exists)
            dinfo.Create();

        var cmdService = new CMDService();
        cmdService.RunCmd($@"net share {shareName.Trim()}={directoryPath} /GRANT:Everyone,CHANGE");
    }

    /// <summary>
    /// 判断一个共享名称和指定文件夹是否已经共享
    /// </summary>
    /// <param name="shareName">共享名称</param>
    /// <param name="directoryPath">文件夹路径</param>
    /// <returns>如果共享存在，返回 true；否则返回 false</returns>
    [SupportedOSPlatform("windows")]
    public bool IsShareExists(string shareName,string directoryPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(shareName);
        ArgumentException.ThrowIfNullOrWhiteSpace(directoryPath);

        var dinfo = new DirectoryInfo(directoryPath);
        if (!dinfo.Exists)
            return false;

        var cmdService = new CMDService();
        var output = cmdService.RunCmd("net share");

        // 检查输出中是否包含指定的共享名称和路径
        var lines = output.Split(['\r','\n'],StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            // 去掉多余的空格
            var trimmedLine = line.Trim();
            // 检查是否包含共享名称和路径
            if (trimmedLine.Contains(shareName,StringComparison.OrdinalIgnoreCase) &&
                trimmedLine.Contains(directoryPath,StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 判断一个共享名称是否已经被共享，还可以限定判断共享的目录是否是指定目录
    /// </summary>
    /// <param name="shareName">共享名称</param>
    /// <param name="directoryPath">限定共享的目录</param>
    /// <returns>如果共享存在且满足要求则返回 true；否则返回 false</returns>
    [SupportedOSPlatform("windows")]
    public bool IsShareExists1(string shareName,string? directoryPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(shareName);

        try
        {
            // 使用 WMI 查询共享信息
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Share WHERE Name = '" + shareName + "'");
            var res = searcher.Get();
            // 如果没有限定共享名称需要和指定目录对应，则查询到结果说明有共享
            if (string.IsNullOrWhiteSpace(directoryPath))
                return res.Count > 0;
            // 否则要比对共享名称指定的共享目录是否是指定的文件夹路径
            else
                foreach (ManagementObject share in res.Cast<ManagementObject>())
                {
                    var it = share["Path"];
                    if (it is null)
                        continue;
                    string sharePath = it.ToString()!.TrimEnd(Path.DirectorySeparatorChar,Path.AltDirectorySeparatorChar);
                    var dinfo = new DirectoryInfo(directoryPath);
                    if (!dinfo.Exists)
                        throw new DirectoryNotFoundException(sharePath);
                    // 规范化路径
                    string fullPath = Path.GetFullPath(directoryPath).TrimEnd(Path.DirectorySeparatorChar,Path.AltDirectorySeparatorChar);
                    if (string.Equals(sharePath,fullPath,StringComparison.OrdinalIgnoreCase))
                        return true;
                }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error querying shares: {ex.Message}");
        }
        return false;
    }
}
