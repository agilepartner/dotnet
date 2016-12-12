using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Structurizr.CoreTests
{
    [TestClass]
    public class ElementStyleTests
    {

        [TestMethod]
        public void Test_Opacity()
        {
            ElementStyle style = new ElementStyle();
            Assert.IsNull(style.Opacity);

            style.Opacity = -1;
            Assert.AreEqual(0, style.Opacity);

            style.Opacity = 0;
            Assert.AreEqual(0, style.Opacity);

            style.Opacity = 50;
            Assert.AreEqual(50, style.Opacity);

            style.Opacity = 100;
            Assert.AreEqual(100, style.Opacity);

            style.Opacity = 101;
            Assert.AreEqual(100, style.Opacity);
        }

    }
}
