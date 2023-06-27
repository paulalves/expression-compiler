namespace ExpressionCompiler.Tests.Visitors
{
  using System;
  using System.Linq.Expressions;
  using ExpressionCompiler.Lexer;
  using ExpressionCompiler.Parser;
  using ExpressionCompiler.Visitors;
  using FluentAssertions;
  using Xunit;

  public class ExpressionLambdaCompilerSyntaxTreeVisitorTests
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
      var interpreter = new ExpressionLambdaCompilerSyntaxTreeVisitor();

      var lambda = (Expression<Func<decimal>>)syntaxTree.Accept(interpreter);
      
      var func = lambda.Compile();

      func().Should().Be(expected);
    }
  }
}