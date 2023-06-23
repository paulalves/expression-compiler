namespace ExpressionCompiler.Repl
{
  internal interface ISyntaxTreeVisitor<T> {
    T Visit(AddExpTree syntaxNode);
    T Visit(SubExpTree syntaxNode);
    T Visit(MultiplyExpTree syntaxNode);
    T Visit(DivideExpTree syntaxNode);
    T Visit(UnaryExpTree syntaxNode);
    T Visit(NumberExp syntaxNode);
  }
}