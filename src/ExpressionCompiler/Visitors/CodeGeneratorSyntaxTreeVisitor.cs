namespace ExpressionCompiler.Visitors
{
  using System;
  using System.CodeDom.Compiler;
  using System.IO;
  using System.Text;
  using ExpressionCompiler.Lexer;
  using ExpressionCompiler.SyntaxTree;

  public class CodeGeneratorSyntaxTreeVisitor : IExpressionSyntaxTreeVisitor<string>, IDisposable
  {
    private readonly IndentedTextWriter source;
    private readonly StringBuilder builder;

    public CodeGeneratorSyntaxTreeVisitor()
    {
      builder = new StringBuilder();
      source = new IndentedTextWriter(new StringWriter(builder), " ");
    }
    
    public string Visit(AdditionExpressionSyntaxTree syntaxTree)
    {
      return VisitBinaryExpression(syntaxTree);
    }

    public string Visit(SubtractionExpressionSyntaxTree syntaxTree)
    {
      return VisitBinaryExpression(syntaxTree);
    }

    public string Visit(NumberExpressionSyntaxTree syntaxTree)
    {
      source.Indent++;
      source.WriteLine("new NumberExpressionSyntaxTree({0})", WriteToken(syntaxTree.Token));
      source.Indent--;
      return builder.ToString();
    }

    public string Visit(MultiplicationExpressionSyntaxTree syntaxTree)
    {
      return VisitBinaryExpression(syntaxTree);
    }

    public string Visit(DivisionExpressionSyntaxTree syntaxTree)
    {
      return VisitBinaryExpression(syntaxTree);
    }

    private string WriteToken(Token token)
    {
      return string.Format("new Token(\"{0}\", {1}, TokenKind.{2})", token.Text, token.Position, token.Kind);
    }

    private string VisitBinaryExpression(BinaryExpressionSyntaxTree syntaxTree)
    {
      var typeName = syntaxTree.GetType().Name;
      source.WriteLine($"new {typeName}(");
      source.Indent++;
      syntaxTree.Lhs.Accept(this);
      source.Write(", ");
      syntaxTree.Rhs.Accept(this);
      source.WriteLine(string.Concat(", ", WriteToken(syntaxTree.Token), ")"));
      source.Indent--;
      return builder.ToString();
    }

    public void Dispose()
    {
      source?.Flush();
      source?.Dispose();
    }
  }
}