using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;

class LiveClient
{
    private string token = ""; public string Token { get { return token; } set { token = value; } }
    private string name = ""; public string Name { get { return name; } set { name = value; } }
    private string user = ""; public string User { get { return user; } set { user = value; } }
    private string platform = ""; public string Platform { get { return platform; } set { platform = value; } }
    private string version = ""; public string Version { get { return version; } set { version = value; } }
    private string time = Utils.GetTimestamp(); public string Time { get { return time; } set { time = value; } }
    private bool elevated = false; public bool Elevated { get { return elevated; } set { elevated = value; } }
    private HttpListenerContext listener = null; public HttpListenerContext HttpListener { get { return listener; } set { listener = value; } }
    private bool active = true;
    int joined = Int32.Parse(Utils.GetUnixTimestamp());
    int timeout = 60; //seconds after the connection expires and is no longer considered active
    private Action<Dictionary<string, object>> onMessage = null; public Action<Dictionary<string, object>> OnMessage { get { return onMessage; } set { onMessage = value; } }


    public LiveClient()
    {

    }

    public void Reactivate() //if reactivated the HttpListenerContext should also be updated
    {
        active = true;
    }

    public bool IsOnline()
    {
        return (listener != null && active && (Int32.Parse(Utils.GetUnixTimestamp()) - joined) < timeout);
    }

    public void Send(Dictionary<string, object> obj)
    {
        if (IsOnline())
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(Utils.Encode(JSON.stringify(obj)));

                listener.Response.ContentLength64 = bytes.Length;
                listener.Response.StatusCode = (int)HttpStatusCode.OK;

                listener.Response.OutputStream.Write(bytes, 0, bytes.Length);
                listener.Response.OutputStream.Close();
            }
            catch (Exception)
            {
            }
        }

        active = false;
    }
}
