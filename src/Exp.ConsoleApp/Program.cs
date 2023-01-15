using System.Text;

namespace Exp.ConsoleApp {
    public static class Program {
        public static void Main(string[] _) {
        }
    }

    internal enum Token {
        Number,
        Plus,
        Minus,
        Eof
    }

    internal class Lexer {
        private char _ch;
        private int _pos; 
        private Token _token;
        private int _number;
        private readonly char[] _stream;
        private const char Eof = '\0'; 

        public Lexer(string stream) {
            _stream = stream.ToCharArray();
            SeekBegin(); 
        }

        private void SeekBegin() {
            Move();
            Walk(); 
        }

        public void Reset() {
            _pos = -1;
        }
        
        private void Move() {
            if (_pos >= _stream.Length) {
                _ch = Eof;
                return;
            }
            int nextChar = _stream[_pos];
            _ch = nextChar > 0 ? (char)nextChar : Eof;
        }
        
        public Token Token {
            get { return _token; }
        }

        public int Number {
            get { return _number; }
        }
        
        public void Walk() {
            while (char.IsWhiteSpace(_ch)) Move();

            switch (_ch) {
                case '+':
                    Move();
                    _token = Token.Plus;
                    return;
                case '-':
                    Move();
                    _token = Token.Minus;
                    return;
                case '\0':
                    _token = Token.Eof;
                    break;
            }

            if (char.IsDigit(_ch)) {
                var sb = new StringBuilder();
                while (char.IsDigit(_ch)) {
                    sb.Append(_ch);
                    Move();
                }

                _number = int.Parse(sb.ToString());
                _token = Token.Number;
            }
        }
    }

    internal class Parser {
    }
    
    internal class SyntaxTree { }
    
    internal class ExpTree : SyntaxTree { }

    internal abstract class BinaryExpTree : ExpTree {
        public BinaryExpTree(SyntaxTree lhs, SyntaxTree rhs) {
            Lhs = lhs;
            Rhs = rhs;
        }

        public SyntaxTree Lhs { get; }
        public SyntaxTree Rhs { get; }
    }
    
    internal class AddExpTree : BinaryExpTree {
        public AddExpTree(SyntaxTree lhs, SyntaxTree rhs) : base(lhs, rhs) {
        }
    }

    internal class SubExpTree : BinaryExpTree {
        public SubExpTree(SyntaxTree lhs, SyntaxTree rhs) : base(lhs, rhs) {
        }
    }

    internal class UnaryExpTree : ExpTree {
        public UnaryExpTree(SyntaxTree rhs) {
            Rhs = rhs is NumberExp number ? new NumberExp(number.Number * -1) : rhs;
        }

        public SyntaxTree Rhs { get; }
    }
    
    internal class NumberExp : ExpTree {
        public NumberExp(decimal number) {
            Number = number;
        }

        public decimal Number { get; }
    }
}