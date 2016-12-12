using FluentAssertions;
using Xunit;

namespace Structurizr.CoreTests
{

    public class ContainerViewTests : AbstractTestBase
    {

        private SoftwareSystem softwareSystem;
        private ContainerView view;

        public ContainerViewTests()
        {
            softwareSystem = model.AddSoftwareSystem("The System", "Description");
            view = workspace.Views.CreateContainerView(softwareSystem, "containers", "Description");
        }

        [Fact]
        public void Test_Construction()
        {
            view.Name.Should().Be("The System - Containers");
            view.Description.Should().Be("Description");
            view.Elements.Should().HaveCount(0);
            view.SoftwareSystem.Should().Be(softwareSystem);
            view.SoftwareSystemId.Should().Be(softwareSystem.Id);
            view.Model.Should().Be(model);
        }

        [Fact]
        public void Test_AddAllSoftwareSystems_DoesNothing_WhenThereAreNoOtherSoftwareSystems()
        {
            view.Elements.Should().HaveCount(0);
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
            view.Elements.Should().HaveCount(0);
            view.AddAllPeople();
            view.Elements.Should().HaveCount(0);
        }

        [Fact]
        public void Test_AddAllPeople_AddsAllPeople_WhenThereAreSomePeopleInTheModel()
        {
            Person userA = model.AddPerson(Location.External, "User A", "Description");
            Person userB = model.AddPerson(Location.External, "User B", "Description");

            view.AddAllPeople();

            view.Elements.Should().HaveCount(2);
            view.Elements.Should().Contain(new ElementView(userA));
            view.Elements.Should().Contain(new ElementView(userB));
        }

        [Fact]
        public void Test_AddAllElements_DoesNothing_WhenThereAreNoSoftwareSystemsOrPeople()
        {
            view.Elements.Should().HaveCount(0);
            view.AddAllElements();
            view.Elements.Should().HaveCount(0);
        }

        [Fact]
        public void Test_AddAllElements_AddsAllSoftwareSystemsAndPeopleAndContainers_WhenThereAreSomeSoftwareSystemsAndPeopleAndContainersInTheModel()
        {
            SoftwareSystem softwareSystemA = model.AddSoftwareSystem(Location.External, "System A", "Description");
            SoftwareSystem softwareSystemB = model.AddSoftwareSystem(Location.External, "System B", "Description");
            Person userA = model.AddPerson(Location.External, "User A", "Description");
            Person userB = model.AddPerson(Location.External, "User B", "Description");
            Container webApplication = softwareSystem.AddContainer("Web Application", "Does something", "Apache Tomcat");
            Container database = softwareSystem.AddContainer("Database", "Does something", "MySQL");

            view.AddAllElements();

            view.Elements.Should().HaveCount(6);
            view.Elements.Should().Contain(new ElementView(softwareSystemA));
            view.Elements.Should().Contain(new ElementView(softwareSystemB));
            view.Elements.Should().Contain(new ElementView(userA));
            view.Elements.Should().Contain(new ElementView(userB));
            view.Elements.Should().Contain(new ElementView(webApplication));
            view.Elements.Should().Contain(new ElementView(database));
        }

        [Fact]
        public void Test_AddAllContainers_DoesNothing_WhenThereAreNoContainers()
        {
            view.Elements.Should().HaveCount(0);
            view.AddAllContainers();
            view.Elements.Should().HaveCount(0);
        }

        [Fact]
        public void Test_AddAllContainers_AddsAllContainers_WhenThereAreSomeContainers()
        {
            Container webApplication = softwareSystem.AddContainer("Web Application", "Does something", "Apache Tomcat");
            Container database = softwareSystem.AddContainer("Database", "Does something", "MySQL");

            view.AddAllContainers();

            view.Elements.Should().HaveCount(2);
            view.Elements.Should().Contain(new ElementView(webApplication));
            view.Elements.Should().Contain(new ElementView(database));
        }

        [Fact]
        public void Test_AddNearestNeightbours_DoesNothing_WhenANullElementIsSpecified()
        {
            view.AddNearestNeighbours(null);

            view.Elements.Should().HaveCount(0);
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

            view = new ContainerView(softwareSystem, "containers", "Description");
            view.AddNearestNeighbours(softwareSystemA);

            view.Elements.Should().HaveCount(4);
            view.Elements.Should().Contain(new ElementView(userA));
            view.Elements.Should().Contain(new ElementView(softwareSystemA));
            view.Elements.Should().Contain(new ElementView(softwareSystem));
            view.Elements.Should().Contain(new ElementView(webApplication));

            view = new ContainerView(softwareSystem, "containers", "Description");
            view.AddNearestNeighbours(webApplication);

            view.Elements.Should().HaveCount(4);
            view.Elements.Should().Contain(new ElementView(softwareSystemA));
            view.Elements.Should().Contain(new ElementView(webApplication));
            view.Elements.Should().Contain(new ElementView(database));
            view.Elements.Should().Contain(new ElementView(softwareSystemB));
        }

        [Fact]
        public void Test_Remove_RemovesContainer()
        {
            Container webApplication = softwareSystem.AddContainer("Web Application", "", "");
            Container database = softwareSystem.AddContainer("Database", "", "");

            view.AddAllContainers();
            view.Elements.Should().HaveCount(2);

            view.Remove(webApplication);
            view.Elements.Should().HaveCount(1);
            view.Elements.Should().Contain(new ElementView(database));
        }
    }
}