#region [ ReSharper ]

// ReSharper disable ArrangeAccessorOwnerBody
// ReSharper disable ConvertSwitchStatementToSwitchExpression
// ReSharper disable PublicConstructorInAbstractClass
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MergeIntoPattern
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable ArrangeNamespaceBody

#endregion [ ReSharper ]

[assembly:System.Runtime.CompilerServices.InternalsVisibleTo("Exp.Tests")]
namespace Exp.ConsoleApp {
    using System.Diagnostics;

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
                switch (source.Trim().ToLowerInvariant()) {
                    case ":q":
                    case "/q":
                    case "q":
                    case "quit":
                    case "exit":
                        cts.Cancel();
                        continue;
                    case "/clear":
                    case "clear":
                    case "cls":
                        Console.Clear();
                        continue;
                    case "":
                        continue;
                }

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
            Console.WriteLine("C# Syntax Tree: {0}", syntax.Accept(new CsharpCodeGenerator()));
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

            Console.WriteLine("Expression Tree: {0}", lambda);

            MyProgram program = lambda.Compile();

            Console.WriteLine("Result: {0}", program());
        }

        private static T Compile<T>(SyntaxTree ast, ISyntaxTreeVisitor<T> syntaxTreeVisitor) {
            Console.WriteLine("C# Syntax Tree: {0}", ast.Accept(new CsharpCodeGenerator()));

            return ast.Accept(syntaxTreeVisitor);
        }
    }
}