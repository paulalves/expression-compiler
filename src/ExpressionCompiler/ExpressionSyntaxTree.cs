namespace ExpressionCompiler
{
  public abstract class ExpressionSyntaxTree
  {
    protected ExpressionSyntaxTree(Token token)
    {
      Token = token;
    }
    
    public Token Token { get; protected set; }
    
    public abstract T Accept<T>(IExpressionSyntaxTreeVisitor<T> visitor);
  }
  
  public interface IExpressionSyntaxTreeVisitor<T>
  {
    T Visit(AdditionExpressionSyntaxTree syntaxTree);
    T Visit(DivisionExpressionSyntaxTree syntaxTree);
    T Visit(MultiplicationExpressionSyntaxTree syntaxTree);
    T Visit(NumberExpressionSyntaxTree syntaxTree);
    T Visit(SubtractionExpressionSyntaxTree syntaxTree);
  }

  public class ExpressionInterpreter : IExpressionSyntaxTreeVisitor<decimal>
  {
    public decimal Visit(AdditionExpressionSyntaxTree syntaxTree)
    {
      return syntaxTree.Lhs.Accept(this) + 
             syntaxTree.Rhs.Accept(this);
    }

    public decimal Visit(DivisionExpressionSyntaxTree syntaxTree)
    {
      return syntaxTree.Lhs.Accept(this) / 
             syntaxTree.Rhs.Accept(this);
    }

    public decimal Visit(MultiplicationExpressionSyntaxTree syntaxTree)
    {
      return syntaxTree.Lhs.Accept(this) * 
             syntaxTree.Rhs.Accept(this);
    }

    public decimal Visit(NumberExpressionSyntaxTree syntaxTree)
    {
      return syntaxTree.Value;
    }

    public decimal Visit(SubtractionExpressionSyntaxTree syntaxTree)
    {
      return syntaxTree.Lhs.Accept(this) - 
             syntaxTree.Rhs.Accept(this);
    }
  }
}