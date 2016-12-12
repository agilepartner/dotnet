using FluentAssertions;
using Xunit;

namespace Structurizr.CoreTests.View
{
    public class RelationshipStyleTests
    {
        [Fact]
        public void Test_Position()
        {
            RelationshipStyle style = new RelationshipStyle();
            style.Position.Should().BeNull();

            style.Position = -1;
            style.Position.Should().Be(0);

            style.Position = 0;
            style.Position.Should().Be(0);

            style.Position = 50;
            style.Position.Should().Be(50);

            style.Position = 100;
            style.Position.Should().Be(100);

            style.Position = 101;
            style.Position.Should().Be(100);
        }

        [Fact]
        public void Test_Opacity()
        {
            RelationshipStyle style = new RelationshipStyle();
            style.Opacity.Should().BeNull();

            style.Opacity = -1;
            style.Opacity.Should().Be(0);

            style.Opacity = 0;
            style.Opacity.Should().Be(0);

            style.Opacity = 50;
            style.Opacity.Should().Be(50);

            style.Opacity = 100;
            style.Opacity.Should().Be(100);

            style.Opacity = 101;
            style.Opacity.Should().Be(100);
        }
    }
}
