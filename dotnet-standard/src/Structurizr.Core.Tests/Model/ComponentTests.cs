using FluentAssertions;
using Xunit;

namespace Structurizr.CoreTests
{
    public class ComponentTests
    {

        private Workspace workspace;
        private Model model;
        private SoftwareSystem softwareSystem;
        private Container container;

        public ComponentTests()
        {
            workspace = new Workspace("Name", "Description");
            model = workspace.Model;
            softwareSystem = model.AddSoftwareSystem("System", "Description");
            container = softwareSystem.AddContainer("Container", "Description", "Some technology");
        }

        [Fact]
        public void Test_Name_ReturnsTheGivenName_WhenANameIsGiven()
        {
            Component component = new Component();
            component.Name = "Some name";

            component.Name.Should().Be("Some name");
        }

        [Fact]
        public void Test_CanonicalName()
        {
            Component component = container.AddComponent("Component", "Description");
            component.CanonicalName.Should().Be("/System/Container/Component");
        }

        [Fact]
        public void Test_CanonicalName_WhenNameContainsASlashCharacter()
        {
            Component component = container.AddComponent("Name1/Name2", "Description");
            component.CanonicalName.Should().Be("/System/Container/Name1Name2");
        }

        [Fact]
        public void Test_Parent_ReturnsTheParentContainer()
        {
            Component component = container.AddComponent("Component", "Description");
            component.Parent.Should().Be(container);
        }

        [Fact]
        public void Test_Container_ReturnsTheParentContainer()
        {
            Component component = container.AddComponent("Name", "Description");
            component.Container.Should().Be(container);
        }

        [Fact]
        public void Test_RemoveTags_DoesNotRemoveRequiredTags()
        {
            Component component = new Component();

            component.Tags.Should().Contain(Tags.Element);
            component.Tags.Should().Contain(Tags.Component);

            component.RemoveTag(Tags.Component);
            component.RemoveTag(Tags.Element);

            component.Tags.Should().Contain(Tags.Element);
            component.Tags.Should().Contain(Tags.Component);
        }

    }
}