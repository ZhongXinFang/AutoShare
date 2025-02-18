using System.DirectoryServices.AccountManagement;
using System.Runtime.Versioning;
namespace MainServicesClassLibrary;
/// <summary>
/// 用户和用户组相关的服务类
/// </summary>
[SupportedOSPlatform("windows")]
public class UserAndUserGroupService
{
    /// <summary>
    /// 创建一个指定名称的用户和为此账户设置密码，如果指定用户已存在，则不执行任何操作
    /// </summary>
    /// <param name="userName"></param>
    /// <exception cref="ArgumentNullException"></exception>
    [SupportedOSPlatform("windows")]
    public void CreateUser(string userName,string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        // 使用本地计算机 Machine 作为上下文来获取 PrincipalContext
        using PrincipalContext context = new PrincipalContext(ContextType.Machine);

        var t = UserPrincipal.FindByIdentity(context,userName);
        if (t is not null)
            return;

        UserPrincipal user = new UserPrincipal(context)
        {
            Name = userName,
            DisplayName = userName,
            PasswordNotRequired = false,        // 如果你想要用户必须设置密码
            PasswordNeverExpires = true,        // 设置账户永不过期
            UserCannotChangePassword = true,    // 禁止用户更改密码
        };

        // 设置用户密码
        user.SetPassword(password);
        user.Save();
    }


    /// <summary>
    /// 创建一个指定名称的用户组，如果指定用户组已存在，则不执行任何操作
    /// </summary>
    /// <param name="groupName"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="Exception"></exception>
    [SupportedOSPlatform("windows")]
    public void CreateUserGroup(string groupName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(groupName);

        using PrincipalContext context = new PrincipalContext(ContextType.Machine);

        if (GroupPrincipal.FindByIdentity(context,groupName) is not null)
            return;

        GroupPrincipal group = new GroupPrincipal(context)
        {
            Name = groupName,
            Description = "This is a new user group created via .NET 8"
        };
        group.Save();
    }

    /// <summary>
    /// 将用户从指定的用户组中剔除
    /// </summary>
    /// <param name="userName">要移除的用户名</param>
    /// <param name="groupName">用户组名称</param>
    public void RemovingUserFromGroup(string userName,string groupName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);
        ArgumentException.ThrowIfNullOrWhiteSpace(groupName);
        using PrincipalContext context = new PrincipalContext(ContextType.Machine);

        var group = GroupPrincipal.FindByIdentity(context,groupName);
        if (group is null)
            return;
        var user = UserPrincipal.FindByIdentity(context,userName);
        if (user is null)
            return;

        if (group.Members.Contains(user))
        {
            group.Members.Remove(user);
            group.Save();
        }
    }

    /// <summary>
    /// 将指定的用户添加到指定的用户组中，如果指定用户在用户组中已存在，则不执行任何操作
    /// </summary>
    /// <param name="username"></param>
    [SupportedOSPlatform("windows")]
    public void AddUserGroupByUser(string userName,string groupName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);
        ArgumentException.ThrowIfNullOrWhiteSpace(groupName);

        using PrincipalContext context = new PrincipalContext(ContextType.Machine);
        GroupPrincipal group = GroupPrincipal.FindByIdentity(context,groupName);
        if (group is null)
            throw new Exception("指定的用户组不存在或者无法访问");

        UserPrincipal user = UserPrincipal.FindByIdentity(context,userName);
        if (user is null)
            throw new Exception("指定的用户不存在或者无法访问");

        if (group.Members.Contains(user))
            return;

        if (group.Members.Contains(user))
            return;

        group.Members.Add(user);
        group.Save();
    }
}
