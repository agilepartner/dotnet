using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Structurizr.CoreTests.View
{
    [TestClass]
    public class RelationshipStyleTests
    {

        [TestMethod]
        public void Test_Position()
        {
            RelationshipStyle style = new RelationshipStyle();
            Assert.IsNull(style.Position);

            style.Position = -1;
            Assert.AreEqual(0, style.Position);

            style.Position = 0;
            Assert.AreEqual(0, style.Position);

            style.Position = 50;
            Assert.AreEqual(50, style.Position);

            style.Position = 100;
            Assert.AreEqual(100, style.Position);

            style.Position = 101;
            Assert.AreEqual(100, style.Position);
        }

        [TestMethod]
        public void Test_Opacity()
        {
            RelationshipStyle style = new RelationshipStyle();
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
