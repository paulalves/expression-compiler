namespace ExpressionCompiler
{
  public interface IExpressionSyntaxTreeVisitor<T>
  {
    T Visit(AdditionExpressionSyntaxTree syntaxTree);
    T Visit(DivisionExpressionSyntaxTree syntaxTree);
    T Visit(MultiplicationExpressionSyntaxTree syntaxTree);
    T Visit(NumberExpressionSyntaxTree syntaxTree);
    T Visit(SubtractionExpressionSyntaxTree syntaxTree);
  }
}