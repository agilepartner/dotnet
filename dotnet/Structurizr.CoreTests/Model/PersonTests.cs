using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Structurizr.CoreTests
{
    [TestClass]
    public class PersonTests
    {

        private Workspace workspace;
        private Model model;
        private Person person;

        public PersonTests()
        {
            workspace = new Workspace("Name", "Description");
            model = workspace.Model;
            person = model.AddPerson("Name", "Description");
        }

        [TestMethod]
        public void Test_CanonicalName()
        {
            Assert.AreEqual("/Name", person.CanonicalName);
        }

        [TestMethod]
        public void Test_CanonicalName_WhenNameContainsASlashCharacter()
        {
            person.Name = "Name1/Name2";
            Assert.AreEqual("/Name1Name2", person.CanonicalName);
        }

        [TestMethod]
        public void Test_Parent_ReturnsNull()
        {
            Assert.IsNull(person.Parent);
        }

        [TestMethod]
        public void Test_RemoveTags_DoesNotRemoveRequiredTags()
        {
            Assert.IsTrue(person.Tags.Contains(Tags.Element));
            Assert.IsTrue(person.Tags.Contains(Tags.Person));

            person.RemoveTag(Tags.Person);
            person.RemoveTag(Tags.Element);

            Assert.IsTrue(person.Tags.Contains(Tags.Element));
            Assert.IsTrue(person.Tags.Contains(Tags.Person));
        }

    }
}