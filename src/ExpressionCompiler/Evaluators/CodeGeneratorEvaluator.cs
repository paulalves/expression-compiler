namespace ExpressionCompiler.Evaluators
{
  using ExpressionCompiler.Lexer;
  using ExpressionCompiler.Parser;
  using ExpressionCompiler.Visitors;

  public class CodeGeneratorEvaluator : IExpressionEvaluator<string>
  {
    private readonly CodeGeneratorSyntaxTreeVisitor codeGenerator;

    public CodeGeneratorEvaluator()
    {
      codeGenerator = new CodeGeneratorSyntaxTreeVisitor();
    }
    public string Evaluate(string source)
    {
      var expressionParser = new ExpressionParser(
        new ExpressionLexer(
          new Scanner(source)));
      
      var expressionTree = expressionParser.Parse();

      return expressionTree.Accept(codeGenerator);
    }
  }
}