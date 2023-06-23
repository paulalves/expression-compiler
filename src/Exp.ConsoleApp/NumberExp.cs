namespace Exp.ConsoleApp
{
  internal class NumberExp : ExpTree {
    public NumberExp(decimal number) {
      Number = number;
    }

    public decimal Number { get; }
        
    public override T Accept<T>(ISyntaxTreeVisitor<T> visitor) {
      return visitor.Visit(this);
    }
  }
}