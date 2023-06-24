namespace ExpressionCompiler
{
  public class AdditionExpressionSyntaxTree : BinaryExpressionSyntaxTree
  {
    public AdditionExpressionSyntaxTree(ExpressionSyntaxTree lhs, ExpressionSyntaxTree rhs, Token token) : base(lhs, rhs, token)
    {
    }

    public override T Accept<T>(IExpressionSyntaxTreeVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }
  }
}