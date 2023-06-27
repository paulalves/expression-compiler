namespace ExpressionCompiler.Visitors
{
  using ExpressionCompiler.SyntaxTree;

  public class ExpressionInterpreter : IExpressionSyntaxTreeVisitor<decimal>
  {
    public decimal Visit(AdditionExpressionSyntaxTree syntaxTree)
    {
      return syntaxTree.Lhs.Accept(this) + 
             syntaxTree.Rhs.Accept(this);
    }

    public decimal Visit(DivisionExpressionSyntaxTree syntaxTree)
    {
      return syntaxTree.Lhs.Accept(this) / 
             syntaxTree.Rhs.Accept(this);
    }

    public decimal Visit(MultiplicationExpressionSyntaxTree syntaxTree)
    {
      return syntaxTree.Lhs.Accept(this) * 
             syntaxTree.Rhs.Accept(this);
    }

    public decimal Visit(NumberExpressionSyntaxTree syntaxTree)
    {
      return syntaxTree.Value;
    }

    public decimal Visit(SubtractionExpressionSyntaxTree syntaxTree)
    {
      return syntaxTree.Lhs.Accept(this) - 
             syntaxTree.Rhs.Accept(this);
    }
  }
}