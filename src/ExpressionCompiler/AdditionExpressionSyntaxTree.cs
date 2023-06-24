namespace ExpressionCompiler
{
  public class AdditionExpressionSyntaxTree : BinaryExpressionSyntaxTree
  {
    public AdditionExpressionSyntaxTree(ExpressionSyntaxTree lhs, ExpressionSyntaxTree rhs, Token token) : base(lhs, rhs, token)
    {
    }
  }
}