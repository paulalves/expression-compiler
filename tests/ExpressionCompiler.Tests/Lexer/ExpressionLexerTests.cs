namespace ExpressionCompiler.Tests.Lexer
{
  using System;
  using ExpressionCompiler.Lexer;
  using FluentAssertions;
  using Xunit;

  public static class ExpressionLexerTests
  {
    [Fact]
    public static void When_ExpressionHasAddition_ItShouldReturnCorrectTokens()
    {
      var expressionLexer = new ExpressionLexer(new Scanner("2+3"));
      expressionLexer.Read().Should().BeEquivalentTo(new Token("2", position: 0, TokenKind.Number));
      expressionLexer.Read().Should().BeEquivalentTo(new Token("+", position: 1, TokenKind.Addition));
      expressionLexer.Read().Should().BeEquivalentTo(new Token("3", position: 2, TokenKind.Number));
    }

    [Fact]
    public static void When_ExpressionHasSubtraction_ItShouldReturnCorrectTokens()
    {
      var expressionLexer = new ExpressionLexer(new Scanner("2-2"));
      expressionLexer.Read().Should().BeEquivalentTo(new Token("2", position: 0, TokenKind.Number));
      expressionLexer.Read().Should().BeEquivalentTo(new Token("-", position: 1, TokenKind.Subtraction));
      expressionLexer.Read().Should().BeEquivalentTo(new Token("2", position: 2, TokenKind.Number));
    }
    
    [Fact]
    public static void When_ExpressionHasMultiplication_ItShouldReturnCorrectTokens()
    {
      var expressionLexer = new ExpressionLexer(new Scanner("2*3"));
      expressionLexer.Read().Should().BeEquivalentTo(new Token("2", position: 0, TokenKind.Number));
      expressionLexer.Read().Should().BeEquivalentTo(new Token("*", position: 1, TokenKind.Multiplication));
      expressionLexer.Read().Should().BeEquivalentTo(new Token("3", position: 2, TokenKind.Number));
    }
    
    [Fact]
    public static void When_ExpressionHasDivision_ItShouldReturnCorrectTokens()
    {
      var expressionLexer = new ExpressionLexer(new Scanner("4/2"));
      expressionLexer.Read().Should().BeEquivalentTo(new Token("4", position: 0, TokenKind.Number));
      expressionLexer.Read().Should().BeEquivalentTo(new Token("/", position: 1, TokenKind.Division));
      expressionLexer.Read().Should().BeEquivalentTo(new Token("2", position: 2, TokenKind.Number));
    }
    
    [Fact]
    public static void When_ExpressionContainsUnexpectedToken_ItMustFails()
    {
      var expressionLexer = new ExpressionLexer(new Scanner("2\"2"));
      expressionLexer.Invoking(x =>
      {
        var token = expressionLexer.Peek();
        do
        {
          expressionLexer.Read();
        } while ((token = expressionLexer.Peek()) != Token.EOF);
      }).Should().Throw<Exception>().WithMessage("An unexpected character '\"' was found at position '1'");
    }
  }
}