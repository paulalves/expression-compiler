namespace ExpressionCompiler.Tests
{
  using System.Collections.Generic;
  using FluentAssertions;
  using Xunit;

  public class ExpressionInterpreterTests
  {
    [Theory]
    [MemberData(nameof(GetMemberData))]
    public static void InterpreterTests(string exp, decimal expected)
    {
      var parser = new ExpressionParser(new ExpressionLexer(new Scanner(exp)));
      var syntaxTree = parser.Parse();
      var interpreter = new ExpressionInterpreter();
      var result = syntaxTree.Accept(interpreter);
      result.Should().Be(expected);
    }

    public static IEnumerable<object[]> GetMemberData()
    {
      yield return new object[ ] { "3/2*1-1*100", -98.5M };
      yield return new object[ ] { "3/2*1-1*100+1", -97.5M };
      yield return new object[ ] { "3/2*1-1*100+1*2", -96.5M };
      yield return new object[ ] { "3/2*1-1*100+1*2/2", -97.5M };
      yield return new object[ ] { "3/2*1-1*100+1*2/2+1", -96.5M };
      yield return new object[ ] { "3/2*1-1*100+1*2/2+1*2", -95.5M };
      yield return new object[ ] { "3/2*1-1*100+1*2/2+1*2/2", -96.5M };
      yield return new object[ ] { "3/2*1-1*100+1*2/2+1*2/2+1", -95.5M };
      yield return new object[ ] { "3/2*1-1*100+1*2/2+1*2/2+1*2", -94.5M };
      yield return new object[ ] { "3/2*1-1*100+1*2/2+1*2/2+1*2/2", -95.5M };
      yield return new object[ ] { "3/2*1-1*100+1*2/2+1*2/2+1*2/2+1", -94.5M };
    }
  }
}