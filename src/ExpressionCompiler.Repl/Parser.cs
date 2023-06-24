namespace ExpressionCompiler.Repl
{
  using System;

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

      if (_lexer.Token == Token.LPar) {
        _lexer.Walk();

        var leafNode = AddOrSubSyntaxNode();
        if (_lexer.Token == Token.RPar) {
          _lexer.Walk();
          return leafNode;
        }

        throw new Exception("RPar ')' missing");
      }

      throw new Exception($"Error! Invalid token {_lexer.Token}");
    }

    public static SyntaxTree Parse(string source) {
      var parser = new Parser(new Lexer(source));
      return parser.Parse();
    }
  }
}