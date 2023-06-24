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
  }
}