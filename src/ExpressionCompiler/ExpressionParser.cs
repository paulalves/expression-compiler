namespace ExpressionCompiler
{
  using System;

  public class ExpressionParser
  {
    private readonly ExpressionLexer expressionLexer;

    public ExpressionParser(ExpressionLexer expressionLexer)
    {
      this.expressionLexer = expressionLexer;
    }

    public ExpressionSyntaxTree Parse()
    {
      var lhs = Term();
      var opToken = expressionLexer.Peek();
      while (opToken.Kind == TokenKind.Addition || opToken.Kind == TokenKind.Subtraction)
      {
        expressionLexer.Read();
        var rhs = Term(); 
        if (opToken.Kind == TokenKind.Addition)
        {
          lhs = new AdditionExpressionSyntaxTree(lhs, rhs, opToken);
        }
        else
        {
          lhs = new SubtractionExpressionSyntaxTree(lhs, rhs, opToken);
        }

        opToken = expressionLexer.Peek();
      }
      return lhs;
    }

    private ExpressionSyntaxTree Term()
    {
      var lhs = Factor();
      var opToken = expressionLexer.Peek();
      
      while (opToken.Kind == TokenKind.Multiplication || opToken.Kind == TokenKind.Division)
      {
        expressionLexer.Read();
       
        var rhs = Factor();
        
        if (opToken.Kind == TokenKind.Multiplication)
        {
          lhs = new MultiplicationExpressionSyntaxTree(lhs, rhs, opToken);
        }
        else
        {
          lhs = new DivisionExpressionSyntaxTree(lhs, rhs, opToken);
        }

        opToken = expressionLexer.Peek();
      }
      return lhs;
    }

    private ExpressionSyntaxTree Factor()
    {
      
      var token = expressionLexer.Peek();
      if (token.Kind == TokenKind.OpenPar)
      {
        return SubExp();
      }
      
      token = expressionLexer.Read();
      return token.Kind != TokenKind.Number ? default(ExpressionSyntaxTree) : new NumberExpressionSyntaxTree(token);
    }

    private ExpressionSyntaxTree SubExp()
    {
      var token = expressionLexer.Read();
      if (token.Kind != TokenKind.OpenPar)
      {
        throw new Exception($"Expected '(' at position '{token.Position}'");
      }
      
      var exp = Parse();
      
      token = expressionLexer.Read();
      if (token.Kind != TokenKind.ClosePar)
      {
        throw new Exception($"Expected ')' at position '{token.Position}'");
      }
      return exp;
    }
  }
}