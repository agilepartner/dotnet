using FluentAssertions;
using System;
using Xunit;

namespace Structurizr.CoreTests
{
    public class ElementTests : AbstractTestBase
    {

        [Fact]
        public void Test_HasEfferentRelationshipWith_ReturnsFalse_WhenANullElementIsSpecified()
        {
            SoftwareSystem softwareSystem1 = model.AddSoftwareSystem("System 1", "");
            softwareSystem1.HasEfferentRelationshipWith(null).Should().BeFalse();
        }

        [Fact]
        public void Test_HasEfferentRelationshipWith_ReturnsFalse_WhenTheSameElementIsSpecifiedAndNoCyclicRelationshipExists()
        {
            SoftwareSystem softwareSystem1 = model.AddSoftwareSystem("System 1", "");
            softwareSystem1.HasEfferentRelationshipWith(softwareSystem1).Should().BeFalse();
        }

        [Fact]
        public void Test_HasEfferentRelationshipWith_ReturnsTrue_WhenTheSameElementIsSpecifiedAndACyclicRelationshipExists()
        {
            SoftwareSystem softwareSystem1 = model.AddSoftwareSystem("System 1", "");
            softwareSystem1.Uses(softwareSystem1, "uses");
            softwareSystem1.HasEfferentRelationshipWith(softwareSystem1).Should().BeTrue();
        }

        [Fact]
        public void Test_HasEfferentRelationshipWith_ReturnsTrue_WhenThereIsARelationship()
        {
            SoftwareSystem softwareSystem1 = model.AddSoftwareSystem("System 1", "");
            SoftwareSystem softwareSystem2 = model.AddSoftwareSystem("System 2", "");
            softwareSystem1.Uses(softwareSystem2, "uses");

            softwareSystem1.HasEfferentRelationshipWith(softwareSystem2).Should().BeTrue();
        }

        [Fact]
        public void Test_GetEfferentRelationshipWith_ReturnsNull_WhenANullElementIsSpecified()
        {
            SoftwareSystem softwareSystem1 = model.AddSoftwareSystem("System 1", "");
            softwareSystem1.GetEfferentRelationshipWith(null).Should().BeNull();
        }

        [Fact]
        public void Test_GetEfferentRelationshipWith_ReturnsNull_WhenTheSameElementIsSpecifiedAndNoCyclicRelationshipExists()
        {
            SoftwareSystem softwareSystem1 = model.AddSoftwareSystem("System 1", "");
            softwareSystem1.GetEfferentRelationshipWith(softwareSystem1).Should().BeNull();
        }

        [Fact]
        public void Test_GetEfferentRelationshipWith_ReturnsCyclicRelationship_WhenTheSameElementIsSpecifiedAndACyclicRelationshipExists()
        {
            SoftwareSystem softwareSystem1 = model.AddSoftwareSystem("System 1", "");
            softwareSystem1.Uses(softwareSystem1, "uses");

            Relationship relationship = softwareSystem1.GetEfferentRelationshipWith(softwareSystem1);

            relationship.Source.Should().Be(softwareSystem1);
            relationship.Description.Should().Be("uses");
            relationship.Destination.Should().Be(softwareSystem1);
        }

        [Fact]
        public void Test_GetEfferentRelationshipWith_ReturnsTheRelationship_WhenThereIsARelationship()
        {
            SoftwareSystem softwareSystem1 = model.AddSoftwareSystem("System 1", "");
            SoftwareSystem softwareSystem2 = model.AddSoftwareSystem("System 2", "");
            softwareSystem1.Uses(softwareSystem2, "uses");

            Relationship relationship = softwareSystem1.GetEfferentRelationshipWith(softwareSystem2);

            relationship.Source.Should().Be(softwareSystem1);
            relationship.Description.Should().Be("uses");
            relationship.Destination.Should().Be(softwareSystem2);
        }

        [Fact]
        public void Test_HasAfferentRelationships_ReturnsFalse_WhenThereAreNoIncomingRelationships()
        {
            SoftwareSystem softwareSystem1 = model.AddSoftwareSystem("System 1", "");
            SoftwareSystem softwareSystem2 = model.AddSoftwareSystem("System 2", "");
            softwareSystem1.Uses(softwareSystem2, "Uses");

            softwareSystem1.HasAfferentRelationships().Should().BeFalse();
        }

        [Fact]
        public void Test_HasAfferentRelationships_ReturnsTrue_WhenThereAreIncomingRelationships()
        {
            SoftwareSystem softwareSystem1 = model.AddSoftwareSystem("System 1", "");
            SoftwareSystem softwareSystem2 = model.AddSoftwareSystem("System 2", "");
            softwareSystem1.Uses(softwareSystem2, "Uses");

            softwareSystem2.HasAfferentRelationships().Should().BeTrue();
        }

        [Fact]
        public void Test_SetUrl_DoesNotThrowAnException_WhenANullUrlIsSpecified()
        {
            SoftwareSystem element = model.AddSoftwareSystem("Name", "Description");
            element.Url = null;
        }

        [Fact]
        public void Test_SetUrl_DoesNotThrowAnException_WhenAnEmptyUrlIsSpecified()
        {
            SoftwareSystem element = model.AddSoftwareSystem("Name", "Description");
            element.Url = "";
        }

        [Fact]
        public void Test_SetUrl_ThrowsAnException_WhenAnInvalidUrlIsSpecified()
        {
            bool thrownException = false;

            try
            {
                SoftwareSystem element = model.AddSoftwareSystem("Name", "Description");
                element.Url = "www.somedomain.com";
            }
            catch (Exception e)
            {
                thrownException = true;
                e.Message.Should().Be("www.somedomain.com is not a valid URL.");
            }

            thrownException.Should().BeTrue();
        }

        [Fact]
        public void Test_SetUrl_DoesNotThrowAnException_WhenAValidUrlIsSpecified()
        {
            SoftwareSystem element = model.AddSoftwareSystem("Name", "Description");
            element.Url = "http://www.somedomain.com";

            element.Url.Should().Be("http://www.somedomain.com");
        }

    }
}
