using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Structurizr.CoreTests
{

    [TestClass]
    public class EnterpriseContextViewTests : AbstractTestBase
    {

        private EnterpriseContextView view;

        public EnterpriseContextViewTests()
        {
            view = workspace.Views.CreateEnterpriseContextView("context", "Description");
        }

        [TestMethod]
        public void Test_Construction()
        {
            Assert.AreEqual("Enterprise Context", view.Name);
            Assert.AreEqual(0, view.Elements.Count);
            Assert.AreSame(model, view.Model);
        }

        [TestMethod]
        public void Test_GetName_WhenNoEnterpriseIsSpecified()
        {
            Assert.AreEqual("Enterprise Context", view.Name);
        }

        [TestMethod]
        public void Test_GetName_WhenAnEnterpriseIsSpecified()
        {
            model.Enterprise = new Enterprise("Widgets Limited");
            Assert.AreEqual("Enterprise Context for Widgets Limited", view.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_GetName_WhenAnEmptyEnterpriseNameIsSpecified()
        {
            model.Enterprise = new Enterprise("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_GetName_WhenANullEnterpriseNameIsSpecified()
        {
            model.Enterprise = new Enterprise(null);
        }

        [TestMethod]
        public void Test_AddAllSoftwareSystems_DoesNothing_WhenThereAreNoOtherSoftwareSystems()
        {
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
            view.AddAllPeople();
            Assert.AreEqual(0, view.Elements.Count);
        }

        [TestMethod]
        public void Test_AddAllPeople_AddsAllPeople_WhenThereAreSomePeopleInTheModel()
        {
            Person userA = model.AddPerson("User A", "Description");
            Person userB = model.AddPerson("User B", "Description");

            view.AddAllPeople();

            Assert.AreEqual(2, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(userA)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(userB)));
        }

        [TestMethod]
        public void Test_AddAllElements_DoesNothing_WhenThereAreNoSoftwareSystemsOrPeople()
        {
            view.AddAllElements();
            Assert.AreEqual(0, view.Elements.Count);
        }

        [TestMethod]
        public void Test_AddAllElements_AddsAllSoftwareSystemsAndPeople_WhenThereAreSomeSoftwareSystemsAndPeopleInTheModel()
        {
            SoftwareSystem softwareSystem = model.AddSoftwareSystem("Software System", "Description");
            Person person = model.AddPerson("Person", "Description");

            view.AddAllElements();

            Assert.AreEqual(2, view.Elements.Count);
            Assert.IsTrue(view.Elements.Contains(new ElementView(softwareSystem)));
            Assert.IsTrue(view.Elements.Contains(new ElementView(person)));
        }

    }

}