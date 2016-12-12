using FluentAssertions;
using Xunit;

namespace Structurizr.CoreTests
{
    public class ConfigurationTests : AbstractTestBase
    {
        [Fact]
        public void test_defaultView_DoesNothing_WhenPassedNull()
        {
            Configuration configuration = new Configuration();
            configuration.SetDefaultView(null);

            configuration.DefaultView.Should().BeNull();
        }

        [Fact]
        public void test_defaultView()
        {
            EnterpriseContextView view = views.CreateEnterpriseContextView("key", "Description");
            Configuration configuration = new Configuration();
            configuration.SetDefaultView(view);
            configuration.DefaultView.Should().Be("key");
        }

        [Fact]
        public void test_copyConfigurationFrom()
        {
            Configuration source = new Configuration();
            source.LastSavedView = "someKey";

            Configuration destination = new Configuration();
            destination.CopyConfigurationFrom(source);

            destination.LastSavedView.Should().Be("someKey");
        }
    }
}