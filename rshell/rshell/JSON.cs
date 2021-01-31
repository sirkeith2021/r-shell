using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

public class JSON
{
    public static Dictionary<string, object> create()
    {
        return new Dictionary<string, object>();
    }
    public static Dictionary<string, object> parse(string text)
    {
        var serializer = new JavaScriptSerializer();
        var arr = serializer.Deserialize<Dictionary<string, object>>(text);
        return arr;
    }
    public static string stringify(Dictionary<string, string> obj)
    {
        var serializer = new JavaScriptSerializer();
        var json = serializer.Serialize(obj);
        return json;
    }
    public static string stringify(Dictionary<string, object> obj)
    {
        var serializer = new JavaScriptSerializer();
        var json = serializer.Serialize(obj);
        return json;
    }
}