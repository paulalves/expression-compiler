using System.Linq.Expressions;
using System.Text;

namespace Exp.ConsoleApp {
    public static class Program {
        public static void Main(string[] _) {
            CompileAndRunFromSource("-2 + 1 * 3 * -5 / 2");
            
            CompileAndRunFromAst(
                new AddExpTree(
                    new UnaryExpTree(new NumberExp(2)), 
                    new DivideExpTree(
                        new MultiplyExpTree(
                            new MultiplyExpTree(
                                new NumberExp(1),
                                new NumberExp(3)), 
                            new UnaryExpTree(new NumberExp(5))), 
                        new NumberExp(2))));
        }
        
        private static void CompileAndRunFromSource(string source) {
            CompileAndRunFromAst(Parser.Parse(source));
        }

        private static void CompileAndRunFromAst(SyntaxTree ast) {
            MyProgram program = Compile(ast);

            Console.WriteLine("Result: {0}", program());
        }

        private static MyProgram Compile(SyntaxTree ast) {
            Console.WriteLine("C# Syntax Tree: {0}", ast.GenerateCsharp());

            var callTree = ast.Compile(new ExpressionLambdaCompiler<MyProgram>());

            Console.WriteLine("Expression Tree: {0}", callTree);

            return callTree.Compile();
        }
    }

    internal delegate decimal MyProgram();
    
    internal enum Token {
        Number,
        Plus,
        Minus,
        Times,
        Division,
        Eof
    }

    internal class Lexer {
        private char _ch;
        private int _pos; 
        private Token _token;
        private decimal _number;
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
        
        public Token Token {
            get { return _token; }
        }

        public decimal Number {
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
                case '*':
                    Move();
                    _token = Token.Times;
                    return;
                case '/':
                    Move();
                    _token = Token.Division;
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

                _number = decimal.Parse(sb.ToString());
                _token = Token.Number;
            }
        }
    }

    internal class Parser {
        private readonly Lexer _lexer;

        public Parser(Lexer lexer) {
            _lexer = lexer;
        }

        public SyntaxTree Parse() {
            return AddOrSubSyntaxNode();
        }

        private SyntaxTree AddOrSubSyntaxNode() {
            var lhs = MultiplyOrDivideNode();
            while (true)
                if (_lexer.Token == Token.Plus) {
                    _lexer.Walk();
                    var rhs = MultiplyOrDivideNode();
                    lhs = new AddExpTree(lhs, rhs);
                }
                else if (_lexer.Token == Token.Minus) {
                    _lexer.Walk();
                    var rhs = MultiplyOrDivideNode();
                    lhs = new SubExpTree(lhs, rhs);
                }
                else {
                    return lhs;
                }
        }
        
        private SyntaxTree MultiplyOrDivideNode() {
            var lhs = UnaryNode();
            while (true)
                if (_lexer.Token == Token.Times) {
                    _lexer.Walk();
                    lhs = new MultiplyExpTree(lhs, UnaryNode());
                }
                else if (_lexer.Token == Token.Division) {
                    _lexer.Walk();
                    lhs = new DivideExpTree(lhs, UnaryNode());
                }
                else {
                    return lhs;
                }
        }
        
        private SyntaxTree UnaryNode() {
            while (true)
                if (_lexer.Token == Token.Plus) {
                    _lexer.Walk();
                }
                else if (_lexer.Token == Token.Minus) {
                    _lexer.Walk();
                    return new UnaryExpTree(UnaryNode());
                }
                else {
                    return LeafNode();
                }
        }

        private SyntaxTree LeafNode() {
            if (_lexer.Token == Token.Number) {
                var number = new NumberExp(_lexer.Number);
                _lexer.Walk();
                return number;
            }

            throw new Exception($"Error! Invalid token {_lexer.Token}");
        }

        public static SyntaxTree Parse(string source) {
            var parser = new Parser(new Lexer(source));
            return parser.Parse();
        }
    }

    internal abstract class SyntaxTree {
        protected string NodeType {
            get { return GetType().Name; }
        }

        public abstract string GenerateCsharp();

        public override string ToString() {
            return GenerateCsharp();
        }

        public Expression<T> Compile<T>(ISyntaxTreeVisitor<T> visitor) where T : Delegate {
            return visitor.Visit((this as ExpTree)!);
        }
    }

    internal interface ISyntaxTreeVisitor<T> where T : Delegate {
        Expression<T> Visit(ExpTree syntaxNode);
        Expression<T> Visit(AddExpTree syntaxNode);
        Expression<T> Visit(SubExpTree syntaxNode);
        Expression<T> Visit(MultiplyExpTree syntaxNode);
        Expression<T> Visit(DivideExpTree syntaxNode);
    }

    internal class ExpTree : SyntaxTree {
        public override string GenerateCsharp() {
            switch (this) {
                case BinaryExpTree binary:
                    return $"new {NodeType}({binary.Lhs.GenerateCsharp()}, {binary.Rhs.GenerateCsharp()})";
                case UnaryExpTree unary when unary.Rhs is NumberExp unaryRhs:
                    return $"new {NodeType}(new {unaryRhs.NodeType}({unaryRhs.Number * -1}))";
                case UnaryExpTree unary:
                    return $"new {NodeType}({unary.Rhs.GenerateCsharp()})";
                case NumberExp number:
                    return $"new {NodeType}({number.Number})";
                default:
                    return "(Unknown)";
            }
        }
    }

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

    internal class MultiplyExpTree : BinaryExpTree {
        public MultiplyExpTree(SyntaxTree lhs, SyntaxTree rhs) : base(lhs, rhs) {
        }
    }

    internal class DivideExpTree : BinaryExpTree {
        public DivideExpTree(SyntaxTree lhs, SyntaxTree rhs) : base(lhs, rhs) {
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
    
    internal class ExpressionLambdaCompiler<T> : ISyntaxTreeVisitor<T> where T : Delegate {
        public Expression<T> Visit(ExpTree syntax) {
            switch (syntax) {
                case AddExpTree addExpTree: return Visit(addExpTree);
                case DivideExpTree divideExpTree: return Visit(divideExpTree);
                case MultiplyExpTree multiplyExpTree: return Visit(multiplyExpTree);
                case NumberExp numberExp: return Visit(numberExp);
                case SubExpTree subExpTree: return Visit(subExpTree);
                case UnaryExpTree unaryExpTree: return Visit(unaryExpTree);
            }

            throw new Exception("Error!");
        }

        public Expression<T> Visit(AddExpTree syntax) {
            return Expression.Lambda<T>(
                Expression.Add(
                    syntax.Lhs.Compile(this).Body,
                    syntax.Rhs.Compile(this).Body));
        }

        public Expression<T> Visit(SubExpTree syntax) {
            return Expression.Lambda<T>(
                Expression.Subtract(
                    syntax.Lhs.Compile(this).Body,
                    syntax.Rhs.Compile(this).Body));
        }

        public Expression<T> Visit(MultiplyExpTree syntax) {
            return Expression.Lambda<T>(
                Expression.Multiply(
                    syntax.Lhs.Compile(this).Body,
                    syntax.Rhs.Compile(this).Body));
        }

        public Expression<T> Visit(DivideExpTree syntax) {
            return Expression.Lambda<T>(
                Expression.Divide(
                    syntax.Lhs.Compile(this).Body,
                    syntax.Rhs.Compile(this).Body));
        }

        public Expression<T> Visit(NumberExp syntax) {
            return Expression.Lambda<T>(Expression.Constant(syntax.Number));
        }

        public Expression<T> Visit(UnaryExpTree syntax) {
            return syntax.Rhs.Compile(this);
        }
    }
}