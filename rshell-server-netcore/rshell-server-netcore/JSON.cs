using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class JSON
{
    public static Dictionary<string, object> create()
    {
        return new Dictionary<string, object>();
    }

    public static string stringify(Dictionary<string, object> obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    public static Dictionary<string, object> parse(string str)
    {
        return JsonConvert.DeserializeObject<Dictionary<string, object>>(
            str, new JsonConverter[] { new MyConverter() });
    }
}

class MyConverter : Newtonsoft.Json.Converters.CustomCreationConverter<IDictionary<string, object>>
{
    public override IDictionary<string, object> Create(Type objectType)
    {
        return new Dictionary<string, object>();
    }

    public override bool CanConvert(Type objectType)
    {
        // in addition to handling IDictionary<string, object>
        // we want to handle the deserialization of dict value
        // which is of type object
        return objectType == typeof(object) || base.CanConvert(objectType);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.StartObject
            || reader.TokenType == JsonToken.Null)
            return base.ReadJson(reader, objectType, existingValue, serializer);

        // if the next token is not an object
        // then fall back on standard deserializer (strings, numbers etc.)
        return serializer.Deserialize(reader);
    }
}