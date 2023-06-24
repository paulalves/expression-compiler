namespace ExpressionCompiler.Tests
{
  using System.Collections.Generic;
  using FluentAssertions;
  using Xunit;

  public class ExpressionInterpreterTests
  {
    [Theory]
    [InlineData("3/2*1-1*100", -98.5)]
    [InlineData("3/2*1-1*100+1", -97.5)]
    [InlineData("3/2*1-1*100+1*2", -96.5)]
    [InlineData("3/2*1-1*100+1*2/2", -97.5)]
    [InlineData("3/2*1-1*100+1*2/2+1", -96.5)]
    [InlineData("3/2*1-1*100+1*2/2+1*2", -95.5)]
    [InlineData("3/2*1-1*100+1*2/2+1*2/2", -96.5)]
    [InlineData("3/2*1-1*100+1*2/2+1*2/2+1", -95.5)]
    [InlineData("3/2*1-1*100+1*2/2+1*2/2+1*2", -94.5)]
    [InlineData("3/2*1-1*100+1*2/2+1*2/2+1*2/2", -95.5)]
    [InlineData("3/2*1-1*100+1*2/2+1*2/2+1*2/2+1", -94.5)]
    public static void InterpreterTests(string exp, decimal expected)
    {
      var parser = new ExpressionParser(new ExpressionLexer(new Scanner(exp)));
      var syntaxTree = parser.Parse();
      var interpreter = new ExpressionInterpreter();
      var result = syntaxTree.Accept(interpreter);
      result.Should().Be(expected);
    }
    
    [Theory]
    [InlineData("(2+(2-1)*1)", 3)]
    [InlineData("(2+(2-1)*1)+1", 4)]
    [InlineData("(2+(2-1)*1)+1*2", 5)]
    public void Tests(string source, decimal expected) {
      var parser = new ExpressionParser(new ExpressionLexer(new Scanner(source)));
      var syntaxTree = parser.Parse();
      var interpreter = new ExpressionInterpreter();
      var result = syntaxTree.Accept(interpreter);
      result.Should().Be(expected);
    }
  }
}