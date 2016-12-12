using FluentAssertions;
using Structurizr.Analysis;
using System;
using Xunit;

namespace Structurizr.CoreTests.Analysis
{
    public class InterfaceImplementationTypeMatcherTests
    {
        [Fact]
        public void Test_Construction_DoesNotThrowAnExceptionWhenAnInterfaceTypeIsSupplied()
        {
            InterfaceImplementationTypeMatcher matcher = new InterfaceImplementationTypeMatcher(typeof(MyApp.IRepository), "", "");
        }

        [Fact]
        public void Test_Construction_ThrowsAnExceptionWhenANonInterfaceTypeIsSupplied()
        {
            Assert.Throws<ArgumentException>(() => new InterfaceImplementationTypeMatcher(typeof(MyApp.MyRepository), "", ""));
        }

        [Fact]
        public void Test_Matches_ReturnsFalse_WhenTheGivenTypeDoesNotImplementTheInterface()
        {
            InterfaceImplementationTypeMatcher matcher = new InterfaceImplementationTypeMatcher(typeof(MyApp.IRepository), "", "");
            matcher.Matches(typeof(MyApp.MyController)).Should().BeFalse();
        }

        [Fact]
        public void Test_Matches_ReturnsTrue_WhenTheGivenTypeDoesImplementTheInterface()
        {
            InterfaceImplementationTypeMatcher matcher = new InterfaceImplementationTypeMatcher(typeof(MyApp.IRepository), "", "");
            matcher.Matches(typeof(MyApp.MyRepository)).Should().BeTrue();
        }
    }
}