namespace ExpressionCompiler.Tests
{
  public class ScannerTests
  {
    [Fact]
    public void PeekTests()
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
    public void ReadTests()
    {
      var scanner = new Scanner("2+1");
      Assert.Equal('2', scanner.Read());
      Assert.Equal(0, scanner.Position);
      Assert.Equal('+', scanner.Read());
      Assert.Equal(1, scanner.Position);
      Assert.Equal('1', scanner.Read());
      Assert.Equal(2, scanner.Position);
    }
  }
}