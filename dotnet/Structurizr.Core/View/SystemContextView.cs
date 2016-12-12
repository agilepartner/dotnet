using System.Runtime.Serialization;

namespace Structurizr
{

    /// <summary>
    /// A system context view.
    /// </summary>
    [DataContract]
    public class SystemContextView : StaticView
    {

        public override string Name
        {
            get
            {
                return SoftwareSystem.Name + " - System Context";
            }
        }

        internal SystemContextView() : base()
        {
        }

        internal SystemContextView(SoftwareSystem softwareSystem, string key, string description) : base(softwareSystem, key, description)
        {
            AddElement(softwareSystem, true);
        }

        /// <summary>
        /// Adds all software systems and all people to this view.
        /// </summary>
        public override void AddAllElements()
        {
            AddAllSoftwareSystems();
            AddAllPeople();
        }

        /// <summary>
        /// Adds people and software systems that are directly related to the given element.
        /// </summary>
        public override void AddNearestNeighbours(Element element)
        {
            AddNearestNeighbours(element, typeof(SoftwareSystem));
            AddNearestNeighbours(element, typeof(Person));
        }

    }
}
