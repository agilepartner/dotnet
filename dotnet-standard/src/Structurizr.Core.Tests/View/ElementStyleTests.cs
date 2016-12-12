using FluentAssertions;
using Xunit;

namespace Structurizr.CoreTests
{
    public class ElementStyleTests
    {
        [Fact]
        public void Test_Opacity()
        {
            ElementStyle style = new ElementStyle();
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