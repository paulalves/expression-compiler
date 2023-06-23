namespace Exp.ConsoleApp
{
  using System.Linq.Expressions;

  internal class ExpressionLambdaCompiler : ISyntaxTreeVisitor<Expression<MyProgram>> {

    public Expression<MyProgram> Visit(AddExpTree syntax) {
      return Expression.Lambda<MyProgram>(
        Expression.Add(
          syntax.Lhs.Accept(this).Body,
          syntax.Rhs.Accept(this).Body));
    }

    public Expression<MyProgram> Visit(SubExpTree syntax) {
      return Expression.Lambda<MyProgram>(
        Expression.Subtract(
          syntax.Lhs.Accept(this).Body,
          syntax.Rhs.Accept(this).Body));
    }

    public Expression<MyProgram> Visit(MultiplyExpTree syntax) {
      return Expression.Lambda<MyProgram>(
        Expression.Multiply(
          syntax.Lhs.Accept(this).Body,
          syntax.Rhs.Accept(this).Body));
    }

    public Expression<MyProgram> Visit(DivideExpTree syntax) {
      return Expression.Lambda<MyProgram>(
        Expression.Divide(
          syntax.Lhs.Accept(this).Body,
          syntax.Rhs.Accept(this).Body));
    }

    public Expression<MyProgram> Visit(NumberExp syntaxNode) {
      return Expression.Lambda<MyProgram>(Expression.Constant(syntaxNode.Number));
    }

    public Expression<MyProgram> Visit(UnaryExpTree syntaxNode) {
      return syntaxNode.Rhs.Accept(this);
    }
  }
}