using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Structurizr.CoreTests
{

    [TestClass]
    public class SystemContextViewTests : AbstractTestBase
    {

        private SoftwareSystem softwareSystem;
        private SystemContextView view;

        public SystemContextViewTests()
        {
            softwareSystem = model.AddSoftwareSystem("The System", "Description");
            view = workspace.Views.CreateSystemContextView(softwareSystem, "context", "Description");
        }

        [TestMethod]
        public void Test_Construction()
        {
            Assert.AreEqual("The System - System Context", view.Name);
            Assert.AreEqual(1, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystem)));
            Assert.AreSame(softwareSystem, view.SoftwareSystem);
            Assert.AreEqual(softwareSystem.Id, view.SoftwareSystemId);
            Assert.AreSame(model, view.Model);
        }

        [TestMethod]
        public void test_AddAllSoftwareSystems_DoesNothing_WhenThereAreNoOtherSoftwareSystems()
        {
            Assert.AreEqual(1, view.Elements.Count);
            view.AddAllSoftwareSystems();
            Assert.AreEqual(1, view.Elements.Count);
        }

        [TestMethod]
        public void test_AddAllSoftwareSystems_AddsAllSoftwareSystems_WhenThereAreSomeSoftwareSystemsInTheModel()
        {
            SoftwareSystem softwareSystemA = model.AddSoftwareSystem(Location.External, "System A", "Description");
            SoftwareSystem softwareSystemB = model.AddSoftwareSystem(Location.External, "System B", "Description");

            view.AddAllSoftwareSystems();

            Assert.AreEqual(3, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystem)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystemA)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystemB)));
        }

        [TestMethod]
        public void test_AddAllPeople_DoesNothing_WhenThereAreNoPeople()
        {
            Assert.AreEqual(1, view.Elements.Count);
            view.AddAllPeople();
            Assert.AreEqual(1, view.Elements.Count);
        }

        [TestMethod]
        public void test_AddAllPeople_AddsAllPeople_WhenThereAreSomePeopleInTheModel()
        {
            Person userA = model.AddPerson(Location.External, "User A", "Description");
            Person userB = model.AddPerson(Location.External, "User B", "Description");

            view.AddAllPeople();

            Assert.AreEqual(3, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystem)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(userA)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(userB)));
        }

        [TestMethod]
        public void test_AddAllElements_DoesNothing_WhenThereAreNoSoftwareSystemsOrPeople()
        {
            Assert.AreEqual(1, view.Elements.Count);
            view.AddAllElements();
            Assert.AreEqual(1, view.Elements.Count);
        }

        [TestMethod]
        public void test_AddAllElements_AddsAllSoftwareSystemsAndPeople_WhenThereAreSomeSoftwareSystemsAndPeopleInTheModel()
        {
            SoftwareSystem softwareSystemA = model.AddSoftwareSystem(Location.External, "System A", "Description");
            SoftwareSystem softwareSystemB = model.AddSoftwareSystem(Location.External, "System B", "Description");
            Person userA = model.AddPerson(Location.External, "User A", "Description");
            Person userB = model.AddPerson(Location.External, "User B", "Description");

            view.AddAllElements();

            Assert.AreEqual(5, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystem)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystemA)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystemB)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(userA)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(userB)));
        }

        [TestMethod]
        public void Test_AddNearestNeightbours_DoesNothing_WhenANullElementIsSpecified()
        {
            view.AddNearestNeighbours(null);

            Assert.AreEqual(1, view.Elements.Count);
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

            Assert.AreEqual(3, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystemA)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystem)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystemB)));

            view = new SystemContextView(softwareSystem, "context", "Description");
            view.AddNearestNeighbours(softwareSystemA);

            Assert.AreEqual(3, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(userA)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystemA)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystem)));
        }

    }

}