namespace Exp.ConsoleApp
{
  internal class DivideExpTree : BinaryExpTree {
    public DivideExpTree(SyntaxTree lhs, SyntaxTree rhs) : base(lhs, rhs) {
    }

    public override T Accept<T>(ISyntaxTreeVisitor<T> visitor) {
      return visitor.Visit(this);
    }
  }
}