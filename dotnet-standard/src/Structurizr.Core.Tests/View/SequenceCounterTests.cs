using FluentAssertions;
using Xunit;

namespace Structurizr.CoreTests
{
    public class SequenceCounterTests
    {
        [Fact]
        public void Test_increment_IncrementsTheCounter_WhenThereIsNoParent()
        {
            SequenceCounter counter = new SequenceCounter();
            counter.AsString().Should().Be("0");

            counter.Increment();
            counter.AsString().Should().Be("1");

            counter.Increment();
            counter.AsString().Should().Be("2");
        }

        [Fact]
        public void Test_counter_WhenThereIsOneParent()
        {
            SequenceCounter parent = new SequenceCounter();
            parent.Increment();
            parent.AsString().Should().Be("1");

            SequenceCounter child = new SequenceCounter(parent);
            child.Increment();
            child.AsString().Should().Be("1.1");

            child.Increment();
            child.AsString().Should().Be("1.2");
        }

        [Fact]
        public void Test_counter_WhenThereAreTwoParents()
        {
            SequenceCounter parent = new SequenceCounter();
            parent.Increment();
            parent.AsString().Should().Be("1");

            SequenceCounter child = new SequenceCounter(parent);
            child.Increment();
            child.AsString().Should().Be("1.1");

            SequenceCounter grandchild = new SequenceCounter(child);
            grandchild.Increment();
            grandchild.AsString().Should().Be("1.1.1");

            grandchild.Increment();
            grandchild.AsString().Should().Be("1.1.2");
        }

    }
}
