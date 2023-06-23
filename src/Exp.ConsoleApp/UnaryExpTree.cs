namespace Exp.ConsoleApp
{
  internal class UnaryExpTree : ExpTree {
    public UnaryExpTree(SyntaxTree rhs) {
      Rhs = rhs is NumberExp number ? new NumberExp(number.Number * -1) : rhs;
    }

    public SyntaxTree Rhs { get; }
        
    public override T Accept<T>(ISyntaxTreeVisitor<T> visitor) {
      return visitor.Visit(this);
    }
  }
}