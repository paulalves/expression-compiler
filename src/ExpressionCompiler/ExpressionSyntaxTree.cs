namespace ExpressionCompiler
{
  using System.Diagnostics;

  [DebuggerDisplay("{DebuggerDisplay()}")]
  public abstract class ExpressionSyntaxTree
  {
    protected ExpressionSyntaxTree(Token token)
    {
      Token = token;
    }
    
    [DebuggerHidden]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Token Token { get; protected set; }
    
    public abstract T Accept<T>(IExpressionSyntaxTreeVisitor<T> visitor);

    internal abstract string DebuggerDisplay();
  }
}