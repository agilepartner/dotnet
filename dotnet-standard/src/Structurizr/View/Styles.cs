using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Structurizr
{

    /// <summary>
    /// The styles associated with this set of views.
    /// </summary>
    [DataContract]
    public class Styles
    {

        /// <summary>
        /// The set of relationship styles.
        /// </summary>
        [DataMember(Name="relationships", EmitDefaultValue=false)]
        public List<RelationshipStyle> Relationships { get; set; }
  
        /// <summary>
        /// The set of element styles.
        /// </summary>
        [DataMember(Name="elements", EmitDefaultValue=false)]
        public List<ElementStyle> Elements { get; set; }
  
        internal Styles()
        {
            this.Elements = new List<ElementStyle>();
            this.Relationships = new List<RelationshipStyle>();
        }

        public void Add(ElementStyle elementStyle)
        {
            if (elementStyle != null)
            {
                this.Elements.Add(elementStyle);
            }
        }

        public void Add(RelationshipStyle relationshipStyle)
        {
            if (relationshipStyle != null)
            {
                this.Relationships.Add(relationshipStyle);
            }
        }

    }
}
