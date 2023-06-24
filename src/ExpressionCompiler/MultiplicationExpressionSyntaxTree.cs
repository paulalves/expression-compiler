namespace ExpressionCompiler
{
  public class MultiplicationExpressionSyntaxTree : BinaryExpressionSyntaxTree
  {
    public MultiplicationExpressionSyntaxTree(ExpressionSyntaxTree lhs, ExpressionSyntaxTree rhs, Token token) : base(lhs, rhs, token)
    {
    }

    public override T Accept<T>(IExpressionSyntaxTreeVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }
  }
}