namespace ExpressionCompiler
{
  public abstract class OperatorExpressionSyntaxTree : ExpressionSyntaxTree
  {
    protected OperatorExpressionSyntaxTree(Token token) : base(token)
    {
    }
  }
}