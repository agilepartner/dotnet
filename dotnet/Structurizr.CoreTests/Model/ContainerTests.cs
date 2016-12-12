using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Structurizr.CoreTests
{
    [TestClass]
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

        [TestMethod]
        public void Test_CanonicalName()
        {
            Assert.AreEqual("/System/Container", container.CanonicalName);
        }

        [TestMethod]
        public void Test_CanonicalName_WhenNameContainsASlashCharacter()
        {
            container.Name = "Name1/Name2";
            Assert.AreEqual("/System/Name1Name2", container.CanonicalName);
        }

        [TestMethod]
        public void Test_Parent_ReturnsTheParentSoftwareSystem()
        {
            Assert.AreEqual(softwareSystem, container.Parent);
        }

        [TestMethod]
        public void Test_SoftwareSystem_ReturnsTheParentSoftwareSystem()
        {
            Assert.AreEqual(softwareSystem, container.SoftwareSystem);
        }

        [TestMethod]
        public void Test_RemoveTags_DoesNotRemoveRequiredTags()
        {
            Assert.IsTrue(container.Tags.Contains(Tags.Element));
            Assert.IsTrue(container.Tags.Contains(Tags.Container));

            container.RemoveTag(Tags.Container);
            container.RemoveTag(Tags.Element);

            Assert.IsTrue(container.Tags.Contains(Tags.Element));
            Assert.IsTrue(container.Tags.Contains(Tags.Container));
        }

    }
}