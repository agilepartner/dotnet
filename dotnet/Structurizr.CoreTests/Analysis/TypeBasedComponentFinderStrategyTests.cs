using Microsoft.VisualStudio.TestTools.UnitTesting;
using Structurizr.Analysis;
using System.Collections.Generic;
using System.Linq;

namespace Structurizr.CoreTests.Analysis
{
    [TestClass]
    public class TypeBasedComponentFinderStrategyTests
    {
        [TestMethod]
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

            Assert.AreEqual(1, components.Count);
            Assert.AreEqual("MyController", components.First().Name);
        }

    }

}