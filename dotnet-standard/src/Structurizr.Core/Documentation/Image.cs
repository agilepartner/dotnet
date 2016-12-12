using System.Runtime.Serialization;

namespace Structurizr
{

    [DataContract]
    public class Image
    {

        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; private set; }

        [DataMember(Name = "content", EmitDefaultValue = false)]
        public string Content { get; private set; }

        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string Type { get; private set; }

        public Image() { }

        public Image(string name, string content, string type)
        {
            this.Name = name;
            this.Content = content;
            this.Type = type;
        }

    }
}
