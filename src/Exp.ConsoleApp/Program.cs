#region [ ReSharper ]

// ReSharper disable ArrangeAccessorOwnerBody
// ReSharper disable ConvertSwitchStatementToSwitchExpression
// ReSharper disable PublicConstructorInAbstractClass
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MergeIntoPattern
// ReSharper disable UnusedMemberInSuper.Global

#endregion [ ReSharper ]

namespace Exp.ConsoleApp {
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Text;
    
    static class Program {
        public static void Main(string[] options) {
            if (options.Length == 0) {
                Sample1();
                Sample2();
                return;
            }

            if (options[0] == "-i" || options[0] == "--interactive")
                Interactive();
            else throw new Exception("Invalid Option!");
        }

        private static void Sample1() {
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
            Console.WriteLine();
        }

        private static void Sample2() {
            CompileAndRunFromSourceUsingEvaluator("-2 + 1 * 3 * -5 / 2");
            
            CompileAndRunFromAstUsingEvaluator(
                new AddExpTree(
                    new UnaryExpTree(new NumberExp(2)),
                    new DivideExpTree(
                        new MultiplyExpTree(
                            new MultiplyExpTree(
                                new NumberExp(1),
                                new NumberExp(3)),
                            new UnaryExpTree(new NumberExp(5))),
                        new NumberExp(2))));
            Console.WriteLine();
        }

        private static void Interactive() {
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += delegate {
                Console.WriteLine("Bye!");
                cts.Cancel();
            };
            
            Console.WriteLine("Welcome!");
            while (!cts.IsCancellationRequested) {
                
                Console.Write("$ ");

                var source = Console.ReadLine()!;
                var parser = new Parser(new Lexer(source));
                
                var sw = Stopwatch.StartNew();
                var syntax = parser.Parse();
                
                var lambda = syntax.Accept(new ExpressionLambdaCompiler());
                Summary(lambda.Body.ToString(), syntax, sw.ElapsedMilliseconds, sw.ElapsedTicks);
                MyProgram program = lambda.Compile();

                Console.WriteLine(">> {0}", program());
            }
        }

        private static void Summary(string body, SyntaxTree syntax, long ms, long ticks) {
            Console.WriteLine();
            Console.WriteLine("Compiled: {0}", body);
            Console.WriteLine("C# Syntax Tree: {0}", syntax.GenerateCsharp());
            Console.WriteLine("Elapsed Compile Time in Ms: {0}", ms);
            Console.WriteLine("Elapsed Compile Time in Ticks: {0}", ticks);
            Console.WriteLine();
        }

        private static void CompileAndRunFromSourceUsingEvaluator(string source) {
            CompileAndRunFromAstUsingEvaluator(Parser.Parse(source));
        }
        
        private static void CompileAndRunFromAstUsingEvaluator(SyntaxTree addExpTree) {
            var result = Compile(addExpTree, new ExpressionEvaluator());
            Console.WriteLine("Result: {0}", result);
        }

        private static void CompileAndRunFromSource(string source) {
            CompileAndRunFromAst(Parser.Parse(source));
        }

        private static void CompileAndRunFromAst(SyntaxTree ast) {
            var lambda = Compile(ast, new ExpressionLambdaCompiler());

            Console.WriteLine("Expression Tree: {0}", lambda.ToString());
            
            MyProgram program = lambda.Compile();
            
            Console.WriteLine("Result: {0}", program());
        }
        
        private static T Compile<T>(SyntaxTree ast, ISyntaxTreeVisitor<T> syntaxTreeVisitor) {
            Console.WriteLine("C# Syntax Tree: {0}", ast.GenerateCsharp());

            return ast.Accept(syntaxTreeVisitor);
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

        public abstract T Accept<T>(ISyntaxTreeVisitor<T> visitor);
    }

    internal interface ISyntaxTreeVisitor<T> {
        T Visit(AddExpTree syntaxNode);
        T Visit(SubExpTree syntaxNode);
        T Visit(MultiplyExpTree syntaxNode);
        T Visit(DivideExpTree syntaxNode);
        T Visit(UnaryExpTree syntaxNode);
        T Visit(NumberExp syntaxNode);
    }

    internal abstract class ExpTree : SyntaxTree {
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

        public override T Accept<T>(ISyntaxTreeVisitor<T> visitor) {
            return visitor.Visit(this);
        }
    }

    internal class SubExpTree : BinaryExpTree {
        public SubExpTree(SyntaxTree lhs, SyntaxTree rhs) : base(lhs, rhs) {
        }

        public override T Accept<T>(ISyntaxTreeVisitor<T> visitor) {
            return visitor.Visit(this);
        }
    }

    internal class MultiplyExpTree : BinaryExpTree {
        public MultiplyExpTree(SyntaxTree lhs, SyntaxTree rhs) : base(lhs, rhs) {
        }

        public override T Accept<T>(ISyntaxTreeVisitor<T> visitor) {
            return visitor.Visit(this);
        }
    }

    internal class DivideExpTree : BinaryExpTree {
        public DivideExpTree(SyntaxTree lhs, SyntaxTree rhs) : base(lhs, rhs) {
        }

        public override T Accept<T>(ISyntaxTreeVisitor<T> visitor) {
            return visitor.Visit(this);
        }
    }

    internal class UnaryExpTree : ExpTree {
        public UnaryExpTree(SyntaxTree rhs) {
            Rhs = rhs is NumberExp number ? new NumberExp(number.Number * -1) : rhs;
        }

        public SyntaxTree Rhs { get; }
        
        public override T Accept<T>(ISyntaxTreeVisitor<T> visitor) {
            return visitor.Visit(this);
        }
    }

    internal class NumberExp : ExpTree {
        public NumberExp(decimal number) {
            Number = number;
        }

        public decimal Number { get; }
        
        public override T Accept<T>(ISyntaxTreeVisitor<T> visitor) {
            return visitor.Visit(this);
        }
    }

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

    internal class ExpressionEvaluator : ISyntaxTreeVisitor<decimal> {
        public decimal Visit(UnaryExpTree syntaxNode) {
            return syntaxNode.Rhs.Accept(this);
        }

        public decimal Visit(NumberExp syntaxNode) {
            return syntaxNode.Number;
        }
        
        public decimal Visit(AddExpTree syntaxNode) {
            return syntaxNode.Lhs.Accept(this) +
                   syntaxNode.Rhs.Accept(this);
        }

        public decimal Visit(SubExpTree syntaxNode) {
            return syntaxNode.Lhs.Accept(this) -
                   syntaxNode.Rhs.Accept(this);
        }

        public decimal Visit(MultiplyExpTree syntaxNode) {
            return syntaxNode.Lhs.Accept(this) *
                   syntaxNode.Rhs.Accept(this);
        }

        public decimal Visit(DivideExpTree syntaxNode) {
            return syntaxNode.Lhs.Accept(this) /
                   syntaxNode.Rhs.Accept(this);
        }
    }
}