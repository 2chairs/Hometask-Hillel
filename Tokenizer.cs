using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hometask1
{
    internal class Tokenizer
    {
        private static readonly Regex Regex;
        static List<(string Pattern, Func<string, Token> ToToken)> Patterns = new()
        {
            ("\\d+(\\.\\d+)?(e[+-]?\\d+?)", match => new Number (double.Parse(match))),

            ("(?<=[\\d|)]\\s*)-", _ => new BinaryOperation((x, y) => x - y, 1)),
            ("\\+", _ => new BinaryOperation((x, y) => x + y, 1)),
            ("\\*", _ => new BinaryOperation((x, y) => x * y, 2)),
            ("/", _ => new BinaryOperation((x, y) => x / y, 2)),

            ("-", _ => new UnaryOperation((x) => -x,3)),
            ("&", _ => new BinaryOperation(Math.Pow, 3, RightAssoiciative: true)),

            ("\\(", _ => new LeftParenthesis()),
            ("\\)", _ => new RightParenthesis()),

            //("\\S", _ => throw new Exception("unknown symbol")),
        };

        static Tokenizer ()

        {
            var patterns = Patterns.Select((item, index) => $"(?<name_{index}>{item.Pattern})");
            var pattern = string.Join ("|", patterns);
            Regex = new Regex(pattern, RegexOptions.IgnoreCase);
        }

        public static List<Token> Tokenize (string expression)
        {
            var tokens  = Regex 
                .Matches (expression)
                .Select(match => match.Groups.Values.Last(gr => gr.Success))
                .Select(gr =>
                {
                    var indexStr = gr.Name.Substring("name_" .Length);
                    var index = int.Parse(indexStr);
                    return Patterns[index].ToToken(gr.Value);
                })
                .ToList ();
            return tokens;
        }
    }

    record Token;

    record Number (double Value) : Token;

    record Operation (int? Priority = null, bool RightAssoiciative = false) :Token
    {
        public bool IsPriosThan(Operation next)
        {
            if (Priority == null)
            {
                return false;
            }

            if (this.Priority > next.Priority)
            {
                return true;
            }

            if (this.Priority == next.Priority && !next.RightAssoiciative)
            {
                 return true;
            }

            return false;
        }
    }

    record BinaryOperation(Func<double, double, double> Calculate, int? Priority, 
            bool RightAssoiciative = false)
                : Operation (Priority, RightAssoiciative);

    record UnaryOperation(Func<double, double> Calculate, int? Priority)
        :Operation (Priority, RightAssoiciative:true);

    record LeftParenthesis : Operation;

    record RightParenthesis : Operation;
}
