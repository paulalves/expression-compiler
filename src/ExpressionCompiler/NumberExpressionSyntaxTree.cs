namespace ExpressionCompiler
{
  using System.Diagnostics;
  using System.Text;

  [DebuggerDisplay("{DebuggerDisplay()}")]
  public class NumberExpressionSyntaxTree : ExpressionSyntaxTree
  {
    public NumberExpressionSyntaxTree(Token token) : base(token)
    {
    }

    public decimal Value
    {
      get { return decimal.Parse(Token.Text); }
    }

    public override T Accept<T>(IExpressionSyntaxTreeVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    internal override string DebuggerDisplay()
    {
      var sb = new StringBuilder();
      sb.Append(Token.Text);
      return sb.ToString();
    }
  }
}