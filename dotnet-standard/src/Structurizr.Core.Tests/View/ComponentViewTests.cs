using FluentAssertions;
using Xunit;

namespace Structurizr.CoreTests
{

    public class ComponentViewTests : AbstractTestBase
    {

        private SoftwareSystem softwareSystem;
        private Container webApplication;
        private ComponentView view;

        public ComponentViewTests()
        {
            softwareSystem = model.AddSoftwareSystem(Location.Internal, "The System", "Description");
            webApplication = softwareSystem.AddContainer("Web Application", "Does something", "Apache Tomcat");
            view = new ComponentView(webApplication, "Key", "Some description");
        }

        [Fact]
        public void Test_Sonstruction()
        {
            view.Name.Should().Be("The System - Web Application - Components");
            view.Description.Should().Be("Some description");
            view.Elements.Should().HaveCount(0);
            view.SoftwareSystem.ShouldBeEquivalentTo(softwareSystem);
            view.SoftwareSystemId.Should().Be(softwareSystem.Id);
            view.ContainerId.Should().Be(webApplication.Id);
            view.Model.Should().Be(view.Model);
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
        public void Test_AddAllElements_AddsAllSoftwareSystemsAndPeopleAndContainersAndComponents_WhenThereAreSomeSoftwareSystemsAndPeopleAndContainersAndComponentsInTheModel()
        {
            SoftwareSystem softwareSystemA = model.AddSoftwareSystem(Location.External, "System A", "Description");
            SoftwareSystem softwareSystemB = model.AddSoftwareSystem(Location.External, "System B", "Description");
            Person userA = model.AddPerson(Location.External, "User A", "Description");
            Person userB = model.AddPerson(Location.External, "User B", "Description");
            Container database = softwareSystem.AddContainer("Database", "Does something", "MySQL");
            Component componentA = webApplication.AddComponent("Component A", "Does something", "Java");
            Component componentB = webApplication.AddComponent("Component B", "Does something", "Java");

            view.AddAllElements();

            view.Elements.Should().HaveCount(7);

            view.Elements.Should().Contain(new ElementView(softwareSystemA));
            view.Elements.Should().Contain(new ElementView(softwareSystemB));
            view.Elements.Should().Contain(new ElementView(userA));
            view.Elements.Should().Contain(new ElementView(userB));
            view.Elements.Should().Contain(new ElementView(database));
            view.Elements.Should().Contain(new ElementView(componentA));
            view.Elements.Should().Contain(new ElementView(componentB));
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
            Container database = softwareSystem.AddContainer("Database", "Stores something", "MySQL");
            Container fileSystem = softwareSystem.AddContainer("File System", "Stores something else", "");

            view.AddAllContainers();

            view.Elements.Should().HaveCount(2);
            view.Elements.Should().Contain(new ElementView(fileSystem));
            view.Elements.Should().Contain(new ElementView(database));
        }

        [Fact]
        public void Test_AddAllComponents_DoesNothing_WhenThereAreNoComponents()
        {
            view.Elements.Should().HaveCount(0);
            view.AddAllComponents();
            view.Elements.Should().HaveCount(0);
        }

        [Fact]
        public void Test_AddAllComponents_AddsAllComponents_WhenThereAreSomeComponents()
        {
            Component componentA = webApplication.AddComponent("Component A", "Does something", "Java");
            Component componentB = webApplication.AddComponent("Component B", "Does something", "Java");

            view.AddAllComponents();

            view.Elements.Should().HaveCount(2);
            view.Elements.Should().Contain(new ElementView(componentA));
            view.Elements.Should().Contain(new ElementView(componentB));
        }

        [Fact]
        public void Test_Add_DoesNothing_WhenANullContainerIsSpecified()
        {
            view.Elements.Should().HaveCount(0);
            view.Add((Container)null);
            view.Elements.Should().HaveCount(0);
        }

        [Fact]
        public void Test_Add_AddsTheContainer_WhenTheContainerIsNoInTheViewAlready()
        {
            Container database = softwareSystem.AddContainer("Database", "Stores something", "MySQL");

            view.Elements.Should().HaveCount(0);
            view.Add(database);
            view.Elements.Should().HaveCount(1);
            view.Elements.Should().Contain(new ElementView(database));
        }

        [Fact]
        public void Test_Add_DoesNothing_WhenTheSpecifiedContainerIsAlreadyInTheView()
        {
            Container database = softwareSystem.AddContainer("Database", "Stores something", "MySQL");
            view.Add(database);
            view.Elements.Should().HaveCount(1);
            view.Elements.Should().Contain(new ElementView(database));

            view.Add(database);
            view.Elements.Should().HaveCount(1);
        }

        [Fact]
        public void Test_Remove_DoesNothing_WhenANullContainerIsPassed()
        {
            view.Elements.Should().HaveCount(0);
            view.Remove((Container)null);
            view.Elements.Should().HaveCount(0);
        }

        [Fact]
        public void Test_Remove_RemovesTheContainer_WhenTheContainerIsInTheView()
        {
            Container database = softwareSystem.AddContainer("Database", "Stores something", "MySQL");
            view.Add(database);
            view.Elements.Should().HaveCount(1);
            view.Elements.Should().Contain(new ElementView(database));

            view.Remove(database);
            view.Elements.Should().HaveCount(0);
        }

        [Fact]
        public void Test_Remove_DoesNothing_WhenTheContainerIsNotInTheView()
        {
            Container database = softwareSystem.AddContainer("Database", "Stores something", "MySQL");
            Container fileSystem = softwareSystem.AddContainer("File System", "Stores something else", "");

            view.Add(database);
            view.Elements.Should().HaveCount(1);
            view.Elements.Should().Contain(new ElementView(database));

            view.Remove(fileSystem);
            view.Elements.Should().HaveCount(1);
            view.Elements.Should().Contain(new ElementView(database));
        }

        [Fact]
        public void Test_Add_DoesNothing_WhenANullComponentIsSpecified()
        {
            view.Elements.Should().HaveCount(0);
            view.Add((Component)null);
            view.Elements.Should().HaveCount(0);
        }

        [Fact]
        public void Test_Add_AddsTheComponent_WhenTheComponentIsNotInTheViewAlready()
        {
            Component componentA = webApplication.AddComponent("Component A", "Does something", "Java");

            view.Elements.Should().HaveCount(0);
            view.Add(componentA);
            view.Elements.Should().HaveCount(1);
            view.Elements.Should().Contain(new ElementView(componentA));
        }

        [Fact]
        public void Test_Add_DoesNothing_WhenTheSpecifiedComponentIsAlreadyInTheView()
        {
            Component componentA = webApplication.AddComponent("Component A", "Does something", "Java");
            view.Add(componentA);
            view.Elements.Should().HaveCount(1);
            view.Elements.Should().Contain(new ElementView(componentA));

            view.Add(componentA);
            view.Elements.Should().HaveCount(1);
        }

        [Fact]
        public void Test_Add_DoesNothing_WhenTheSpecifiedComponentIsInADifferentContainer()
        {
            SoftwareSystem softwareSystemA = model.AddSoftwareSystem("System A", "Description");

            Container containerA1 = softwareSystemA.AddContainer("Container A1", "Description", "Tec");
            Component componentA1_1 = containerA1.AddComponent("Component A1-1", "Description");

            Container containerA2 = softwareSystemA.AddContainer("Container A2", "Description", "Tec");
            Component componentA2_1 = containerA2.AddComponent("Component A2-1", "Description");

            view = new ComponentView(containerA1, "components", "Description");
            view.Add(componentA1_1);
            view.Add(componentA2_1);

            view.Elements.Should().HaveCount(1);
            view.Elements.Should().Contain(new ElementView(componentA1_1));
        }

        [Fact]
        public void Test_Add_DoesNothing_WhenTheContainerOfTheViewIsAdded()
        {
            view.Add(webApplication);
            view.Elements.Should().NotContain(new ElementView(webApplication));
        }

        [Fact]
        public void Test_Remove_DoesNothing_WhenANullComponentIsPassed()
        {
            view.Elements.Should().HaveCount(0);
            view.Remove((Component)null);
            view.Elements.Should().HaveCount(0);
        }

        [Fact]
        public void Test_Remove_RemovesTheComponent_WhenTheComponentIsInTheView()
        {
            Component componentA = webApplication.AddComponent("Component A", "Does something", "Java");
            view.Add(componentA);
            view.Elements.Should().HaveCount(1);
            view.Elements.Should().Contain(new ElementView(componentA));

            view.Remove(componentA);
            view.Elements.Should().HaveCount(0);
        }

        [Fact]
        public void Test_Remove_RemovesTheComponentAndRelationships_WhenTheComponentIsInTheViewAndHasArelationshipToAnotherElement()
        {
            Component componentA = webApplication.AddComponent("Component A", "Does something", "Java");
            Component componentB = webApplication.AddComponent("Component B", "Does something", "Java");
            componentA.Uses(componentB, "uses");

            view.Add(componentA);
            view.Add(componentB);
            view.Elements.Should().HaveCount(2);
            view.Relationships.Should().HaveCount(1);

            view.Remove(componentB);
            view.Elements.Should().HaveCount(1);
            view.Relationships.Should().HaveCount(0);
        }

        [Fact]
        public void Test_Remove_DoesNothing_WhenTheComponentIsNotInTheView()
        {
            Component componentA = webApplication.AddComponent("Component A", "Does something", "Java");
            Component componentB = webApplication.AddComponent("Component B", "Does something", "Java");

            view.Add(componentA);
            view.Elements.Should().HaveCount(1);
            view.Elements.Should().Contain(new ElementView(componentA));

            view.Remove(componentB);
            view.Elements.Should().HaveCount(1);
            view.Elements.Should().Contain(new ElementView(componentA));
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

            view = new ComponentView(webApplication, "components", "Description");
            view.AddNearestNeighbours(softwareSystemA);

            view.Elements.Should().HaveCount(5);
            view.Elements.Should().Contain(new ElementView(userA));
            view.Elements.Should().Contain(new ElementView(softwareSystemA));
            view.Elements.Should().Contain(new ElementView(softwareSystem));
            view.Elements.Should().Contain(new ElementView(webApplication));
            view.Elements.Should().Contain(new ElementView(controller));

            view = new ComponentView(webApplication, "components", "Description");
            view.AddNearestNeighbours(webApplication);

            view.Elements.Should().HaveCount(4);
            view.Elements.Should().Contain(new ElementView(softwareSystemA));
            view.Elements.Should().Contain(new ElementView(webApplication));
            view.Elements.Should().Contain(new ElementView(database));
            view.Elements.Should().Contain(new ElementView(softwareSystemB));

            view = new ComponentView(webApplication, "components", "Description");
            view.AddNearestNeighbours(controller);

            view.Elements.Should().HaveCount(3);
            view.Elements.Should().Contain(new ElementView(softwareSystemA));
            view.Elements.Should().Contain(new ElementView(controller));
            view.Elements.Should().Contain(new ElementView(service));

            view = new ComponentView(webApplication, "components", "Description");
            view.AddNearestNeighbours(service);

            view.Elements.Should().HaveCount(4);
            view.Elements.Should().Contain(new ElementView(controller));
            view.Elements.Should().Contain(new ElementView(service));
            view.Elements.Should().Contain(new ElementView(repository));
            view.Elements.Should().Contain(new ElementView(softwareSystemB));
        }

    }

}
