using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

static class Request
{
    //GET

    public static Dictionary<string, object> Get(string url, int timeout = 60000)
    {
        return JSON.parse(Encoding.UTF8.GetString(GetBytes(url, timeout)));
    }

    public static string GetString(string url, int timeout = 60000)
    {
        return Encoding.UTF8.GetString(GetBytes(url, timeout));
    }

    public static byte[] GetBytes(string url, int timeout = 60000)
    {
        var req = (HttpWebRequest)WebRequest.Create(url);
        req.Timeout = timeout;

        var res = (HttpWebResponse)req.GetResponse();
        using (MemoryStream ms = new MemoryStream())
        {
            res.GetResponseStream().CopyTo(ms);
            return ms.ToArray();
        }
    }

    //POST

    public static Dictionary<string, object> Post(string url, Dictionary<string, object> data, int timeout = 60000)
    {
        return JSON.parse(Encoding.UTF8.GetString(PostBytes(url, Encoding.UTF8.GetBytes(JSON.stringify(data)), timeout)));
    }

    public static string PostString(string url, string data, int timeout = 60000)
    {
        return Encoding.UTF8.GetString(PostBytes(url, Encoding.UTF8.GetBytes(data), timeout));
    }

    public static byte[] PostBytes(string url, byte[] bytes, int timeout = 60000)
    {
        var req = (HttpWebRequest)WebRequest.Create(url);

        req.Method = "POST";
        req.Timeout = timeout;
        req.ContentType = "application/json";
        req.ContentLength = bytes.Length;

        using (var stream = req.GetRequestStream())
        {
            stream.Write(bytes, 0, bytes.Length);
        }

        var res = (HttpWebResponse)req.GetResponse();
        using (MemoryStream ms = new MemoryStream())
        {
            res.GetResponseStream().CopyTo(ms);
            return ms.ToArray();
        }
    }
}