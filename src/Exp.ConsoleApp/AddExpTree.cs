namespace Exp.ConsoleApp
{
  internal class AddExpTree : BinaryExpTree {
    public AddExpTree(SyntaxTree lhs, SyntaxTree rhs) : base(lhs, rhs) {
    }

    public override T Accept<T>(ISyntaxTreeVisitor<T> visitor) {
      return visitor.Visit(this);
    }
  }
}