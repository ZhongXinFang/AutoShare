using System.Diagnostics;

namespace MainServicesClassLibrary.CMDServices;
public class CMDService
{
    /// <summary>
    /// 在新的进程中执行cmd命令，如何返回命令的执行结果
    /// </summary>
    /// <param name="command">要执行的命令</param>
    /// <returns></returns>
    /// <exception cref="CMDServiceException">如果执行过程中出现错误，则抛出此异常，并且附带命令执行时的错误信息</exception>
    public string RunCmd(string command)
    {
        // 创建一个新的进程启动信息
        ProcessStartInfo processStartInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c {command}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        // 创建并启动进程
        using Process process = new();
        process.StartInfo = processStartInfo;
        process.Start();
        // 读取标准输出和错误输出
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();
        if (!string.IsNullOrEmpty(error))
            throw new CMDServiceException(error);
        return output;
    }

}
