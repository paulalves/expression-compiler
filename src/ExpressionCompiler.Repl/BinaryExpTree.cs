namespace ExpressionCompiler.Repl
{
  internal abstract class BinaryExpTree : ExpTree {
    public BinaryExpTree(SyntaxTree lhs, SyntaxTree rhs) {
      Lhs = lhs;
      Rhs = rhs;
    }

    public SyntaxTree Lhs { get; }
    public SyntaxTree Rhs { get; }
  }
}