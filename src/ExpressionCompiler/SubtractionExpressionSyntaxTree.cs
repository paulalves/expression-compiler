namespace ExpressionCompiler
{
  public class SubtractionExpressionSyntaxTree : BinaryExpressionSyntaxTree
  {
    public SubtractionExpressionSyntaxTree(ExpressionSyntaxTree lhs, ExpressionSyntaxTree rhs, Token token) : base(lhs, rhs, token)
    {
    }
  }
}