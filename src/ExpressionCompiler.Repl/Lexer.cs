namespace ExpressionCompiler.Repl
{
  using System.Text;

  internal class Lexer {
    private char _ch;
    private int _pos;
    private readonly char[] _stream;
    private const char Eof = '\0';

    public Lexer(string stream) {
      _stream = stream.ToCharArray();
      SeekBegin();
    }

    private void SeekBegin() {
      Reset();
      Move();
      Walk();
    }

    public void Reset() {
      _pos = -1;
    }

    private void Move() {
      if (++_pos >= _stream.Length) {
        _ch = Eof;
        return;
      }

      int nextChar = _stream[_pos];
      _ch = nextChar > 0 ? (char)nextChar : Eof;
    }

    public Token Token { get; private set; }

    public decimal Number { get; private set; }

    public void Walk() {
      while (char.IsWhiteSpace(_ch)) Move();

      switch (_ch) {
        case '+':
          Move();
          Token = Token.Plus;
          return;
        case '-':
          Move();
          Token = Token.Minus;
          return;
        case '*':
          Move();
          Token = Token.Times;
          return;
        case '/':
          Move();
          Token = Token.Division;
          return;
        case '(':
          Move();
          Token = Token.LPar;
          return;
        case ')':
          Move();
          Token = Token.RPar;
          return;
        case '\0':
          Token = Token.Eof;
          break;
      }

      if (char.IsDigit(_ch)) {
        var sb = new StringBuilder();
        while (char.IsDigit(_ch)) {
          sb.Append(_ch);
          Move();
        }

        Number = decimal.Parse(sb.ToString());
        Token = Token.Number;
      }
    }
  }
}