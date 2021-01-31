using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using Microsoft.Win32;
using System.Threading;
using System.Diagnostics;
using System.Security.Principal;

class Utils
{
    public static Random r = new Random();
    public static string cd = Environment.CurrentDirectory;

    public static void Wait(int ms)
    {
        Thread.Sleep(ms);
    }

    public static string Repeat(string str, int count = 1)
    {
        string result = "";
        for (int i = 0; i < count; i++)
        {
            result += str;
        }
        return result;
    }

    public static string Pad(string left, string right, int score = 25, string padding = " ")
    {
        if (left.Length >= score) left = left.Remove(score - 1);

        string result = "";
        result += left; //left
        result += Repeat(padding, score - left.Length);
        result += right; //right
        return result;
    }

    static List<string> directoryEntries = new List<string>();
    public static string[] EnumDirEntries(string path, string filter = "*.*", int maxDepth = 0, int depth = 0, int fdepth = 0, bool parent = true, bool lastDir = false, bool lastFile = false)
    {
        int indent = 4;

        if (Directory.Exists(path))
        {
            //directory
            DirectoryInfo di = new DirectoryInfo(path);
            char[] space = Repeat(" ", (depth >= indent ? depth - indent : depth)).ToCharArray();
            for (int i = 0; i < space.Length; i += indent)
            {
                space[i] = '│';
            }

            directoryEntries.Add(String.Join("", space) + (depth != 0 ? (lastDir ? "└─" : "├─") : "") + "[" + di.Name + "]");
            if (lastDir) directoryEntries.Add(String.Join("", space) + "│");

            if (fdepth > maxDepth) return null;

            try
            {
                string[] directories = Directory.GetDirectories(path, filter, SearchOption.TopDirectoryOnly);
                for (int i = 0; i < directories.Length; i++)
                {
                    if (i != directories.Length - 1)
                    {
                        EnumDirEntries(directories[i], filter, maxDepth, depth + indent, fdepth + 1, false);
                    }
                    else
                    {
                        EnumDirEntries(directories[i], filter, maxDepth, depth + indent, fdepth + 1, false, true, false);
                    }
                }
            }
            catch (Exception)
            {
            }

            try
            {
                string[] files = Directory.GetFiles(path, filter, SearchOption.TopDirectoryOnly);
                for (int i = 0; i < files.Length; i++)
                {
                    if (i != files.Length - 1)
                    {
                        EnumDirEntries(files[i], filter, maxDepth, depth, fdepth, false);
                    }
                    else
                    {
                        EnumDirEntries(files[i], filter, maxDepth, depth, fdepth, false, false, true);
                    }
                }
            }
            catch (Exception)
            {
            }
        } else if (File.Exists(path))
        {
            //file
            FileInfo fi = new FileInfo(path);
            char[] space = Repeat(" ", depth).ToCharArray();
            for (int i = 0; i<space.Length; i+=indent)
            {
                space[i] = '│';
            }

            directoryEntries.Add(String.Join("", space) + (lastFile ? "└─" : "├─") + fi.Name);
            if (lastFile) directoryEntries.Add(String.Join("", space));
        }

        if (parent == true)
        {
            var copy = directoryEntries.ToArray();
            directoryEntries.Clear();
            return copy;
        }
        return null;
    }

    public static byte[] CompressBytes(byte[] bytes)
    {
        MemoryStream output = new MemoryStream();
        using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal))
        {
            dstream.Write(bytes, 0, bytes.Length);
        }
        return output.ToArray();
    }

    public static byte[] DecompressBytes(byte[] bytes)
    {
        MemoryStream input = new MemoryStream(bytes);
        MemoryStream output = new MemoryStream();
        using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
        {
            dstream.CopyTo(output);
        }
        return output.ToArray();
    }

    public static string GetOSName()
    {
        try
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false);
            string productName = (string)key.GetValue("ProductName");
            key.Close();
            return productName;
        }
        catch (Exception)
        {
        }
        return "";
    }

    public static bool IsAdministrator()
    {
        using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
        {
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }

    public static string[] GetInstalledSoftware(string searchKeyword = "")
    {
        List<string> software = new List<string>();
        try
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE", false);
            string[] vendors = key.GetSubKeyNames();
            foreach (string vendor in vendors)
            {
                RegistryKey vendorKey = key.OpenSubKey(vendor, false);
                string[] programs = vendorKey.GetSubKeyNames();
                foreach (string program in programs)
                {
                    RegistryKey programKey = vendorKey.OpenSubKey(program, false);
                    if (programKey.GetValueNames().Contains("DisplayName"))
                    {
                        string displayName = ((string)programKey.GetValue("DisplayName")).Trim();
                        if (!software.Contains(displayName))
                        {
                            if (searchKeyword.Length == 0)
                            {
                                software.Add(displayName);
                            }
                            else if (searchKeyword.Length > 0 && displayName.ToLower().Contains(searchKeyword.ToLower()))
                            {
                                software.Add(displayName);
                            }
                        }
                    }
                    programKey.Close();
                }
                vendorKey.Close();
            }
            key.Close();

            RegistryKey key1 = Registry.CurrentUser.OpenSubKey(@"Software", false);
            string[] vendors1 = key1.GetSubKeyNames();
            foreach (string vendor in vendors1) {
                RegistryKey vendorKey = key1.OpenSubKey(vendor, false);
                string[] programs = vendorKey.GetSubKeyNames();
                foreach (string program in programs)
                {
                    RegistryKey programKey = vendorKey.OpenSubKey(program, false);
                    if (programKey.GetValueNames().Contains("DisplayName"))
                    {
                        string displayName = ((string)programKey.GetValue("DisplayName")).Trim();
                        if (!software.Contains(displayName))
                        {
                            if (searchKeyword.Length == 0)
                            {
                                software.Add(displayName);
                            }
                            else if (searchKeyword.Length > 0 && displayName.ToLower().Contains(searchKeyword.ToLower()))
                            {
                                software.Add(displayName);
                            }
                        }
                    }
                    programKey.Close();
                }
                vendorKey.Close();
            }
            key1.Close();

            using (RegistryKey uKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"))
            {
                foreach (string sKeyStr in uKey.GetSubKeyNames())
                {
                    using (RegistryKey sKey = uKey.OpenSubKey(sKeyStr))
                    {
                        if (sKey.GetValueNames().Contains("DisplayName"))
                        {
                            string displayName = ((string)sKey.GetValue("DisplayName")).Trim();
                            if (!software.Contains(displayName))
                            {
                                if (searchKeyword.Length == 0)
                                {
                                    software.Add(displayName);
                                }
                                else if (searchKeyword.Length > 0 && displayName.ToLower().Contains(searchKeyword.ToLower()))
                                {
                                    software.Add(displayName);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
        }
        software.Sort();
        return software.ToArray();
    }

    public static string Encode(string text)
    {
        return Convert.ToBase64String(CompressBytes(Encoding.UTF8.GetBytes(text)));
    }
    public static string Decode(string text)
    {
        return Encoding.UTF8.GetString(DecompressBytes(Convert.FromBase64String(text)));
    }

    public static string[] Parse(string text)
    {
        var words = text.Replace("\t", " ").Split(" "[0]); var result = new List<string>(); bool insideBlock = false;
        var str = "";
        for (var i = 0; i < words.Length; i++)
        {
            if (insideBlock == true)
            {
                str += words[i] + " ";
            }
            if (insideBlock == false && !words[i].StartsWith("\""))
            {
                result.Add(words[i]);
            }
            if (words[i].StartsWith("\"") && insideBlock == false)
            {
                str += words[i].Remove(0, 1) + " ";
                insideBlock = true;
            }
            if (words[i].EndsWith("\"") && insideBlock == true)
            {
                result.Add(str.Remove(str.Length - 2, 2)); str = "";
                insideBlock = false;
            }
        }
        return result.ToArray();
    }

    public static string Exec(string program, string arguments, int timeoutSeconds = 5)
    {
        try
        {
            ProcessStartInfo pi = new ProcessStartInfo(program, arguments);
            if (cd.Length > 0)
            {
                pi.WorkingDirectory = cd;
            }
            pi.CreateNoWindow = true;
            pi.UseShellExecute = false;
            pi.RedirectStandardOutput = true;
            pi.RedirectStandardError = true;
            pi.ErrorDialog = false;

            Process p = Process.Start(pi);
            new Thread(() => {
                if (timeoutSeconds > 0)
                {
                    Thread.Sleep(timeoutSeconds * 1000);
                    if (!p.HasExited) p.Kill();
                }
                else
                {
                    while (!p.HasExited)
                    {
                        Thread.Sleep(1000);
                        if (p.StandardOutput.Peek() > 0)
                        {
                            p.Kill();
                        }
                    }
                }
            }).Start();
            p.WaitForExit();
            if (p.HasExited)
            {
                string output = p.StandardOutput.ReadToEnd();
                if (output == "")
                {
                    string error = p.StandardError.ReadToEnd();
                    return error.Trim();
                }
                else
                {
                    return output.Trim();
                }
            }
        }
        catch (Exception)
        {
        }

        return "";
    }

    public static string RandomString(int len = 25)
    {
        byte[] buffer = new byte[len + 10];
        r.NextBytes(buffer);
        string str = Convert.ToBase64String(buffer);
        return str.Remove(len).Replace(@"/", "3").Replace(@"+", "1").ToUpper();
    }
}