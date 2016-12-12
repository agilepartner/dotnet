using System;
using Xunit;
using FluentAssertions;

namespace Structurizr.CoreTests
{

    public class CodeElementTests
    {

        private readonly CodeElement _codeElement = new CodeElement("Wibble.Wobble, Foo.Bar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=xyz");

        [Fact]
        public void Test_Construction_WhenAFullyQualifiedTypeIsSpecified()
        {
            _codeElement.Name.Should().Be("Wobble");
        }

        [Fact]
        public void Test_SetUrl_DoesNotThrowAnException_WhenANullUrlIsSpecified()
        {
            _codeElement.Url = null;
        }

        [Fact]
        public void Test_SetUrl_DoesNotThrowAnException_WhenAnEmptyUrlIsSpecified()
        {
            _codeElement.Url = "";
        }

        [Fact]
        public void Test_SetUrl_ThrowsAnException_WhenAnInvalidUrlIsSpecified()
        {
            bool thrownException = false;

            try
            {
                _codeElement.Url = "www.somedomain.com";
            }
            catch (Exception e)
            {
                thrownException = true;
                e.Message.Should().Be("www.somedomain.com is not a valid URL.");
            }

            thrownException.Should().BeTrue();
        }

        [Fact]
        public void Test_SetUrl_DoesNotThrowAnException_WhenAValidUrlIsSpecified()
        {
            _codeElement.Url = "http://www.somedomain.com";
            _codeElement.Url.Should().Be("http://www.somedomain.com");
        }
    }
}