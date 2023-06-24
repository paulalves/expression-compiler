namespace ExpressionCompiler
{
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
  }
}