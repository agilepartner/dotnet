using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Structurizr.CoreTests
{

    [TestClass]
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

        [TestMethod]
        public void Test_Sonstruction()
        {
            Assert.AreEqual("The System - Web Application - Components", view.Name);
            Assert.AreEqual("Some description", view.Description);
            Assert.AreEqual(0, view.Elements.Count);
            Assert.AreSame(softwareSystem, view.SoftwareSystem);
            Assert.AreEqual(softwareSystem.Id, view.SoftwareSystemId);
            Assert.AreEqual(webApplication.Id, view.ContainerId);
            Assert.AreSame(model, view.Model);
        }

        [TestMethod]
        public void Test_AddAllSoftwareSystems_DoesNothing_WhenThereAreNoOtherSoftwareSystems()
        {
            Assert.AreEqual(0, view.Elements.Count);
            view.AddAllSoftwareSystems();
            Assert.AreEqual(0, view.Elements.Count);
        }

        [TestMethod]
        public void Test_AddAllSoftwareSystems_AddsAllSoftwareSystems_WhenThereAreSomeSoftwareSystemsInTheModel()
        {
            SoftwareSystem softwareSystemA = model.AddSoftwareSystem(Location.External, "System A", "Description");
            SoftwareSystem softwareSystemB = model.AddSoftwareSystem(Location.External, "System B", "Description");

            view.AddAllSoftwareSystems();

            Assert.AreEqual(2, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystemA)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystemB)));
        }

        [TestMethod]
        public void Test_AddAllPeople_DoesNothing_WhenThereAreNoPeople()
        {
            Assert.AreEqual(0, view.Elements.Count);
            view.AddAllPeople();
            Assert.AreEqual(0, view.Elements.Count);
        }

        [TestMethod]
        public void Test_AddAllPeople_AddsAllPeople_WhenThereAreSomePeopleInTheModel()
        {
            Person userA = model.AddPerson(Location.External, "User A", "Description");
            Person userB = model.AddPerson(Location.External, "User B", "Description");

            view.AddAllPeople();

            Assert.AreEqual(2, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(userA)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(userB)));
        }

        [TestMethod]
        public void Test_AddAllElements_DoesNothing_WhenThereAreNoSoftwareSystemsOrPeople()
        {
            Assert.AreEqual(0, view.Elements.Count);
            view.AddAllElements();
            Assert.AreEqual(0, view.Elements.Count);
        }

        [TestMethod]
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

            Assert.AreEqual(7, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystemA)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystemB)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(userA)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(userB)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(database)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(componentA)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(componentB)));
        }

        [TestMethod]
        public void Test_AddAllContainers_DoesNothing_WhenThereAreNoContainers()
        {
            Assert.AreEqual(0, view.Elements.Count);
            view.AddAllContainers();
            Assert.AreEqual(0, view.Elements.Count);
        }

        [TestMethod]
        public void Test_AddAllContainers_AddsAllContainers_WhenThereAreSomeContainers()
        {
            Container database = softwareSystem.AddContainer("Database", "Stores something", "MySQL");
            Container fileSystem = softwareSystem.AddContainer("File System", "Stores something else", "");

            view.AddAllContainers();

            Assert.AreEqual(2, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(database)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(fileSystem)));
        }

        [TestMethod]
        public void Test_AddAllComponents_DoesNothing_WhenThereAreNoComponents()
        {
            Assert.AreEqual(0, view.Elements.Count);
            view.AddAllComponents();
            Assert.AreEqual(0, view.Elements.Count);
        }

        [TestMethod]
        public void Test_AddAllComponents_AddsAllComponents_WhenThereAreSomeComponents()
        {
            Component componentA = webApplication.AddComponent("Component A", "Does something", "Java");
            Component componentB = webApplication.AddComponent("Component B", "Does something", "Java");

            view.AddAllComponents();

            Assert.AreEqual(2, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(componentA)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(componentB)));
        }

        [TestMethod]
        public void Test_Add_DoesNothing_WhenANullContainerIsSpecified()
        {
            Assert.AreEqual(0, view.Elements.Count);
            view.Add((Container)null);
            Assert.AreEqual(0, view.Elements.Count);
        }

        [TestMethod]
        public void Test_Add_AddsTheContainer_WhenTheContainerIsNoInTheViewAlready()
        {
            Container database = softwareSystem.AddContainer("Database", "Stores something", "MySQL");

            Assert.AreEqual(0, view.Elements.Count);
            view.Add(database);
            Assert.AreEqual(1, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(database)));
        }

        [TestMethod]
        public void Test_Add_DoesNothing_WhenTheSpecifiedContainerIsAlreadyInTheView()
        {
            Container database = softwareSystem.AddContainer("Database", "Stores something", "MySQL");
            view.Add(database);
            Assert.AreEqual(1, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(database)));

            view.Add(database);
            Assert.AreEqual(1, view.Elements.Count);
        }

        [TestMethod]
        public void Test_Remove_DoesNothing_WhenANullContainerIsPassed()
        {
            Assert.AreEqual(0, view.Elements.Count);
            view.Remove((Container)null);
            Assert.AreEqual(0, view.Elements.Count);
        }

        [TestMethod]
        public void Test_Remove_RemovesTheContainer_WhenTheContainerIsInTheView()
        {
            Container database = softwareSystem.AddContainer("Database", "Stores something", "MySQL");
            view.Add(database);
            Assert.AreEqual(1, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(database)));

            view.Remove(database);
            Assert.AreEqual(0, view.Elements.Count);
        }

        [TestMethod]
        public void Test_Remove_DoesNothing_WhenTheContainerIsNotInTheView()
        {
            Container database = softwareSystem.AddContainer("Database", "Stores something", "MySQL");
            Container fileSystem = softwareSystem.AddContainer("File System", "Stores something else", "");

            view.Add(database);
            Assert.AreEqual(1, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(database)));

            view.Remove(fileSystem);
            Assert.AreEqual(1, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(database)));
        }

        [TestMethod]
        public void Test_Add_DoesNothing_WhenANullComponentIsSpecified()
        {
            Assert.AreEqual(0, view.Elements.Count);
            view.Add((Component)null);
            Assert.AreEqual(0, view.Elements.Count);
        }

        [TestMethod]
        public void Test_Add_AddsTheComponent_WhenTheComponentIsNotInTheViewAlready()
        {
            Component componentA = webApplication.AddComponent("Component A", "Does something", "Java");

            Assert.AreEqual(0, view.Elements.Count);
            view.Add(componentA);
            Assert.AreEqual(1, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(componentA)));
        }

        [TestMethod]
        public void Test_Add_DoesNothing_WhenTheSpecifiedComponentIsAlreadyInTheView()
        {
            Component componentA = webApplication.AddComponent("Component A", "Does something", "Java");
            view.Add(componentA);
            Assert.AreEqual(1, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(componentA)));

            view.Add(componentA);
            Assert.AreEqual(1, view.Elements.Count);
        }

        [TestMethod]
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

            Assert.AreEqual(1, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(componentA1_1)));
        }

        [TestMethod]
        public void Test_Add_DoesNothing_WhenTheContainerOfTheViewIsAdded()
        {
            view.Add(webApplication);
            Assert.IsFalse(view.Elements.Contains(new ElementView(webApplication)));
        }

        [TestMethod]
        public void Test_Remove_DoesNothing_WhenANullComponentIsPassed()
        {
            Assert.AreEqual(0, view.Elements.Count);
            view.Remove((Component)null);
            Assert.AreEqual(0, view.Elements.Count);
        }

        [TestMethod]
        public void Test_Remove_RemovesTheComponent_WhenTheComponentIsInTheView()
        {
            Component componentA = webApplication.AddComponent("Component A", "Does something", "Java");
            view.Add(componentA);
            Assert.AreEqual(1, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(componentA)));

            view.Remove(componentA);
            Assert.AreEqual(0, view.Elements.Count);
        }

        [TestMethod]
        public void Test_Remove_RemovesTheComponentAndRelationships_WhenTheComponentIsInTheViewAndHasArelationshipToAnotherElement()
        {
            Component componentA = webApplication.AddComponent("Component A", "Does something", "Java");
            Component componentB = webApplication.AddComponent("Component B", "Does something", "Java");
            componentA.Uses(componentB, "uses");

            view.Add(componentA);
            view.Add(componentB);
            Assert.AreEqual(2, view.Elements.Count);
            Assert.AreEqual(1, view.Relationships.Count);

            view.Remove(componentB);
            Assert.AreEqual(1, view.Elements.Count);
            Assert.AreEqual(0, view.Relationships.Count);
        }

        [TestMethod]
        public void Test_Remove_DoesNothing_WhenTheComponentIsNotInTheView()
        {
            Component componentA = webApplication.AddComponent("Component A", "Does something", "Java");
            Component componentB = webApplication.AddComponent("Component B", "Does something", "Java");

            view.Add(componentA);
            Assert.AreEqual(1, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(componentA)));

            view.Remove(componentB);
            Assert.AreEqual(1, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(componentA)));
        }

        [TestMethod]
        public void Test_AddNearestNeightbours_DoesNothing_WhenANullElementIsSpecified()
        {
            view.AddNearestNeighbours(null);

            Assert.AreEqual(0, view.Elements.Count);
        }

        [TestMethod]
        public void Test_AddNearestNeighbours_DoesNothing_WhenThereAreNoNeighbours()
        {
            view.AddNearestNeighbours(softwareSystem);

            Assert.AreEqual(1, view.Elements.Count);
        }

        [TestMethod]
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

            Assert.AreEqual(3, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystemA)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystem)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystemB)));

            view = new ComponentView(webApplication, "components", "Description");
            view.AddNearestNeighbours(softwareSystemA);

            Assert.AreEqual(5, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(userA)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystemA)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystem)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(webApplication)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(controller)));

            view = new ComponentView(webApplication, "components", "Description");
            view.AddNearestNeighbours(webApplication);

            Assert.AreEqual(4, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystemA)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(webApplication)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(database)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystemB)));

            view = new ComponentView(webApplication, "components", "Description");
            view.AddNearestNeighbours(controller);

            Assert.AreEqual(3, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystemA)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(controller)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(service)));

            view = new ComponentView(webApplication, "components", "Description");
            view.AddNearestNeighbours(service);

            Assert.AreEqual(4, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(controller)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(service)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(repository)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystemB)));
        }

    }

}
