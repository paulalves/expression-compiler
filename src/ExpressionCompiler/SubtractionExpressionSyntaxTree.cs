namespace ExpressionCompiler
{
  public class SubtractionExpressionSyntaxTree : BinaryExpressionSyntaxTree
  {
    public SubtractionExpressionSyntaxTree(ExpressionSyntaxTree lhs, ExpressionSyntaxTree rhs, Token token) : base(lhs, rhs, token)
    {
    }

    public override T Accept<T>(IExpressionSyntaxTreeVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }
  }
}