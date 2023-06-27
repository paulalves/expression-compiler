namespace ExpressionCompiler.SyntaxTree
{
  using ExpressionCompiler.Lexer;

  public abstract class BinaryExpressionSyntaxTree : OperatorExpressionSyntaxTree
  {
    public BinaryExpressionSyntaxTree(ExpressionSyntaxTree lhs, ExpressionSyntaxTree rhs, Token token) : base(token)
    {
      Lhs = lhs;
      Rhs = rhs;
    }

    public ExpressionSyntaxTree Lhs { get; protected set; }
    public ExpressionSyntaxTree Rhs { get; protected set; }
  }
}