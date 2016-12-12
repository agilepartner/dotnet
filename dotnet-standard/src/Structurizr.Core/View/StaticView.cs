using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Structurizr
{

    [DataContract]
    public abstract class StaticView : View
    {

        internal StaticView() : base()
        {
        }

        internal StaticView(SoftwareSystem softwareSystem, string key, string description) : base(softwareSystem, key, description)
        {
        }

        public abstract void AddAllElements();

        /// <summary>
        /// Adds all software systems in the model to this view.
        /// </summary>
        public void AddAllSoftwareSystems()
        {
            foreach (SoftwareSystem softwareSystem in this.Model.SoftwareSystems)
            {
                Add(softwareSystem);
            }
        }

        /// <summary>
        /// Adds the given SoftwareSystem to this view.
        /// </summary>
        public virtual void Add(SoftwareSystem softwareSystem)
        {
            AddElement(softwareSystem, true);
        }

        /// <summary>
        /// Removes the given SoftwareSystem from this view.
        /// </summary>
        /// <param name="softwareSystem"></param>
        public void Remove(SoftwareSystem softwareSystem)
        {
            RemoveElement(softwareSystem);
        }

        /// <summary>
        /// Adds all people in the model to this view.
        /// </summary>
        public void AddAllPeople()
        {
            foreach (Person person in this.Model.People)
            {
                Add(person);
            }
        }

        /// <summary>
        /// Adds the given Person to this view.
        /// </summary>
        public void Add(Person person)
        {
            AddElement(person, true);
        }

        /// <summary>
        /// Removes the given Person from this view.
        /// </summary>
        /// <param name="person"></param>
        public void Remove(Person person)
        {
            RemoveElement(person);
        }

        public abstract void AddNearestNeighbours(Element element);

        protected void AddNearestNeighbours(Element element, Type typeOfElement)
        {
            if (element == null)
            {
                return;
            }

            AddElement(element, true);

            ICollection<Relationship> relationships = Model.Relationships;
            foreach (Relationship relationship in relationships)
            {
                if (relationship.Source.Equals(element) && relationship.Destination.GetType() == typeOfElement)
                {
                    AddElement(relationship.Destination, true);
                }

                if (relationship.Destination.Equals(element) && relationship.Source.GetType() == typeOfElement)
                {
                    AddElement(relationship.Source, true);
                }
            }
        }

    }
}
