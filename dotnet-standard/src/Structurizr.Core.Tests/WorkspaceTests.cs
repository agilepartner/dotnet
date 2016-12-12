using FluentAssertions;
using System;
using Xunit;

namespace Structurizr.CoreTests
{

    public class WorkspaceTests
    {

        private Workspace workspace = new Workspace("Name", "Description");

        [Fact]
        public void Test_SetSource_DoesNotThrowAnException_WhenANullUrlIsSpecified()
        {
            workspace.Source = null;
        }

        [Fact]
        public void Test_SetSource_DoesNotThrowAnException_WhenAnEmptyUrlIsSpecified()
        {
            workspace.Source = "";
        }

        [Fact]
        public void Test_SetSource_ThrowsAnException_WhenAnInvalidUrlIsSpecified()
        {
            bool thrownException = false;

            try
            {
                workspace.Source = "www.somedomain.com";
            }
            catch (Exception e)
            {
                thrownException = true;
                e.Message.Should().Be("www.somedomain.com is not a valid URL.");
            }

            thrownException.Should().BeTrue();
        }

        [Fact]
        public void Test_SetSource_DoesNotThrowAnException_WhenAValidUrlIsSpecified()
        {
            workspace.Source = "http://www.somedomain.com";
            workspace.Source.Should().Be("http://www.somedomain.com");
        }

        [Fact]
        public void Test_SetApi_DoesNotThrowAnException_WhenANullUrlIsSpecified()
        {
            workspace.Api = null;
        }

        [Fact]
        public void Test_SetApi_DoesNotThrowAnException_WhenAnEmptyUrlIsSpecified()
        {
            workspace.Api = "";
        }

        [Fact]
        public void Test_SetApi_ThrowsAnException_WhenAnInvalidUrlIsSpecified()
        {
            bool thrownException = false;

            try
            {
                workspace.Api = "www.somedomain.com";
            }
            catch (Exception e)
            {
                thrownException = true;
                e.Message.Should().Be("www.somedomain.com is not a valid URL.");
            }
            thrownException.Should().BeTrue();
        }

        [Fact]
        public void Test_SetApi_DoesNotThrowAnException_WhenAValidUrlIsSpecified()
        {
            workspace.Api = "http://www.somedomain.com";
            workspace.Api.Should().Be("http://www.somedomain.com");
        }
    }
}