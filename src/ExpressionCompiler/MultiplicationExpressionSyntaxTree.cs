namespace ExpressionCompiler
{
  public class MultiplicationExpressionSyntaxTree : BinaryExpressionSyntaxTree
  {
    public MultiplicationExpressionSyntaxTree(ExpressionSyntaxTree lhs, ExpressionSyntaxTree rhs, Token token) : base(lhs, rhs, token)
    {
    }
  }
}