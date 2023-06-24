namespace ExpressionCompiler.Tests
{
  using FluentAssertions;
  using Xunit;

  public class ScannerTests
  {
    [Fact]
    public void When_Peek_PositionShoultNotChange()
    {
      var scanner = new Scanner("2+1");
      Assert.Equal('2', scanner.Read());
      Assert.Equal(0, scanner.Position);
      Assert.Equal('+', scanner.Read());
      Assert.Equal(1, scanner.Position);
      Assert.Equal('1', scanner.Peek());
      Assert.Equal(1, scanner.Position);
      Assert.Equal('1', scanner.Read());
      Assert.Equal(2, scanner.Position);
    }
    
    [Fact]
    public void When_Read_PositionShouldChange()
    {
      var scanner = new Scanner("2+1");
      scanner.Read().Should().Be('2');
      scanner.Position.Should().Be(0);
      scanner.Read().Should().Be('+');
      scanner.Position.Should().Be(1);
      scanner.Read().Should().Be('1');
      scanner.Position.Should().Be(2);
    }
  }
}