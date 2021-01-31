using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Collections.Specialized;

class Server
{
    Action<Dictionary<string, object>> onRequest = null;
    public Action<Dictionary<string, object>> OnRequest { get { return onRequest; } set { onRequest = value; } }

    HttpListener listener;
    public Server(int port)
    {
        listener = new HttpListener();
        listener.Prefixes.Add("http://*:" + port.ToString() + "/");

        listener.Start();

        listener.BeginGetContext(new AsyncCallback(Callback), listener);

        //new Thread(() => {
        //    try
        //    {
        //        listener.Start();

        //        while (true)
        //        {
        //            try
        //            {
        //                HttpListenerContext context = listener.GetContext();
        //                string path = context.Request.Url.AbsoluteUri.ToString();
        //                string method = context.Request.HttpMethod.ToUpper();
        //                Process(context);
        //            }
        //            catch (Exception)
        //            {
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}).Start();
    }

    public void Callback(IAsyncResult result)
    {
        //start accepting new
        HttpListener listener = (HttpListener)result.AsyncState;
        listener.BeginGetContext(new AsyncCallback(Callback), listener);

        try
        {
            //handle this request
            HttpListenerContext context = listener.EndGetContext(result);
            Process(context);
        }
        catch (Exception)
        {
        }
    }

    public void SendResponse(HttpListenerContext ctx, byte[] bytes, Encoding encoding)
    {
        Stream stream = Utils.ToStream(bytes, encoding);
        ctx.Response.ContentLength64 = stream.Length;
        ctx.Response.StatusCode = (int)HttpStatusCode.OK;

        byte[] buffer = new byte[1024 * 16];
        int n;
        while ((n = stream.Read(buffer, 0, buffer.Length)) > 0)
            ctx.Response.OutputStream.Write(buffer, 0, n);
        ctx.Response.OutputStream.Close();
    }

    public void SendString(HttpListenerContext ctx, string text)
    {
        SendResponse(ctx, Encoding.UTF8.GetBytes(text), Encoding.UTF8);
    }

    public void SendJsonResponse(HttpListenerContext ctx, Dictionary<string, object> obj)
    {
        SendResponse(ctx, Encoding.UTF8.GetBytes(JSON.stringify(obj)), Encoding.UTF8);
    }

    private void Process(HttpListenerContext context)
    {
        try
        {
            IPEndPoint remoteAddress = context.Request.RemoteEndPoint;
            string ip = remoteAddress.Address.ToString();
            string METHOD = context.Request.HttpMethod.ToUpper();
            NameValueCollection GET = context.Request.QueryString;
            Stream inputStream = context.Request.InputStream;
            byte[] POST = new byte[] { };
            if (METHOD == "POST")
            {
                POST = Encoding.UTF8.GetBytes(new StreamReader(inputStream).ReadToEnd());
            }

            if (OnRequest != null)
            {
                var var = new Dictionary<string, object>();
                var.Add("ctx", context); // HttpListenerContext
                var.Add("ip", ip); // string
                var.Add("method", METHOD); // string
                var.Add("get", GET); // NameValueCollection
                var.Add("post", POST); // byte[]
                var.Add("query", context.Request.RawUrl); //string
                OnRequest(var);
            }
        }
        catch (Exception)
        {
        }
    }
}