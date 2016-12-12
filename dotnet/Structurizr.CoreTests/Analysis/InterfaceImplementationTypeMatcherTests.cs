using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Structurizr.Analysis;

namespace Structurizr.CoreTests.Analysis
{
    [TestClass]
    public class InterfaceImplementationTypeMatcherTests
    {

        [TestMethod]
        public void Test_Construction_DoesNotThrowAnExceptionWhenAnInterfaceTypeIsSupplied()
        {
            InterfaceImplementationTypeMatcher matcher = new InterfaceImplementationTypeMatcher(typeof(MyApp.IRepository), "", "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Construction_ThrowsAnExceptionWhenANonInterfaceTypeIsSupplied()
        {
            InterfaceImplementationTypeMatcher matcher = new InterfaceImplementationTypeMatcher(typeof(MyApp.MyRepository), "", "");
        }

        [TestMethod]
        public void Test_Matches_ReturnsFalse_WhenTheGivenTypeDoesNotImplementTheInterface()
        {
            InterfaceImplementationTypeMatcher matcher = new InterfaceImplementationTypeMatcher(typeof(MyApp.IRepository), "", "");
            Assert.IsFalse(matcher.Matches(typeof(MyApp.MyController)));
        }

        [TestMethod]
        public void Test_Matches_ReturnsTrue_WhenTheGivenTypeDoesImplementTheInterface()
        {
            InterfaceImplementationTypeMatcher matcher = new InterfaceImplementationTypeMatcher(typeof(MyApp.IRepository), "", "");
            Assert.IsTrue(matcher.Matches(typeof(MyApp.MyRepository)));
        }

    }

}
