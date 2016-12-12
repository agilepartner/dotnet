using FluentAssertions;
using Xunit;

namespace Structurizr.CoreTests
{
    public class SequenceNumberTests
    {
        [Fact]
        public void Test_Increment()
        {
            SequenceNumber sequenceNumber = new SequenceNumber();

            sequenceNumber.GetNext().Should().Be("1");
            sequenceNumber.GetNext().Should().Be("2");
        }

        [Fact]
        public void Test_ChildSequence()
        {
            SequenceNumber sequenceNumber = new SequenceNumber();
            sequenceNumber.GetNext().Should().Be("1");

            sequenceNumber.StartChildSequence();
            sequenceNumber.GetNext().Should().Be("1.1");
            sequenceNumber.GetNext().Should().Be("1.2");

            sequenceNumber.EndChildSequence();
            sequenceNumber.GetNext().Should().Be("2");
        }

        [Fact]
        public void Test_ParallelSequences()
        {
            SequenceNumber sequenceNumber = new SequenceNumber();
            sequenceNumber.GetNext().Should().Be("1");

            sequenceNumber.StartParallelSequence();
            sequenceNumber.GetNext().Should().Be("2");
            sequenceNumber.EndParallelSequence();

            sequenceNumber.StartParallelSequence();
            sequenceNumber.GetNext().Should().Be("2");

            sequenceNumber.EndParallelSequence();

            sequenceNumber.GetNext().Should().Be("2");
        }
    }
}
