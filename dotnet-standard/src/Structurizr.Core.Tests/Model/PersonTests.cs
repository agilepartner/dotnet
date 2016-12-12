using FluentAssertions;
using Xunit;

namespace Structurizr.CoreTests
{
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

        [Fact]
        public void Test_CanonicalName()
        {
            person.CanonicalName.Should().Be("/Name");
        }

        [Fact]
        public void Test_CanonicalName_WhenNameContainsASlashCharacter()
        {
            person.Name = "Name1/Name2";
            person.CanonicalName.Should().Be("/Name1Name2");
        }

        [Fact]
        public void Test_Parent_ReturnsNull()
        {
            person.Parent.Should().BeNull();
        }

        [Fact]
        public void Test_RemoveTags_DoesNotRemoveRequiredTags()
        {
            person.Tags.Should().Contain(Tags.Element);
            person.Tags.Should().Contain(Tags.Person);

            person.RemoveTag(Tags.Person);
            person.RemoveTag(Tags.Element);

            person.Tags.Should().Contain(Tags.Element);
            person.Tags.Should().Contain(Tags.Person);
        }

    }
}