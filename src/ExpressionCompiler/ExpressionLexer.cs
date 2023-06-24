namespace ExpressionCompiler
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public class ExpressionLexer
  {
    private readonly Scanner scanner;
    private readonly Dictionary<char, TokenKind> operators;

    public ExpressionLexer(Scanner scanner)
    {
      this.scanner = scanner;
      this.operators = new Dictionary<char, TokenKind>
      {
        { '+', TokenKind.Addition },
        { '-', TokenKind.Subtraction },
        { '*', TokenKind.Multiplication },
        { '/', TokenKind.Division },
        { '(', TokenKind.OpenPar },
        { ')', TokenKind.ClosePar }
      };
    }

    public Token Read()
    {
      Token token;
      SkipWhiteSpace();
      char? ch = scanner.Peek();
      if (ch.HasValue)
      {
        if (operators.ContainsKey(ch.Value))
        {
          scanner.Read();
          token = new Token(ch.Value.ToString(), scanner.Position, operators[ch.Value]);
        }
        else if (char.IsDigit(ch.Value) || ch.Value == '.')
        {
          token = Parse();
        }
        else
        {
          throw new Exception($"An unexpected character '{scanner.Read()}' was found at position '{scanner.Position}'");
        }
      }
      else
      {
        token = Token.EOF;
      }

      return token;
    }

    public Token Peek()
    {
      scanner.Push();
      var token = Read();
      scanner.Pop();
      return token;
    }

    public Token Parse()
    {
      var sb = new StringBuilder();
      var pos = scanner.Position + 1;
      var next = scanner.Peek();
      while (next.HasValue && char.IsDigit(next.Value))
      {
        sb.Append(next.Value);
        scanner.Read();
        next = scanner.Peek();
      }
      return new Token(sb.ToString(), pos, TokenKind.Number);
    }

    private void SkipWhiteSpace()
    {
      var next = scanner.Peek();
      while (next.HasValue && char.IsWhiteSpace(next.Value))
      {
        scanner.Read();
        next = scanner.Peek();
      }
    }
  }
}