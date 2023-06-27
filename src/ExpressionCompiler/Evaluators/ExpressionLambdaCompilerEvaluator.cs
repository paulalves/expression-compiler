namespace ExpressionCompiler.Evaluators
{
  using System.Linq.Expressions;
  using ExpressionCompiler.Lexer;
  using ExpressionCompiler.Parser;
  using ExpressionCompiler.Visitors;

  public class ExpressionLambdaCompilerEvaluator : IExpressionEvaluator<LambdaExpression>
  {
    private readonly IExpressionSyntaxTreeVisitor<LambdaExpression> expressionLambdaCompiler;

    public ExpressionLambdaCompilerEvaluator()
    {
      expressionLambdaCompiler = new ExpressionLambdaCompilerSyntaxTreeVisitor();
    }

    public LambdaExpression Evaluate(string source)
    {
      var expressionParser = new ExpressionParser(new ExpressionLexer(new Scanner(source)));
      var expressionTree = expressionParser.Parse();
      return expressionTree.Accept(expressionLambdaCompiler);
    }
  }
}