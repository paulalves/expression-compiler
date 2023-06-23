namespace ExpressionCompiler.Repl
{
  internal abstract class SyntaxTree {
    public string NodeType {
      get { return GetType().Name; }
    }

    public abstract T Accept<T>(ISyntaxTreeVisitor<T> visitor);
  }
}