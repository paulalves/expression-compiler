namespace ExpressionCompiler.Evaluators
{
  using ExpressionCompiler.Lexer;
  using ExpressionCompiler.Parser;
  using ExpressionCompiler.Visitors;

  public class ExpressionInterpreterEvaluator : IExpressionEvaluator<decimal>
  {
    private readonly IExpressionSyntaxTreeVisitor<decimal> expressionInterpreter;

    public ExpressionInterpreterEvaluator()
    {
      expressionInterpreter = new ExpressionInterpreterSyntaxTreeVisitor();
    }
    
    public decimal Evaluate(string source)
    {
      var expressionParser = new ExpressionParser(new ExpressionLexer(new Scanner(source)));
      var expressionTree = expressionParser.Parse();
      return expressionTree.Accept(expressionInterpreter);
    }
  }
}