using FluentAssertions;
using Structurizr.Client;
using Xunit;

namespace Structurizr.CoreTests.Client
{
    public class Md5DigestTests
    {

        private Md5Digest md5;

        public Md5DigestTests()
        {
            md5 = new Md5Digest();
        }

        [Fact]
        public void Test_Generate_TreatsNullAsEmptyContent()
        {
            md5.Generate("").Should().BeEquivalentTo(md5.Generate(null));
        }

        [Fact]
        public void Test_Generate_WorksForUTF8CharacterEncoding()
        {
            md5.Generate("è").Should().Be("0a35e149dbbb2d10d744bf675c7744b1");
        }

        [Fact]
        public void Test_Generate()
        {
            md5.Generate("Hello World!").Should().Be("ed076287532e86365e841e92bfc50d8c");
            md5.Generate("").Should().Be("d41d8cd98f00b204e9800998ecf8427e");
        }
    }
}
