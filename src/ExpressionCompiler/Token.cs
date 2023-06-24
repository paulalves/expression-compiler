namespace ExpressionCompiler
{
  public class Token
  {
    public Token(string text, int position, TokenKind kind)
    {
      Text = text;
      Position = position;
      Kind = kind;
    }

    public string Text { get; private set; }
    public int Position { get; private set; }
    public TokenKind Kind { get; private set; }

    public static Token EOF { get; } = new Token(null, -1, TokenKind.EOF);
  }
}