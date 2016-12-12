using System.Collections.Generic;

namespace Structurizr.Analysis
{
    public class ComponentFinder
    {

        public Container Container { get; }
        public string Namespace { get; }

        private List<ComponentFinderStrategy> ComponentFinderStrategies = new List<ComponentFinderStrategy>();

        public ComponentFinder(Container container, string namespaceToScan, params ComponentFinderStrategy[] componentFinderStrategies)
        {
            this.Container = container;
            this.Namespace = namespaceToScan;

            foreach (ComponentFinderStrategy componentFinderStrategy in componentFinderStrategies)
            {
                this.ComponentFinderStrategies.Add(componentFinderStrategy);
                componentFinderStrategy.ComponentFinder = this;
            }
        }

        public ICollection<Component> FindComponents()
        {
            List<Component> componentsFound = new List<Component>();

            foreach (ComponentFinderStrategy componentFinderStrategy in ComponentFinderStrategies) {
                componentsFound.AddRange(componentFinderStrategy.FindComponents());
            }

            foreach (ComponentFinderStrategy componentFinderStrategy in ComponentFinderStrategies)
            {
                componentFinderStrategy.FindDependencies();
            }

            return componentsFound;
        }

    }
}
