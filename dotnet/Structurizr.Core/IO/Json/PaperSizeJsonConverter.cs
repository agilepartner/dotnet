using Newtonsoft.Json;
using System;

namespace Structurizr.IO.Json
{
    internal class PaperSizeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(PaperSize).IsAssignableFrom(objectType);
        }

        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return PaperSize.GetPaperSize(reader.Value as string);
        }

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, JsonSerializer serializer)
        {
            PaperSize paperSize = value as PaperSize;
            if (paperSize != null)
            {
                writer.WriteValue(paperSize.Key);
            }
        }
    }
}
