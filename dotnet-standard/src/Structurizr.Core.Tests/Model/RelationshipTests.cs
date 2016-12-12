using FluentAssertions;
using Xunit;

namespace Structurizr.CoreTests
{
    public class RelationshipTests : AbstractTestBase
    {

        private SoftwareSystem _softwareSystem1, _softwareSystem2;

        public RelationshipTests()
        {
            _softwareSystem1 = model.AddSoftwareSystem("1", "Description");
            _softwareSystem2 = model.AddSoftwareSystem("2", "Description");
        }

        [Fact]
        public void Test_Description_NeverReturnsNull()
        {
            Relationship relationship = new Relationship();
            relationship.Description = null;

            relationship.Description.Should().Be("");
        }

        [Fact]
        public void Test_Tags_WhenThereAreNoTags()
        {
            Relationship relationship = _softwareSystem1.Uses(_softwareSystem2, "uses");
            relationship.Tags.Should().Be("Relationship,Synchronous");
        }

        [Fact]
        public void Test_Tags_ReturnsTheListOfTags_WhenThereAreSomeTags()
        {
            Relationship relationship = _softwareSystem1.Uses(_softwareSystem2, "uses");
            relationship.AddTags("tag1", "tag2", "tag3");
            relationship.Tags.Should().Be("Relationship,Synchronous,tag1,tag2,tag3");
        }

        [Fact]
        public void Test_Tags_DoesNotDoAnything_WhenPassedNull()
        {
            Relationship relationship = _softwareSystem1.Uses(_softwareSystem2, "uses");
            relationship.Tags = null;
            relationship.Tags.Should().Be("Relationship,Synchronous");
        }

        [Fact]
        public void Test_AddTags_DoesNotDoAnything_WhenPassedNull()
        {
            Relationship relationship = _softwareSystem1.Uses(_softwareSystem2, "uses");
            relationship.AddTags((string)null);
            relationship.Tags.Should().Be("Relationship,Synchronous");

            relationship.AddTags(null, null, null);
            relationship.Tags.Should().Be("Relationship,Synchronous");
        }

        [Fact]
        public void Test_AddTags_AddsTags_WhenPassedSomeTags()
        {
            Relationship relationship = _softwareSystem1.Uses(_softwareSystem2, "uses");
            relationship.AddTags(null, "tag1", null, "tag2");
            relationship.Tags.Should().Be("Relationship,Synchronous,tag1,tag2");
        }

        [Fact]
        public void Test_InteractionStyle_ReturnsSynchronous_WhenNotExplicitlySet()
        {
            Relationship relationship = _softwareSystem1.Uses(_softwareSystem2, "uses");
            relationship.InteractionStyle.Should().Be(InteractionStyle.Synchronous);
        }

        [Fact]
        public void test_Tags_IncludesTheInteractionStyleWhenSpecified()
        {
            Relationship relationship = _softwareSystem1.Uses(_softwareSystem2, "uses");
            relationship.Tags.Should().Contain(Tags.Synchronous);
            relationship.Tags.Should().NotContain(Tags.Asynchronous);

            relationship.InteractionStyle = InteractionStyle.Asynchronous;
            relationship.Tags.Should().NotContain(Tags.Synchronous);
            relationship.Tags.Should().Contain(Tags.Asynchronous);
        }
    }
}