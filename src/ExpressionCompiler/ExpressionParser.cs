namespace ExpressionCompiler
{
  public class ExpressionParser
  {
  }

  public abstract class ExpressionNode
  {
    protected ExpressionNode(Token token)
    {
      Token = token;
    }
    
    public Token Token { get; protected set; }
  }

  public class NumberExpressionNode : ExpressionNode
  {
    public NumberExpressionNode(Token token) : base(token)
    {
    }

    public decimal Value
    {
      get { return decimal.Parse(Token.Text); }
    }
  }

  public abstract class OperatorExpressionNode : ExpressionNode
  {
    protected OperatorExpressionNode(Token token) : base(token)
    {
    }
  }

  public class BinaryExpressionNode : OperatorExpressionNode
  {
    public BinaryExpressionNode(ExpressionNode lhs, ExpressionNode rhs, Token token) : base(token)
    {
      Lhs = lhs;
      Rhs = rhs;
    }

    public ExpressionNode Lhs { get; protected set; }
    public ExpressionNode Rhs { get; protected set; }
  }

  public class AdditionExpressionNode : BinaryExpressionNode
  {
    public AdditionExpressionNode(ExpressionNode lhs, ExpressionNode rhs, Token token) : base(lhs, rhs, token)
    {
    }
  }

  public class SubtractionExpressionNode : BinaryExpressionNode
  {
    public SubtractionExpressionNode(ExpressionNode lhs, ExpressionNode rhs, Token token) : base(lhs, rhs, token)
    {
    }
  }
  
  public class MultiplicationExpressionNode : BinaryExpressionNode
  {
    public MultiplicationExpressionNode(ExpressionNode lhs, ExpressionNode rhs, Token token) : base(lhs, rhs, token)
    {
    }
  }
    
  public class DivisionExpressionNode : BinaryExpressionNode
  {
    public DivisionExpressionNode(ExpressionNode lhs, ExpressionNode rhs, Token token) : base(lhs, rhs, token)
    {
    }
  }
}