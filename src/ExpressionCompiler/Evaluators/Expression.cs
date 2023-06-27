namespace ExpressionCompiler.Evaluators
{
  using System.Linq.Expressions;

  public class Expression
  {
    private Expression()
    {
      Interpreter = new ExpressionInterpreterEvaluator();
      Lambda = new ExpressionLambdaCompilerEvaluator();
      Code = new CodeGeneratorEvaluator();
    }

    public IExpressionEvaluator<decimal> Interpreter { get; }
    public IExpressionEvaluator<LambdaExpression> Lambda { get; }
    public IExpressionEvaluator<string> Code { get; }

    public static Expression Evaluator { get; } = new Expression();
  }
}