using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Structurizr.CoreTests
{
    public class DynamicViewTests : AbstractTestBase
    {

        private Person person;
        private SoftwareSystem softwareSystemA;
        private Container containerA1;
        private Container containerA2;
        private Container containerA3;
        private Component componentA1;
        private Component componentA2;

        private SoftwareSystem softwareSystemB;
        private Container containerB1;
        private Component componentB1;

        private Relationship relationship;

        public DynamicViewTests()
        {
            person = model.AddPerson("Person", "");
            softwareSystemA = model.AddSoftwareSystem("Software System A", "");
            containerA1 = softwareSystemA.AddContainer("Container A1", "", "");
            componentA1 = containerA1.AddComponent("Component A1", "");
            containerA2 = softwareSystemA.AddContainer("Container A2", "", "");
            componentA2 = containerA2.AddComponent("Component A2", "");
            containerA3 = softwareSystemA.AddContainer("Container A3", "", "");
            relationship = containerA1.Uses(containerA2, "uses");

            softwareSystemB = model.AddSoftwareSystem("Software System B", "");
            containerB1 = softwareSystemB.AddContainer("Container B1", "", "");
        }

        private void AssertFail(Action action, string expectedMessage)
        {
            bool thrownException = false;

            try
            {
                action();
            }
            catch (Exception e)
            {
                thrownException = true;
                e.Message.Should().Be(expectedMessage);
            }

            thrownException.Should().BeTrue();
        }

        [Fact]
        public void Test_Add_ThrowsAnException_WhenTheScopeOfTheDynamicViewIsASoftwareSystemButAContainerInAnotherSoftwareSystemIsAdded()
        {
            AssertFail(() =>
            {
                DynamicView dynamicView = workspace.Views.CreateDynamicView(softwareSystemA, "key", "Description");
                dynamicView.Add(containerB1, containerA1);
            }, "Only containers that reside inside Software System A can be added to this view.");
        }

        [Fact]
        public void Test_Add_ThrowsAnException_WhenTheScopeOfTheDynamicViewIsASoftwareSystemButAComponentIsAdded()
        {
            AssertFail(() =>
            {
                DynamicView dynamicView = workspace.Views.CreateDynamicView(softwareSystemA, "key", "Description");
                dynamicView.Add(componentA1, containerA1);
            }, "Components can't be added to a dynamic view when the scope is a software system.");
        }

        [Fact]
        public void Test_Add_ThrowsAnException_WhenTheScopeOfTheDynamicViewIsASoftwareSystemAndTheSameSoftwareSystemIsAdded()
        {
            AssertFail(() =>
            {
                DynamicView dynamicView = workspace.Views.CreateDynamicView(softwareSystemA, "key", "Description");
                dynamicView.Add(softwareSystemA, containerA1);
            }, "Software System A is already the scope of this view and cannot be added to it.");
        }

        [Fact]
        public void Test_Add_ThrowsAnException_WhenTheScopeOfTheDynamicViewIsAContainerAndTheSameContainerIsAdded()
        {
            AssertFail(() =>
            {
                DynamicView dynamicView = workspace.Views.CreateDynamicView(containerA1, "key", "Description");
                dynamicView.Add(containerA1, containerA2);
            }, "Container A1 is already the scope of this view and cannot be added to it.");
        }

        [Fact]
        public void Test_Add_ThrowsAnException_WhenTheScopeOfTheDynamicViewIsAContainerAndTheParentSoftwareSystemIsAdded()
        {
            AssertFail(() =>
            {
                DynamicView dynamicView = workspace.Views.CreateDynamicView(containerA1, "key", "Description");
                dynamicView.Add(softwareSystemA, containerA2);
            }, "Software System A is already the scope of this view and cannot be added to it.");
        }

        [Fact]
        public void Test_Add_ThrowsAnException_WhenTheScopeOfTheDynamicViewIsAContainerAndAContainerInAnotherSoftwareSystemIsAdded()
        {
            AssertFail(() =>
            {
                DynamicView dynamicView = workspace.Views.CreateDynamicView(containerA1, "key", "Description");
                dynamicView.Add(containerB1, containerA2);
            }, "Only containers that reside inside Software System A can be added to this view.");
        }

        [Fact]
        public void Test_Add_ThrowsAnException_WhenTheScopeOfTheDynamicViewIsAContainerAndAComponentInAnotherContainerIsAdded()
        {
            AssertFail(() =>
            {
                DynamicView dynamicView = workspace.Views.CreateDynamicView(containerA1, "key", "Description");
                dynamicView.Add(componentA2, containerA2);
            } , "Only components that reside inside Container A1 can be added to this view.");
        }

        [Fact]
        public void Test_Add_AddsTheSourceAndDestinationElements_WhenARelationshipBetweenThemExists()
        {
            DynamicView dynamicView = workspace.Views.CreateDynamicView(softwareSystemA, "key", "Description");
            dynamicView.Add(containerA1, containerA2);

            dynamicView.Elements.Should().HaveCount(2);
        }

        [Fact]
        public void Test_Add_ThrowsAnException_WhenThereIsNoRelationshipBetweenTheSourceAndDestinationElements()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                DynamicView dynamicView = workspace.Views.CreateDynamicView(softwareSystemA, "key", "Description");
                dynamicView.Add(containerA1, containerA3);
            });
        }

        [Fact]
        public void Test_AddRelationshipDirectly()
        {
            DynamicView dynamicView = workspace.Views.CreateDynamicView(softwareSystemA, "key", "Description");
            dynamicView.Add(relationship);

            dynamicView.Elements.Should().HaveCount(2);
        }

        [Fact]
        public void Test_Add_AddsTheSourceAndDestinationElements_WhenARelationshipBetweenThemExistsAndTheDestinationIsAnExternalSoftwareSystem()
        {
            DynamicView dynamicView = workspace.Views.CreateDynamicView(softwareSystemA, "key", "Description");
            containerA2.Uses(softwareSystemB, "", "");
            dynamicView.Add(containerA2, softwareSystemB);

            dynamicView.Elements.Should().HaveCount(2);
        }

        [Fact]
        public void Test_NormalSequence()
        {
            workspace = new Workspace("Name", "Description");
            model = workspace.Model;

            SoftwareSystem softwareSystem = model.AddSoftwareSystem("Software System", "Description");
            Container container1 = softwareSystem.AddContainer("Container 1", "Description", "Technology");
            Container container2 = softwareSystem.AddContainer("Container 2", "Description", "Technology");
            Container container3 = softwareSystem.AddContainer("Container 3", "Description", "Technology");

            container1.Uses(container2, "Uses");
            container1.Uses(container3, "Uses");

            DynamicView view = workspace.Views.CreateDynamicView(softwareSystem, "key", "Description");

            view.Add(container1, container2);
            view.Add(container1, container3);

            view.Relationships.First(rv => rv.Order.Equals("1")).Relationship.Destination.Should().Be(container2);
            view.Relationships.First(rv => rv.Order.Equals("2")).Relationship.Destination.Should().Be(container3);
        }

        [Fact]
        public void Test_ParallelSequence()
        {
            workspace = new Workspace("Name", "Description");
            model = workspace.Model;
            SoftwareSystem softwareSystem = model.AddSoftwareSystem("Name", "Description");
            Person user = model.AddPerson("User", "Description");
            Container microservice1 = softwareSystem.AddContainer("Microservice 1", "", "");
            Container database1 = softwareSystem.AddContainer("Database 1", "", "");
            Container microservice2 = softwareSystem.AddContainer("Microservice 2", "", "");
            Container database2 = softwareSystem.AddContainer("Database 2", "", "");
            Container microservice3 = softwareSystem.AddContainer("Microservice 3", "", "");
            Container database3 = softwareSystem.AddContainer("Database 3", "", "");
            Container messageBus = softwareSystem.AddContainer("Message Bus", "", "");

            user.Uses(microservice1, "Updates using");
            microservice1.Delivers(user, "Sends updates to");

            microservice1.Uses(database1, "Stores data in");
            microservice1.Uses(messageBus, "Sends messages to");
            microservice1.Uses(messageBus, "Sends messages to");

            messageBus.Uses(microservice2, "Sends messages to");
            messageBus.Uses(microservice3, "Sends messages to");

            microservice2.Uses(database2, "Stores data in");
            microservice3.Uses(database3, "Stores data in");

            DynamicView view = workspace.Views.CreateDynamicView(softwareSystem, "key", "Description");

            view.Add(user, "1", microservice1);
            view.Add(microservice1, "2", database1);
            view.Add(microservice1, "3", messageBus);

            view.StartParallelSequence();
            view.Add(messageBus, "4", microservice2);
            view.Add(microservice2, "5", database2);
            view.EndParallelSequence();

            view.StartParallelSequence();
            view.Add(messageBus, "4", microservice3);
            view.Add(microservice3, "5", database3);
            view.EndParallelSequence();

            view.Add(microservice1, "5", database1);

            view.Relationships.Count(r => r.Order.Equals("1")).Should().Be(1);
            view.Relationships.Count(r => r.Order.Equals("2")).Should().Be(1);
            view.Relationships.Count(r => r.Order.Equals("3")).Should().Be(1);
            view.Relationships.Count(r => r.Order.Equals("4")).Should().Be(3);
            view.Relationships.Count(r => r.Order.Equals("5")).Should().Be(2);
        }
    }
}