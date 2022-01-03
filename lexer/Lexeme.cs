// ReSharper disable MemberCanBePrivate.Global
namespace lexer;

public class Lexeme
{
    public enum LexemeTypes
    {
        Name,
        LitNumber,
        LitBoolean,
        Operator,
        Function
    };
    
    public string Value { get; }
    
    public LexemeTypes Type { get; }

    public Lexeme(string value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
        Type = FindType(value);
    }
    
    public Lexeme(string value, LexemeTypes type)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
        Type = type;
    }

    public override string ToString() => $"[{Type}({Value})]";

    public static LexemeTypes FindType(string value) =>
        IsBoolean(value) ? LexemeTypes.LitBoolean :
        IsOperator(value) ? LexemeTypes.Operator :
        IsFunction(value) ? LexemeTypes.Function :
        IsNumber(value) ? LexemeTypes.LitNumber : LexemeTypes.Name;

    public static bool IsNumber(string value) => double.TryParse(value, out double _);

    public static bool IsBoolean(string value) => value is "true" or "True" or "false" or "False";

    public static bool IsOperator(string value) => value is "+" or "-" or "*" or "/" or "^";
    
    public static bool IsFunction(string value) => value is "sin" or "cos" or "tan" or "cot";
}