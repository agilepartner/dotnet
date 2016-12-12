using FluentAssertions;
using Structurizr.Analysis;
using System;
using Xunit;

namespace Structurizr.CoreTests.Analysis
{
    public class NameSuffixTypeMatcherTests
    {

        [Fact]
        public void Test_Construction_DoesNotThrowAnExceptionWhenASuffixIsSupplied()
        {
            NameSuffixTypeMatcher matcher = new NameSuffixTypeMatcher("Component", "", "");
        }

        [Fact]
        public void Test_Construction_ThrowsAnExceptionWhenANullSuffixIsSupplied()
        {
            Assert.Throws<ArgumentException>(() => new NameSuffixTypeMatcher(null, "", ""));
        }

        [Fact]
        public void Test_Construction_ThrowsAnExceptionWhenAnEmptyStringSuffixIsSupplied()
        {
            Assert.Throws<ArgumentException>(() => new NameSuffixTypeMatcher(" ", "", ""));
        }

        [Fact]
        public void Test_Matches_ReturnsFalse_WhenTheNameOfTheGivenTypeDoesNotHaveTheSuffix()
        {
            NameSuffixTypeMatcher matcher = new NameSuffixTypeMatcher("Component", "", "");
            matcher.Matches(typeof(MyApp.MyController)).Should().BeFalse();
        }

        [Fact]
        public void Test_Matches_ReturnsTrue_WhenTheNameOfTheGivenTypeDoesHaveTheSuffix()
        {
            NameSuffixTypeMatcher matcher = new NameSuffixTypeMatcher("Controller", "", "");
            matcher.Matches(typeof(MyApp.MyController)).Should().BeTrue();
        }
    }
}