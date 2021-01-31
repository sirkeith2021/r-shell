using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

class Utils
{
    static Random r = new Random();

    public static string GetUnixTimestamp()
    {
        var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
        return ((long)timeSpan.TotalSeconds).ToString();
    }

    public static string GetTimestamp()
    {
        return DateTime.Now.ToString();
    }

    public static string Encode(string text)
    {
        return Convert.ToBase64String(CompressBytes(Encoding.UTF8.GetBytes(text)));
    }
    public static string Decode(string text)
    {
        return Encoding.UTF8.GetString(DecompressBytes(Convert.FromBase64String(text)));
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

    public static string RemoveUnsupportedChars(string str)
    {
        char[] supported = "\t\n\r\0─│└├ABCDEFGHIJKLMNOPQRSTUVWXYZÖÄÅabcdefghijklmnopqrstuvwxyzöäå1234567890!\"£$€{[]}\\`<>'*^.:,;-_!#¤%&/()=?€~|@+ ".ToCharArray();
        string newstr = "";
        foreach (char c in str)
        {
            if (!supported.Contains(c))
            {
                newstr += " ";
            }
            else
            {
                newstr += c;
            }
        }

        return newstr;
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

    public static Stream ToStream(byte[] bytes, Encoding encoding)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(encoding.GetString(bytes)); writer.Flush();
        stream.Position = 0;
        return stream;
    }

    public static string RandomString(int len = 25)
    {
        byte[] buffer = new byte[len + 10];
        Utils.r.NextBytes(buffer);
        string str = Convert.ToBase64String(buffer);
        return str.Remove(len).Replace(@"/", "3").Replace(@"+", "1").ToUpper();
    }
}