namespace ExpressionCompiler.Tests.Evaluators
{
  using Xunit;
  using Xunit.Abstractions;
  using ExpressionCompiler.Evaluators;
  
  public class ExpressionEvaluatorTests
  {
    private readonly ITestOutputHelper output;

    public ExpressionEvaluatorTests(ITestOutputHelper output)
    {
      this.output = output;
    }
    
    [Fact]
    public void Eval()
    {
      var exp = "3/2*1-1*100+1*2/2+1*2/2+1*2/2+1";
      var eval1 = Expression.Evaluator.Code.Evaluate(exp);
      var eval2 = Expression.Evaluator.Interpreter.Evaluate(exp);
      var eval3 = Expression.Evaluator.Lambda.Evaluate(exp);

      output.WriteLine(eval1);
      output.WriteLine("{0:N}", eval2);
      output.WriteLine(eval3.ToString());
    }
  }
}