using System.Runtime.Serialization;

namespace Structurizr
{

    [DataContract]
    public class Section
    {

        public Element Element { get; internal set; }

        private string elementId;

        /// <summary>
        /// The ID of the element.
        /// </summary>
        [DataMember(Name = "elementId", EmitDefaultValue = false)]
        public string ElementId
        {
            get
            {
                if (this.Element != null)
                {
                    return this.Element.Id;
                }
                else
                {
                    return this.elementId;
                }
            }

            set
            {
                this.elementId = value;
            }
        }

        [DataMember(Name = "type", EmitDefaultValue = true)]
        public SectionType Type { get; private set; }

        [DataMember(Name = "format", EmitDefaultValue = true)]
        public DocumentationFormat Format { get; private set; }

        [DataMember(Name = "content", EmitDefaultValue = false)]
        public string Content { get; private set; }

        internal Section() { }

        internal Section(Element element, SectionType type, DocumentationFormat format, string content)
        {
            this.Element = element;
            this.Type = type;
            this.Format = format;
            this.Content = content;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Section);
        }

        public bool Equals(Section section)
        {
            if (section == null)
            {
                return false;
            }
            if (section == this)
            {
                return true;
            }

            return ElementId.Equals(section.ElementId) && Type == section.Type;
        }

        public override int GetHashCode()
        {
            int result = ElementId.GetHashCode();
            result = 31 * result + Type.GetHashCode();
            return result;
        }

    }
}
