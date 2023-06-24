namespace ExpressionCompiler
{
  public class DivisionExpressionSyntaxTree : BinaryExpressionSyntaxTree
  {
    public DivisionExpressionSyntaxTree(ExpressionSyntaxTree lhs, ExpressionSyntaxTree rhs, Token token) : base(lhs, rhs, token)
    {
    }

    public override T Accept<T>(IExpressionSyntaxTreeVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }
  }
}