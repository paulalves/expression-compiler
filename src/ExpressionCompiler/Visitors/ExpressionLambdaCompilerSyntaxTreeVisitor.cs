namespace ExpressionCompiler.Visitors
{
  using System;
  using System.Linq.Expressions;
  using ExpressionCompiler.SyntaxTree;


  public class ExpressionLambdaCompilerSyntaxTreeVisitor : IExpressionSyntaxTreeVisitor<LambdaExpression>
  {
    public LambdaExpression Visit(AdditionExpressionSyntaxTree syntaxTree)
    {
      return Expression.Lambda<Func<decimal>>(
        Expression.Add(
          syntaxTree.Lhs.Accept(this).Body,
          syntaxTree.Rhs.Accept(this).Body));
    }

    public LambdaExpression Visit(DivisionExpressionSyntaxTree syntaxTree)
    {
      return Expression.Lambda<Func<decimal>>(
        Expression.Divide(
          syntaxTree.Lhs.Accept(this).Body,
          syntaxTree.Rhs.Accept(this).Body));
    }

    public LambdaExpression Visit(MultiplicationExpressionSyntaxTree syntaxTree)
    {
      return Expression.Lambda<Func<decimal>>(
        Expression.Multiply(
          syntaxTree.Lhs.Accept(this).Body,
          syntaxTree.Rhs.Accept(this).Body));
    }

    public LambdaExpression Visit(NumberExpressionSyntaxTree syntaxTree)
    {
      return Expression.Lambda<Func<decimal>>(
        Expression.Constant(syntaxTree.Value));
    }

    public LambdaExpression Visit(SubtractionExpressionSyntaxTree syntaxTree)
    {
      return Expression.Lambda<Func<decimal>>(
        Expression.Subtract(
          syntaxTree.Lhs.Accept(this).Body,
          syntaxTree.Rhs.Accept(this).Body));
    }
  }
}