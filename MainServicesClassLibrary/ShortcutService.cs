using IWshRuntimeLibrary;
using System.Runtime.Versioning;
namespace MainServicesClassLibrary;
/// <summary>
/// 创建快捷方式服务
/// </summary>
[SupportedOSPlatform("windows")]
public class ShortcutService
{
    /// <summary>
    /// 创建一个快捷方式
    /// </summary>
    /// <param name="originalFileOrDirectoryPath">需要创建快捷方式的文件或者文件夹路径</param>
    /// <param name="shortcutName">快捷方式的名称，不需要添加后缀</param>
    /// <param name="shortcutDirectoryPath">快捷方式存放目录（如果为null，则保存在当前用户的桌面目录下，如果重复创建相同的快捷方式，根据Windows特性会覆盖创建）</param>
    /// <exception cref="FileNotFoundException">originalFileOrDirectoryPath 指定的目标不存在</exception>
    /// <exception cref="ArgumentException">originalFileOrDirectoryPath 或 shortcutName 提供的是无效字符串</exception>
    [SupportedOSPlatform("windows")]
    public void CreateShortcut(string originalFileOrDirectoryPath,string shortcutName,string? shortcutDirectoryPath = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(originalFileOrDirectoryPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(shortcutName);

        if (!System.IO.File.Exists(originalFileOrDirectoryPath) && !System.IO.Directory.Exists(originalFileOrDirectoryPath))
            throw new FileNotFoundException(originalFileOrDirectoryPath);

        if (string.IsNullOrWhiteSpace(shortcutDirectoryPath))
        {
            shortcutDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (string.IsNullOrWhiteSpace(shortcutDirectoryPath))
                throw new ArgumentException("shortcutPath is null,and Environment.GetFolderPath(Environment.SpecialFolder.Desktop) is null");
        }
        var shortcutPath = Path.Combine(shortcutDirectoryPath,shortcutName + ".lnk");

        // 创建 WshShell 对象
        WshShell shell = new WshShell();
        // 创建快捷方式
        IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);

        // 设置快捷方式属性
        shortcut.TargetPath = originalFileOrDirectoryPath;
        shortcut.WorkingDirectory = Path.GetDirectoryName(originalFileOrDirectoryPath);
        shortcut.Description = "";
        shortcut.IconLocation = originalFileOrDirectoryPath; // 可以设置图标，如果文件有自己的图标
        shortcut.Save(); // 保存快捷方式
    }
}
