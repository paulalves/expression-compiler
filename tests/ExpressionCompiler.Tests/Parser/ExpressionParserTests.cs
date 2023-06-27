namespace ExpressionCompiler.Tests.Parser
{
  using ExpressionCompiler.Lexer;
  using ExpressionCompiler.Parser;
  using ExpressionCompiler.SyntaxTree;
  using FluentAssertions;
  using Xunit;

  public static class ExpressionParserTests
  {
    [Fact]
    public static void When_ParseExpression_SyntaxTreeMustBeBuilt()
    {
      var exp = "3/2*1-1*100";
      var parser = new ExpressionParser(new ExpressionLexer(new Scanner(exp)));
      
      var syntaxTree = parser.Parse();
      syntaxTree.Should().NotBeNull();
      
      var lhs = new MultiplicationExpressionSyntaxTree(
        new DivisionExpressionSyntaxTree(
          new NumberExpressionSyntaxTree(
            new Token("3", position: 0, TokenKind.Number)),
          new NumberExpressionSyntaxTree(
            new Token("2", position: 2, TokenKind.Number)),
          new Token("/", position: 1, TokenKind.Division)), new NumberExpressionSyntaxTree(new Token("1", position: 4, TokenKind.Number)),
        new Token("*", position: 3, TokenKind.Multiplication));

      var rhs = new MultiplicationExpressionSyntaxTree(
        new NumberExpressionSyntaxTree(new Token("1", position: 6, TokenKind.Number)),
        new NumberExpressionSyntaxTree(new Token("100", position: 8, TokenKind.Number)),
        new Token("*", position: 7, TokenKind.Multiplication));

      syntaxTree.Should().BeEquivalentTo(
        new SubtractionExpressionSyntaxTree(lhs, rhs, 
          new Token("-", position: 5, TokenKind.Subtraction)));
    }
  }
}