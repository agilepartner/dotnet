using FluentAssertions;
using Xunit;

namespace Structurizr.CoreTests
{
    public class ContainerTests
    {

        private Workspace workspace;
        private Model model;
        private SoftwareSystem softwareSystem;
        private Container container;

        public ContainerTests()
        {
            workspace = new Workspace("Name", "Description");
            model = workspace.Model;
            softwareSystem = model.AddSoftwareSystem("System", "Description");
            container = softwareSystem.AddContainer("Container", "Description", "Some technology");
        }

        [Fact]
        public void Test_CanonicalName()
        {
            container.CanonicalName.Should().Be("/System/Container");
        }

        [Fact]
        public void Test_CanonicalName_WhenNameContainsASlashCharacter()
        {
            container.Name = "Name1/Name2";
            container.CanonicalName.Should().Be("/System/Name1Name2");
        }

        [Fact]
        public void Test_Parent_ReturnsTheParentSoftwareSystem()
        {
            container.Parent.Should().Be(softwareSystem);
        }

        [Fact]
        public void Test_SoftwareSystem_ReturnsTheParentSoftwareSystem()
        {
            container.SoftwareSystem.Should().Be(softwareSystem);
        }

        [Fact]
        public void Test_RemoveTags_DoesNotRemoveRequiredTags()
        {
            container.Tags.Should().Contain(Tags.Element);
            container.Tags.Should().Contain(Tags.Container);

            container.RemoveTag(Tags.Container);
            container.RemoveTag(Tags.Element);

            container.Tags.Should().Contain(Tags.Element);
            container.Tags.Should().Contain(Tags.Container);
        }
    }
}