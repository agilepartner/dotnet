using System.Collections.Generic;
using System.Reflection;

namespace Structurizr.Analysis
{
    public abstract class ComponentFinderStrategy
    {

        public ComponentFinder ComponentFinder { get; internal set; }

        public abstract ICollection<Component> FindComponents();

        public abstract void FindDependencies();

    }
}
