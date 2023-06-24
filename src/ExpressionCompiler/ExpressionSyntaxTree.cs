namespace ExpressionCompiler
{
  public abstract class ExpressionSyntaxTree
  {
    protected ExpressionSyntaxTree(Token token)
    {
      Token = token;
    }
    
    public Token Token { get; protected set; }
  }
}