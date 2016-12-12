using FluentAssertions;
using Xunit;

namespace Structurizr.CoreTests
{
    public class SystemContextViewTests : AbstractTestBase
    {

        private SoftwareSystem softwareSystem;
        private SystemContextView view;

        public SystemContextViewTests()
        {
            softwareSystem = model.AddSoftwareSystem("The System", "Description");
            view = workspace.Views.CreateSystemContextView(softwareSystem, "context", "Description");
        }

        [Fact]
        public void Test_Construction()
        {
            view.Name.Should().Be("The System - System Context");
            view.Elements.Should().HaveCount(1);
            view.Elements.Should().Contain(new ElementView(softwareSystem));
            view.SoftwareSystem.Should().Be(softwareSystem);
            view.SoftwareSystemId.Should().Be(softwareSystem.Id);
            view.Model.Should().Be(model);
        }

        [Fact]
        public void test_AddAllSoftwareSystems_DoesNothing_WhenThereAreNoOtherSoftwareSystems()
        {
            view.Elements.Should().HaveCount(1);
            view.AddAllSoftwareSystems();
            view.Elements.Should().HaveCount(1);
        }

        [Fact]
        public void test_AddAllSoftwareSystems_AddsAllSoftwareSystems_WhenThereAreSomeSoftwareSystemsInTheModel()
        {
            SoftwareSystem softwareSystemA = model.AddSoftwareSystem(Location.External, "System A", "Description");
            SoftwareSystem softwareSystemB = model.AddSoftwareSystem(Location.External, "System B", "Description");

            view.AddAllSoftwareSystems();

            view.Elements.Should().HaveCount(3);
            view.Elements.Should().Contain(new ElementView(softwareSystem));
            view.Elements.Should().Contain(new ElementView(softwareSystemA));
            view.Elements.Should().Contain(new ElementView(softwareSystemB));
        }

        [Fact]
        public void test_AddAllPeople_DoesNothing_WhenThereAreNoPeople()
        {
            view.Elements.Should().HaveCount(1);
            view.AddAllPeople();
            view.Elements.Should().HaveCount(1);
        }

        [Fact]
        public void test_AddAllPeople_AddsAllPeople_WhenThereAreSomePeopleInTheModel()
        {
            Person userA = model.AddPerson(Location.External, "User A", "Description");
            Person userB = model.AddPerson(Location.External, "User B", "Description");

            view.AddAllPeople();

            view.Elements.Should().HaveCount(3);
            view.Elements.Should().Contain(new ElementView(softwareSystem));
            view.Elements.Should().Contain(new ElementView(userA));
            view.Elements.Should().Contain(new ElementView(userB));
        }

        [Fact]
        public void test_AddAllElements_DoesNothing_WhenThereAreNoSoftwareSystemsOrPeople()
        {
            view.Elements.Should().HaveCount(1);
            view.AddAllElements();
            view.Elements.Should().HaveCount(1);
        }

        [Fact]
        public void test_AddAllElements_AddsAllSoftwareSystemsAndPeople_WhenThereAreSomeSoftwareSystemsAndPeopleInTheModel()
        {
            SoftwareSystem softwareSystemA = model.AddSoftwareSystem(Location.External, "System A", "Description");
            SoftwareSystem softwareSystemB = model.AddSoftwareSystem(Location.External, "System B", "Description");
            Person userA = model.AddPerson(Location.External, "User A", "Description");
            Person userB = model.AddPerson(Location.External, "User B", "Description");

            view.AddAllElements();

            view.Elements.Should().HaveCount(5);
            view.Elements.Should().Contain(new ElementView(softwareSystem));
            view.Elements.Should().Contain(new ElementView(softwareSystemA));
            view.Elements.Should().Contain(new ElementView(softwareSystemB));
            view.Elements.Should().Contain(new ElementView(userA));
            view.Elements.Should().Contain(new ElementView(userB));
        }

        [Fact]
        public void Test_AddNearestNeightbours_DoesNothing_WhenANullElementIsSpecified()
        {
            view.AddNearestNeighbours(null);
            view.Elements.Should().HaveCount(1);
        }

        [Fact]
        public void Test_AddNearestNeighbours_DoesNothing_WhenThereAreNoNeighbours()
        {
            view.AddNearestNeighbours(softwareSystem);
            view.Elements.Should().HaveCount(1);
        }

        [Fact]
        public void Test_AddNearestNeighbours_AddsNearestNeighbours_WhenThereAreSomeNearestNeighbours()
        {
            SoftwareSystem softwareSystemA = model.AddSoftwareSystem("System A", "Description");
            SoftwareSystem softwareSystemB = model.AddSoftwareSystem("System B", "Description");
            Person userA = model.AddPerson("User A", "Description");
            Person userB = model.AddPerson("User B", "Description");

            // userA -> systemA -> system -> systemB -> userB
            userA.Uses(softwareSystemA, "");
            softwareSystemA.Uses(softwareSystem, "");
            softwareSystem.Uses(softwareSystemB, "");
            softwareSystemB.Delivers(userB, "");

            // userA -> systemA -> web application -> systemB -> userB
            // web application -> database
            Container webApplication = softwareSystem.AddContainer("Web Application", "", "");
            Container database = softwareSystem.AddContainer("Database", "", "");
            softwareSystemA.Uses(webApplication, "");
            webApplication.Uses(softwareSystemB, "");
            webApplication.Uses(database, "");

            // userA -> systemA -> controller -> service -> repository -> database
            Component controller = webApplication.AddComponent("Controller", "");
            Component service = webApplication.AddComponent("Service", "");
            Component repository = webApplication.AddComponent("Repository", "");
            softwareSystemA.Uses(controller, "");
            controller.Uses(service, "");
            service.Uses(repository, "");
            repository.Uses(database, "");

            // userA -> systemA -> controller -> service -> systemB -> userB
            service.Uses(softwareSystemB, "");

            view.AddNearestNeighbours(softwareSystem);

            view.Elements.Should().HaveCount(3);
            view.Elements.Should().Contain(new ElementView(softwareSystemA));
            view.Elements.Should().Contain(new ElementView(softwareSystem));
            view.Elements.Should().Contain(new ElementView(softwareSystemB));

            view = new SystemContextView(softwareSystem, "context", "Description");
            view.AddNearestNeighbours(softwareSystemA);

            view.Elements.Should().HaveCount(3);
            view.Elements.Should().Contain(new ElementView(userA));
            view.Elements.Should().Contain(new ElementView(softwareSystemA));
            view.Elements.Should().Contain(new ElementView(softwareSystem));
        }
    }
}