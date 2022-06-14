using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConwayLife
{
    public class Patterns
    {
        private const string NewLine = "\r\n";

        public static string Test => @"
xxx
 xxx
  xxx

 x x x

x
";

        // Lives forever
        public static string Square => @"
xx
xx
";

        // Oscilates
        public static string ThreeInLine => @"

 xxx
";

        // ???
        public static string Glider => @"
 x
  x
xxx
";
        public static string Mine => @"
x
x
x
x

x
x
x x x x x x x x x
x  x xx xx x
x x x x xxxx
x x xxxx
xxxx x x x x x x 
xxxx x xxxxxxxx
x
x
x
xx x x xxx x  x
x x 
x 
x 
x 
x
x

x
";

        public static string Line(int n)
        {
            return NewLine + new string('x', n) + NewLine;
        }

        public static string Cube(int n)
        {
            var pattern = new StringBuilder();
            pattern.Append(NewLine);

            for (var i = 0; i < n; i++)
            {
                pattern.AppendLine(new string('x', n));
            }

            return pattern.ToString();
        }
        public static string CubeWithImortalCorners(int n)
        {
            var pattern = new StringBuilder();
            pattern.Append(NewLine);

            pattern.AppendLine($"#{new string('x', n - 2)}#");

            for (var i = 1; i < n - 1; i++)
            {
                pattern.AppendLine(new string('x', n));
            }

            pattern.AppendLine($"#{new string('x', n - 2)}#");

            return pattern.ToString();
        }

        public static string GliderGun
        {
            get
            {
                // A pattern can be represented by encoded lines for convenience
                // e = empty cell; a = alive cell; number after a letter = number of cells in a row
                // E.g. "e24a1" means "24 empty cells, then 1 alive cell"
                var encodedLines = new[]
                {
                  "e24a1",
                  "e22a1e1a1",
                  "e12a2e6a2e12a2",
                  "e11a1e3a1e4a2e12a2",
                  "a2e8a1e5a1e3a2",
                  "a2e8a1e3a1e1a2e4a1e1a1",
                  "e10a1e5a1e7a1",
                  "e11a1e3a1",
                  "e12a2",
                };

                return GeneratePlainPatternFromEncodedLines(encodedLines);
            }
        }

        private static string GeneratePlainPatternFromEncodedLines(IEnumerable<string> encodedLines)
        {
            var sb = new StringBuilder();

            sb.AppendLine();

            foreach (var encodedLine in encodedLines)
            {
                sb.AppendLine(GenerateLine(encodedLine));
            }

            sb.AppendLine();

            return sb.ToString();


            // Local method
            string GenerateLine(string encodedLine)
            {
                var regex = new Regex(@"(\w)(\d+)");
                var line = new StringBuilder();

                foreach (Match match in regex.Matches(encodedLine))
                {
                    var symbol = GetLineSymbol(match.Groups[1].Value.ToLower()[0]);
                    var number = int.Parse(match.Groups[2].Value);
                    line.Append(new string(symbol, number));
                }

                return line.ToString(); ;
            }

            // Local method
            char GetLineSymbol(char encodedSymbol)
            {
                switch (encodedSymbol)
                {
                    case 'e':
                        return ' ';
                    case 'a':
                        return 'x';
                    default:
                        throw new ArgumentException("encodedSymbol");
                }
            }
        }
    }
}
