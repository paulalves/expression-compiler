namespace ExpressionCompiler.Repl
{
  using System;

  internal class CsharpCodeGenerator : ISyntaxTreeVisitor<string> {
    public string Visit(AddExpTree syntaxNode) {
      return VisitBinaryNode(syntaxNode);
    }

    public string Visit(SubExpTree syntaxNode) {
      return VisitBinaryNode(syntaxNode);
    }

    public string Visit(MultiplyExpTree syntaxNode) {
      return VisitBinaryNode(syntaxNode);
    }

    public string Visit(DivideExpTree syntaxNode) {
      return VisitBinaryNode(syntaxNode);
    }

    public string Visit(UnaryExpTree syntaxNode) {
      return string.Format("new {0}({1})", 
        syntaxNode.NodeType, 
        syntaxNode.Rhs.Accept(this));
    }

    public string Visit(NumberExp syntaxNode) {
      return string.Format("new {0}({1})", 
        syntaxNode.NodeType,
        Math.Abs(syntaxNode.Number));
    }
        
    private string VisitBinaryNode(BinaryExpTree syntaxNode) {
      return string.Format("new {0}({1}, {2})", syntaxNode.NodeType,
        syntaxNode.Lhs.Accept(this),
        syntaxNode.Rhs.Accept(this));
    }
  }
}