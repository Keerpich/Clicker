using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clicker
{
    class ActionsJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);

            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                JObject o = (JObject)t;

                String objectType = value.GetType().FullName;
                o.AddFirst(new JProperty("ObjectType", objectType));

                o.WriteTo(writer);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            JObject jObject = JObject.Load(reader);

            string actionTypeName = jObject.Property("ObjectType").Value.ToString();
            Type actionType = Type.GetType(actionTypeName);
            Object action = Activator.CreateInstance(actionType);

            jObject.Remove("ObjectType");

            serializer.Populate(jObject.CreateReader(), action);

            return action;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Action).IsAssignableFrom(objectType);
        }
    }
}
