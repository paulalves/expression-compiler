namespace ExpressionCompiler
{
  using System;
  using System.Collections.Generic;

  public class Scanner
  {
    private readonly string source;
    private readonly Stack<int> position;

    public Scanner(string source)
    {
      this.source = source;
      this.position = new Stack<int>();
      this.Position = -1; 
    }

    public int Position { get; private set; }

    public bool EndOfSource
    {
      get { return Position >= (source.Length - 1); }
    }
    
    public char? Read()
    {
      return IndexOf(++Position);
    }

    public char? Peek()
    {
      return IndexOf(Position + 1);
    }

    public void Push()
    {
      this.position.Push(this.Position);
    }

    public void Pop()
    {
      this.Position = position.Pop();
    }
    
    private char? IndexOf(int index)
    {
      return IsWithinBounds(index) ? source[index] : (char?)null;
    }

    private bool IsWithinBounds(int index)
    {
      return index >= 0 && index < source.Length;
    }
  }

  public class Token
  {
  }

  public class TokenKind
  {
  }

  public class ExpressionLexer
  {
  }

  public class ExpressionParser
  {
  }
}