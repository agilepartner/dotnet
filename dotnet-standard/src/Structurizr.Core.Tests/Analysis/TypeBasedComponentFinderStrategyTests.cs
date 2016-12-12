using FluentAssertions;
using Structurizr.Analysis;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Structurizr.CoreTests.Analysis
{
    public class TypeBasedComponentFinderStrategyTests
    {
        [Fact]
        public void Test_FindComponents()
        {
            Workspace workspace = new Workspace("Name", "Description");
            Container container = workspace.Model.AddSoftwareSystem("Name", "Description").AddContainer("Name", "Description", "Technology");

            ComponentFinder componentFinder = new ComponentFinder(
                container,
                typeof(MyApp.MyController).Namespace,
                new TypeBasedComponentFinderStrategy(
                    new NameSuffixTypeMatcher("Controller", "", "ASP.NET MVC")
                ));

            ICollection<Component> components = componentFinder.FindComponents().ToList();

            components.Should().HaveCount(1);
            components.Single().Name.Should().Be("MyController");
        }
    }
}