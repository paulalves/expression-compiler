namespace Exp.ConsoleApp
{
  internal class SubExpTree : BinaryExpTree {
    public SubExpTree(SyntaxTree lhs, SyntaxTree rhs) : base(lhs, rhs) {
    }

    public override T Accept<T>(ISyntaxTreeVisitor<T> visitor) {
      return visitor.Visit(this);
    }
  }
}