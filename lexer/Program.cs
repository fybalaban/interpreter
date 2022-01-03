using System.Text;
using System.Text.RegularExpressions;
using static System.Console;

namespace lexer
{
    internal static class Program
    {
        internal static readonly Regex UnnecessaryWhitespaces = new("\\s+(?=((\\\\[\\\\\"]|[^\\\\\"])*\"(\\\\[\\\\\"]|[^\\\\\"])*\")*(\\\\[\\\\\"]|[^\\\\\"])*$)", RegexOptions.Compiled);
        
        internal static int Main(string[] args)
        {
            if (args.Length is 1 && args[0] is "--interactive" or "-i")
            {
                while (true)
                {
                    string? input = ReadLine();
                    if (string.IsNullOrWhiteSpace(input) is false)
                        WriteLine(Tokenize(input));
                }
            }

            if (args.Length is 1 && args[0].Contains((char)34))
            {
                File.WriteAllText(Path.Combine(AppContext.BaseDirectory, "_tokenoutput"), Tokenize(args[0]));
                return 0;
            }
            
            HelpMessage();
            return -1;
        }

        private static string Tokenize(string inputString)
        {
            string[] tokens = TokenizeIntoArray(inputString);
            string concatenatedTokens = string.Empty;
            for (int i = 0; i < tokens.Length; i++)
                concatenatedTokens += tokens[i];
            return concatenatedTokens;
        }

        private static string[] TokenizeIntoArray(string inputString)
        {
            List<Lexeme> tokens = new();
            int cursor = 0;
            string token = "";

            while (cursor < inputString.Length)
            {
                string currentCharacter = inputString[cursor].ToString();
                string nextCharacter = cursor + 1 < inputString.Length ? inputString[cursor + 1].ToString() : "";

                token += currentCharacter;
                Lexeme.LexemeTypes currentType = Lexeme.FindType(token);
                Lexeme.LexemeTypes nextType = nextCharacter is not "" ? Lexeme.FindType(nextCharacter) : Lexeme.LexemeTypes.None;
                
                if (nextCharacter is not "" && nextType != currentType || nextType is Lexeme.LexemeTypes.Operator && currentType is Lexeme.LexemeTypes.Operator)
                {
                    tokens.Add(new Lexeme(token));
                    token = "";
                }
                else if (nextCharacter is "")
                    tokens.Add(new Lexeme(token));

                cursor++;
            }

            return (from x in tokens select x.ToString()).ToArray();
        }

        private static void HelpMessage()
        {
            throw new NotImplementedException();
        }
    }
}