using FluentAssertions;
using System;
using Xunit;

namespace Structurizr.CoreTests
{
    public class EnterpriseContextViewTests : AbstractTestBase
    {

        private EnterpriseContextView view;

        public EnterpriseContextViewTests()
        {
            view = workspace.Views.CreateEnterpriseContextView("context", "Description");
        }

        [Fact]
        public void Test_Construction()
        {
            view.Name.Should().Be("Enterprise Context");
            view.Elements.Should().HaveCount(0);
            view.Model.Should().Be(model);
        }

        [Fact]
        public void Test_GetName_WhenNoEnterpriseIsSpecified()
        {
            view.Name.Should().Be("Enterprise Context");
        }

        [Fact]
        public void Test_GetName_WhenAnEnterpriseIsSpecified()
        {
            model.Enterprise = new Enterprise("Widgets Limited");
            view.Name.Should().Be("Enterprise Context for Widgets Limited");
        }

        [Fact]
        public void Test_GetName_WhenAnEmptyEnterpriseNameIsSpecified()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                model.Enterprise = new Enterprise("");
            });
        }

        [Fact]
        public void Test_GetName_WhenANullEnterpriseNameIsSpecified()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                model.Enterprise = new Enterprise(null);
            });
        }

        [Fact]
        public void Test_AddAllSoftwareSystems_DoesNothing_WhenThereAreNoOtherSoftwareSystems()
        {
            view.AddAllSoftwareSystems();
            view.Elements.Should().HaveCount(0);
        }

        [Fact]
        public void Test_AddAllSoftwareSystems_AddsAllSoftwareSystems_WhenThereAreSomeSoftwareSystemsInTheModel()
        {
            SoftwareSystem softwareSystemA = model.AddSoftwareSystem(Location.External, "System A", "Description");
            SoftwareSystem softwareSystemB = model.AddSoftwareSystem(Location.External, "System B", "Description");

            view.AddAllSoftwareSystems();

            view.Elements.Should().HaveCount(2);
            view.Elements.Should().Contain(new ElementView(softwareSystemA));
            view.Elements.Should().Contain(new ElementView(softwareSystemB));
        }

        [Fact]
        public void Test_AddAllPeople_DoesNothing_WhenThereAreNoPeople()
        {
            view.AddAllPeople();
            view.Elements.Should().HaveCount(0);
        }

        [Fact]
        public void Test_AddAllPeople_AddsAllPeople_WhenThereAreSomePeopleInTheModel()
        {
            Person userA = model.AddPerson("User A", "Description");
            Person userB = model.AddPerson("User B", "Description");

            view.AddAllPeople();

            view.Elements.Should().HaveCount(2);
            view.Elements.Should().Contain(new ElementView(userA));
            view.Elements.Should().Contain(new ElementView(userB));
        }

        [Fact]
        public void Test_AddAllElements_DoesNothing_WhenThereAreNoSoftwareSystemsOrPeople()
        {
            view.AddAllElements();
            view.Elements.Should().HaveCount(0);
        }

        [Fact]
        public void Test_AddAllElements_AddsAllSoftwareSystemsAndPeople_WhenThereAreSomeSoftwareSystemsAndPeopleInTheModel()
        {
            SoftwareSystem softwareSystem = model.AddSoftwareSystem("Software System", "Description");
            Person person = model.AddPerson("Person", "Description");

            view.AddAllElements();

            view.Elements.Should().HaveCount(2);
            view.Elements.Should().Contain(new ElementView(softwareSystem));
            view.Elements.Should().Contain(new ElementView(person));
        }
    }
}