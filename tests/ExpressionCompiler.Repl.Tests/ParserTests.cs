namespace ExpressionCompiler.Repl.Tests {
    using ExpressionCompiler.Repl;
    using Xunit;
    using Xunit.Abstractions;

    public class ParserTests {
        private readonly ITestOutputHelper _output;

        public ParserTests(ITestOutputHelper output) {
            _output = output;
        }
        
        [Theory]
        [InlineData(" 2 + 2 ",4)]
        [InlineData(" 2 + ( - 5 )",-3)]
        [InlineData(" 2 + ( - 1 )",1)]
        [InlineData(" 2 + ( - 2 )",-0)]
        [InlineData(" 2 + ( - 3 )",-1)]
        public void Add(string source, decimal expected) {
            Assert.Equal(expected, Calc(source));
        }

        [Theory]
        [InlineData(" 2 -2", 0)]
        [InlineData(" 7 -2 - ( -1 )", 6)]
        [InlineData(" 5 -2 - ( -2 )", 5)]
        [InlineData(" 3 -2 - ( -2 )", 3)]
        public void Sub(string source, decimal expected) {
            Assert.Equal(expected, Calc(source));
        }
            
        [Theory]
        [InlineData("2 * 1", 2)]
        [InlineData("2 * 2", 4)]
        [InlineData("2 * ( -2 * -1 )", 4)]
        [InlineData("2 * -1 * -1 * ( -2 * -1 ) * -1", -4)]
        public void Multiply(string source, decimal expected) {
            Assert.Equal(expected, Calc(source));
        }

        [Theory]
        [InlineData(" 2 / 2", 1)]
        [InlineData(" 100 / 2/ 2", 25)]
        [InlineData(" 100 / 2 / -2", -25)]
        [InlineData(" 50 / 2 / -1", -25)]
        [InlineData(" 50 / 2", 25)]
        public void Divide(string source, decimal expected) {
            Assert.Equal(expected, Calc(source));
        }
        
        [Theory]
        [InlineData("25 - 50 / 2 * 2", -25)]
        [InlineData("25 - -50 / 2 * 2", 75)]
        [InlineData("25 + -1 / 2 * 2 -1 + 2", 25)]
        [InlineData("25 + (-1 / 2 * (2 - 2)) + 2", 27)]
        public void AllIn(string source, decimal expected) {
            Assert.Equal(expected, Calc(source));
        }
        
        private decimal Calc(string source) {
            var syntaxTree = Repl.Parser.Parse(source);
            _output.WriteLine(syntaxTree.Accept(new Repl.CsharpCodeGenerator()));
            
            var lambda = syntaxTree.Accept(new Repl.ExpressionLambdaCompiler());
            _output.WriteLine(string.Empty);
            _output.WriteLine(lambda.ToString());
            
            var program = lambda.Compile();
            return program(); 
        }
    }
}