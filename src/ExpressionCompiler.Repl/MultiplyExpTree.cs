namespace ExpressionCompiler.Repl
{
  internal class MultiplyExpTree : BinaryExpTree {
    public MultiplyExpTree(SyntaxTree lhs, SyntaxTree rhs) : base(lhs, rhs) {
    }

    public override T Accept<T>(ISyntaxTreeVisitor<T> visitor) {
      return visitor.Visit(this);
    }
  }
}