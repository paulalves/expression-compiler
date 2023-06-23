namespace ExpressionCompiler.Repl
{
  internal class ExpressionEvaluator : ISyntaxTreeVisitor<decimal> {
    public decimal Visit(UnaryExpTree syntaxNode) {
      return syntaxNode.Rhs.Accept(this);
    }

    public decimal Visit(NumberExp syntaxNode) {
      return syntaxNode.Number;
    }
        
    public decimal Visit(AddExpTree syntaxNode) {
      return syntaxNode.Lhs.Accept(this) +
             syntaxNode.Rhs.Accept(this);
    }

    public decimal Visit(SubExpTree syntaxNode) {
      return syntaxNode.Lhs.Accept(this) -
             syntaxNode.Rhs.Accept(this);
    }

    public decimal Visit(MultiplyExpTree syntaxNode) {
      return syntaxNode.Lhs.Accept(this) *
             syntaxNode.Rhs.Accept(this);
    }

    public decimal Visit(DivideExpTree syntaxNode) {
      return syntaxNode.Lhs.Accept(this) /
             syntaxNode.Rhs.Accept(this);
    }
  }
}