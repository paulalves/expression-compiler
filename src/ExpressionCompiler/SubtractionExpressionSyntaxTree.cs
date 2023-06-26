namespace ExpressionCompiler
{
  using System.Diagnostics;
  using System.Text;

  [DebuggerDisplay("{DebuggerDisplay()}")]
  public class SubtractionExpressionSyntaxTree : BinaryExpressionSyntaxTree
  {
    public SubtractionExpressionSyntaxTree(ExpressionSyntaxTree lhs, ExpressionSyntaxTree rhs, Token token) : base(lhs, rhs, token)
    {
    }

    public override T Accept<T>(IExpressionSyntaxTreeVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    internal override string DebuggerDisplay()
    {
      var sb = new StringBuilder();
      sb.Append(Lhs.DebuggerDisplay());
      sb.Append("-");
      sb.Append(Rhs.DebuggerDisplay());
      return sb.ToString();
    }
  }
}