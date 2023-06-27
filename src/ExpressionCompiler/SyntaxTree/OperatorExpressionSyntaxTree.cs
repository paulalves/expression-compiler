namespace ExpressionCompiler.SyntaxTree
{
  using ExpressionCompiler.Lexer;

  public abstract class OperatorExpressionSyntaxTree : ExpressionSyntaxTree
  {
    protected OperatorExpressionSyntaxTree(Token token) : base(token)
    {
    }
  }
}