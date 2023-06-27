namespace ExpressionCompiler.Evaluators
{
  public interface IExpressionEvaluator<T>
  {
    T Evaluate(string source);
  }
}