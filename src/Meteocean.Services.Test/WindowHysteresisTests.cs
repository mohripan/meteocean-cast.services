namespace Meteocean.Services.Test
{
    public class WindowHysteresisTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(120)]
        [InlineData(-5)] // current design allows negatives; documents behavior
        public void Window_ToTimeSpan_ReturnsExpectedMinutes(int minutes)
        {
            var w = new Window(minutes);
            w.ToTimeSpan().TotalMinutes.Should().Be(minutes);
        }

        [Fact]
        public void Hysteresis_RecordEquality_Works()
        {
            var a = new Hysteresis(true, 60);
            var b = new Hysteresis(true, 60);
            a.Should().Be(b);

            var c = a with { ExitAfterMinutes = 90 };

            c.Should().NotBe(b);
            c.Enter.Should().BeTrue();
            c.ExitAfterMinutes.Should().Be(90);
        }
    }
}
