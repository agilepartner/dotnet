using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Structurizr.Analysis;

namespace Structurizr.CoreTests.Analysis
{
    [TestClass]
    public class NameSuffixTypeMatcherTests
    {

        [TestMethod]
        public void Test_Construction_DoesNotThrowAnExceptionWhenASuffixIsSupplied()
        {
            NameSuffixTypeMatcher matcher = new NameSuffixTypeMatcher("Component", "", "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Construction_ThrowsAnExceptionWhenANullSuffixIsSupplied()
        {
            NameSuffixTypeMatcher matcher = new NameSuffixTypeMatcher(null, "", "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Construction_ThrowsAnExceptionWhenAnEmptyStringSuffixIsSupplied()
        {
            NameSuffixTypeMatcher matcher = new NameSuffixTypeMatcher(" ", "", "");
        }

        [TestMethod]
        public void Test_Matches_ReturnsFalse_WhenTheNameOfTheGivenTypeDoesNotHaveTheSuffix()
        {
            NameSuffixTypeMatcher matcher = new NameSuffixTypeMatcher("Component", "", "");
            Assert.IsFalse(matcher.Matches(typeof(MyApp.MyController)));
        }

        [TestMethod]
        public void Test_Matches_ReturnsTrue_WhenTheNameOfTheGivenTypeDoesHaveTheSuffix()
        {
            NameSuffixTypeMatcher matcher = new NameSuffixTypeMatcher("Controller", "", "");
            Assert.IsTrue(matcher.Matches(typeof(MyApp.MyController)));
        }

    }

}
