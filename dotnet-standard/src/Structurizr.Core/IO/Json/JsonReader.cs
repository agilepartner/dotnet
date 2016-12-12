using Newtonsoft.Json;
using System.IO;

namespace Structurizr.IO.Json
{
    public class JsonReader
    {

        public Workspace Read(StringReader reader)
        {
            Workspace workspace = JsonConvert.DeserializeObject<Workspace>(
                reader.ReadToEnd(),
                new Newtonsoft.Json.Converters.StringEnumConverter(),
                new PaperSizeJsonConverter());
            workspace.Hydrate();

            return workspace;
        }

    }
}
