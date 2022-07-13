using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayLife
{
    public class Field
    {
        private const char VerticalBorderSymbol = '║';
        private const char HorizontalBorderSymbol = '=';
        private const char AliveCellSymbol = 'X'; // '¤' '■'
        private const char ImmortalCellSymbol = '#'; //
        private const char EmptyCellSymbol = ' '; //

        private int _rows;
        private int _columns;
        private CellStatus[,] _currentStateOfField;
        private long _generation;
        private Dictionary<uint, long> _dictionary;
        private IRules _setOfRules;
        

        public Field(
            int rows, 
            int columns, 
            IRules setOfRules)
        {
            _dictionary = new Dictionary<uint, long>();
            _rows = rows;
            _columns = columns;
            _generation = 1;
            _setOfRules = setOfRules;            
            _currentStateOfField = new CellStatus[_rows, _columns];
        }

        public int AliveCells // Alive cell is every cell except empty one. Immortal cells are alive cells too.
        {
            get
            {
                var aliveCellsNumber = 0;

                foreach (var item in _currentStateOfField)
                {
                    if (item != CellStatus.Empty)
                    {
                        aliveCellsNumber++;
                    }
                }

                return aliveCellsNumber;
            }
        }

        public long CycleLength { get; private set; }

        public bool AllAreDead => AliveCells == 0;

        public bool CycleAchieved { get; private set; }

        public long Generation
        {
            get 
            {
                return _generation; 
            }

            private set 
            {
                if (!CycleAchieved)
                {
                    _generation = value;
                }            
            }
        }

        public void MakeMove()
        {
            Show();

            if (!CycleAchieved)
            {
                FillHashDict();
            }

            CalculateNextGeneration();
        }

        public void InitializeLife(string generationZeroPattern)
        {
            var lines = generationZeroPattern.Split("\r\n").ToList();
            lines.RemoveAt(0);
            lines.RemoveAt(lines.Count - 1);
            lines = lines.Take(Math.Min(lines.Count, _rows)).ToList();

            var row = 0;

            foreach (var line in lines)
            {
                var column = 0;
                var cutLine = line.Substring(0, Math.Min(_columns, line.Length));

                foreach (var symbol in cutLine.ToUpper())
                {
                    switch (symbol)
                    {
                        case AliveCellSymbol:
                            _currentStateOfField[row, column] = CellStatus.Alive;
                            break;
                        case ImmortalCellSymbol:
                            _currentStateOfField[row, column] = CellStatus.Immortal;
                            break;
                        default:
                            _currentStateOfField[row, column] = CellStatus.Empty;
                            break;
                    }

                    column++;
                }

                row++;
            }
        }

        public void SetImmortalCell(int row, int column)
        {
            _currentStateOfField[row, column] = CellStatus.Immortal;
        }

        public void CalculateNextGeneration()
        {
            var temporaryStateOfField = _setOfRules.SurviveDieOrBorn(_rows, _columns, _currentStateOfField);

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    _currentStateOfField[i, j] = temporaryStateOfField[i, j];
                }
            }

            Generation++;
        }
        
        public void Show()
        {
            Console.Clear();
            PrintHorizontalBorder();

            for (int i = 0; i < _rows; i++)
            {
                PrintLine(i);
            }

            PrintHorizontalBorder();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("---Statistics---");

            var needToSkipCurrentGenDisplay = CycleAchieved || AllAreDead;

            if (!needToSkipCurrentGenDisplay)
            {
                Console.WriteLine($"Current generation: {_generation}");
            }

            Console.WriteLine($"Alive cells: {AliveCells}");

            void PrintLine(int rowNumber)
            {
                Console.Write(VerticalBorderSymbol);

                for (int i = 0; i < _columns; i++)
                {
                    switch (_currentStateOfField[rowNumber, i])
                    {
                        case CellStatus.Alive:
                            Console.Write(AliveCellSymbol);
                            break;
                        case CellStatus.Immortal:
                            Console.Write(ImmortalCellSymbol);
                            break;
                        case CellStatus.Empty:
                            Console.Write(EmptyCellSymbol);
                            break;
                    }
                }

                Console.Write(VerticalBorderSymbol);
                Console.WriteLine();
            }

            void PrintHorizontalBorder()
            {
                Console.WriteLine(new string(HorizontalBorderSymbol, _columns + 2));
            }
        }

        public void Center()
        {
            var rightShift = (_columns - GetVerticalBound()) / 2;
            var downShift = (_rows - GetHorizontalBound()) / 2;

            ShiftToRight(rightShift);
            ShiftDown(downShift);

            int GetVerticalBound()
            {
                int verticalBound = 0;
                int row = 0;
                int falses = 0;

                do
                {
                    falses = 0;

                    for (int i = 0; i < _columns; i++) // пройти по всем элементам в ряду.
                    {
                        if (_currentStateOfField[row, i] == CellStatus.Alive)
                        {
                            falses = 0;
                        }
                        else
                        {
                            falses++;
                        }
                    }

                    var oneRowLength = _columns - falses;
                    verticalBound = oneRowLength > verticalBound ? oneRowLength : verticalBound;
                    row++;

                } while (row <= _rows - 1);

                return verticalBound;
            }

            int GetHorizontalBound()
            {
                int horizontalBound = 0;
                int column = 0;
                int falses = 0;

                do
                {
                    falses = 0;

                    for (int i = 0; i < _rows; i++) // пройти по всем элементам в колонке.
                    {
                        if (_currentStateOfField[i, column] == CellStatus.Alive)
                        {
                            falses = 0;
                        }
                        else
                        {
                            falses++;
                        }
                    }

                    var oneRowLength = _rows - falses;
                    horizontalBound = oneRowLength > horizontalBound ? oneRowLength : horizontalBound;
                    column++;

                } while (column <= _columns - 1);

                return horizontalBound;
            }
        } // Центрировать паттерн на поле.

        protected void ShiftToRight(int n)
        {
            var temporaryStateOfField = new CellStatus[_rows, _columns];

            for (int i = 0; i < _rows; i++) // Запись во временный массив новых координат с учётом сдвига.
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (_currentStateOfField[i, j] != CellStatus.Empty)
                    {
                        temporaryStateOfField[i, j + n] = _currentStateOfField[i, j];
                    }
                }
            }

            for (int i = 0; i < _rows; i++) // запись из временного массива в текущий.
            {
                for (int j = 0; j < _columns; j++)
                {
                    _currentStateOfField[i, j] = temporaryStateOfField[i, j];
                }
            }
        }

        protected void ShiftDown(int n)
        {
            var temporaryStateOfField = new CellStatus[_rows, _columns];

            for (int i = 0; i < _rows; i++) // Запись во временный массив новых координат с учётом сдвига.
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (_currentStateOfField[i, j] != CellStatus.Empty)
                    {
                        temporaryStateOfField[i + n, j] = _currentStateOfField[i, j];
                    }
                }
            }

            for (int i = 0; i < _rows; i++) // запись из временного массива в текущий.
            {
                for (int j = 0; j < _columns; j++)
                {
                    _currentStateOfField[i, j] = temporaryStateOfField[i, j];
                }
            }
        }

        public void FillHashDict()
        {
            var hash = GetMyHashCode();
            
            if (_dictionary.ContainsKey(hash)) // Если данный хэш уже встречался...
            {
                CycleAchieved = true; // ...возвести флаг цикл достигнут.               
                CycleLength = Generation - _dictionary[hash]; // Вычислить длину цикла. Это разница между текущим поколением и поколением, когда данный хэш впервые встретился.
            }
            else // Если данный хэш никогда не встречался - добавить его в словарь как ключ. Значение - номер поколения, на котором он встретился.
            {
                _dictionary.Add(hash, Generation);
            }
        }

        public uint GetMyHashCode()
        {
            var currentStateOfField = ConvertCurrentStateOfField();

            return (uint)currentStateOfField.GetHashCode();
        }

        private string ConvertCurrentStateOfField()
        {
            StringBuilder sb = new StringBuilder(_rows * _columns);

            foreach (var item in _currentStateOfField)
            {
                sb.Append((int)item);
            }

            return sb.ToString();
        }
    }
}