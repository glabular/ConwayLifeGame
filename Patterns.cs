using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConwayLife
{
    /// <summary>
    /// Almost everything in this class, including all properties and methods are written by another person! 
    /// </summary>
    public class Patterns
    {
        private const string NewLine = "\r\n";

        public static string Glider => @"
 x
  x
xxx
";

        public static string Custom => @"
     x
    x x
   x   x
  x     x
 x       x
x         x
 x       x
  x     x
   x   x
    x x
     x
";

        public static string Line(int n)
        {
            return NewLine + new string('x', n) + NewLine;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sideSize">Length of one cross beam. Total size of the created cross will be (2 * sideSize + 1)</param>
        public static string StraightCross(int sideSize)
        {
            var pattern = new StringBuilder();
            pattern.Append(NewLine);

            for (var i = 0; i < sideSize; i++)
            {
                pattern.Append(new string(' ', sideSize));
                pattern.Append('x');
                pattern.AppendLine(new string(' ', sideSize));
            }

            pattern.AppendLine(new string('x', 2 * sideSize + 1));

            for (var i = 0; i < sideSize; i++)
            {
                pattern.Append(new string(' ', sideSize));
                pattern.Append('x');
                pattern.AppendLine(new string(' ', sideSize));
            }

            return pattern.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sideSize">Length of one cross beam. Total size of the created cross will be (2 * sideSize + 1)</param>
        public static string ObliqueCross(int sideSize)
        {
            var pattern = new StringBuilder();
            pattern.Append(NewLine);

            for (var i = 0; i < sideSize; i++)
            {
                pattern.Append(new string(' ', i));
                pattern.Append('x');
                pattern.Append(new string(' ', sideSize - i - 1));
                pattern.Append(new string(' ', 1));
                pattern.Append(new string(' ', sideSize - i - 1));
                pattern.AppendLine("x");
            }

            pattern.Append(new string(' ', sideSize));
            pattern.AppendLine("x");

            for (var i = sideSize - 1; i >= 0; i--)
            {
                pattern.Append(new string(' ', i));
                pattern.Append('x');
                pattern.Append(new string(' ', sideSize - i - 1));
                pattern.Append(new string(' ', 1));
                pattern.Append(new string(' ', sideSize - i - 1));
                pattern.AppendLine("x");
            }

            return pattern.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sideSize">Length of one cross beam. Total size of the created cross will be (2 * sideSize + 1)</param>
        public static string EightCross(int sideSize)
        {
            var straightCrossPattern = StraightCross(sideSize);
            var obliqueCrossPattern = ObliqueCross(sideSize);

            return PerformOperationOnPatterns(straightCrossPattern, Add, obliqueCrossPattern);
        }

        public static string Angle(int sideSize, int width = 1)
        {
            var largeSquarePattern = Square(sideSize);
            var smallSquarePattern = Square(sideSize - width);

            return PerformOperationOnPatterns(largeSquarePattern, Subtract, smallSquarePattern);
        }

        public static string HollowSquare(int sideSize, int width = 1)
        {
            var largeSquarePattern = Square(sideSize);
            var smallSquarePattern = Square(sideSize - width * 2);            
            smallSquarePattern = ShiftDown(smallSquarePattern, width);
            smallSquarePattern = ShiftRight(smallSquarePattern, width);

            return PerformOperationOnPatterns(largeSquarePattern, Subtract, smallSquarePattern);
        }

        public static string Square(int n)
        {
            var pattern = new StringBuilder();
            pattern.Append(NewLine);

            for (var i = 0; i < n; i++)
            {
                pattern.AppendLine(new string('x', n));
            }

            return pattern.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sideSize">Length of side. Total size of the created diamond will be (2 * sideSize - 1)</param>
        public static string Diamond(int sideSize)
        {
            var pattern = new StringBuilder();
            pattern.Append(NewLine);

            for (var i = 0; i < (sideSize - 1); i++)
            {
                pattern.Append(new string(' ', sideSize - i - 1));
                pattern.AppendLine(new string('x', 2 * i + 1));
            }

            pattern.AppendLine(new string('x', 2 * (sideSize - 1) + 1));

            for (var i = (sideSize - 1) - 1; i >= 0; i--)
            {
                pattern.Append(new string(' ', sideSize - i - 1));
                pattern.AppendLine(new string('x', 2 * i + 1));
            }

            return pattern.ToString();
        }

        public static string SquareWithImortalCorners(int n)
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

        #region Operations on patterns

        private static string PerformOperationOnPatterns(string pattern1, Func<char, char, char> operationOnchars, string pattern2)
        {
            // Turn one-string pattern into lines
            var pattern1Lines = pattern1.Split("\r\n").ToList();
            var pattern2Lines = pattern2.Split("\r\n").ToList();

            // Make patterns have the same number of lines (empty lines included)
            var maxNumberOfLines = Math.Max(pattern1Lines.Count, pattern2Lines.Count);
            for (var lineNumber = 0; lineNumber < maxNumberOfLines; lineNumber++)
            {
                if (pattern1Lines.Count <= lineNumber)
                {
                    pattern1Lines.Add(string.Empty);
                }

                if (pattern2Lines.Count <= lineNumber)
                {
                    pattern2Lines.Add(string.Empty);
                }
            }

            // Make all lines have the same length (trailing spaces included)
            var maxLineLength = pattern1Lines.Select(line => line.Length)
                .Union(pattern2Lines.Select(line => line.Length))
                .Max();
            for (var lineNumber = 0; lineNumber < maxNumberOfLines; lineNumber++)
            {
                pattern1Lines[lineNumber] = pattern1Lines[lineNumber].PadRight(maxLineLength);
                pattern2Lines[lineNumber] = pattern2Lines[lineNumber].PadRight(maxLineLength);
            }

            // Perform operation on lines
            var resultingPattern = new StringBuilder();
            for (var lineNumber = 0; lineNumber < maxNumberOfLines; lineNumber++)
            {
                var pattern1Line = pattern1Lines[lineNumber];
                var pattern2Line = pattern2Lines[lineNumber];

                // Perform operation on chars
                for (var charIndex = 0; charIndex < pattern1Line.Length; charIndex++)
                {
                    var pattern1Char = pattern1Line[charIndex];
                    var pattern2Char = pattern2Line[charIndex];
                    var newChar = operationOnchars(pattern1Char, pattern2Char);
                    resultingPattern.Append(newChar);
                }

                resultingPattern.AppendLine();
            }

            return resultingPattern.ToString();
        }

        private static char Add(char pattern1Char, char pattern2Char)
        {
            return pattern1Char == ' ' && pattern2Char == ' ' ? ' ' : 'x';
        }

        private static char Subtract(char pattern1Char, char pattern2Char)
        {
            return pattern1Char == ' ' || pattern2Char == 'x' ? ' ' : 'x';
        }

        private static string ShiftRight(string pattern, int shift = 1)
        {
            var shiftSpaces = new string(' ', shift);
            return pattern.Replace("\r\n", $"\r\n{shiftSpaces}");
        }

        private static string ShiftDown(string pattern, int shift = 1)
        {
            for (var i = 0; i < shift; i++)
            {
                pattern = $"\r\n{pattern}";
            }

            return pattern;
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

        #endregion
    }
}
