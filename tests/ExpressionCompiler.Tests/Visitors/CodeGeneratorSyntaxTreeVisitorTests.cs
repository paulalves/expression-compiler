namespace ExpressionCompiler.Tests.Visitors
{
  using ExpressionCompiler.Lexer;
  using ExpressionCompiler.Parser;
  using ExpressionCompiler.Visitors;
  using Xunit;
  using Xunit.Abstractions;

  public class CodeGeneratorSyntaxTreeVisitorTests
  {
    private readonly ITestOutputHelper output;

    public CodeGeneratorSyntaxTreeVisitorTests(ITestOutputHelper output)
    {
      this.output = output;
    }
    
    [Theory]
    [InlineData("3/2*1-1*100")]
    [InlineData("3/2*1-1*100+1")]
    [InlineData("3/2*1-1*100+1*2")]
    [InlineData("3/2*1-1*100+1*2/2")]
    [InlineData("3/2*1-1*100+1*2/2+1")]
    [InlineData("3/2*1-1*100+1*2/2+1*2")]
    [InlineData("3/2*1-1*100+1*2/2+1*2/2")]
    [InlineData("3/2*1-1*100+1*2/2+1*2/2+1")]
    [InlineData("3/2*1-1*100+1*2/2+1*2/2+1*2")]
    [InlineData("3/2*1-1*100+1*2/2+1*2/2+1*2/2")]
    [InlineData("3/2*1-1*100+1*2/2+1*2/2+1*2/2+1")]
    public void InterpreterTests(string exp)
    {
      using (var codeGenerator = new CodeGeneratorSyntaxTreeVisitor())
      {
        var parser = new ExpressionParser(new ExpressionLexer(new Scanner(exp)));
        var code = parser.Parse().Accept(codeGenerator);
        output.WriteLine(code);
      }
    }
  }
}